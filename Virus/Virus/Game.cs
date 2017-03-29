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
            for (int a = 5; a < 6; a++)
            {
                PlayGame(a);
                QLearningComputer player1 = new QLearningComputer(board, 1);
                SemiSmartComputer player2 = new SemiSmartComputer(board, 2);

                int[] result = new int[2];
                for (int i = 0; i < 5; i++)
                {
                    while (!board.IsDone())
                    {
                        player1.play();
                        board.Display();
                        Thread.Sleep(1000);
                        player2.play();
                        board.Display();
                        Thread.Sleep(1000);
                    }
                    int[] result2 = board.GetScore();
                    for (int b = 0; b < result2.Count(); b++)
                    {
                        result[b] += result2[b];
                    }
                    Thread.Sleep(1000);
                    board.reset();
                }
                for (int i = 0; i < result.Count(); i++)
                {
                    Console.WriteLine("Gameboard : "+a+" Player " + (i + 1) + " points: " + result[i]);
                }
            }
        }
    }
}
