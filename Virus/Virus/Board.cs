﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    public class Board
    {
        protected sbyte[,] board;
        public int boardSize { get; private set; }

        protected sbyte playerTurn;
        private bool jumping = false;
        private VirusPlayer player1, player2, currentPlayer;
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
            currentPlayer = player1;
        }
        public void SetPlayer1(VirusPlayer player)
        {
            player1 = player;
        }
        public void SetPlayer2(VirusPlayer player)
        {
            player2 = player;
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
        public virtual sbyte MoveBrick(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            sbyte move = Move(playerTurn, brickToMoveX, brickToMoveY, moveToHereX, moveToHereY); ;
            if (move != -1)
            {
                if (playerTurn == 1)
                {
                    playerTurn = 2;
                    currentPlayer = player2;
                }
                else if (playerTurn == 2)
                {
                    playerTurn = 1;
                    currentPlayer = player1;
                }
            }
            return move;
        }

        /// <summary>
        /// The method returns a list with the player number followed by the index of the brick
        /// </summary>
        /// <returns></returns>
        public List<Tuple<sbyte, sbyte, sbyte>> GetBricks()
        {
            List<Tuple<sbyte, sbyte, sbyte>> result = new List<Tuple<sbyte, sbyte, sbyte>>();
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] != 0)
                    {
                        result.Add(new Tuple<sbyte, sbyte, sbyte>(board[x, y], (sbyte)x, (sbyte)y));
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// The method returns a list with the player number followed by the index of the brick, but only for the bricks corresponding
        /// to the playerNumber
        /// </summary>
        /// <returns></returns>
        public List<Tuple<sbyte, sbyte, sbyte>> GetBricks(int playerNumber)
        {
            List<Tuple<sbyte, sbyte, sbyte>> result = new List<Tuple<sbyte, sbyte, sbyte>>();
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] != 0 && board[x, y] == playerNumber)
                    {
                        result.Add(new Tuple<sbyte, sbyte, sbyte>(board[x, y], (sbyte)x, (sbyte)y));
                    }
                }
            }
            return result;
        }
        protected sbyte Move(sbyte playerNumber, sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (IsOnTheBoard(moveToHereX, moveToHereY))
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
        /// <summary>
        /// Returns the number of bricks captured or -1 if move is not possible
        /// Though it doesn't make the move
        /// </summary>
        /// <param name="brickToMoveX"></param>
        /// <param name="brickToMoveY"></param>
        /// <param name="moveToHereX"></param>
        /// <param name="moveToHereY"></param>
        /// <returns></returns>
        public sbyte TryMakeMove(sbyte playerNumber, sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            if (!IsAPiece(playerNumber, brickToMoveX, brickToMoveY))
                return -1;
            if (IsOccupied(moveToHereX, moveToHereY))
                return -1;
            if (!IsMoveEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                return -1;
            if (!IsOnTheBoard(moveToHereX, moveToHereY))
                return -1;

            sbyte player = board[brickToMoveX, brickToMoveY];
            sbyte taken = 0;

            //Jump move
            if (jumping)
            {
                //TODO if it "touches" other players they become you...
                taken++;
                return taken;
            }
            else
            {
                //Normal move
                for (int i = -1; i <= 1; i++)
                {
                    try
                    {
                        if (i == -1)
                        {
                            if (board[moveToHereX - i, moveToHereY - 1] != 0 && board[moveToHereX - i, moveToHereY - 1] != player)
                            {
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY] != 0 && board[moveToHereX - i, moveToHereY] != player)
                            {
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY + 1] != 0 && board[moveToHereX - i, moveToHereY + 1] != player)
                            {
                                taken++;
                            }
                        }
                        if (i == 0)
                        {
                            if (board[moveToHereX, moveToHereY - 1] != 0 && board[moveToHereX, moveToHereY - 1] != player)
                            {
                                taken++;
                            }
                            if (board[moveToHereX, moveToHereY + 1] != 0 && board[moveToHereX, moveToHereY + 1] != player)
                            {
                                taken++;
                            }
                        }
                        if (i == 1)
                        {
                            if (board[moveToHereX - i, moveToHereY - 1] != 0 && board[moveToHereX - i, moveToHereY - 1] != player)
                            {
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY] != 0 && board[moveToHereX - i, moveToHereY] != player)
                            {
                                taken++;
                            }
                            if (board[moveToHereX - i, moveToHereY + 1] != 0 && board[moveToHereX - i, moveToHereY + 1] != player)
                            {
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

        private bool IsOnTheBoard(sbyte moveToHereX, sbyte moveToHereY)
        {
            try
            {
                if (board[moveToHereX, moveToHereY] == -1)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private sbyte MakeMove(sbyte brickToMoveX, sbyte brickToMoveY, sbyte moveToHereX, sbyte moveToHereY)
        {
            sbyte player = board[brickToMoveX, brickToMoveY];
            board[moveToHereX, moveToHereY] = player;
            sbyte taken = 0;

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
                    try
                    {
                        if (i == -1)
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
            try
            {
                if (board[moveToHereX, moveToHereY] != 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
