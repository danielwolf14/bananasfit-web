using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class PessoaFisica : Usuario
    {
        public virtual string Nome { get; set; }
        public virtual string CPF { get; set; }
        public virtual bool IsAdministrador { get; set; }
    }
}
