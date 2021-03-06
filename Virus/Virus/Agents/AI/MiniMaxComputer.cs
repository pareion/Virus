﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Virus
{
    class MiniMaxComputer : VirusPlayer
    {
        Board board;
        private int counter, maxcounter;
        int playerNumber;
        Node root;
        List<Node> pointsVisistedBFS;
        private bool pruning;

        public MiniMaxComputer(Board board, int playerNumber, int depth, bool pruning)
        {
            counter = 0;
            maxcounter = depth;
            this.board = board;
            this.playerNumber = playerNumber;
            this.pruning = pruning;
        }
        public void play()
        {
            MiniMax(board);
        }
        private void BFS(Node start)
        {
            Queue queue = new Queue();
            pointsVisistedBFS = new List<Node>();
            pointsVisistedBFS.Add(start);
            queue.Enqueue(start);

            int cc = -1;

            while (queue.Count != 0)
            {
                Node visiting = (Node)queue.Dequeue();
        
                for (int i = 0; i < visiting.children.Count; i++)
                {
                    Node child = visiting.children[i];

                    cc++;
                    child.id = cc;

                    queue.Enqueue(child);
                }
            }
        }
        private void MiniMax(Board board)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            try
            {
                Move bestMove = null;
                int bestScorer = 9999;
                int minscore;

                List<Move> moves = board.FindAvailableMoves(playerNumber);
                root = new Node();
                if (moves != null)
                {
                    Board tempBoard;
                    for (int b = 0; b < moves.Count; b++)
                    {
                        tempBoard = board.Copy();
                        Node tmp = new Node();
                        counter++;
                        tempBoard.IsMoveEligable(moves[b].fromX, moves[b].fromY, moves[b].toX, moves[b].toY);
                        int a = tempBoard.MakeMove(moves[b].fromX, moves[b].fromY, moves[b].toX, moves[b].toY);
                        minscore = MIN(tmp, tempBoard, alpha, beta);
                        tmp.value = minscore;
                        root.children.Add(tmp);
                        if (minscore < bestScorer)
                        {
                            bestMove = moves[b];
                            bestScorer = minscore;
                        }
                        counter--;
                    }
                }
                if (bestMove == null)
                {
                    bestMove = moves[0];
                    board.MoveBrick(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
                }
                else
                {
                    board.MoveBrick(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);
                }
            }
            catch (Exception)
            {

            }

        }

        private void WriteToDatabase()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            BFS(root);
            watch.Stop();
            Console.WriteLine("Time elapsed while writing to the database: " + watch.Elapsed.TotalSeconds);
        }

        private int MIN(Node tmp, Board tempBoard, int alpha, int beta)
        {
            if (GameEnded(tempBoard))
            {
                return EvalEnding(tempBoard);
            }
            else if (maxDepth())
                return EVAL(tempBoard);
            else
            {
                int bestScore = 999;
                List<Move> moves;
                if (playerNumber == 1)
                {
                    moves = tempBoard.FindAvailableMoves(2);
                }
                else
                {
                    moves = tempBoard.FindAvailableMoves(1);
                }

                Board previousBoard = tempBoard.Copy();
                for (int i = 0; i < moves.Count; i++)
                {
                    Node temp = new Node();

                    tempBoard.IsMoveEligable(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    tempBoard.MakeMove(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    counter++;
                    int score = MAX(temp, tempBoard, alpha, beta);
                    counter--;

                    temp.value = score;
                    tmp.children.Add(temp);

                    if (score < bestScore)
                    {
                        bestScore = score;
                    }
                    tempBoard = previousBoard.Copy();

                    if (pruning)
                    {
                        beta = score;

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
                return bestScore;
            }
        }
        private int MAX(Node tmp, Board tempBoard, int alpha, int beta)
        {
            if (GameEnded(tempBoard))
            {
                return EvalEnding(tempBoard);
            }
            else if (maxDepth())
            {
                return EVAL(tempBoard);
            }
            else
            {
                int bestScore = -999;


                List<Move> moves = tempBoard.FindAvailableMoves(playerNumber);
                Board previousBoard = tempBoard.Copy();
                for (int i = 0; i < moves.Count; i++)
                {

                    Node temp = new Node();
                    tempBoard.IsMoveEligable(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    tempBoard.MakeMove(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    counter++;
                    int score = MIN(temp, tempBoard, alpha, beta);
                    counter--;

                    temp.value = score;
                    tmp.children.Add(temp);

                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                    tempBoard = previousBoard.Copy();

                    if (pruning)
                    {
                        alpha = score;

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }

                return bestScore;
            }
        }

        private bool GameEnded(Board tempBoard)
        {
            int Cpoints = 0;
            int Hpoints = 0;
            for (int x = 0; x < tempBoard.boardSize; x++)
            {
                for (int y = 0; y < tempBoard.boardSize; y++)
                {
                    if (tempBoard.board[x, y] == 2)
                    {
                        Cpoints++;
                    }
                    if (tempBoard.board[x, y] == 1)
                    {
                        Hpoints++;
                    }
                }
            }
            if (Cpoints == 0 || Hpoints == 0)
            {
                return true;
            }
            return false;
        }

        private int EVAL(Board tempBoard)
        {
            int points = 0;
            for (int x = 0; x < tempBoard.boardSize; x++)
            {
                for (int y = 0; y < tempBoard.boardSize; y++)
                {
                    if (playerNumber == 1)
                    {
                        if (tempBoard.board[x, y] == 1)
                        {
                            points--;
                        }
                        else if (tempBoard.board[x, y] == 2)
                        {
                            points++;
                        }
                    }
                    else
                    {
                        if (tempBoard.board[x, y] == 1)
                        {
                            points++;
                        }
                        else if (tempBoard.board[x, y] == 2)
                        {
                            points--;
                        }
                    }
                }
            }
            return points;
        }
        private bool maxDepth()
        {
            if (counter >= maxcounter)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private int EvalEnding(Board tempBoard)
        {
            int humanPoints = 0;
            int computerPoints = 0;
            for (int x = 0; x < tempBoard.boardSize; x++)
            {
                for (int y = 0; y < tempBoard.boardSize; y++)
                {
                    if (playerNumber == 1)
                    {
                        if (tempBoard.board[x, y] == 1)
                        {
                            humanPoints++;
                        }
                        else if (tempBoard.board[x, y] == 2)
                        {
                            computerPoints++;
                        }
                    }
                    else
                    {
                        if (tempBoard.board[x, y] == 2)
                        {
                            humanPoints++;
                        }
                        else if (tempBoard.board[x, y] == 1)
                        {
                            computerPoints++;
                        }
                    }
                }
            }
            if (humanPoints > computerPoints)
            {
                return -9999;
            }
            else if (computerPoints > humanPoints)
            {
                return 9999;
            }

            return 0;
        }
        Tuple<Board,Move> previous;
        public Tuple<Board, Move> PredictMiniMaxMove(Board tempboard)
        {
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            Move bestMove = null;
            if (previous == null || previous.Item1.playerTurn != tempboard.playerTurn)
            {
                try
                {
                    int bestScorer = 9999;
                    int minscore;

                    List<Move> moves = tempboard.FindAvailableMoves(playerNumber);
                    root = new Node();
                    if (moves != null)
                    {
                        Board tempBoard;
                        for (int b = 0; b < moves.Count; b++)
                        {
                            tempBoard = tempboard.Copy();
                            Node tmp = new Node();
                            counter++;
                            tempBoard.IsMoveEligable(moves[b].fromX, moves[b].fromY, moves[b].toX, moves[b].toY);
                            tempBoard.MakeMove(moves[b].fromX, moves[b].fromY, moves[b].toX, moves[b].toY);
                            minscore = MIN(tmp, tempBoard, alpha, beta);
                            tmp.value = minscore;
                            root.children.Add(tmp);
                            if (minscore < bestScorer)
                            {
                                bestMove = moves[b];
                                bestScorer = minscore;
                            }
                            counter--;
                        }
                    }

                    if (bestMove == null)
                    {
                        bestMove = moves[0];
                    }
                }
                catch (Exception)
                {

                }

                tempboard.MoveBrick(bestMove.fromX, bestMove.fromY, bestMove.toX, bestMove.toY);

                previous = new Tuple<Board, Move>(tempboard, bestMove);
            }

            return previous;
        }

        public void AfterGame()
        {
        }

        private class Node
        {
            public List<Node> children = new List<Node>();
            public int value = 0;
            public int id = -1;
            public override string ToString()
            {
                return value.ToString();
            }
        }
    }
}
