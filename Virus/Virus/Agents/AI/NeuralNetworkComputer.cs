using Neural_Network;
using System;
using System.Collections.Generic;

namespace Virus
{
    public class NeuralNetworkComputer : VirusPlayer
    {
        public bool training;
        NeuralNet net;
        Board board;
        bool log;
        int playerNumber;
        MiniMaxComputer trainer;
        Random random = new Random();
        public NeuralNetworkComputer(Board board, int playerNumber, int depth, bool log, bool pruning)
        {
            counter = 0;
            this.board = board;
            this.playerNumber = playerNumber;
            trainer = new MiniMaxComputer(board, playerNumber, depth, pruning);
            
            this.log = log;

            training = true;

            int inputN, hiddenN, outputN;
            inputN = board.boardSize * board.boardSize * 3;
            hiddenN = board.boardSize * board.boardSize * 3 * 2/3;
            outputN = board.boardSize * board.boardSize * 3;
            net = new NeuralNet(inputN, new[] { hiddenN }, outputN);
        }

        Move move;
        Board temp;
        double[][] output;
        double[] vecOutput;
        double[][] input;
        double[] vec;
        int[,] neuralBoard;
        int row;
        int coloumn;
        int count;
        int counter;
        List<Move> moves;
        int x, y, i;
        Tuple<Board, Move> newBoard;
        bool correct;
        double error;
        public void play()
        {
            if (board.FindAvailableMoves(playerNumber).Count > 0)
            {
                bool skip = false;
                skip:
                if (!skip)
                {
                    for (int x = 0; x < board.boardSize; x++)
                    {
                        for (int y = 0; y < board.boardSize; y++)
                        {
                            if (playerNumber == 1)
                            {
                                if (board.board[x, y] == 2)
                                {
                                    skip = true;
                                    goto skip;
                                }
                            }
                            else
                            {
                                if (board.board[x, y] == 1)
                                {
                                    skip = true;
                                    goto skip;
                                }
                            }
                        }
                    }
                    Move move = board.FindAvailableMoves(playerNumber)[random.Next(board.FindAvailableMoves(playerNumber).Count)];
                    board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                    return;
                }
                //Define input for the neural network
                input = new double[1][];
                vec = new double[board.boardSize * board.boardSize * 3];
                count = 0;
                for (x = 0; x < board.boardSize; x++)
                {
                    for (y = 0; y < board.boardSize; y++)
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



                //Setting up the neural network
                for (int i = 0; i < net.InputLayer.Neurons.Count; i++)
                {
                    net.InputLayer.Neurons[i].Output = input[0][i];
                }
                net.Pulse();
                //end

                //Convert the output from the neural network to an actual move
                neuralBoard = new int[board.boardSize, board.boardSize];
                row = 0;
                coloumn = 0;
                count = 0;

                for (i = 0; i < net.OutputLayer.Neurons.Count; i++)
                {
                    if (net.OutputLayer.Neurons[i].Output > 0.9)
                    {
                        neuralBoard[row, coloumn] = 2;
                    }
                    else if (net.OutputLayer.Neurons[i + 1].Output > 0.9)
                    {
                        neuralBoard[row, coloumn] = 1;
                    }

                    else if (net.OutputLayer.Neurons[i + 2].Output > 0.9)
                    {
                        neuralBoard[row, coloumn] = 0;
                    }

                    coloumn++;
                    i += 2;
                    if (coloumn > board.boardSize - 1)
                    {
                        row++;
                        coloumn = 0;
                    }
                    count++;
                }

                moves = board.FindAvailableMoves(playerNumber);

                //Figure out if the move is actually a valid move if so take the move if not retrain the network
                temp = board.Copy();

                foreach (var item in moves)
                {
                    temp.IsMoveEligable(item.fromX, item.fromY, item.toX, item.toY);
                    temp.MakeMove(item.fromX, item.fromY, item.toX, item.toY);
                    correct = true;
                    for (x = 0; x < board.boardSize; x++)
                    {
                        for (y = 0; y < board.boardSize; y++)
                        {
                            if (neuralBoard[x, y] != temp.board[x, y])
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
                    move = null;
                }
                //end
                catch (Exception)
                {
                    //Define output for the neural network and train it with the help of minimax
                    newBoard = null;
                    try
                    {
                        newBoard = trainer.PredictMiniMaxMove(board.Copy());
                    }
                    catch (Exception)
                    {

                    }
                    if (newBoard == null)
                    {
                        return;
                    }
                    output = new double[1][];
                    vecOutput = new double[board.boardSize * board.boardSize * 3];
                    count = 0;
                    for (x = 0; x < board.boardSize; x++)
                    {
                        for (y = 0; y < board.boardSize; y++)
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
                            count += 3;
                        }
                    }
                    output[0] = vecOutput;

                    //net.CalculateErrors(net, output[0]);
                    
                    /*if (log)
                    {
                        double error = 0;
                        foreach (var item in net.outputLayer.neurons)
                        {
                            error += Math.Abs(item.error);
                        }
                        Log.WriteErrorGameLog(board.boardSize, "Error: " + error.ToString());
                    }*/
                    foreach (var outputNeuron in net.OutputLayer.Neurons)
                    {
                        error += Math.Abs(outputNeuron.Error);
                    }
                    Console.WriteLine("Error: " + error.ToString() + " " + counter);
                    error = 0;
                    counter++;
                    Retrain(input, output);
                }
            }



        }

        private void Retrain(double[][] input, double[][] output)
        {
            //Train the network and try to make a valid move
            net.Train(input, output, 0.1, 1);
            play();
            //end
        }

        public void AfterGame()
        {
            counter = 0;
        }
    }
}
