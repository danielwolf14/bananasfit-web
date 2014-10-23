using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Negocio.Interfaces
{
    public interface IServicoPessoaJuridicaNegocio : INegocioBase<ServicoPessoaJuridica>
    {
        void Comprar(string qrCode, int chavePessoaFisica);

    }
}
