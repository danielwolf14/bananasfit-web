using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class ServicoMap : EntidadeBaseMap<Servico>
    {
        public ServicoMap()
        {
            ToTable("Servico");

            Property(e => e.Chave)
                .HasColumnName("idServico")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome).HasColumnName("nome");
            Property(e => e.Imagem).HasColumnName("imagem");
            Property(e => e.IsHabilitado).HasColumnName("isHabilitado");

              
        }

    }
}
