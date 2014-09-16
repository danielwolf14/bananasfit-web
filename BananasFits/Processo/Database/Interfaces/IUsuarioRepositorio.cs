﻿using Processo.Entidades;
using Processo.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Interfaces
{
    internal interface IUsuarioRepositorio<TEntidade> : IRepositorioBase<TEntidade>
        where TEntidade : Usuario
    {
    }
}
