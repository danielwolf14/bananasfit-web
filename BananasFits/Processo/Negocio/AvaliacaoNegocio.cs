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

        public Avaliacao Avaliar(Avaliacao avaliacao)
        {
            var avaliacaoExistente = Consultar(e => e.PessoaFisica.Chave == avaliacao.PessoaFisica.Chave
                && e.PessoaJuridica.Chave == avaliacao.PessoaJuridica.Chave).SingleOrDefault();

            if (avaliacaoExistente != null)
            {
                base.Inserir(avaliacao);
            }
            else
            {
                avaliacaoExistente.Pontuacao = avaliacao.Pontuacao;
                base.Atualizar(avaliacaoExistente);
            }
            return avaliacaoExistente;
        }
    }
}
