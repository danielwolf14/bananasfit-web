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
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string CEP { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-Z]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Rua { get; set; }
        [MaxLength(50)]
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Numero { get; set; }
        [MaxLength(50)]
        public virtual string Complemento { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-Z]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Bairro { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-Z]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Cidade { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Estado { get; set; }
    }
}