using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
   public class Avaliacao : EntidadeBase
    {
        public virtual int avaliacao { get; set; }
        public virtual PessoaFisica pessoaFisica { get; set; }
        public virtual PessoaJuridica pessoaJuridica { get; set; }
    }
}
