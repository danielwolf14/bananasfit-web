using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class HistoricoCompraServico : EntidadeBase
    {
        public virtual ServicoPessoaJuridica Servico { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual int Valor { get; set; }
        public virtual string NomeServico { get; set; }
        public virtual string NomePessoaFisica { get; set; }
        public virtual string NomePessoaJuridica { get; set; }
    }
}
