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
    public class AvaliacaoNegocio : NegocioBase<Avaliacao>, IAvaliacaoNegocio
    {
        internal AvaliacaoNegocio(IAvaliacaoRepositorio repositorio)
            : base(repositorio)
        {
        }

        public void Cadastrar(Avaliacao avaliacao)
        {
            var mensagens = new List<string>();
            VerificarNegocioException(mensagens);
            base.Inserir(avaliacao);
        }


    }
}
