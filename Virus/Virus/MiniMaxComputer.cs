using Neo4jClient;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Virus
{
    class MiniMaxComputer : VirusPlayer
    {
        Board board;
        private int counter, maxcounter;
        private int alpha = int.MinValue, beta = int.MaxValue;
        bool done = false;
        int playerNumber;
        Node root;
        List<Node> pointsVisistedBFS;
        GraphClient client;
        public MiniMaxComputer(Board board, int playerNumber)
        {
            counter = 0;
            maxcounter = 3;
            this.board = board;
            this.playerNumber = playerNumber;
        }
        public void play()
        {
            /*client = new GraphClient(new Uri("http://localhost:32772/db/data"), "neo4j", "root");
            client.Connect();*/
            MiniMax(board);
        }
        /* private void BFS(Node start)
         {
             Queue queue = new Queue();
             pointsVisistedBFS = new List<Node>();
             pointsVisistedBFS.Add(start);
             queue.Enqueue(start);
             while (queue.Count != 0)
             {
                 Node visiting = (Node)queue.Dequeue();
                 for (int i = 0; i < visiting.children.Count; i++)
                 {
                     try
                     {
                         client.Create<Node>(root, root.children[i] , root.children[i]);
                         queue.Enqueue(visiting.children[i]);
                     }
                     catch (Exception e)
                     {

                     }

                 }
             }
         }*/
        private void MiniMax(Board board)
        {
            try
            {
                Move bestMove = null;
                int bestScorer = 9999;
                int minscore;
                // find bricks you can use
                List<Move> moves = board.FindAvailableMoves(1);
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
                        minscore = MIN(tmp, tempBoard);
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
                Console.WriteLine("Moving "+ bestMove.fromX + " " +bestMove.fromY+ " to: " + bestMove.toX+ " "+ bestMove.toY);
                //BFS(root);
            }
            catch (Exception e)
            {

            }

        }
        private int MIN(Node tmp, Board tempBoard)
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

                /* done = false;
                 done: if (!done)
                 {*/
                List<Move> moves = tempBoard.FindAvailableMoves(2);
                Board previousBoard = tempBoard.Copy();
                for (int i = 0; i < moves.Count; i++)
                {
                    Node temp = new Node();

                    tempBoard.IsMoveEligable(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    tempBoard.MakeMove(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    counter++;
                    int score = MAX(temp, tempBoard);
                    counter--;
                    beta = score;
                    /*if (beta <= alpha)
                    {
                        done = true;
                        goto done;
                    }*/
                    temp.value = score;
                    tmp.children.Add(temp);

                    if (score < bestScore)
                    {
                        bestScore = score;
                    }
                    tempBoard = previousBoard.Copy();
                    //  }
                }
                return bestScore;
            }
        }
        private int MAX(Node tmp, Board tempBoard)
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

                /*done = false;
                done: if (!done)
                {*/
                List<Move> moves = tempBoard.FindAvailableMoves(1);
                Board previousBoard = tempBoard.Copy();
                for (int i = 0; i < moves.Count; i++)
                {
                    
                    Node temp = new Node();
                    tempBoard.IsMoveEligable(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    tempBoard.MakeMove(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                    counter++;
                    int score = MIN(temp, tempBoard);
                    counter--;
                    alpha = score;

                    /*if (beta <= alpha)
                    {
                        done = true;
                        goto done;
                    }*/
                    temp.value = score;
                    tmp.children.Add(temp);

                    if (score > bestScore)
                    {
                        bestScore = score;
                    }
                    tempBoard = previousBoard.Copy();

                    //  }

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
                    if (tempBoard.board[x, y] == 1)
                    {
                        points--;
                    }
                    else if (tempBoard.board[x, y] == 2)
                    {
                        points++;
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
                    if (tempBoard.board[x, y] == 1)
                    {
                        humanPoints++;
                    }
                    else if (tempBoard.board[x, y] == 2)
                    {
                        computerPoints++;
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
        private class Node
        {
            public List<Node> children = new List<Node>();
            public int value = 0;
        }
        /* private class RelationNode : Relationship, IRelationshipAllowingSourceNode<Node>, IRelationshipAllowingTargetNode<Node>
         {
             public int parent;
             public int child;

             public override string RelationshipTypeKey => throw new NotImplementedException();
         }*/
    }
}
