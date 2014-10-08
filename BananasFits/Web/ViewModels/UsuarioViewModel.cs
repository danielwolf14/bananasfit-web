using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    #region Cadastro
    public class CadastroUsuarioViewModel
    {
        [Required]
        public virtual string Email { get; set; }
        [Required]
        public virtual string Password { get; set; }
        public virtual bool IsHabilitado { get; set; }
        [RegularExpression("[0-9()-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Celular { get; set; }
        [Required]
        [RegularExpression("[0-9()-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Telefone { get; set; }
        public virtual CadastrarEnderecoViewModel Endereco { get; set; }
        public virtual int Tipo { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-ZáéíóúÁÉÍÓÚÇçÃÕãõ]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Nome { get; set; }
    }

    public class CadastroPessoaJuridicaViewModel : CadastroUsuarioViewModel
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("[0-9.-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string CNPJ { get; set; }
        public virtual string LocalizacaoX { get; set; }
        public virtual string LocalizacaoY { get; set; }
        public virtual HttpPostedFileBase Imagem { get; set; }
        [Required]
        [MaxLength(200)]
        [RegularExpression("[a-z A-ZáéíóúÁÉÍÓÚÇçÃÕãõ]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Descricao { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-ZáéíóúÁÉÍÓÚÇçÃÕãõ0-9]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string RazaoSocial { get; set; }
    }

    public class CadastroPessoaFisicaViewModel : CadastroUsuarioViewModel
    {
        [Required]
        [MaxLength(15)]
        [RegularExpression("[0-9.-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string CPF { get; set; }
        public virtual int QuantidadeMoedas { get; set; }
        public virtual bool IsAdministrador { get; set; }
    }
    #endregion

    #region Detalhar
    public class DetalharUsuarioViewModel
    {

        public virtual int Chave { get; set; }
        [Required]
        public virtual string Email { get; set; }
        [Required]
        public virtual string Password { get; set; }
        public virtual bool IsHabilitado { get; set; }
        public virtual string Celular { get; set; }
        public virtual string Telefone { get; set; }
        public virtual CadastrarEnderecoViewModel Endereco { get; set; }
        public virtual int QuantidadeMoedas { get; set; }
        public virtual string Nome { get; set; }
    }
    public class DetalharPessoaFisicaViewModel : DetalharUsuarioViewModel
    {
        public virtual string CPF { get; set; }
    }

    public class DetalharPessoaJuridicaViewModel : DetalharUsuarioViewModel
    {
        public virtual string CNPJ { get; set; }
        public virtual string Imagem { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual IList<ServicoPessoaJuridica> Servicos { get; set; }
    }
    public class DetalharBuscaPessoaJuridicaViewModel : DetalharPessoaJuridicaViewModel
    {
        public virtual int Avaliacao { get; set; }
        public virtual int QuantidadeAvaliacao { get; set; }
    }
    #endregion

    #region Login
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
    #endregion

    #region Endereço
    public class CadastrarEnderecoViewModel
    {
        [Required]
        [MaxLength(50)]
        [RegularExpression("[0-9.-]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string CEP { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-ZáéíóúÁÉÍÓÚÇçÃÕãõ]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Rua { get; set; }
        [MaxLength(50)]
        [RegularExpression("[0-9]+", ErrorMessage = "Este campo aceita apenas números")]
        public virtual string Numero { get; set; }
        [MaxLength(50)]
        public virtual string Complemento { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-ZáéíóúÁÉÍÓÚÇçÃÕãõ]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Bairro { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("[a-z A-ZáéíóúÁÉÍÓÚÇçÃÕãõ]+", ErrorMessage = "Este campo aceita apenas letras")]
        public virtual string Cidade { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Estado { get; set; }
    }
    #endregion
}