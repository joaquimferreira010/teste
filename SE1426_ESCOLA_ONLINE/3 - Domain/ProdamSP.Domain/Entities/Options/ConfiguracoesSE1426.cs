namespace ProdamSP.Domain.Entities.Options
{
    public class ConfiguracoesSE1426
    {
        public BatchAccessOptions batchAccessOptions { get; set; }
        public class BatchAccessOptions
        {
            public string CaminhoArquivoAccess { get; set; }
        }
    }
}
