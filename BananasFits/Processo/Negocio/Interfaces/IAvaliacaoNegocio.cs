using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Negocio.Interfaces
{
    public interface IAvaliacaoNegocio : INegocioBase<Avaliacao>       
    {
        Avaliacao Avaliar(Avaliacao avaliacao);
    }
}
