using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal class ServicoPessoaJuridicaMap : EntidadeBaseMap<ServicoPessoaJuridica>
    {
        public ServicoPessoaJuridicaMap()
        {
            ToTable("P_Juridica_Servico");

            Property(e => e.Chave)
                .HasColumnName("idP_Juridica_Servico")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Valor).HasColumnName("valor");
            Property(e => e.IsHabilitado).HasColumnName("isHabilitado");
            Property(e => e.QRCode).HasColumnName("qrcode");
            Property(e => e.PessoaJuridicaId).HasColumnName("id_P_Juridica");

            HasRequired(e => e.PessoaJuridica)
                .WithMany( d => d.Servicos)
                .HasForeignKey(e => e.PessoaJuridicaId);

            HasRequired(e => e.Servico)
               .WithMany()
               .Map(e => e.MapKey("idServico"));
              
        }

    }
}
