using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using ProdamSP.Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;

using ProdamSP.Business;


namespace SE1426_Domain.Tests
{
    [TestClass]
    public class PessoaSeviceUnitTest
    {
        [TestMethod]
        public void PesquisarTestMethod()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

            var p = new PessoaServiceBusiness(configuration);
            p.PesquisarAsPessoaSIGA("835503028196068");

        }
    }
}
