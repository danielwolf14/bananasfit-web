using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class AvaliacaoMap : EntidadeBaseMap<Avaliacao>
    {
        public AvaliacaoMap()
        {
            ToTable("Avaliacao");

            Property(e => e.Chave)
                .HasColumnName("idAvaliacao")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.avaliacao).HasColumnName("avaliacao");
            Property(e => e.pessoaJuridica.Chave).HasColumnName("idP_Juridica");
            Property(e => e.pessoaFisica.Chave).HasColumnName("idP_Fisica");

              
        }

    }
}
