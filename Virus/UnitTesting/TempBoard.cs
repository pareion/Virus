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
            board[6, 3] = 1;
            board[5, 5] = 2;
            board[5, 6] = 2;
            board[5, 7] = 2;
            board[7, 5] = 2;
            board[7, 6] = 2;
            board[7, 7] = 2;
            board[6, 7] = 2;
        }
        public override sbyte MoveBrick(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (playerTurnsOn == false)
            {                
                return Move(playerTurn,brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
            }
            else
                return base.MoveBrick(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
        }
    }
}
