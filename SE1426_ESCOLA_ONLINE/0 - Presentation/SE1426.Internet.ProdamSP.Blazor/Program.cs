using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using SE1426.Internet.ProdamSP.Blazor.Services;
using SE1426.Internet.ProdamSP.Blazor.Models;
using Blazored.SessionStorage;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Linq;



namespace SE1426.Internet.ProdamSP.Blazor
{
    
    public class Program
    {
        //public static async Task Main(string[] args)
        //{
        //    var builder = WebAssemblyHostBuilder.CreateDefault(args);
        //    builder.RootComponents.Add<App>("app");

        //    builder.Services.AddSingleton<RestService>();

        //    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


        //    builder.Services.AddSingleton<DadosPreNatalEOLSIGAModel>();

        //    builder.Services.AddScoped<DadosPreNatalEOLSIGAModel>();
        //    builder.Services.AddScoped<CaptchaModel>();
        //    builder.Services.AddBlazoredSessionStorage();

        //    await builder.Build().RunAsync();
        //}

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
