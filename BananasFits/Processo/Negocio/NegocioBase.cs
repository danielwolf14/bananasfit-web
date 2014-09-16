using Processo.Database;
using Processo.Entidades;
using Processo.Negocio.Interfaces;
using Processo.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Negocio
{
    public abstract class NegocioBase<TEntidade> : INegocioBase<TEntidade>
        where TEntidade : EntidadeBase
    {
        private IRepositorioBase<TEntidade> repositorio;

        internal NegocioBase(IRepositorioBase<TEntidade> repositorio)
        {
            this.repositorio = repositorio;
        }

        public TEntidade BuscarPorChave(long chave)
        {
            return repositorio.BuscarPorChave(chave);
        }

        public IEnumerable<TEntidade> Consultar(System.Linq.Expressions.Expression<Func<TEntidade, bool>> criterios)
        {
            return repositorio.Consultar(criterios);
        }

        public void Atualizar(TEntidade entidade)
        {
            repositorio.Atualizar(entidade);
        }

        public virtual void Inserir(TEntidade entidade)
        {
            repositorio.Inserir(entidade);
        }

        public void Deletar(TEntidade entidade)
        {
            repositorio.Deletar(entidade);
        }

        public void VerificarNegocioException(IList<string> mensagens)
        {
            if (mensagens.Count > 0)
                throw new NegocioException(mensagens);
        }

    }
}
