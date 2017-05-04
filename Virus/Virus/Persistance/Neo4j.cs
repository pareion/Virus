using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Persistance
{
    public class Neo4j
    {
        private static Neo4j client = null;
        private GraphClient db;
        private Neo4j()
        {
            db = new GraphClient(new Uri("http://localhost:7474/db/data"), "anders", "anders2");
            db.Connect();
        }
        public static Neo4j GetClient()
        {
            if (client == null)
            {
                client = new Neo4j();
            }
            return client;
        }
        public void Create(NeoNode node)
        {
            db.Cypher
                    .Create("(node:NeoNode {node})")
                    .WithParam("node", node)
                    .ExecuteWithoutResults();
        }
        public bool CreateChild(int id, NeoNode node)
        {
            try
            {
                db.Cypher
                       .Match("(node:NeoNode)")
                       //.Where((NeoNode dnode) => dnode.id == id)
                       .Where("node.id = " + id)
                       .Create("(node)-[:CHILD]->(child:NeoNode {node4})")
                       .WithParam("node4", node)
                       .ExecuteWithoutResults();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
