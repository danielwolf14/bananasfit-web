using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class HistoricoCompraServico : EntidadeBase
    {
        public virtual ServicoPessoaJuridica ServicoPessoaJuridica { get; set; }
        public virtual ServicoPessoaJuridica PessoaFisica { get; set; }
        public virtual DateTime data { get; set; }
        public virtual int valor { get; set; }
    }
}
