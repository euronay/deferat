using Deferat.Models;
using Deferat.Repository;
using Deferat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Deferat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //     options.CheckConsentNeeded = context => true;
            //     options.MinimumSameSitePolicy = SameSiteMode.None;
            // });
            var postsDir = Environment.GetEnvironmentVariable("POSTS");
            var authorDir = Environment.GetEnvironmentVariable("AUTHORS");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IFormatterService, FormatterService>();
            services.AddSingleton<IFileReader<Post>, FileReader<Post>>();
            services.AddSingleton<IFileReader<Author>, FileReader<Author>>();
            services.AddScoped<IRepository<Post>>(ctx => new Repository<Post>(
                postsDir, 
                post => {
                    // TODO: tidy this mess up
                    var formatter = ctx.GetService<IFormatterService>();
                    post.Html = formatter.FixImages(post.Html, post.Id);;
                    post.Image = $"/posts/{post.Id}/{post.Image}";
                    post.ShortContent = formatter.CreateTruncatedContent(post.Html, 200);
                    post.Categories = post.Categories.OrderBy(c => c);
                    return post;
                }, 
                ctx.GetService<ILogger<Repository<Post>>>(), 
                ctx.GetService<IFileReader<Post>>()));
            services.AddScoped<IRepository<Author>>(ctx => new Repository<Author>(authorDir,
                author => author, 
                ctx.GetService<ILogger<Repository<Author>>>(), 
                ctx.GetService<IFileReader<Author>>()));

            services.AddScoped<IRepositoryContainer, RepositoryContainer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger, IFormatterService formatter, 
            IRepository<Post> postRepository, IRepository<Author> authorRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var postsDir = Environment.GetEnvironmentVariable("POSTS");
            logger.LogInformation($"Posts directory: {postsDir}");

            if(!String.IsNullOrWhiteSpace(postsDir))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(postsDir),
                    RequestPath = "/posts"
                });
            }

            var authorDir = Environment.GetEnvironmentVariable("AUTHORS");
            logger.LogInformation($"Authors directory: {authorDir}");

            if(!String.IsNullOrWhiteSpace(authorDir))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(authorDir),
                    RequestPath = "/authors"
                });
            }

            app.UseStaticFiles(new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(env.WebRootPath),
                RequestPath = ""
            });

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "tags",
                    template: "Posts/Tag/{tag}/Page/{pageNumber?}/",
                    defaults: new { Controller = "Posts", Action = "Index", tag = "", pageNumber = 1 });
                routes.MapRoute(
                    name: "posts",
                    template: "Posts/Page/{pageNumber?}",
                    defaults: new { Controller = "Posts", Action = "Index", pageNumber = 1 });
                routes.MapRoute(
                    name: "post",
                    template: "Posts/{id}",
                    defaults: new {Controller = "Posts", Action="Post" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Posts}/{action=Index}");
            });
        }
    }
}
