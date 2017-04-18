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
        private Board tempBoard, previousBoard;
        private int alpha = int.MinValue, beta = int.MaxValue;
        bool done = false;
        int bestScoreMin;
        int bestScoreMax;
        int playerNumber;
        Node root;
        List<Node> pointsVisistedBFS;
        GraphClient client;
        public MiniMaxComputer(Board board, int playerNumber)
        {
            counter = 0;
            maxcounter = 10;
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
                int bestScore = -9999;
                int minscore;
                counter = 0;
                // find bricks you can use
                List<Move> moves = board.FindAvailableMoves(playerNumber);
                root = new Node();
                if (moves != null)
                {
                    for (int b = 0; b < moves.Count; b++)
                    {
                        tempBoard = board.Copy();
                        Node tmp = new Node();
                        minscore = MIN(tmp);
                        tempBoard.MoveBrick(moves[b].fromX, moves[b].fromY, moves[b].toX, moves[b].toY);
                        tmp.value = minscore;
                        root.children.Add(tmp);
                        if (minscore > bestScore)
                        {
                            bestMove = moves[b];
                            bestScore = minscore;
                        }
                        counter = 0;
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
                
                //BFS(root);
            }
            catch (Exception e)
            {

            }

        }
        private int MIN(Node tmp)
        {
            counter++;
            if (maxDepth())
                return EVAL();
            else
            {
                try
                {
                    bestScoreMin = 9999;
                    done = false;
                    done: if (!done)
                    {
                        List<Move> moves = tempBoard.FindAvailableMoves(2);

                        for (int i = 0; i < moves.Count; i++)
                        {
                            previousBoard = tempBoard.Copy();
                            Node temp = new Node();

                            tempBoard.MoveBrick(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                            int score = MAX(temp);
                            beta = score;
                            /*if (beta <= alpha)
                            {
                                done = true;
                                goto done;
                            }*/
                            temp.value = score;
                            tmp.children.Add(temp);

                            if (score < bestScoreMin)
                            {
                                bestScoreMin = score;
                            }
                            tempBoard = previousBoard;

                        }
                    }
                    counter--;
                }
                catch (Exception e)
                {
                    
                }
                return bestScoreMin;
            }
        }
        private int MAX(Node tmp)
        {
            counter++;
            if (maxDepth())
            {
                return EVAL();
            }
            else
            {
                try
                {
                    bestScoreMax = -9999;
                    done = false;
                    done: if (!done)
                    {
                        List<Move> moves = tempBoard.FindAvailableMoves(1);

                        for (int i = 0; i < moves.Count; i++)
                        {
                            previousBoard = tempBoard.Copy();
                            Node temp = new Node();


                            tempBoard.MoveBrick(moves[i].fromX, moves[i].fromY, moves[i].toX, moves[i].toY);
                            int score = MIN(temp);
                            alpha = score;

                            /*if (beta <= alpha)
                            {
                                done = true;
                                goto done;
                            }*/
                            temp.value = score;
                            tmp.children.Add(temp);

                            if (score > bestScoreMax)
                            {
                                bestScoreMax = score;
                            }
                            tempBoard = previousBoard;

                        }

                    }
                    counter--;
                    
                }
                catch (Exception e )
                {
                    
                }
                return bestScoreMax;
            }
        }
        private int EVAL()
        {
            int points = 0;
            for (int x = 0; x < tempBoard.boardSize; x++)
            {
                for (int y = 0; y < tempBoard.boardSize; y++)
                {
                    if (tempBoard.board[x, y] == 1)
                    {
                        points = points - 1;
                    }
                    if (tempBoard.board[x, y] == 2)
                    {
                        points = points + 1;
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
        private bool isGameOverHuman()
        {
            if (tempBoard.FindAvailableMoves(1).Count > 0)
            {
                return false;
            }
            return true;
        }
        private bool isGameOverComputer()
        {
            if (tempBoard.FindAvailableMoves(2).Count > 0)
            {
                return false;
            }
            return true;
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
