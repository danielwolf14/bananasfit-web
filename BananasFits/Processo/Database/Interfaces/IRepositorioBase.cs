using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Repositorio
{
    public interface IRepositorioBase<TEntidade> where TEntidade : EntidadeBase
    {
        TEntidade BuscarPorChave(long chave);

        IEnumerable<TEntidade> Consultar(Expression<Func<TEntidade, bool>> criterios);
        
        void Atualizar(TEntidade entidade);

        void Inserir(TEntidade entidade);

        void Deletar(TEntidade entidade);
    }
}
