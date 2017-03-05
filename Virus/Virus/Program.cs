using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(10);
            board.StartGame();
            board.Display();
            Console.WriteLine();
            int a = board.MoveBrick(1, 6, 3, 6, 4);
            board.Display();
           /* int b = board.MoveBrick(1, 6, 4, 6, 5);
            board.Display();
            int c = board.MoveBrick(1, 6, 5, 6, 6);
            board.Display();*/
        }
    }
}
