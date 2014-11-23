using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Processo.Entidades;
namespace Web.ViewModels
{
    public class ComprarFitsViewModel
    {
        public virtual int QuantidadeFits { get; set; }
        public virtual string NumeroCartao { get; set; }
        public virtual string Ano { get; set; }
        public virtual string Mes { get; set; }
        public virtual string Cvv { get; set; }
        public virtual string TipoCartao { get; set; }
    }


}