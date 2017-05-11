using NeuralNetwork;
using System;
using Virus.Persistance;

namespace Virus
{
    public class NeuralNetworkComputer : VirusPlayer
    {
        NeuralNet net;
        Board board;
        int playerNumber;
        MiniMaxComputer trainer;
        public NeuralNetworkComputer(Board board, int playerNumber, ActivationFunction activation, bool storage, int depth)
        {
            this.board = board;
            this.playerNumber = playerNumber;
            trainer = new MiniMaxComputer(board, playerNumber, SQL.GetClient(), storage, depth);
            net = new NeuralNet(activation);
        }

        public void play()
        {
            //Define input for the neural network
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
            //end

            //Define output for the neural network and train it with the help of minimax
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

            inputN = 16 * 3;
            hiddenN = 16 * 2;
            outputN = 16 * 3;

            net.Init(3, inputN, hiddenN, outputN);

            net.Train(input, output, 0.5, 20);
            //end

            //Help debug
            Console.WriteLine("Start board");
            board.Display();
            Console.WriteLine("New board after minimax");
            newBoard.Item1.Display();
            Console.WriteLine("-----");
            //end

            //Setting up the neural network
            for (int i = 0; i < net.inputLayer.neurons.Count; i++)
            {
                net.inputLayer.neurons[i].SetOutput(input[0][i]);
            }
            net.Pulse();
            //end

            //Testing
            for (int i = 0; i < net.inputLayer.neurons.Count; i++)
            {
                Console.WriteLine("Input " + i + " : " + net.inputLayer.neurons[i].GetOutput());
            }
          
            for (int i = 0; i < net.outputLayer.neurons.Count; i++)
            {
                Console.WriteLine("Output " + i + " : " + net.outputLayer.neurons[i].GetOutput().ToString() + " Expected output: " + output[0][i]);
            }
            //end

            //Convert the output from the neural network to an actual move
            int[,] convBoard = new int[board.boardSize, board.boardSize];

            for (int i = 0; i < net.outputLayer.neurons.Count / 3; i++)
            {
                for (int j = 0; j < board.boardSize; j++)
                {
                    if (net.outputLayer.neurons[i].Output > 0.90)
                        convBoard[i, j] = 2;
                    else if (net.outputLayer.neurons[i].Output > 0.90)
                        convBoard[i + 1, j] = 1;
                    else if (net.outputLayer.neurons[i].Output > 0.90)
                        convBoard[i + 2, j] = 0;
                }
            }
            //end


            //board.MakeMove(newBoard.Item2.fromX, newBoard.Item2.fromY, newBoard.Item2.toX, newBoard.Item2.toY);
        }
        private void PrintOut(double val1, double val2)
        {
            NeuralNet net2 = new NeuralNet(ActivationFunction.SigmoidDerivative);

            double[][] input2 = new double[4][];
            input2[0] = new double[] { 1, 1 };
            input2[1] = new double[] { 0, 1 };
            input2[2] = new double[] { 1, 0 };
            input2[3] = new double[] { 0, 0 };
            double[][] output2 = new double[4][];
            output2[0] = new double[] { 0 };
            output2[1] = new double[] { 1 };
            output2[2] = new double[] { 1 };
            output2[3] = new double[] { 0 };

            net2.Init(3, 2, 4, 1);
            net2.Train(input2, output2, 0.3, 200);

            bool result;

            net2.inputLayer.neurons[0].SetOutput(val1);
            net2.inputLayer.neurons[1].SetOutput(val2);

            net2.Pulse();

            result = net2.outputLayer.neurons[0].GetOutput() > .5;

            Console.WriteLine("Input 1 / 2: " + val1 + " " + val2);
            Console.WriteLine("The actual result: " + result.ToString());
            Console.WriteLine(net2.outputLayer.neurons[0].GetOutput() + " % ");
        }
    }
}
