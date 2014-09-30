using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ApiModel
{
    public class InserirAvaliacaoApiModel
    {
        public virtual int Pontuacao { get; set; }
        public virtual int PessoaFisica { get; set; }
        public virtual int PessoaJuridica { get; set; }
    }
}