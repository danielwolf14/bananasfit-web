using Processo.Database.Interfaces;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Repositorios
{
    internal class EnderecoRepositorio : RepositorioBase<Endereco>, IEnderecoRepositorio
    {
        public EnderecoRepositorio(DatabaseContext contexto)
            : base(contexto)
        {

        }
    }
}
