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
            Property(e => e.Pontuacao).HasColumnName("avaliacao");
           
            HasRequired(e => e.PessoaJuridica)
                .WithMany()
                .Map(e => e.MapKey("idP_Juridica"));
            
            HasRequired(e => e.PessoaFisica)
                .WithMany()
                .Map(e => e.MapKey("idP_Fisica"));
        
        }

    }
}
