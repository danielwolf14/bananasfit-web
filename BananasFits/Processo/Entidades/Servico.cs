using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class Servico : EntidadeBase
    {
        public virtual string Nome { get; set; }
        public virtual string Imagem { get; set; }
        public virtual bool IsHabilitado { get; set; }

        
    }

  
}
