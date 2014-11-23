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
            usuario.IsHabilitado = true;
            var mensagens = new List<string>();
            ValidarEmail(usuario, mensagens);
            ValidarCamposObrigatorios(usuario, mensagens);
            VerificarNegocioException(mensagens);
            base.Inserir(usuario);
        }

        public void AtualizarConta(PessoaFisica usuario)
        {
            var mensagens = new List<string>();
            //ValidarEmail(usuario, mensagens);
            ValidarCamposObrigatorios(usuario, mensagens);
            VerificarNegocioException(mensagens);
            base.Atualizar(usuario);
        }

        public void CreditarFits(PessoaFisica pessoaFisica, int quantidadeFits)
        {
            pessoaFisica.QuantidadeMoedas += quantidadeFits;
            //Criar histórico
            //var historicoCompraFits = new HistoricoCompraFits 
            //{
            //    PessoaFisica = pessoaFisica,
            //    QuantidadeFits = quantidadeFits,
            //    Valor = valor,    
            //    DataCompra = DateTime.Now
            //};
            
            this.Atualizar(pessoaFisica);

            throw new NotImplementedException();
        }
    }
}
    