using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class PessoaJuridica : Usuario
    {
        public virtual string Nome { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string LocalizacaoX { get; set; }
        public virtual string LocalizacaoY { get; set; }
        public virtual string Imagem { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual IList<ServicoPessoaJuridica> Servicos { get; set; }
        public virtual IList<Avaliacao> Avaliacoes { get; set; }
    }
}
