using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Domain.Constants
{
    public static class Constantes
    {

        public const string MAXIMO_DE_REGISTROS_POR_PAGINA = "MAXIMO_DE_REGISTROS_POR_PAGINA";
        public const string PASSWORD_HASH = "P@ss0dCAC";
        public const string AMBIENTE_EXECUCAO = "AmbienteExecucao";
        public const string SALT_KEY = "S@ltkeyCACWeb";
        public const string VI_KEY = "@2c4&6G8i0kCACWeb";
    }

    public static class STATUS_CODE_WEBAPI
    {
        public const string STATUS_404_NOTFOUND = "404 - NotFound";
    }
}
