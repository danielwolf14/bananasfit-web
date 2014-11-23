using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class HistoricoCompraFits : EntidadeBase
    {
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual DateTime DataCompra { get; set; }
        public virtual string Valor { get; set; }
        public virtual string NomePessoaFisica { get; set; }
        public virtual int QuantidadeFits { get; set; }
         }
}
