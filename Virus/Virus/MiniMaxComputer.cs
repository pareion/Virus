using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    class MiniMaxComputer : VirusPlayer
    {
        Board board;
        private int counter, maxcounter;
        private Board tempBoard, previousBoard;
        private float alpha = int.MinValue, beta = int.MaxValue;
        bool done = false;
        float bestScoreMin;
        float bestScoreMax;
        sbyte playerNumber;

        public MiniMaxComputer(Board board, sbyte playerNumber)
        {
            counter = 0;
            maxcounter = 5;
            this.board = board;
            this.playerNumber = playerNumber;
        }
        public void play()
        {
            MiniMax(board);
        }

        private void MiniMax(Board board)
        {
            Move bestMove = null;
            float bestScore = -9999;
            float minscore;
            counter = 0;
            // find bricks you can use
            List<Move> moves = board.FindAvailableMoves(playerNumber);

            try
            {
                bestMove = moves[0];
                if (moves != null)
                    for (int b = 0; b < moves.Count; b++)
                    {
                        tempBoard = board.Copy();
                        tempBoard.MoveBrick(moves[b].fromX, moves[b].fromY, moves[b].toX, moves[b].toY);
                        minscore = MIN();
                        if (minscore > bestScore)
                        {
                            bestMove = moves[b];
                            bestScore = minscore;
                        }
                        this.counter = 0;
                        maxcounter = 0;
                    }
                board.MoveBrick(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
            }
            catch (Exception)
            {

            }

        }
        private float MIN()
        {
            if (isGameOverHuman())
                return -9999;
            else if (maxDepth())
                return EVAL();
            else
            {
                bestScoreMin = 9999;
                done = false;
            done: if (!done)
                {
                    List<Move> moves;
                    if (playerNumber == 2)
                    {
                        moves = tempBoard.FindAvailableMoves(2);
                    }
                    else
                        moves = tempBoard.FindAvailableMoves(1);

                    foreach (var item in moves)
                    {
                        previousBoard = tempBoard.Copy();
                        tempBoard.MoveBrick(item.fromX, item.fromY, item.toX, item.toY);
                        float score = MAX();
                        beta = score;

                        if (beta <= alpha)
                        {
                            done = true;
                            goto done;
                        }
                        if (score < bestScoreMin)
                        {
                            bestScoreMin = score;
                        }
                        tempBoard = previousBoard.Copy();
                    }
                }
                return bestScoreMin;
            }
        }
        private float MAX()
        {
            if (isGameOverComputer())
                return 9999;
            else if (maxDepth())
            {
                return EVAL();
            }
            else
            {
                bestScoreMax = -9999;
                done = false;
            done: if (!done)
                {
                    List<Move> moves;
                    if (playerNumber == 1)
                    {
                        moves = tempBoard.FindAvailableMoves(2);
                    }else
                        moves = tempBoard.FindAvailableMoves(1);

                    foreach (var item in moves)
                    {
                        previousBoard = tempBoard.Copy();
                        tempBoard.MoveBrick(item.fromX, item.fromY, item.toX, item.toY);
                        float score = MIN();
                        beta = score;

                        if (beta <= alpha)
                        {
                            done = true;
                            goto done;
                        }
                        if (score > bestScoreMax)
                        {
                            bestScoreMax = score;
                        }
                        tempBoard = previousBoard.Copy();
                    }
                }
                return bestScoreMin;
            }
        }

        private float EVAL()
        {
            float points = 0;
            for (int x = 0; x < tempBoard.boardSize; x++)
            {
                for (int y = 0; y < tempBoard.boardSize; y++)
                {
                    if (tempBoard.board[x, y] == playerNumber)
                    {
                        points += 1;
                    }
                    if (tempBoard.board[x, y] != playerNumber)
                    {
                        points -= 1;
                    }
                }
            }
            return points;
        }

        private bool maxDepth()
        {
            if (counter >= maxcounter)
                return true;
            else
                counter++;
            return false;
        }

        private bool isGameOverHuman()
        {
            if (tempBoard.FindAvailableMoves(1).Count > 0)
            {
                return false;
            }
            return true;
        }
        private bool isGameOverComputer()
        {
            if (tempBoard.FindAvailableMoves(2).Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}
