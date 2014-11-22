using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Processo.Entidades;
namespace Web.ApiModel
{
    public class UsuarioApiModel
    {
        public virtual long Chave { get; set; }
        public virtual string Email { get; set; }
        public virtual string Nome { get; set; }
        public virtual bool IsAdministrador { get; set; }
        public virtual bool IsPessoaFisica { get; set; }
        public virtual int QuantidadeMoedas { get; set; }
    }

    public class ParametrosPessoaJuridicaModel
    {
        public int ChavePessoaJuridica { get; set; }
        public int ChavePessoaFisica { get; set; }
    }

    public class DetalhePessoaJuridicaModel
    {
        public virtual string Nome { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string Imagem { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string Servicos { get; set; }
        public virtual int UltimaAvaliacao { get; set; }

        public virtual string Email { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Celular { get; set; }
        public virtual EnderecoModel Endereco { get; set; }
    }

    public class EnderecoModel
    {
        public virtual string CEP { get; set; }
        public virtual string Rua { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
    }

}