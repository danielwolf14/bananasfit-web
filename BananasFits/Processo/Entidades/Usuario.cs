using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public abstract class Usuario : EntidadeBase
    {
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsHabilitado { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Telefone { get; set; }
        public virtual Endereco Endereco { get; set; }
    }
}
