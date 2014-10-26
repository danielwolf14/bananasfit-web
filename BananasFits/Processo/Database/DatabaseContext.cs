using Processo.Database.Interfaces;
using Processo.Database.Mapeamento;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext() : base("Name=DefaultConnection")
        {
            System.Data.Entity.Database.SetInitializer<DatabaseContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PessoaFisicaMap());
            modelBuilder.Configurations.Add(new PessoaJuridicaMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new ServicoMap());
            modelBuilder.Configurations.Add(new AvaliacaoMap());
            modelBuilder.Configurations.Add(new ServicoPessoaJuridicaMap());
            modelBuilder.Configurations.Add(new HistoricoCompraServicoMap());

            base.OnModelCreating(modelBuilder);
        }

        public void Commit()
        {
            base.SaveChanges() ;
        }

        public void Rollback()
        {
            base.Dispose();
        }
    }
}
