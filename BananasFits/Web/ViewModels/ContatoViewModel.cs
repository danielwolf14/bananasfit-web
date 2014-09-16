using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class ContatoViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string De { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        public string Mensagem { get; set; }
    }
}