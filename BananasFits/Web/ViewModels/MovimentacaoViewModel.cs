using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Processo.Entidades;
namespace Web.ViewModels
{
    public class MovimentacaoViewModel
    {
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual int QuantidadeFits { get; set; }
    }
}