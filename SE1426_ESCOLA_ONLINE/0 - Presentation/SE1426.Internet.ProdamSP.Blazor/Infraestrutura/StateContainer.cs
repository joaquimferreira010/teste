using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SE1426.Internet.ProdamSP.Blazor.Infraestrutura
{
   
    public class StateContainer
    {
        private bool conexaoValida;
        private string token;

        public bool ConexaoValida
        {
            get => conexaoValida;
            set
            {
                conexaoValida = value;
                NotifyStateChanged();
            }
        }

        public string Token
        {
            get => token;
            set
            {
                token = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
