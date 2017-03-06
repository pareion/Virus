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
            PlayGame(8);
            //   RandomComputer player1 = new RandomComputer(board, 1);
            MiniMaxComputer player1 = new MiniMaxComputer(board, 1);
            SemiSmartComputer player2 = new SemiSmartComputer(board, 2);
            board.SetPlayer1(player1);
            board.SetPlayer2(player2);
            int[] result = new int[2];
            for (int i = 0; i < 200; i++)
            {
                while (!board.IsDone())
                {
                    player1.play();
                    player2.play();
                    board.Display();
                }
                sbyte[] result2 = board.GetScore();
                for (int a = 0; a < result2.Count(); a++)
                {
                    result[a] += result2[a];
                }
                board.reset();
            }
            
            for (int i = 0; i < result.Count(); i++)
            {
                Console.WriteLine("Player " + i + " points: " + result[i]);
            }
            Console.Read();
        }
    }
}
