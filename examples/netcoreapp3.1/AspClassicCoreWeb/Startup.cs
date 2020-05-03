using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspClassicCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspClassicCoreWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddDistributedMemoryCache();
            services.AddSession(o =>
            {
                o.IdleTimeout = new TimeSpan(0, 30, 0);
                o.IOTimeout = new TimeSpan(0, 0, 0, 5);
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // serve htm & html files
            app.UseStaticFiles();


            // serve classic asp core
            app.UseSession();
            app.UseAspClassic();

            // other routing shit cool developers like...
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
