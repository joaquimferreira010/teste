using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using ProdamSP.Business;
using ProdamSP.Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;

using ProdamSP.Business;

namespace SE1426_Domain.Tests
{
    [TestClass]
    public class PreNatalServiceUnitTest
    {
        [TestMethod]
        public void PesquisarTestMethod()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

            var p = new PreNatalServiceBusiness(configuration);
            p.PesquisarPreNatal("801440432351340", "35202906310");        
        }
    }
}
