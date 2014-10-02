using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Processo.Entidades;

namespace Web.ViewModels
{
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

}