using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    public class Board
    {
        protected sbyte[,] board;
        protected int boardSize;
        protected sbyte playerTurn;
        private bool jumping = false;
        public Board(int size)
        {
            board = new sbyte[size, size];
            boardSize = size;
        }

        public void StartGame()
        {
            board[0, 0] = 1;
            board[boardSize - 1, boardSize - 1] = 1;
            board[boardSize - 1, 0] = 2;
            board[0, boardSize - 1] = 2;
            playerTurn = 1;
        }
        public void Display()
        {
            for (int a = 0; a < boardSize; a++)
            {
                for (int i = 0; i < boardSize; i++)
                {
                    Console.Write(board[a, i]);
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Returns -1 if it's not your turn or the move is not eligable
        /// if succesfull returns the number of spaces you have captured
        /// </summary>
        /// <param name="playerNumber"></param>
        /// <param name="brickToMove"></param>
        /// <param name="moveToHere"></param>
        /// <returns></returns>
        public virtual sbyte MoveBrick(sbyte playerNumber, sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                    playerTurn = 2;
                else if (playerTurn == 2)
                    playerTurn = 1;
                return Move(playerNumber, brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
            }
            else
                return -1;
        }

        protected sbyte Move(sbyte playerNumber, sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (IsAPiece(playerNumber, brickToMoveX, brickToMoveY))
                if (!IsOccupied(moveToHereX, moveToHereY))
                    if (IsMoveEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                        return MakeMove(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
            return -1;
        }

        private bool IsAPiece(sbyte playerNumber, sbyte brickToMoveX, sbyte brickToMoveY)
        {
            if (board[brickToMoveX, brickToMoveY] == playerNumber)
                return true;
            return false;
        }

        private sbyte MakeMove(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            sbyte player = board[brickToMoveX, brickToMoveY];
            board[moveToHereX, moveToHereY] = player;
            sbyte taken = 1;

            //Jump move
            if (jumping)
            {
                board[brickToMoveX, brickToMoveY] = 0;
                return taken;
            }
            else
            {
                //Normal move
                for (int i = -1; i <= 1; i++)
                {
                    //Can improve so it doesn't check the 3 squares if you are placing a brick on the boundary
                    try
                    {
                        if (i == -1)
                        {
                            if (board[moveToHereX - i,moveToHereY - 1] != 0 && board[moveToHereX - i, moveToHereY - 1] != player)
                            {
                                board[moveToHereX - i, moveToHereY - 1] = player;
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY] != 0 && board[moveToHereX - i, moveToHereY] != player)
                            {
                                board[moveToHereX - i, moveToHereY] = player;
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY + 1] != 0 && board[moveToHereX - i, moveToHereY + 1] != player)
                            {
                                board[moveToHereX - i, moveToHereY + 1] = player;
                                taken++;
                            }
                        }
                        if (i == 0)
                        {
                            if (board[moveToHereX, moveToHereY - 1] != 0 && board[moveToHereX, moveToHereY - 1] != player)
                            {
                                board[moveToHereX, moveToHereY - 1] = player;
                                taken++;
                            }
                            if (board[moveToHereX, moveToHereY + 1] != 0 && board[moveToHereX, moveToHereY + 1] != player)
                            {
                                board[moveToHereX, moveToHereY + 1] = player;
                                taken++;
                            }
                        }
                        if (i == 1)
                        {
                            if (board[moveToHereX - i, moveToHereY - 1] != 0 && board[moveToHereX - i, moveToHereY - 1] != player)
                            {
                                board[moveToHereX - i, moveToHereY - 1] = player;
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY] != 0 && board[moveToHereX - i, moveToHereY] != player)
                            {
                                board[moveToHereX - i, moveToHereY] = player;
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY + 1] != 0 && board[moveToHereX - i, moveToHereY + 1] != player)
                            {
                                board[moveToHereX - i, moveToHereY + 1] = player;
                                taken++;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return taken;
        }

        private bool IsMoveEligable(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (brickToMoveX - moveToHereX < 3 && brickToMoveX - moveToHereX > -3 && brickToMoveY - moveToHereY < 3 && brickToMoveY - moveToHereY > -3)
            {
                if (IsJumpEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                {
                    jumping = true;
                    return jumping;
                }
                else if (IsNormalMoveEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                {
                    jumping = false;
                    return true;
                }
            }
            return false;
        }

        private bool IsNormalMoveEligable(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (brickToMoveX - moveToHereX == 1 && brickToMoveY - moveToHereY != 1 || brickToMoveX - moveToHereX == 1 && brickToMoveY - moveToHereY != -1)
                return true;
            else if (brickToMoveX - moveToHereX == -1 && brickToMoveY - moveToHereY != 1 || brickToMoveX - moveToHereX == -1 && brickToMoveY - moveToHereY != -1)
                return true;
            else if (brickToMoveY - moveToHereY == 1 && brickToMoveX - moveToHereX != 1 || brickToMoveY - moveToHereY == 1 && brickToMoveX - moveToHereX != -1)
                return true;
            else if (brickToMoveY - moveToHereY == -1 && brickToMoveX - moveToHereX != 1 || brickToMoveY - moveToHereY == -1 && brickToMoveX - moveToHereX != -1)
                return true;
            return false;
        }

        private bool IsJumpEligable(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (brickToMoveX - moveToHereX == 0 && brickToMoveY - moveToHereY == 2 || brickToMoveX - moveToHereX == 0 && brickToMoveY - moveToHereY == -2)
                return true;
            else if (brickToMoveX - moveToHereX == 2 && brickToMoveY - moveToHereY == 2 || brickToMoveX - moveToHereX == 2 && brickToMoveY - moveToHereY == -2)
                return true;
            else if (brickToMoveX - moveToHereX == -2 && brickToMoveY - moveToHereY == 2 || brickToMoveX - moveToHereX == -2 && brickToMoveY - moveToHereY == -2)
                return true;
            else if (brickToMoveX - moveToHereX == 2 && brickToMoveY - moveToHereY == 0 || brickToMoveX - moveToHereX == -2 && brickToMoveY - moveToHereY == 0)
                return true;
            return false;
        }

        private bool IsOccupied(sbyte moveToHereX, sbyte moveToHereY)
        {
            if (board[moveToHereX, moveToHereY] != 0)
            {
                return true;
            }
            return false;
        }
    }
}
