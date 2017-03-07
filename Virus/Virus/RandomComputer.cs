using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    public class RandomComputer : VirusPlayer
    {
        Board board;
        int playerNumber;
        Random rng = new Random();
        public RandomComputer(Board board, int playerNumber)
        {
            this.board = board;
            this.playerNumber = playerNumber;
        }

        public void play()
        {
            List<Tuple<int, int, int, int>> result = FindAvailableMoves();
            if (result.Count > 0)
            {
                int a = rng.Next(result.Count - 1);
                board.MoveBrick(result[a].Item1, result[a].Item2, result[a].Item3, result[a].Item4);
            }
            else
            {
                //Can't move
            }
        }

        private List<Tuple<int, int, int, int>> FindAvailableMoves()
        {
            List<Tuple<int, int, int>> bricks = board.GetBricks(playerNumber);
            List<Tuple<int, int, int, int>> moves = new List<Tuple<int, int, int, int>>();
            for (int i = 0; i < bricks.Count; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int x2 = -1;
                        int y2 = -1;
                        if (x > bricks[i].Item2)
                            x2 = (int)(x - bricks[i].Item2);
                        else
                            x2 = (int)(bricks[i].Item2 - x);
                        if (y > bricks[i].Item3)
                            y2 = (int)(y - bricks[i].Item3);
                        else
                            y2 = (int)(bricks[i].Item3 - y);

                        int result = board.TryMakeMove(playerNumber, bricks[i].Item2, bricks[i].Item3, x2, y2);
                        if (result != -1)
                        {
                            if (!moves.Contains(new Tuple<int, int, int, int>(bricks[i].Item2, bricks[i].Item3, x2, y2)))
                            {
                                moves.Add(new Tuple<int, int, int, int>(bricks[i].Item2, bricks[i].Item3, x2, y2));
                            }
                        }
                    }
                }
                for (int x2 = -2; x2 < 3; x2 = x2 + 2)
                {
                    for (int y2 = -2; y2 < 3; y2 = y2 + 2)
                    {
                        int x3 = -1;
                        int y3 = -1;
                        if (x2 > bricks[i].Item2)
                            x3 = (x2 - bricks[i].Item2);
                        else
                            x3 = (bricks[i].Item2 - x2);
                        if (y2 > bricks[i].Item3)
                            y3 = (y2 - bricks[i].Item3);
                        else
                            y3 = (bricks[i].Item3 - y2);

                        int result = board.TryMakeMove(playerNumber, bricks[i].Item2, bricks[i].Item3, x3, y3);
                        if (result != -1)
                        {
                            if (!moves.Contains(new Tuple<int, int, int, int>(bricks[i].Item2, bricks[i].Item3, x3, y3)))
                            {
                                moves.Add(new Tuple<int, int, int, int>(bricks[i].Item2, bricks[i].Item3, x3, y3));
                            }
                        }
                    }
                }
            }
            return moves;
        }
    }
}
