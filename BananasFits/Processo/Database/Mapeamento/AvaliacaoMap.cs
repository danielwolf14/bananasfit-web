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
           
            HasRequired(e => e.pessoaJuridica)
                .WithMany()
                .Map(e => e.MapKey("idP_Juridica"));
            
            HasRequired(e => e.pessoaFisica)
                .WithMany()
                .Map(e => e.MapKey("idP_Fisica"));
        
        }

    }
}
