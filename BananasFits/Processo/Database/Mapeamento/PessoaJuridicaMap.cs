using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class PessoaJuridicaMap : UsuarioMap<PessoaJuridica>
    {
        public PessoaJuridicaMap()
        {
            ToTable("P_Juridica");

            Property(e => e.Chave)
                .HasColumnName("idP_Juridica")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.CNPJ).HasColumnName("cnpj");
            Property(e => e.Descricao).HasColumnName("descricao");
            Property(e => e.RazaoSocial).HasColumnName("razaoSocial");
            Property(e => e.Imagem).HasColumnName("imagem");
            Property(e => e.LocalizacaoX).HasColumnName("locX");
            Property(e => e.LocalizacaoY).HasColumnName("locY");           
        }
    }
}
