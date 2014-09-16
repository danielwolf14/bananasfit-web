using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Negocio
{
    public class NegocioException : Exception
    {

        public IList<string> Mensagens { get; set; }

        public NegocioException()
            : this(new List<string>())
        {

        }

        public NegocioException(string mensagem)
            : base(mensagem)
        {
            this.Mensagens = new List<String>();
            this.Mensagens.Add(mensagem);
        }

        public NegocioException(IList<string> mensagens)
        {
            Mensagens = mensagens;
        }
    }
}
