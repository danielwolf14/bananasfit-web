using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Entidades
{
    public class Endereco : EntidadeBase
    {
        public virtual string CEP { get; set; }
        public virtual string Rua { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cidade { get; set; }
        public virtual EstadoEnum Estado { get; set; }
    }

    public enum EstadoEnum
    {
        AC, // Acre
        AL, // Alagoas
        AP, // Amapá
        AM, // Amazonas
        BA, // Bahia
        CE, // Ceará
        DF, // Distrito Federal
        ES, // Espírito Santo
        GO, // Goiás
        MA, // Maranhão
        MT, // Mato Grosso
        MS, // Mato Grosso do Sul
        MG, // Minas Gerais
        PA, // Pará
        PB, // Paraíba
        PR, // Paraná
        PE, // Pernambuco
        PI, // Piauí
        RR, // Roraima
        RO, // Rondônia
        RJ, // Rio de Janeiro
        RN, // Rio Grande do Norte
        RS, // Rio Grande do Sul
        SC, // Santa Catarina
        SP, // São Paulo
        SE, // Sergipe
        TO // Tocantins
    }
}
