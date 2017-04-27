using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    class NeuralNetworkComputer : VirusPlayer
    {
        NeuralNet net;
        Board board;
        int playerNumber;
        MiniMaxComputer trainer;
        public NeuralNetworkComputer(Board board, int playerNumber, ActivationFunction activation)
        {
            this.board = board;
            this.playerNumber = playerNumber;
            trainer = new MiniMaxComputer(board, playerNumber);
            net = new NeuralNet(activation);
        }

        public void play()
        {
            //Define input
            double[][] input = new double[1][];
            double[] vec = new double[board.boardSize * board.boardSize * 3];
            int count = 0;
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    if (board.board[x, y] == 2)
                    {
                        vec[count] = 1;
                    }
                    else if (board.board[x, y] == 1)
                    {
                        vec[count + 1] = 1;
                    }
                    else
                    {
                        vec[count + 2] = 1;
                    }
                    count = count + 3;
                }
            }
            input[0] = vec;

            //Define output
            Tuple<Board, Move> newBoard = trainer.PredictMiniMaxMove(board.Copy());
            double[][] output = new double[1][];
            double[] vecOutput = new double[board.boardSize * board.boardSize * 3];
            count = 0;

            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    if (newBoard.Item1.board[x, y] == 2)
                    {
                        vecOutput[count] = 1;
                    }
                    else if (newBoard.Item1.board[x, y] == 1)
                    {
                        vecOutput[count + 1] = 1;
                    }
                    else
                    {
                        vecOutput[count + 2] = 1;
                    }
                    count = count + 3;
                }
            }
            output[0] = vecOutput;

            int inputN, hiddenN, outputN;

            inputN = board.boardSize * board.boardSize * 3;
            hiddenN = board.boardSize * board.boardSize * 3 * 2 / 3;
            outputN = board.boardSize * board.boardSize * 3;

            net.Init(3, inputN, hiddenN, outputN);
            net.Train(input, output, 2, 100);

            output = new double[1][];
            for (int i = 0; i < net.inputLayer.neurons.Count; i++)
            {
                net.inputLayer.neurons[i].Output = input[0][i];
            }
            net.Pulse();

            //Read the neurons from the output layer and convert it to a move
            for (int i = 0; i < net.outputLayer.neurons.Count; i++)
            {
                Console.WriteLine("Output " + i + " : " + net.outputLayer.neurons[i].Output);
            }

            //Make the move from the output layer
            //board.MakeMove(newBoard.Item2.fromX, newBoard.Item2.fromY, newBoard.Item2.toX, newBoard.Item2.toY);
        }
    }
}
