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
        internal PessoaFisicaNegocio(IPessoaFisicaRepositorio repositorio)
            : base(repositorio)
        {
        }

        public override void Cadastrar(PessoaFisica usuario)
        {
            var mensagens = new List<string>();
            ValidarEmail(usuario, mensagens);
            ValidarCamposObrigatorios(usuario, mensagens);
            VerificarNegocioException(mensagens);
            base.Inserir(usuario);
        }
    }
}
    