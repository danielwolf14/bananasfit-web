using Processo.Database;
using Processo.Database.Repositorios;
using Processo.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo
{
    public class UnityOfWork
    {
        private static UnityOfWork instancia;

        private DatabaseContext contexto;

        public virtual PessoaFisicaNegocio PessoaFisicaNegocio { get; private set; }

        public virtual PessoaJuridicaNegocio PessoaJuridicaNegocio { get; private set; }

        public virtual ServicoNegocio ServicoNegocio { get; private set; }

        public virtual AvaliacaoNegocio AvaliacaoNegocio { get; private set; }

        public virtual ServicoPessoaJuridicaNegocio ServicoPessoaJuridicaNegocio { get; private set; }

        public virtual HistoricoCompraServicoNegocio HistoricoCompraServicoNegocio { get; private set; }

        public void Commit()
        {
            contexto.Commit();
        }

        public void Rollback()
        {
            contexto.Rollback();
        }

        private UnityOfWork(DatabaseContext contexto)
        {
            this.contexto = contexto;
            PessoaFisicaNegocio = new PessoaFisicaNegocio(contexto);
            PessoaJuridicaNegocio = new PessoaJuridicaNegocio(contexto);
            ServicoNegocio = new ServicoNegocio(contexto);
            AvaliacaoNegocio = new AvaliacaoNegocio(contexto);
            ServicoPessoaJuridicaNegocio = new ServicoPessoaJuridicaNegocio(contexto);
            HistoricoCompraServicoNegocio = new HistoricoCompraServicoNegocio(contexto);
        }
        
        public static UnityOfWork GetInstancia()
        {
            if (instancia == null)
                instancia = new UnityOfWork(new DatabaseContext());
            return instancia;
        }
    }
}
