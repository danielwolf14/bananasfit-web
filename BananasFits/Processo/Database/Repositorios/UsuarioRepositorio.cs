using Processo.Database.Interfaces;
using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Repositorios
{
    internal abstract class UsuarioRepositorio<TEntidade> : RepositorioBase<TEntidade>, IUsuarioRepositorio<TEntidade>
        where TEntidade : Usuario
    {
        public UsuarioRepositorio(DatabaseContext contexto)
            : base(contexto) { }
    }
}
