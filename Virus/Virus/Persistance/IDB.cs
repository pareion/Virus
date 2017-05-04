using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Persistance
{
    interface IDB
    {
        void Create(NeoNode node);
        bool CreateChild(int id, NeoNode node);
    }
}
