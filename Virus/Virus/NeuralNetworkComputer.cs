using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Threading;
using Virus.Persistance;

namespace Virus
{
    public class NeuralNetworkComputer : VirusPlayer
    {
        public bool training;
        NeuralNet net;
        Board board;
        int playerNumber;
        MiniMaxComputer trainer;
        Random random = new Random();
        public NeuralNetworkComputer(Board board, int playerNumber, ActivationFunction activation, bool storage, int depth)
        {
            this.board = board;
            this.playerNumber = playerNumber;
            trainer = new MiniMaxComputer(board, playerNumber, SQL.GetClient(), storage, depth);
            net = new NeuralNet(activation);

            training = true;

            int inputN, hiddenN, outputN;
            inputN = 16 * 3;
            hiddenN = 16 * 4;
            outputN = 16 * 3;
            net.Init(3, inputN, hiddenN, outputN);
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

            //Define output for the neural network for retraining purpose
            //Define output for the neural network and train it with the help of minimax
            Tuple<Board, Move> newBoard = null;
            try
            {
                newBoard = trainer.PredictMiniMaxMove(board.Copy());
            }
            catch (Exception)
            {
                return;
            }

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
            //end

            //Setting up the neural network
            for (int i = 0; i < net.inputLayer.neurons.Count; i++)
            {
                net.inputLayer.neurons[i].SetOutput(input[0][i]);
            }
            net.Pulse();
            //end

            //Testing
            /*
            for (int i = 0; i < net.inputLayer.neurons.Count; i++)
            {
                Console.WriteLine("Input " + i + " : " + net.inputLayer.neurons[i].GetOutput());
            }

            for (int i = 0; i < net.outputLayer.neurons.Count; i++)
            {
                Console.WriteLine("Output " + i + " : " + net.outputLayer.neurons[i].GetOutput().ToString() + " Expected output: " + output[0][i]);
            }
            */
            //end

            //Convert the output from the neural network to an actual move
            int[,] neuralBoard = new int[board.boardSize, board.boardSize];
            int row = 0;
            int coloumn = 0;
            for (int i = 0; i < net.outputLayer.neurons.Count; i++)
            {
                if (net.outputLayer.neurons[i].GetOutput() > 0.90)
                    neuralBoard[row, coloumn] = 2;
                else if (net.outputLayer.neurons[i + 1].GetOutput() > 0.90)
                    neuralBoard[row, coloumn] = 1;
                else if (net.outputLayer.neurons[i + 2].GetOutput() > 0.90)
                    neuralBoard[row, coloumn] = 0;

                coloumn++;
                i = i + 2;
                if (coloumn > 3)
                {
                    row++;
                    coloumn = 0;
                }
            }
            //end
            
            List<Move> moves = board.FindAvailableMoves(playerNumber);

            //Figure out if the move is actually a valid move if so take the move if not retrain the network
            Move move = null;
            Board temp = board.Copy();

            foreach (var item in moves)
            {
                temp.IsMoveEligable(item.fromX, item.fromY, item.toX, item.toY);
                temp.MakeMove(item.fromX, item.fromY, item.toX, item.toY);
                bool correct = true;
                for (int x = 0; x < board.boardSize; x++)
                {
                    for (int y = 0; y < board.boardSize; y++)
                    {
                        if (neuralBoard[x,y] != temp.board[x,y])
                        {
                            correct = false;
                        }
                    }
                }
                if (correct)
                {
                    move = item;
                    break;
                }
                temp = board.Copy();
            }
            try
            {
                board.IsMoveEligable(move.fromX, move.fromY, move.toX, move.toY);
                board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
            }
            //end
            catch (Exception)
            {
                int reTrain = 0;
                for (int x = 0; x < board.boardSize; x++)
                {
                    for (int y = 0; y < board.boardSize; y++)
                    {
                        if (board.board[x,y] == 2)
                        {
                            reTrain++;
                        }
                    }
                }
                if (training)
                {
                    if (reTrain != 0)
                    {
                        Retrain(input, output);
                    }
                    else
                    {
                        List<Move> tmp = board.FindAvailableMoves(playerNumber);
                        move = tmp[random.Next(0, tmp.Count)];
                        board.IsMoveEligable(move.fromX, move.fromY, move.toX, move.toY);
                        board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                    }
                }
                else
                {
                    try
                    { 
                        board.IsMoveEligable(move.fromX, move.fromY, move.toX, move.toY);
                        board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                    }
                    catch (Exception)
                    {
                        List<Move> tmp = board.FindAvailableMoves(playerNumber);
                        move = tmp[random.Next(0, tmp.Count)];
                        board.IsMoveEligable(move.fromX, move.fromY, move.toX, move.toY);
                        board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                    }
                }
            }
        }

        private void Retrain(double[][] input, double[][] output)
        {
            //Train the network and try to make a valid move
            net.Train(input, output, 0.5, 20);
            play();
            //end
        }
    }
}
