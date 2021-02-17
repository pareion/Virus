using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace Virus
{
    public class Game
    {
        private readonly Board Board;
        private int GameSize;
        public Game(int initSize)
        {
            Board = new Board(initSize);
            GameSize = initSize;
        }
        private void PlayGame(int size)
        {
            Board.reset();
            GameSize = size;
        }
        public void StartGame()
        {
            VirusPlayer player1 = new NeuralNetworkComputer(Board, 1, 2, false, true);
            //VirusPlayer player1 = new MiniMaxComputer(board, 1, 4, true);
            //VirusPlayer player1 = new QLearningComputer(board, 1, 1, 10, 1);
            VirusPlayer player2 = new SemiSmartComputer(Board, 2);
            bool visual = true;
            int[] result = new int[2];
            int[] result2 = new int[2];
            for (int i = 0; i < 1; i++)
            {
                Stopwatch time = new Stopwatch();
                time.Start();
                for (int j = 0; j < 1; j++)
                {
                    Console.WriteLine("Starting a new game");
                    while (!Board.IsDone())
                    {
                        player1.play();
                        if (visual)
                        {
                            Board.Display();
                        }
                        Thread.Sleep(1000);
                        player2.play();
                        if (visual)
                        {
                            Board.Display();
                        }
                        Thread.Sleep(1000);
                    }
                    player1.AfterGame();
                    player2.AfterGame();

                    result2 = Board.GetScore();
                    Board.reset();

                    for (int b = 0; b < result2.Count(); b++)
                    {
                        result[b] += result2[b];
                    }
                }
                time.Stop();
                Console.WriteLine("Time taken: "+time.Elapsed);
                Console.WriteLine("Game size " + GameSize + " Player 1 points: " + result[0]);
                Console.WriteLine("Game size " + GameSize + " Player 2 points: " + result[1]);
                result = new int[2];
            }
        }
    }
}
