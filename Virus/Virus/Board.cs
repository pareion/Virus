using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Virus
{
    public class Board
    {
        public int[,] board;
        public int boardSize { get; set; }
        int bricks = 0;
        int lastTime = 0;
        public int playerTurn;
        public bool jumping = false;
        int roundWithoutChange = 0;
        public Board(int size)
        {
            board = new int[size, size];
            boardSize = size;
            StartGame();
        }
        public void StartGame()
        {
            roundWithoutChange = 0;
            board[0, 0] = 1;
            board[boardSize - 1, boardSize - 1] = 1;
            board[boardSize - 1, 0] = 2;
            board[0, boardSize - 1] = 2;
            playerTurn = 1;
        }
        /// <summary>
        /// Returns the number of points for all players starting with player 1 (0 indexed)
        /// </summary>
        /// <returns></returns>
        internal int[] GetScore()
        {
            int[] result = new int[2];
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] == 1)
                    {
                        result[0] += 1;
                    }
                    if (board[x, y] == 2)
                    {
                        result[1] += 1;
                    }
                }
            }
            return result;
        }

        internal void reset()
        {
            board = new int[boardSize, boardSize];
            StartGame();
        }
        public bool IsDone()
        {
            bricks = 0;
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] == 1 || board[x, y] == 2)
                    {
                        bricks++;
                    }
                }
            }
            if (bricks > lastTime)
            {
                lastTime = bricks;
                roundWithoutChange = 0;
            }
            else
            {
                roundWithoutChange++;
            }
            if (GetBricks().Count == boardSize * boardSize || roundWithoutChange > 50)
                return true;
            return false;
        }
        public void Display()
        {
            try
            {
                for (int a = 0; a < boardSize; a++)
                {
                    for (int i = 0; i < boardSize; i++)
                    {
                        Console.Write(board[a, i] + " ");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception)
            {

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
        public virtual int MoveBrick(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            if (board[brickToMoveX, brickToMoveY] != playerTurn)
            {
                return -1;
            }
            int move = Move(playerTurn, brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);

            if (move != -1)
            {
                if (playerTurn == 1)
                {
                    playerTurn = 2;
                }
                else if (playerTurn == 2)
                {
                    playerTurn = 1;
                }
                if (CantMove())
                {
                    if (playerTurn == 1)
                    {
                        playerTurn = 2;
                    }
                    else if (playerTurn == 2)
                    {
                        playerTurn = 1;
                    }
                }
            }
            return move;
        }
        public virtual int Move(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            return Move(playerTurn, brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
        }
        public List<Move> FindAvailableMoves(int playerNumber)
        {
            List<Tuple<int, int, int>> bricks = GetBricks(playerNumber);
            List<Move> moves = new List<Move>();
            for (int i = 0; i < bricks.Count; i++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int x2 = -1;
                        int y2 = -1;
                        if (x > bricks[i].Item2)
                            x2 = (x - bricks[i].Item2);
                        else
                            x2 = (bricks[i].Item2 - x);
                        if (y > bricks[i].Item3)
                            y2 = (y - bricks[i].Item3);
                        else
                            y2 = (bricks[i].Item3 - y);

                        int result = TryMakeMove(bricks[i].Item1, bricks[i].Item2, bricks[i].Item3, x2, y2);
                        if (result != -1)
                        {
                            Move curnMove = new Move() { fromX = bricks[i].Item2, fromY = bricks[i].Item3, toX = x2, toY = y2, moveValue = result };
                            bool exists = false;
                            foreach (var item in moves)
                            {
                                if (item.fromX == curnMove.fromX && item.fromY == curnMove.fromY && item.toX == curnMove.toX && item.toY == curnMove.toY)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
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
                        int x3 = bricks[i].Item2 - x2;
                        int y3 = bricks[i].Item3 - y2;

                        int result = TryMakeMove(bricks[i].Item1, bricks[i].Item2, bricks[i].Item3, x3, y3);
                        if (result != -1)
                        {
                            Move curnMove = new Move() { fromX = bricks[i].Item2, fromY = bricks[i].Item3, toX = x3, toY = y3, moveValue = (result) };
                            bool exists = false;
                            foreach (var item in moves)
                            {
                                if (item.fromX == curnMove.fromX && item.fromY == curnMove.fromY && item.toX == curnMove.toX && item.toY == curnMove.toY)
                                {
                                    exists = true;
                                }
                            }
                            if (!exists)
                            {
                                moves.Add(curnMove);
                            }
                        }
                    }
                }
            }
            return moves;
        }


        private bool CantMove()
        {

            if (FindAvailableMoves(playerTurn).Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// The method returns a list with the player number followed by the index of the brick
        /// </summary>
        /// <returns></returns>
        public List<Tuple<int, int, int>> GetBricks()
        {
            List<Tuple<int, int, int>> result = new List<Tuple<int, int, int>>();
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] != 0)
                    {
                        result.Add(new Tuple<int, int, int>(board[x, y], x, y));
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
        public List<Tuple<int, int, int>> GetBricks(int playerNumber)
        {
            List<Tuple<int, int, int>> result = new List<Tuple<int, int, int>>();
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (board[x, y] == playerNumber)
                    {
                        result.Add(new Tuple<int, int, int>(playerNumber, x, y));
                    }
                }
            }
            return result;
        }
        protected int Move(int playerNumber, int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            if (IsOnTheBoard(moveToHereX, moveToHereY))
                if (IsAPiece(playerNumber, brickToMoveX, brickToMoveY))
                    if (IsNotOccupied(moveToHereX, moveToHereY))
                        if (IsMoveEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                            return MakeMove(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY);
            return -1;
        }

        private bool IsAPiece(int playerNumber, int brickToMoveX, int brickToMoveY)
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
        public int TryMakeMove(int playerNumber, int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            int player = playerNumber;
            if (!IsAPiece(playerNumber, brickToMoveX, brickToMoveY))
                return -1;
            if (!IsNotOccupied(moveToHereX, moveToHereY))
                return -1;
            if (!IsMoveEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                return -1;
            if (!IsOnTheBoard(moveToHereX, moveToHereY))
                return -1;

            int taken = 0;

            if (jumping)
            {
                taken += AssumeCapturedPieces(moveToHereX, moveToHereY, player);

                return taken;
            }
            else
            {
                taken += AssumeCapturedPieces(moveToHereX, moveToHereY, player);
            }
            return taken;
        }

        private bool IsOnTheBoard(int moveToHereX, int moveToHereY)
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

        public int MakeMove(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            int player = board[brickToMoveX, brickToMoveY];

            int taken = 0;
            
            if (jumping)
            {
                taken += CapturePieces(moveToHereX, moveToHereY, player);

                board[brickToMoveX, brickToMoveY] = 0;
                board[moveToHereX, moveToHereY] = player;
                return taken;
            }
            else
            {
                taken += CapturePieces(moveToHereX, moveToHereY, player);
                taken += 1;
                board[moveToHereX, moveToHereY] = player;

            }

            return taken;
        }
        private int AssumeCapturedPieces(int moveToHereX, int moveToHereY, int player)
        {
            int taken = 0;
            try
            {
                if (board[moveToHereX - 1, moveToHereY - 1] != 0 && board[moveToHereX - 1, moveToHereY - 1] != player)
                {
                    taken++;
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX, moveToHereY - 1] != 0 && board[moveToHereX, moveToHereY - 1] != player)
                {
                    taken++;
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX + 1, moveToHereY - 1] != 0 && board[moveToHereX + 1, moveToHereY - 1] != player)
                {
                    taken++;
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX + 1, moveToHereY] != 0 && board[moveToHereX + 1, moveToHereY] != player)
                {
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX - 1, moveToHereY] != 0 && board[moveToHereX - 1, moveToHereY] != player)
                {
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX + 1, moveToHereY + 1] != 0 && board[moveToHereX + 1, moveToHereY + 1] != player)
                {
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX, moveToHereY + 1] != 0 && board[moveToHereX, moveToHereY + 1] != player)
                {
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX - 1, moveToHereY + 1] != 0 && board[moveToHereX - 1, moveToHereY + 1] != player)
                {
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            return taken;
        }
        private int CapturePieces(int moveToHereX, int moveToHereY, int player)
        {
            int taken = 0;
            try
            {
                if (board[moveToHereX - 1, moveToHereY - 1] != 0 && board[moveToHereX - 1, moveToHereY - 1] != player)
                {
                    board[moveToHereX - 1, moveToHereY - 1] = player;
                    taken++;
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX, moveToHereY - 1] != 0 && board[moveToHereX, moveToHereY - 1] != player)
                {
                    board[moveToHereX, moveToHereY - 1] = player;
                    taken++;
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX + 1, moveToHereY - 1] != 0 && board[moveToHereX + 1, moveToHereY - 1] != player)
                {
                    board[moveToHereX + 1, moveToHereY - 1] = player;
                    taken++;
                }

            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX + 1, moveToHereY] != 0 && board[moveToHereX + 1, moveToHereY] != player)
                {
                    board[moveToHereX + 1, moveToHereY] = player;
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX - 1, moveToHereY] != 0 && board[moveToHereX - 1, moveToHereY] != player)
                {
                    board[moveToHereX - 1, moveToHereY] = player;
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX + 1, moveToHereY + 1] != 0 && board[moveToHereX + 1, moveToHereY + 1] != player)
                {
                    board[moveToHereX + 1, moveToHereY + 1] = player;
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX, moveToHereY + 1] != 0 && board[moveToHereX, moveToHereY + 1] != player)
                {
                    board[moveToHereX, moveToHereY + 1] = player;
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX - 1, moveToHereY + 1] != 0 && board[moveToHereX - 1, moveToHereY + 1] != player)
                {
                    board[moveToHereX - 1, moveToHereY + 1] = player;
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            return taken;
        }

        public bool IsMoveEligable(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
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
            return false;
        }

        private bool IsNormalMoveEligable(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            if (brickToMoveX - moveToHereX == 1 || -1 == brickToMoveX - moveToHereX || 0 == brickToMoveX - moveToHereX && brickToMoveY - moveToHereY == 0 || brickToMoveY - moveToHereY == 1 || brickToMoveY - moveToHereY == -1)
            {
                return true;
            }
            return false;
        }

        private bool IsJumpEligable(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            if (brickToMoveX - moveToHereX == 2 || -2 == brickToMoveX - moveToHereX || 0 == brickToMoveX - moveToHereX && brickToMoveY - moveToHereY == 0 || brickToMoveY - moveToHereY == 2 || brickToMoveY - moveToHereY == -2)
            {
                return true;
            }
            return false;
        }

        private bool IsNotOccupied(int moveToHereX, int moveToHereY)
        {
            try
            {
                if (board[moveToHereX, moveToHereY] == 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public Board Copy()
        {
            Board board = new Board(boardSize);
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    board.board[x, y] = this.board[x, y];
                }
            }
            return board;
        }
        public Board GetNewBoard(Board board, Move move)
        {
            Board temp = board.Copy();
            temp.MakeMove(move.fromX, move.fromY, move.toX, move.toY);
            return temp;
        }
    }
}
