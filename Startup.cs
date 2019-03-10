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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IFormatterService, FormatterService>();
            services.AddSingleton<IFileReader<Post>, FileReader<Post>>();
            services.AddSingleton<IFileReader<Author>, FileReader<Author>>();
            services.AddSingleton<IRepository<Post>, Repository<Post>>();
            services.AddSingleton<IRepository<Author>, Repository<Author>>();
            services.AddSingleton<IRepositoryContainer, RepositoryContainer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IFormatterService formatter, 
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

            app.UseStaticFiles();

            var postsDir = Environment.GetEnvironmentVariable("POSTS_DIR");
            if(!String.IsNullOrWhiteSpace(postsDir))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(postsDir),
                    RequestPath = "/Posts"
                });
            }

            var authorDir = Environment.GetEnvironmentVariable("AUTHORS_DIR");
            if(!String.IsNullOrWhiteSpace(authorDir))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(authorDir),
                    RequestPath = "/Authors"
                });
            }

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

            postRepository.Initialize(Path.Combine(env.WebRootPath, "posts"), post => {
                // TODO: tidy this mess up
                post.Html = formatter.FixImages(post.Html, post.Id);;
                post.Image = $"/posts/{post.Id}/{post.Image}";
                post.ShortContent = formatter.CreateTruncatedContent(post.Html, 200);
                return post;
            });

            authorRepository.Initialize(Path.Combine(env.WebRootPath, "authors"));
        }
    }
}
