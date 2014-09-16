using Processo.Database.Interfaces;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Repositorios
{
    internal class ServicoRepositorio : RepositorioBase<Servico>, IServicoRepositorio
    {
        public ServicoRepositorio(DatabaseContext contexto)
            : base(contexto)
        {

        }
    }
}
