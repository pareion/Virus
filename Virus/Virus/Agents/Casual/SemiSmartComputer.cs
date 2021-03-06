﻿using System;
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

        public void AfterGame()
        {
        }

        public void play()
        {
            List<Move> result = board.FindAvailableMoves(playerNumber);
            if (result.Count > 0)
            {
                Move bestMove = result[0];
                for (int i = 0; i < result.Count(); i++)
                {
                    if (bestMove.moveValue < (result[i].moveValue))
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
