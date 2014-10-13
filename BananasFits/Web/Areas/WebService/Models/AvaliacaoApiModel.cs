using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ApiModel
{
    public class AvaliacaoApiModel
    {
        public virtual int MediaDeAvaliacoes { get; set; }
        public virtual int TotalDeAvaliacoes { get; set; }
        public virtual int Pontuacao { get; set; }

           
    }
}