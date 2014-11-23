using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class HistoricoCompraFitsMap : EntidadeBaseMap<HistoricoCompraFits>
    {
        public HistoricoCompraFitsMap()
        {
            ToTable("Historico_Compra_Fits");

            Property(e => e.Chave)
                .HasColumnName("idHistorico_Compra_Fits")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Valor).HasColumnName("valor");
            Property(e => e.DataCompra).HasColumnName("dataCompra");
            Property(e => e.NomePessoaFisica).HasColumnName("nomePessoaFisica");
            Property(e => e.QuantidadeFits).HasColumnName("quantidadeFits");

            HasRequired(e => e.PessoaFisica)
               .WithMany()
               .Map(e => e.MapKey("idP_Fisica"));

        }

    }
}
