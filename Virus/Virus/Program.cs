using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTesting;

namespace Virus
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 7; i < 12; i++)
            {
                Game game = new Game(i);
                game.StartGame();
            }
        }
    }
}
