using Microsoft.EntityFrameworkCore;
using ProdamSP.Domain.Entities;
/// <summary>
/// QUANDO FIZER O MAPEAMENTO NOVO, FAZER A CLASSE SH0891CONTEXT HERDAR DESTA CLASSE E INCLUIR NA PRIMEIRA LINHA DO METODO 
/// OnModelCreating, DA CLASSE SH0891CONTEXT, O CÓDIGO: base.OnModelCreating(modelBuilder);
/// </summary>
namespace ProdamSP.Data.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext()
        {
        }
        public BaseContext(DbContextOptions<SE1426Context> options)
            : base(options)
        {
        }
        public DbSet<SolicitacaoMatriculaPreNatal> SolicitacaoMatriculaPreNatal { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SolicitacaoMatriculaPreNatal>().HasNoKey().ToView(null);
        }
            
    }
}
