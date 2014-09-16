using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class PessoaFisicaMap : UsuarioMap<PessoaFisica>
    {
        public PessoaFisicaMap()
        {
            ToTable("P_Fisica");

            Property(e => e.Chave)
                .HasColumnName("idP_Fisica")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.CPF).HasColumnName("cpf");
            Property(e => e.IsAdministrador).HasColumnName("isAdmin");
            Property(e => e.Nome).HasColumnName("nome");   
            Property(e => e.QuantidadeMoedas).HasColumnName("qtdMoeda");   
        }

    }
}
