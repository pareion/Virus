using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
            board[6, 4] = 1;
            board[6, 5] = 2;
            board[5, 5] = 2;
            board[5, 6] = 2;
            board[5, 7] = 2;
            board[7, 5] = 2;
            board[7, 6] = 2;
            board[7, 7] = 2;
            board[6, 7] = 2;
        }
        public override int MoveBrick(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
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
