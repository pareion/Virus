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
        int playerNumber;
        Random rng = new Random();
        public SemiSmartComputer(Board board, int playerNumber)
        {
            this.board = board;
            this.playerNumber = playerNumber;
        }
        public void play()
        {
            List<Move> result = board.FindAvailableMoves(playerNumber);
            if (result.Count > 0)
            {
                Move bestMove = result[0];
                for (int i = 1; i < result.Count(); i++)
                {
                    if (bestMove.moveValue < (result[i].moveValue + rng.Next(3)))
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
        
    }
}
