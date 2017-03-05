using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Virus
{
    public class Game
    {
        private Board board;
        private void PlayGame(int size)
        {
            board = new Board(size);
            board.StartGame();
        }
        public void StartGame()
        {
            PlayGame(10);
            RandomComputer player1 = new RandomComputer(ref board, 1);
            RandomComputer player2 = new RandomComputer(ref board, 2);
            board.SetPlayer1(player1);
            board.SetPlayer2(player2);
            while (true)
            {
                player1.play();
                player2.play();
                board.Display();
            }
        }
    }
}
