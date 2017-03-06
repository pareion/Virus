using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    class SemiSmartComputer : VirusPlayer
    {
        Board board;
        sbyte playerNumber;
        Random rng = new Random();
        public SemiSmartComputer(ref Board board, sbyte playerNumber)
        {
            this.board = board;
            this.playerNumber = playerNumber;
        }
        public void play()
        {
            List<Move> result = FindAvailableMoves();
            if (result.Count > 0)
            {
                Move bestMove = result[0];
                for (int i = 1; i < result.Count(); i++)
                {
                    if (bestMove.moveValue < result[i].moveValue)
                    {
                        bestMove = result[i];
                    }
                }
                board.MoveBrick(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
            }
            else
            {
                //Can't move 
            }
        }
        private List<Move> FindAvailableMoves()
        {
            List<Tuple<sbyte, sbyte, sbyte>> bricks = board.GetBricks(playerNumber);
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

                        sbyte result = board.TryMakeMove(playerNumber, bricks[i].Item2, bricks[i].Item3, x2, y2);
                        if (result != -1)
                        {
                            Move curnMove = new Move() { fromX = bricks[i].Item2, fromY = bricks[i].Item3, toX = x2, toY = y2, moveValue = (sbyte)(result + rng.Next(5)) };
                            if (!moves.Contains(curnMove))
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
                            if (!moves.Contains(curnMove))
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
