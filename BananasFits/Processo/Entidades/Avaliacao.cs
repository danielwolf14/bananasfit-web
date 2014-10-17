using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
   public class Avaliacao : EntidadeBase
    {
       public virtual int PessoaJuridicaId { get; set; }
        public virtual int Pontuacao { get; set; }
        public PessoaFisica PessoaFisica { get; set; }
        public PessoaJuridica PessoaJuridica { get; set; }
    }
}
