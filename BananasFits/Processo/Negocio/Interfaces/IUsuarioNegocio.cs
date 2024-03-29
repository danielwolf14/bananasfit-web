﻿using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Negocio.Interfaces
{
    public interface IUsuarioNegocio<TEntidade> : INegocioBase<TEntidade>
        where TEntidade : Usuario
    {
        void Cadastrar(TEntidade usuario);
        TEntidade BuscarUsuarioPorEmail(string email);
    }
}
