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
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var postsDir = Environment.GetEnvironmentVariable("POSTS") ?? Path.Combine(_env.ContentRootPath, "../Posts");
            var authorDir = Environment.GetEnvironmentVariable("AUTHORS") ?? Path.Combine(_env.ContentRootPath, "../Authors");
            var settingsDir = Environment.GetEnvironmentVariable("SETTINGS") ?? Path.Combine(_env.ContentRootPath, "../Settings");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<IFormatterService, FormatterService>();
            services.AddSingleton<IFileReader<Post>, FileReader<Post>>();
            services.AddSingleton<IFileReader<Author>, FileReader<Author>>();
            services.AddSingleton<IFileReader<Settings>, FileReader<Settings>>();
            services.AddScoped<ISiteInfo>(ctx => new SiteInfo(
                settingsDir,
                ctx.GetService<ILogger<SiteInfo>>(), 
                ctx.GetService<IFileReader<Settings>>()));
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
        public void Configure(IApplicationBuilder app, ILogger<Startup> logger, IFormatterService formatter, 
           ISiteInfo siteInfo, IRepository<Post> postRepository, IRepository<Author> authorRepository)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            logger.LogInformation($"Settings directory: {siteInfo.BasePath}");
            if(!String.IsNullOrWhiteSpace(siteInfo.BasePath))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(Path.Combine(siteInfo.BasePath, "Images")),
                    RequestPath = "/images"
                });
            }

            logger.LogInformation($"Posts directory: {postRepository.BasePath}");
            if(!String.IsNullOrWhiteSpace(postRepository.BasePath))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(postRepository.BasePath),
                    RequestPath = "/posts"
                });
            }

            logger.LogInformation($"Authors directory: {authorRepository.BasePath}");
            if(!String.IsNullOrWhiteSpace(authorRepository.BasePath))
            {
                app.UseStaticFiles(new StaticFileOptions(){
                    FileProvider = new PhysicalFileProvider(authorRepository.BasePath),
                    RequestPath = "/authors"
                });
            }

            app.UseStaticFiles(new StaticFileOptions(){
                FileProvider = new PhysicalFileProvider(_env.WebRootPath),
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
