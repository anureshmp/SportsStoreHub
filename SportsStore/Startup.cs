using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));  
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes => {
            routes.MapRoute(
                name: null,
                template: "{category}/Page{ProductPage:int}",
                defaults: new { Controller = "Product", Action = "List" }
                    ); 

                routes.MapRoute(
                name: null,
                template: "Page{ProductPage}",
                defaults: new { Controller = "Product", Action = "List", ProductPage = 1 }

           );
                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new {Controller = "Product", Action = "List", ProductPage = 1}

                    );

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new {Controller = "Product", Action = "List", ProductPage = 1}
                    );

                routes.MapRoute(
                    name: null,
                    template: "{controller}/{action}/{id?}");

                 });

            

            SeedData.EnsurePopulated(app);
        }
    }
}
