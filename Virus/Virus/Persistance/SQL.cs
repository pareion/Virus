using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Persistance
{
    public class SQL : IDB
    {
        private static SQL client = null;
        private EntityFramework.VirusGameEntities db;

        private SQL()
        {
            db = new EntityFramework.VirusGameEntities();
        }
        public static SQL GetClient()
        {
            if (client == null)
            {
                client = new SQL();
            }
            return client;
        }
        public bool CreateChild(int id, NeoNode node)
        {
            EntityFramework.Node temp = new EntityFramework.Node();

            temp.Id = node.id;
            temp.Value = node.value;

            EntityFramework.Node find = db.Node.First(x => x.Id == id);

            if (find != null)
            {
                find.Node1.Add(temp);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public void Create(NeoNode node)
        {
            EntityFramework.Node temp = new EntityFramework.Node();

            temp.Id = node.id;
            temp.Value = node.value;

            db.Node.Add(temp);
            db.SaveChanges();
        }
    }
}
