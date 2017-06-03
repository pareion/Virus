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
            //VirusPlayer player2 = new NeuralNetworkComputer(board, 2, ActivationFunction.Sigmoid, false, 2, false);
            VirusPlayer player2 = new MiniMaxComputer(board, 2, Neo4j.GetClient(), true, 3);
            VirusPlayer player1 = new SemiSmartComputer(board, 1);
            bool visual = false;
            int[] result = new int[2];
            int[] result2 = new int[2];

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
            Console.WriteLine("Game size " + gameSize + " Player 1 points: " + result[0]);
            Console.WriteLine("Game size " + gameSize + " Player 2 points: " + result[1]);
        }
    }
}
