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
        sbyte playerNumber;
        Random rng = new Random();
        public RandomComputer(ref Board board, sbyte playerNumber)
        {
            this.board = board;
            this.playerNumber = playerNumber;
        }

        public void play()
        {
            List<Tuple<sbyte, sbyte, sbyte, sbyte>> result = FindAvailableMoves();
            try
            {
                int a = rng.Next(result.Count - 1);
                board.MoveBrick(result[a].Item1, result[a].Item2, result[a].Item3, result[a].Item4);
            }
            catch (Exception)
            {
                
            }
            
        }

        private List<Tuple<sbyte, sbyte, sbyte, sbyte>> FindAvailableMoves()
        {
            List<Tuple<sbyte, sbyte, sbyte>> bricks = board.GetBricks(playerNumber);
            List<Tuple<sbyte, sbyte, sbyte, sbyte>> moves = new List<Tuple<sbyte, sbyte, sbyte, sbyte>>();
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
                            if (!moves.Contains(new Tuple<sbyte, sbyte, sbyte, sbyte>(bricks[i].Item2, bricks[i].Item3, x2, y2)))
                            {
                                moves.Add(new Tuple<sbyte, sbyte, sbyte, sbyte>(bricks[i].Item2, bricks[i].Item3, x2, y2));
                            }
                        }
                    }
                }
            }
            return moves;
        }
    }
}
