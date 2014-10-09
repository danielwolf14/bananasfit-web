using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Processo.Database.Interfaces;
using Processo.Database.Repositorios;
using Processo.Entidades;
using Processo.Database;
using Processo.Negocio.Interfaces;

namespace Processo.Negocio
{
    public class PessoaJuridicaNegocio : UsuarioNegocio<PessoaJuridica>, IPessoaJuridicaNegocio
    {

        internal PessoaJuridicaNegocio(DatabaseContext contexto)
            : base(contexto)
        {
            this.repositorio = new PessoaJuridicaRepositorio(contexto);
        }

        public override void Cadastrar(PessoaJuridica usuario)
        {
            var mensagens = new List<string>();
            usuario.IsHabilitado = true;
            ValidarEmail(usuario, mensagens);
            ValidarCamposObrigatorios(usuario, mensagens);
            VerificarNegocioException(mensagens);
            base.Inserir(usuario);
        }

        public override void ValidarCamposObrigatorios(PessoaJuridica usuario, IList<string> mensagens)
        {
            if (string.IsNullOrEmpty(usuario.CNPJ))
                mensagens.Add("CNPJ é um campo obrigatório.");
            if (string.IsNullOrEmpty(usuario.RazaoSocial))
                mensagens.Add("A razão social da empresa é um campo obrigatório.");
            base.ValidarCamposObrigatorios(usuario, mensagens);
        }

        public IEnumerable<PessoaJuridica> ListarTodos()
        {
            return base.Consultar(e => e.IsHabilitado);
        }

        public void AtualizarConta(PessoaJuridica usuario)
        {
            var mensagens = new List<string>();
            //ValidarEmail(usuario, mensagens);
            ValidarCamposObrigatorios(usuario, mensagens);
            VerificarNegocioException(mensagens);
            base.Atualizar(usuario);
        }
    }
}
