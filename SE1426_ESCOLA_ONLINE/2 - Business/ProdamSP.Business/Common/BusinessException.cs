using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdamSP.Business.Common
{
    [Serializable]
    public class BusinessException : ApplicationException
    {
        #region Propriedades
        public List<string> erros { get; set; }
        public bool finalizarExecucao = true;

        public string MensagemErro
        {
            get
            {
                string retorno = "";
                for (int i = 0; i < erros.Count; i++)
                {
                    if (erros.Count - 1 == i)
                    {
                        retorno += erros[i];
                    }
                    else
                    {
                        retorno += erros[i] + "<br>";
                    }
                }
                return retorno;
            }
        }

        #endregion

        public BusinessException()
            : base()
        {
            if (erros == null)
            {
                erros = new List<string>();
            }
        }

        public BusinessException(List<string> pErros)
            : base()
        {
            erros = pErros;
        }

        public BusinessException(string erro)
            : base()
        {
            if (erros == null)
            {
                erros = new List<string>();
            }
            erros.Add(erro);
        }

    }
}
