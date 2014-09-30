using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class CadastrarEnderecoViewModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression("[0-9]+", ErrorMessage = "CEP Aceita apenas número")]
        public virtual string CEP { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Rua { get; set; }
        [MaxLength(50)]
        public virtual string Numero { get; set; }
        [MaxLength(50)]
        public virtual string Complemento { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Bairro { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Cidade { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Estado { get; set; }
    }
}