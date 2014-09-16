using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Processo.Database.Interfaces
{
    internal interface IDatabaseContext 
    {
        void Commit();
        void Rollback();
    }
}
