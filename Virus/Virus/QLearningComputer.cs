using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    public class QLearningComputer : VirusPlayer
    {
        private double learningRate;
        private double discountFactor;
        private int playerNumber;
        private Board board;
        private double[,] Qreward;
        private double[,] Rreward;
        private List<Move> movesMade;
        /// <summary>
        /// Takes in the game as a board and then the learning rate which should be a number between 0 and 1
        /// then the discount rate which also should be a number between 0 and 1 but it determines the importance
        /// of future rewards.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="learningRate"></param>
        /// <param name="playerNumber"></param>
        public QLearningComputer(Board board, double learningRate, double discountFactor, int playerNumber)
        {
            this.board = board;
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
            this.playerNumber = playerNumber;
            Qreward = new double[board.boardSize, board.boardSize];
            Rreward = new double[board.boardSize, board.boardSize];
            movesMade = new List<Move>();
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    Rreward[x, y] = 1000;
                }
            }
        }

        public void AfterGame()
        {
            int[] result = board.GetScore();
            foreach (Move move in movesMade)
            {
                if (result[playerNumber - 1] > result[0] || result[playerNumber - 1] > result[1])
                {
                    Rreward[move.toX, move.toY]++;
                }
                else
                {
                    Rreward[move.toX, move.toY]--;
                }
                Qreward[move.toX, move.toY]++;
            }
            movesMade.Clear();
        }

        public void play()
        {
            Move moveToTake = null;

            List<Move> movesAvailable = board.FindAvailableMoves(playerNumber);

            double currentBestMove = 0;
           
            foreach (Move move in movesAvailable)
            {
                double tmp = Max(board.GetNewBoard(board, move));
                if (currentBestMove > (Reward(move) + learningRate * tmp))
                {
                    currentBestMove = Reward(move) + learningRate * tmp;
                    moveToTake = move;
                }
            }
            if (movesAvailable.Count > 0)
            {
                if (moveToTake == null)
                {
                    Move move = movesAvailable[new Random().Next(movesAvailable.Count - 1)];
                    board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                    movesMade.Add(move);
                }
                else
                {
                    board.MoveBrick(moveToTake.fromX, moveToTake.fromY, moveToTake.toX, moveToTake.toY);
                    movesMade.Add(moveToTake);
                }
            }
        }

        private double Reward(Move move)
        {
            return Rreward[move.fromX, move.fromY];
        }

        public double Max(Board board)
        {
            double best = 0;
            foreach (Move move in board.FindAvailableMoves(playerNumber))
            {
                if (Qreward[move.fromX,move.fromY] > best)
                {
                    best = discountFactor * Qreward[move.fromX, move.fromY];
                }
            }
            return best;
        }
    }
}
