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
    public abstract class UsuarioNegocio<TEntidade> : NegocioBase<TEntidade>, IUsuarioNegocio<TEntidade>
        where TEntidade : Usuario
    {
        internal UsuarioNegocio(IRepositorioBase<TEntidade> repositorio)
            : base(repositorio)
        {
        }

        public void ValidarEmail(TEntidade usuario, IList<string> mensagens)
        {
            if (Consultar(e => e.Email == usuario.Email).ToList().Any())
                mensagens.Add("Este e-mail já está cadastrado.");
        }

        public virtual void ValidarCamposObrigatorios(TEntidade usuario, IList<string> mensagens)
        {
            if (string.IsNullOrEmpty(usuario.Email))
                mensagens.Add("E-mail é um campo obrigatório.");
            if (usuario.Endereco == null)
                mensagens.Add("Endereço é um campo obrigatório.");
            if (string.IsNullOrEmpty(usuario.Password))
                mensagens.Add("Senha é um campo obrigatório.");
            if (string.IsNullOrEmpty(usuario.Telefone))
                mensagens.Add("Telefone é um campo obrigatório.");
            

        }

        public TEntidade BuscarUsuarioPorEmail(string email)
        {
            return base.Consultar(e => e.Email == email).SingleOrDefault();
        }

        public abstract void Cadastrar(TEntidade usuario);
    }
}
    