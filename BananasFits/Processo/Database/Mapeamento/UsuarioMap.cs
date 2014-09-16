using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal abstract class UsuarioMap<TUsuario> : EntidadeBaseMap<TUsuario> 
        where TUsuario : Usuario
    {
        public UsuarioMap()
        {
            Property(e => e.Celular).HasColumnName("celular");
            Property(e => e.Telefone).HasColumnName("telefone");
            Property(e => e.Email).HasColumnName("email");
            Property(e => e.IsHabilitado).HasColumnName("isHabilitado");
            Property(e => e.Password).HasColumnName("senha");

            //Ignore(e => e.Endereco);
            HasRequired(e => e.Endereco)
                .WithMany()
                .Map(e => e.MapKey("idEndereco"));
        }
    }
}
