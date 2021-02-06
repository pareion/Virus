using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetwork;
using Virus.Persistance;
using System.Diagnostics;

namespace Virus
{
    public class Game
    {
        private Board board;
        private int gameSize;
        public Game(int initSize)
        {
            board = new Board(initSize);
            gameSize = initSize;
        }
        private void PlayGame(int size)
        {
            board.reset();
            gameSize = size;
        }
        public void StartGame()
        {
            VirusPlayer player1 = new NeuralNetworkComputer(board, 2, ActivationFunction.Sigmoid, false, 3, false, true);
            //VirusPlayer player1 = new MiniMaxComputer(board, 1, SQL.GetClient(), false, 4, true);
            //VirusPlayer player1 = new QLearningComputer(board, 1, 1, 10, 1);
            VirusPlayer player2 = new SemiSmartComputer(board, 2);
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
                    while (!board.IsDone())
                    {
                        player1.play();
                        if (visual)
                        {
                            board.Display();
                        }
                        player2.play();
                        if (visual)
                        {
                            board.Display();
                        }
                    }
                    player1.AfterGame();
                    player2.AfterGame();

                    result2 = board.GetScore();
                    board.reset();

                    for (int b = 0; b < result2.Count(); b++)
                    {
                        result[b] += result2[b];
                    }
                }
                time.Stop();
                Console.WriteLine("Time taken: "+time.Elapsed);
                Console.WriteLine("Game size " + gameSize + " Player 1 points: " + result[0]);
                Console.WriteLine("Game size " + gameSize + " Player 2 points: " + result[1]);
                result = new int[2];
            }
        }
    }
}
