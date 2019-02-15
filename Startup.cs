using Deferat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            services.AddSingleton<IPostService, PostService>();
            services.AddSingleton<IAuthorService, AuthorService>();
            services.AddSingleton<IFormatterService, FormatterService>();
            services.AddSingleton<IFileReader, FileReader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IPostService postService, 
            IAuthorService authorService)
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

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
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
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            postService.LoadPosts(Path.Combine(env.WebRootPath, "posts"));
            authorService.LoadAuthors(Path.Combine(env.WebRootPath, "authors"));
        }
    }
}
