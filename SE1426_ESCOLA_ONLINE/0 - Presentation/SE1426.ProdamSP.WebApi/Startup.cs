using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ProdamSP.CrossCutting.InversionOfControl;
using ProdamSP.Data.Context;
using ElmahCore.LogCorporativoProdam;
using ElmahCore.Mvc;
using Microsoft.Net.Http.Headers;
using ProdamSP.CrossCutting.Services;
using ProdamSP.CAC.Token.Std;
using Microsoft.OpenApi.Models;

namespace SE1426.ProdamSP.WebApi
{
    public class Startup
    {         
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ENV = env;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment ENV;
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
        {
            string conString = Configuration["ConnectionStrings:EOLConnection"];

            services.AddSingleton<LocalidadeService>(new LocalidadeService(
                    Configuration["SOALocalidade:Url"],
                    Configuration["SOALocalidade:txSenhaSOA"],
                    Configuration["SOALocalidade:txSenhaCAC"],
                    Configuration["SOALocalidade:nomUsuarioSistema"])
                );


            services.AddDbContext<SE1426Context>(options =>
            {
                options.UseSqlServer(conString);
                options.EnableSensitiveDataLogging(true);
            });

            services.Configure<ElmahCore.ElmahOptions>(Configuration.GetSection("ElmahLogCorporativo"));
            services.AddElmah<ServiceErrorLog>();
            services.AddTransient<ServiceErrorLog, ServiceErrorLog>();

            services.AddApplicationLibrary();
            services.AddBusinessLibrary();
            services.AddRepositoryLibrary();

            services.AddTransient<ServiceErrorLog, ServiceErrorLog>();

            if (Environment.GetEnvironmentVariable("Ambiente") != "Producao")
            {
                services.AddSwaggerGen();
            }


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api EOL ONLINE", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });



            services.AddApiVersioning(options => { options.ReportApiVersions = true; });

            services.AddControllers();

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseCors(policy =>
                policy.WithOrigins("http://localhost:8000", "https://localhost:8001", "http://localhost:9000", "https://localhost:9001")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
                .AllowCredentials());
            
            app.UseSwagger();

            if (Environment.GetEnvironmentVariable("Ambiente") != "Producao")
            {
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(Configuration["DiretorioVirtual"] + "swagger/v1/swagger.json", "My API V1");
                });

            }

            app.UseApiCac();
            app.UseElmah();
            app.UseAuthorization();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
