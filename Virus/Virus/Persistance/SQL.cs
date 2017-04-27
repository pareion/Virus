using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Persistance
{
    class SQL : IDB
    {
        private static SQL client = null;
        private SQL()
        {
        }
        public static SQL GetClient()
        {
            if (client == null)
            {
                client = new SQL();
            }
            return client;
        }
        public void CreateChild(int id, NeoNode node)
        {
            throw new NotImplementedException();
        }

        public void Create(NeoNode node)
        {
            throw new NotImplementedException();
        }
    }
}
