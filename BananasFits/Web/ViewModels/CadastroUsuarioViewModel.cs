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
        public virtual string Celular { get; set; }
        public virtual string Telefone { get; set; }
        public virtual CadastrarEnderecoViewModel Endereco { get; set; }
        //public virtual Endereco Endereco { get; set; }
        /// <summary>
        /// Se é do tipo Jurídica ou Física
        /// </summary>
        public virtual int Tipo { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Nome { get; set; }
    }

    public class CadastroPessoaJuridicaViewModel : CadastroUsuarioViewModel
    {
        [Required]
        [MaxLength(20)]
        public virtual string CNPJ { get; set; }
        public virtual string LocalizacaoX { get; set; }
        public virtual string LocalizacaoY { get; set; }
        public virtual HttpPostedFileBase Imagem { get; set; }
        [Required]
        [MaxLength(200)]
        public virtual string Descricao { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string RazaoSocial { get; set; }
    }

    public class CadastroPessoaFisicaViewModel : CadastroUsuarioViewModel
    {
        [Required]
        [MaxLength(15)]
        public virtual string CPF { get; set; }
        public virtual int QuantidadeMoedas { get; set; }
        public virtual bool IsAdministrador { get; set; }
    }
}