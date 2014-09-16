using Processo.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Mapeamento
{
    internal abstract class EntidadeBaseMap<TEntidade> : EntityTypeConfiguration<TEntidade>
       where TEntidade : EntidadeBase
    {
        public EntidadeBaseMap()
        {
            HasKey(entity => entity.Chave);
        }
    }
}
