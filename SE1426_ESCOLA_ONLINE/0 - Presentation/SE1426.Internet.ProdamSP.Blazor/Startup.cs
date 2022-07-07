using Blazored.SessionStorage;
using SE1426.Internet.ProdamSP.Blazor.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SE1426.Internet.ProdamSP.Blazor.Models;
using SE1426.Internet.ProdamSP.Blazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProdamSP.Domain.Interfaces.Business;
using ProdamSP.Business;
using SE1426.Internet.ProdamSP.Blazor.Util;
using SE1426.Internet.ProdamSP.Blazor.Infraestrutura;

using ElmahCore.LogCorporativoProdam;
using ElmahCore.Mvc;

namespace SE1426.Internet.ProdamSP.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton<RestService>();
            services.AddTransient<CaptchaService>();

            services.AddSingleton<DadosPreNatalEOLSIGAModel>();
            services.AddSingleton<System.Net.Http.HttpClient>();
            services.AddScoped<DadosPreNatalEOLSIGAModel>();
            services.AddScoped<CaptchaModel>();
            services.AddBlazoredSessionStorage();

            services.AddSingleton<StateContainer>();

            services.Configure<ElmahCore.ElmahOptions>(Configuration.GetSection("ElmahLogCorporativo"));
            services.AddElmah<ServiceErrorLog>();
            services.AddTransient<ServiceErrorLog, ServiceErrorLog>();


            services.AddTransient<ICaptchaBusiness, CaptchaBusiness>();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
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
                app.UseExceptionHandler("/Error");
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapWhen(
        context => context.Request.Path.ToString().EndsWith("Error"),
        appBranch => {
            appBranch.UseCaptchaHandler();
        });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

           
        }
    }
}
