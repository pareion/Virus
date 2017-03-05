using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Virus;

namespace UnitTesting
{
    public class TempBoard : Board
    {
        public bool playerTurnsOn = false;
        public TempBoard(int size) : base(size)
        {
            board[3, 3] = 1;
        }
        public void SetupBoardForCapture()
        {
            board[3, 0] = 1;
            board[2, 2] = 2;
            board[2, 3] = 2;
            board[2, 4] = 2;
            board[4, 2] = 2;
            board[4, 3] = 2;
            board[4, 4] = 2;
            board[3, 4] = 2;
        }
        public override sbyte MoveBrick(sbyte playerNumber, sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (playerTurnsOn == false)
            {
                if (playerNumber == playerTurn)
                {
                    return Move(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
                }
                else
                    return -1;
            }
            else
                return base.MoveBrick(playerNumber,brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
        }
    }
}
