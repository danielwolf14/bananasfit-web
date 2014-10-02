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
    public class ServicoNegocio : NegocioBase<Servico>, IServicoNegocio
    {
        internal ServicoNegocio(DatabaseContext contexto)
            : base(contexto)
        {
            this.repositorio = new ServicoRepositorio(contexto);
        }

        public void Cadastrar(Servico servico)
        {
            var mensagens = new List<string>();
            VerificarNegocioException(mensagens);
            base.Inserir(servico);
        }      
    }
}
