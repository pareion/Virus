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
         /*   board.MoveBrick(1, 3, 3, 3, 4);
            board.MoveBrick(1, 3, 3, 3, 2);
            board.MoveBrick(1, 3, 3, 2, 4);
            board.MoveBrick(1, 3, 3, 2, 3);
            board.MoveBrick(1, 3, 3, 2, 2);
            board.MoveBrick(1, 3, 3, 4, 4);
            board.MoveBrick(1, 3, 3, 4, 3);
            board.MoveBrick(1, 3, 3, 4, 2);*/
            board.Display();
        }
    }
}
