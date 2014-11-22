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
            Property(e => e.PessoaJuridicaId).HasColumnName("idP_Juridica");
            Property(e => e.PessoaFisicaId).HasColumnName("idP_Fisica");
           
            HasRequired(e => e.PessoaJuridica)
                .WithMany(e => e.Avaliacoes)
                .HasForeignKey(e => e.PessoaJuridicaId);

            HasRequired(e => e.PessoaFisica)
                .WithMany(e => e.Avaliacoes)
                .HasForeignKey(e => e.PessoaFisicaId);
        
        }

    }
}
