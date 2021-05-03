using AllInOneMusic.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AllInOneMusic
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
            //se agrega el HTTP Client con la interface implementada de Spotify acount service and its concrete implementation
            //tambien se debe crear la configuracion de la hhtp  Client base Address
            // con este codigo la interface ISpotifyAccountService se resolvera en la instancia de SpotifyAccountService. El cliente Http sera injectado en la clase SpotifyAccountService
            //Esta hecho mediante HttpClient Factory
            services.AddHttpClient<ISpotifyAccountService, SpotifyAccountService>(c =>
          {
              c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
          });
            services.AddControllersWithViews();
            services.AddHttpClient<ISpotifyService, SpotifyService>(c =>
           {
               //addres of the https client
               c.BaseAddress = new Uri("https://api.spotify.com/v1/");
               //add request headers that say we excpect a json as a return type
               c.DefaultRequestHeaders.Add("Accept", "application/.json");

           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
