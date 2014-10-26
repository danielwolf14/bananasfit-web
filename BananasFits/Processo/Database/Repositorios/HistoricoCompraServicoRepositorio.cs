using Processo.Database.Interfaces;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Repositorios
{
    internal class HistoricoCompraServicoRepositorio : RepositorioBase<HistoricoCompraServico>, IHistoricoCompraServicoRepositorio
    {
        public HistoricoCompraServicoRepositorio(DatabaseContext contexto)
            : base(contexto)
        {

        }
    }
}
