using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.WebService.Models
{
    public class MovimentacaoApiModel
    {
        public string QrCode { get; set; }
        public int IdPessoaFisica { get; set; }
        public bool FinalizarCompra { get; set; }
    }

    public class ComprarFitsApiModel
    {
        public virtual int ChavePessoaFisica { get; set; }
        public virtual int QuantidadeFits { get; set; }
        public virtual string NumeroCartao { get; set; }
        public virtual string Ano { get; set; }
        public virtual string Mes { get; set; }
        public virtual string Cvv { get; set; }
        public virtual string TipoCartao { get; set; }
    }
}