using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProdamSP.Business;
using ProdamSP.Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;


namespace SE1426_Domain.Tests
{
    [TestClass]
    public class CaptchaUnitTest
    {

        

        [TestMethod]
        public void ObterCaptchaMethod()
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", false)
              .Build();

            CaptchaBusiness p = new CaptchaBusiness(configuration);
            var r = p.ObterCaptcha("mp3");
            
            var r1 = p.ValidarCaptcha(r.Result.RetornoValidacao, "teste");
        }
    }
}
