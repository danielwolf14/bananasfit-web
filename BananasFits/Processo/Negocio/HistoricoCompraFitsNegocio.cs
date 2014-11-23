﻿using System;
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
    public class HistoricoCompraFitsNegocio : NegocioBase<HistoricoCompraFits>, IHistoricoCompraFitsNegocio
    {
        internal HistoricoCompraFitsNegocio(DatabaseContext contexto)
            : base(contexto)
        {
            this.repositorio = new HistoricoCompraFitsRepositorio(contexto);
        }

        public void Cadastrar(HistoricoCompraFits historicoCompraFits)
        {
            var mensagens = new List<string>();
            VerificarNegocioException(mensagens);
            base.Inserir(historicoCompraFits);
        }      
    }
}
