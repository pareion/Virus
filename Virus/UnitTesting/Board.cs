using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class Board
    {
        public int[,] board;
        public int boardSize { get; set; }

        public int playerTurn;
        public bool jumping = false;
        private VirusPlayer player1, player2;
        public Board(int size)
        {
            board = new int[size, size];
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
            board[0, 0] = 1;
            board[boardSize - 1, boardSize - 1] = 1;
            board[boardSize - 1, 0] = 2;
            board[0, boardSize - 1] = 2;
            playerTurn = 1;
        }
        public bool IsDone()
        {
            if (GetBricks().Count == boardSize * boardSize)
                return true;
            return false;
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
            Console.Clear();
            for (int a = 0; a < boardSize; a++)
            {
                for (int i = 0; i < boardSize; i++)
                {
                    Console.Write(board[a, i] + " ");
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
        public virtual int MoveBrick(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
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

        private bool CantMove()
        {
            List<Tuple<int, int, int, int>> moves = new List<Tuple<int, int, int, int>>();
            foreach (var item in GetBricks(playerTurn))
            {

                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int x2 = -1;
                        int y2 = -1;
                        if (x > item.Item2)
                            x2 = (int)(x - item.Item2);
                        else
                            x2 = (int)(item.Item2 - x);
                        if (y > item.Item3)
                            y2 = (int)(y - item.Item3);
                        else
                            y2 = (int)(item.Item3 - y);

                        int result = TryMakeMove(playerTurn, item.Item2, item.Item3, x2, y2);
                        if (result != -1)
                        {
                            if (!moves.Contains(new Tuple<int, int, int, int>(item.Item2, item.Item3, x2, y2)))
                            {
                                moves.Add(new Tuple<int, int, int, int>(item.Item2, item.Item3, x2, y2));
                            }
                        }
                    }
                }
            }
            if (moves.Count > 0)
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
                        result.Add(new Tuple<int, int, int>(board[x, y], (int)x, (int)y));
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
                    if (board[x, y] != 0 && board[x, y] == playerNumber)
                    {
                        result.Add(new Tuple<int, int, int>((int)playerNumber, (int)x, (int)y));
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
            if (!IsAPiece(playerNumber, brickToMoveX, brickToMoveY))
                return -1;
            if (!IsNotOccupied(moveToHereX, moveToHereY))
                return -1;
            if (!IsMoveEligable(brickToMoveX, brickToMoveY, moveToHereX, moveToHereY))
                return -1;
            if (!IsOnTheBoard(moveToHereX, moveToHereY))
                return -1;

            int player = board[brickToMoveX, brickToMoveY];
            int taken = 0;

            //Jump move
            if (jumping)
            {
                taken += AssumeCapturedPieces(moveToHereX, moveToHereY, player);
                
                return taken;
            }
            else
            {
                //Normal move
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

        private int MakeMove(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
        {
            int player = board[brickToMoveX, brickToMoveY];

            int taken = 0;

            //Jump move
            if (jumping)
            {
                taken += CapturePieces(moveToHereX, moveToHereY, player);

                board[brickToMoveX, brickToMoveY] = 0;
            }
            else
            {
                //Normal move
                taken += CapturePieces(moveToHereX, moveToHereY, player);
                
            }
            board[moveToHereX, moveToHereY] = player;
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
                if (board[moveToHereX + 1, moveToHereY] != 0 && board[moveToHereX, moveToHereY] != player)
                {
                    taken++;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (board[moveToHereX - 1, moveToHereY] != 0 && board[moveToHereX, moveToHereY] != player)
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
                if (board[moveToHereX + 1, moveToHereY] != 0 && board[moveToHereX, moveToHereY] != player)
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
                if (board[moveToHereX - 1, moveToHereY] != 0 && board[moveToHereX, moveToHereY] != player)
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

        private bool IsMoveEligable(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
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
            for (int i = -1; i <= 1; i++)
            {

                if (i == -1)
                {
                    try
                    {
                        if (board[moveToHereX - i, moveToHereY - 1] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        if (board[moveToHereX - i, moveToHereY] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        if (board[moveToHereX - i, moveToHereY + 1] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
                if (i == 0)
                {
                    try
                    {
                        if (board[moveToHereX, moveToHereY - 1] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        if (board[moveToHereX, moveToHereY + 1] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                if (i == 1)
                {
                    try
                    {
                        if (board[moveToHereX - i, moveToHereY - 1] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        if (board[moveToHereX - i, moveToHereY] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        if (board[moveToHereX - i, moveToHereY + 1] != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return false;
        }

        private bool IsJumpEligable(int brickToMoveX, int brickToMoveY, int moveToHereX, int moveToHereY)
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
            var serialized = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<Board>(serialized);
        }
    }
}
