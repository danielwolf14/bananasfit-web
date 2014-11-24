using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Processo.Entidades;
using System.ComponentModel.DataAnnotations;
namespace Web.ViewModels
{
    public class ComprarFitsViewModel
    {
        [Required]
        public virtual int QuantidadeFits { get; set; }
        [Required]
        [MaxLength(16)]
        [RegularExpression("[0-9()-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string NumeroCartao { get; set; }
        [Required]
        public virtual string Ano { get; set; }
        [Required]
        public virtual string Mes { get; set; }
        [Required]
        [MaxLength(4)]
        [RegularExpression("[0-9()-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Cvv { get; set; }
        [Required]
        public virtual string TipoCartao { get; set; }
    }


}