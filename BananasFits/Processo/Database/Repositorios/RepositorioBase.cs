using Processo.Database.Interfaces;
using Processo.Entidades;
using Processo.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Processo.Database.Repositorios
{
    internal abstract class RepositorioBase<TEntidade> : IRepositorioBase<TEntidade>
        where TEntidade : EntidadeBase
    {

        public virtual DbSet<TEntidade> DataSet { get; private set; }
        public virtual DatabaseContext Contexto { get; set; }

        public RepositorioBase(DatabaseContext contexto)
        {
            DataSet = contexto.Set<TEntidade>();
            Contexto = contexto;
        }

        public TEntidade BuscarPorChave(long chave)
        {
            return DataSet.Find(chave);
        }

        public IEnumerable<TEntidade> Consultar(System.Linq.Expressions.Expression<Func<TEntidade, bool>> criterios)
        {
            return DataSet.Where(criterios);
        }

        public void Atualizar(TEntidade entidade)
        {
            DataSet.Attach(entidade);
            Contexto.Entry(entidade).State = EntityState.Modified;
        }

        public void Inserir(TEntidade entidade)
        {
            DataSet.Add(entidade);
        }

        public void Deletar(TEntidade entidade)
        {
            DataSet.Remove(entidade);
        }

        public void DeletarPorChave(int chave)
        {
            var entidade = DataSet.Find(chave);
            DataSet.Remove(entidade);
        }

    }
}
