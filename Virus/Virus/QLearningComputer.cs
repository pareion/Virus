using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    public class QLearningComputer : VirusPlayer
    {
        private Board board;
        private int playerNumber;
        private float gamma;
        private float[,] Q;
        private Index[,] R;
        private int Rsize, Qsize;
        private Random random;
        public QLearningComputer(Board board, int playerNumber)
        {
            this.board = board;
            this.playerNumber = playerNumber;
            gamma = 0.8F;
            Qsize = 0;
            Rsize = 0;
            Q = null;
            R = null;
            random = new Random();
        }
        public void play()
        {
            QLearning(board);
        }

        private void QLearning(Board board)
        {
            //Build state / action matrix by finding all moves for each piece
            List<Move> moves = board.FindAvailableMoves(playerNumber);
            if (moves.Count > 0)
            {
                R = new Index[16,16];
                List<int> x = new List<int>();
                List<int> y = new List<int>();
                Rsize = 0;
                foreach (var item in moves)
                {
                    if (!x.Contains(item.fromX) && !y.Contains(item.fromY))
                    {
                        Rsize++;
                        x.Add(item.fromX);
                        y.Add(item.fromY);
                    }
                }
                foreach (Move move in moves)
                {
                    Index index = new Index() { State = new State() { X = move.fromX, Y = move.fromY } };
                    for (int i = 0; i < Rsize; i++)
                    {
                        if (R[i, 0] != null)
                        {
                            if (R[i, 0].State.X == index.State.X && R[i, 0].State.Y == index.State.Y)
                            {
                                bool toBig = true;
                                for (int a = 0; a < R.Length; a++)
                                {
                                    if (R[i, a] == null)
                                    {
                                        R[i, a] = new Index() { State = R[i, 0].State, Action = new Action { moveX = move.toX, moveY = move.toY, value = move.moveValue } };
                                        toBig = false;
                                        break;
                                    }
                                }
                                if (!toBig)
                                {
                                    Index[,] R2 = new Index[R.Length, R.Length + 1];
                                    R2 = R;
                                }
                            }
                        }
                        else
                        {
                            R[i, 0] = index;
                            R[i, 0].Action = new Action() { moveX = move.toX, moveY = move.toY, value = move.moveValue };
                        }
                    }
                }
                ComputeMove();
            }
        }

        private void ComputeMove()
        {
            if (Qsize == 0)
            {
                Q = new float[16, 16];
                Qsize = Rsize;
            }
            
            int count = 0;
            for (int i = 0; i < Rsize; i++)
            {
                if (R[i, 0] != null)
                {
                    count++;
                }
            }
            int Rindex = random.Next(count);

            int biggestValue = 0;
            int biggestIndex = 0;
            for (int i = 0; i < Rsize; i++)
            {
                if (R[Rindex, i] != null)
                {
                    if (R[Rindex, i].Action.value > biggestValue)
                    {
                        biggestValue = R[Rindex, i].Action.value;
                        biggestIndex = i;
                    }
                }
            }
            if (Q[Rindex, biggestIndex] == 0)
            {
                Q[Rindex, biggestIndex] = (float)R[Rindex, biggestIndex].Action.value + gamma * (float)biggestValue;
            }
           
            board.MoveBrick(R[Rindex, biggestIndex].State.X, R[Rindex, biggestIndex].State.Y, R[Rindex, biggestIndex].Action.moveX, R[Rindex, biggestIndex].Action.moveY);
            Console.WriteLine("Matrix for R");
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    if (R[x,y] != null)
                    {
                        Console.Write(R[x, y].Action.value + " ");
                    }else
                        Console.Write(0+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Matrix for Q");
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    Console.Write(Q[x, y] + " ");
                }
                Console.WriteLine();
            }
        }

        private class Index
        {
            public State State;
            public Action Action;
        }
        private class State
        {
            public int X;
            public int Y;
        }
        private class Action
        {
            public int moveX;
            public int moveY;
            public int value;
        }
    }
}
