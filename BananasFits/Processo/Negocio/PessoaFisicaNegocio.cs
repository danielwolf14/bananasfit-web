using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processo.Database.Interfaces;
using Processo.Database.Repositorios;
using Processo.Entidades;
using Processo.Database;
using Processo.Repositorio;
using Processo.Negocio.Interfaces;

namespace Processo.Negocio
{
    public class PessoaFisicaNegocio : UsuarioNegocio<PessoaFisica>, IPessoaFisicaNegocio
    {
        private IPessoaJuridicaNegocio pessoaJuridicaNegocio;

        internal PessoaFisicaNegocio(DatabaseContext contexto)
            : base(contexto)
        {
            this.repositorio = new PessoaFisicaRepositorio(contexto);
            this.pessoaJuridicaNegocio = new PessoaJuridicaNegocio(contexto);
        }

        public override void Cadastrar(PessoaFisica usuario)
        {
            var mensagens = new List<string>();
            ValidarEmail(usuario, mensagens);
            ValidarCamposObrigatorios(usuario, mensagens);
            VerificarEmailExistente(usuario.Email, mensagens);
            VerificarNegocioException(mensagens);
            base.Inserir(usuario);
        }

        public void VerificarEmailExistente(string email, List<string> mensagens)
        {
            if(this.pessoaJuridicaNegocio.Consultar(e => e.Email == email).Any())
            {
                mensagens.Add("E-mail já cadastrado.");
            }
        }
    }
}
    