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
    public class HistoricoCompraServicoNegocio : NegocioBase<HistoricoCompraServico>, IHistoricoCompraServicoNegocio
    {
        internal HistoricoCompraServicoNegocio(DatabaseContext contexto)
            : base(contexto)
        {
            this.repositorio = new HistoricoCompraServicoRepositorio(contexto);
        }

        public void Cadastrar(HistoricoCompraServico historicoCompraServico)
        {
            var mensagens = new List<string>();
            VerificarNegocioException(mensagens);
            base.Inserir(historicoCompraServico);
        }      
    }
}
