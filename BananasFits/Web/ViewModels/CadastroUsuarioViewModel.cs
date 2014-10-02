using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class CadastroUsuarioViewModel
    {
        [Required]
        public virtual string Email { get; set; }
        [Required]
        public virtual string Password { get; set; }
        public virtual bool IsHabilitado { get; set; }
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Celular { get; set; }
        [Required]
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Telefone { get; set; }
        public virtual CadastrarEnderecoViewModel Endereco { get; set; }
        public virtual int Tipo { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-Z]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Nome { get; set; }
    }

    public class CadastroPessoaJuridicaViewModel : CadastroUsuarioViewModel
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string CNPJ { get; set; }
        public virtual string LocalizacaoX { get; set; }
        public virtual string LocalizacaoY { get; set; }
        public virtual HttpPostedFileBase Imagem { get; set; }
        [Required]
        [MaxLength(200)]
        [RegularExpression("[a-z A-Z]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Descricao { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-Z]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string RazaoSocial { get; set; }
    }

    public class CadastroPessoaFisicaViewModel : CadastroUsuarioViewModel
    {
        [Required]
        [MaxLength(15)]
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string CPF { get; set; }
        public virtual int QuantidadeMoedas { get; set; }
        public virtual bool IsAdministrador { get; set; }
    }
}