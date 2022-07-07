using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


using ProdamSP.Domain.Interfaces.Business;

namespace ProdamSP.Business
{
    public class FoneticaBusiness : IFoneticaBusiness
    {
        public string Fonetizar(string textoFonetizacao)
        {
           Fonetica.NET.Fonetica fonetica = new Fonetica.NET.Fonetica();

            try
            {
                return fonetica.GeraCodigoFonetico(textoFonetizacao);
            } catch(Exception e)
            {
                throw e;
            }
        }
    }
}
