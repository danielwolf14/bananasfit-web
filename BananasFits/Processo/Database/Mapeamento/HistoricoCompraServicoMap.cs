using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class HistoricoCompraServicoMap : EntidadeBaseMap<HistoricoCompraServico>
    {
        public HistoricoCompraServicoMap()
        {
            ToTable("Historico_Compra_Servico");

            Property(e => e.Chave)
                .HasColumnName("idHistorico_Compra_Servico")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Valor).HasColumnName("valor");
            Property(e => e.Data).HasColumnName("data");
            Property(e => e.NomePessoaJuridica).HasColumnName("nomePessoaJuridica");
            Property(e => e.NomePessoaFisica).HasColumnName("nomePessoaFisica");
            Property(e => e.NomeServico).HasColumnName("nomeServico");

            HasRequired(e => e.Servico)
               .WithMany()
               .Map(e => e.MapKey("idP_Juridica_Servico"));

            HasRequired(e => e.PessoaFisica)
               .WithMany()
               .Map(e => e.MapKey("idP_Fisica"));

        }

    }
}
