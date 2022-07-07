using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Models
{
    public  class DadosPreNatalEOLSIGAContainer
    {       
        public string numeroCns { get; private set; }
        public string numeroSisPreNatal { get; private set; }
        public int? codEOLPreNatal { get;  set; }
        public DateTime? dataPrevisaoParto { get;  set; }
        public DateTime? dataCadastroPreNatal { get;  set; }
        public int? codRetorno { get;  set; }
        public string msgRetorno { get;  set; }

        
        public event Action OnChange;

        public void SetCodEOLPreNatal(int? value)
        {
            codEOLPreNatal = value;
            NotifyStateChanged();
        }



        public void SetNumeroCns(string value)
        {
            numeroCns = value;
            NotifyStateChanged();
        }
        public void SetNumeroSisPreNatal(string value)
        {
            numeroSisPreNatal = value;
        }

        public void SetDataPrevisaoParto(DateTime? value)
        {
            dataPrevisaoParto = value;
        }
        public void SetDataCadastroPreNatal(DateTime? value)
        {
            dataCadastroPreNatal = value;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
        
    }
}

