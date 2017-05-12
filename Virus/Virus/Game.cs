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
            NeuralNetworkComputer player1 = new NeuralNetworkComputer(board, 1, ActivationFunction.SigmoidDerivative, false, 3);
            //MiniMaxComputer player1 = new MiniMaxComputer(board, 1, SQL.GetClient(), false, 3);
            SemiSmartComputer player2 = new SemiSmartComputer(board, 2);
            bool visual = true;
            int[] result = new int[2];
            player1.training = true;

            for (int i = 0; i < 10; i++)
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
                board.reset();
            }


            Console.Clear();
            Console.WriteLine("Real game started");
            player1.training = false;
            visual = true;

            PlayGame(gameSize);
            for (int i = 0; i < 5; i++)
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
                int[] result2 = board.GetScore();
                for (int b = 0; b < result2.Count(); b++)
                {
                    result[b] += result2[b];
                }
                board.reset();
            }
            for (int i = 0; i < result.Count(); i++)
            {
                Console.WriteLine("Gameboard : " + gameSize + " Player " + (i + 1) + " points: " + result[i]);
            }

        }
    }
}
