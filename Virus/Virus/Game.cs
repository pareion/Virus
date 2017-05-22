using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetwork;
using Virus.Persistance;

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
            //VirusPlayer player1 = new NeuralNetworkComputer(board, 1, ActivationFunction.SigmoidDerivative, false, 2);
            //VirusPlayer player1 = new MiniMaxComputer(board, 1, SQL.GetClient(), true, 3);
            VirusPlayer player1 = new QLearningComputer(board, 0.1, 0.1, 1);
            VirusPlayer player2 = new RandomComputer(board, 2);
            bool visual = false;
            int[] result = new int[2];

            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 3; j++)
                {
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
                    int[] result2 = board.GetScore();
                    for (int b = 0; b < result2.Count(); b++)
                    {
                        result[b] += result2[b];
                    }
                    
                    board.reset();
                }
                Console.WriteLine("Game size " + gameSize + " Player 1 points: " + result[0]);
                Console.WriteLine("Game size " + gameSize + " Player 2 points: " + result[1]);
                result = new int[2];
            }
            

            Log.WriteLineToLog("Game size " + gameSize + " Player 1 points: " + result[0]);
            Log.WriteLineToLog("Game size " + gameSize + " Player 2 points: " + result[1]);

        }
    }
}
