using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class ServicoPessoaJuridica : EntidadeBase
    {
        public virtual Servico Servico { get; set; }
        public virtual int Valor { get; set; }
        public virtual string QRCode { get; set; }
        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual bool IsHabilitado { get; set; }
    }
}
