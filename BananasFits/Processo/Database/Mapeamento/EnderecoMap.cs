using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class EnderecoMap : EntidadeBaseMap<Endereco>
    {
        public EnderecoMap()
        {
            ToTable("Endereco");

            Property(e => e.Chave)
                .HasColumnName("idEndereco")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.CEP).HasColumnName("cep");
            Property(e => e.Bairro).HasColumnName("bairro");
            Property(e => e.Cidade).HasColumnName("cidade");
            Property(e => e.Complemento).HasColumnName("complemento");
            Property(e => e.Estado).HasColumnName("estado");
            Property(e => e.Numero).HasColumnName("numero");
            Property(e => e.Rua).HasColumnName("rua");   
        }

    }
}
