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
            maxcounter = 30;
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
            List<Move> moves = FindAvailableMoves(board, playerNumber);

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
                    List<Move> moves = FindAvailableMoves(tempBoard, 1);
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
                    List<Move> moves = FindAvailableMoves(tempBoard, 2);
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
                    if (tempBoard.board[x, y] == 1)
                    {
                        points += 1;
                    }
                    if (tempBoard.board[x, y] == 2)
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
            if (FindAvailableMoves(tempBoard, 2).Count > 0)
            {
                return false;
            }
            return true;
        }
        private bool isGameOverComputer()
        {
            if (FindAvailableMoves(tempBoard, 1).Count > 0)
            {
                return false;
            }
            return true;
        }
        private List<Move> FindAvailableMoves(Board board, sbyte playerNum)
        {
            List<Tuple<sbyte, sbyte, sbyte>> bricks = board.GetBricks(playerNum);
            List<Move> moves = new List<Move>();
            for (int i = 0; i < bricks.Count; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        sbyte x2 = -1;
                        sbyte y2 = -1;
                        if (x > bricks[i].Item2)
                            x2 = (sbyte)(x - bricks[i].Item2);
                        else
                            x2 = (sbyte)(bricks[i].Item2 - x);
                        if (y > bricks[i].Item3)
                            y2 = (sbyte)(y - bricks[i].Item3);
                        else
                            y2 = (sbyte)(bricks[i].Item3 - y);

                        sbyte result = board.TryMakeMove(playerNum, bricks[i].Item2, bricks[i].Item3, x2, y2);
                        if (result != -1)
                        {
                            Move curnMove = new Move() { fromX = bricks[i].Item2, fromY = bricks[i].Item3, toX = x2, toY = y2, moveValue = result };
                            bool exists = false;
                            foreach (var item in moves)
                            {
                                if (item.toX == curnMove.toX && item.toY == curnMove.toY)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                moves.Add(curnMove);
                            }
                        }
                    }
                }
                for (int x2 = -2; x2 < 3; x2 = x2 + 2)
                {
                    for (int y2 = -2; y2 < 3; y2 = y2 + 2)
                    {
                        sbyte x3 = -1;
                        sbyte y3 = -1;
                        if (x2 > bricks[i].Item2)
                            x3 = (sbyte)(x2 - bricks[i].Item2);
                        else
                            x3 = (sbyte)(bricks[i].Item2 - x2);
                        if (y2 > bricks[i].Item3)
                            y3 = (sbyte)(y2 - bricks[i].Item3);
                        else
                            y3 = (sbyte)(bricks[i].Item3 - y2);

                        sbyte result = board.TryMakeMove(playerNumber, bricks[i].Item2, bricks[i].Item3, x3, y3);
                        if (result != -1)
                        {
                            Move curnMove = new Move() { fromX = bricks[i].Item2, fromY = bricks[i].Item3, toX = x3, toY = y3, moveValue = result };
                            bool exists = false;
                            foreach (var item in moves)
                            {
                                if (item.toX == curnMove.toX && item.toY == curnMove.toY)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                moves.Add(curnMove);
                            }
                        }
                    }
                }
            }
            return moves;
        }

        private class Move
        {
            public sbyte fromX;
            public sbyte fromY;
            public sbyte toX;
            public sbyte toY;
            public sbyte moveValue;
        }
    }
}
