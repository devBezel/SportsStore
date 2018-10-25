using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;

namespace SportsStore
{
    public class Startup
    {
        public Startup(IConfiguration configuraion) =>
            Configuration = configuraion;
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();/*(options =>*/
            //options.UseMySql(Configuration["Data:SportStoreProducts:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseStaticFiles();
                app.UseSession();
                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: null,
                        template: "{category}/Strona{productPage:int}",
                        defaults: new { controller = "Product", action = "List" });
                    routes.MapRoute(
                        name: null,
                        template: "Strona{productPage:int}",
                        defaults: new { controller = "Product", action = "List", productPage = 1 });
                    routes.MapRoute(
                        name: null,
                        template: "{category}",
                        defaults: new { controller = "Product", action = "List", productPage = 1 });
                    routes.MapRoute(
                        name: null,
                        template: "",
                        defaults: new { controller = "Product", action = "List", productPage = 1 });
                    routes.MapRoute(name: null, template: "{controller=Product}/{action=List}/{id?}");
                });
                SeedData.EnsurePopulated();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
