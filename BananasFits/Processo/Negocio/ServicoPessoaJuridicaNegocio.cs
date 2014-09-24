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
    public class ServicoPessoaJuridicaNegocio : NegocioBase<ServicoPessoaJuridica>, IServicoPessoaJuridicaNegocio
    {
        internal ServicoPessoaJuridicaNegocio(IServicoPessoaJuridicaRepositorio repositorio)
            : base(repositorio)
        {
        }

        public void Cadastrar(ServicoPessoaJuridica servicoPessoaJuridica)
        {
            var mensagens = new List<string>();
            VerificarNegocioException(mensagens);
            base.Inserir(servicoPessoaJuridica);
        }

      
    }
}
