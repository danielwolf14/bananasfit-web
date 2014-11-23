using Processo.Database.Interfaces;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Repositorios
{
    internal class HistoricoCompraFitsRepositorio : RepositorioBase<HistoricoCompraFits>, IHistoricoCompraFitsRepositorio
    {
        public HistoricoCompraFitsRepositorio(DatabaseContext contexto)
            : base(contexto)
        {

        }
    }
}
