using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus
{
    public class QLearningComputer : VirusPlayer
    {
        private double learningRate;
        private double discountFactor;
        public double explorationFactor;
        private int playerNumber;
        private Board board;
        private List<State> states;
        private List<BeenThrough> statesBeenThrough;
        List<QMove> actionsAvailable;
        private Random random;

        public QLearningComputer(Board board, double learningRate, double discountFactor, double explorationFactor, int playerNumber)
        {
            this.board = board;
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
            this.explorationFactor = explorationFactor;
            this.playerNumber = playerNumber;
            random = new Random();
            actionsAvailable = new List<QMove>();
            states = new List<State>();
            statesBeenThrough = new List<BeenThrough>();
        }
        bool contained = false;
        public void AfterGame()
        {
            if (board.GetScore()[playerNumber - 1] > (board.boardSize * board.boardSize / 2))
            {
                foreach (BeenThrough item in statesBeenThrough)
                {
                    contained = false;
                    foreach (var item2 in states)
                    {
                        if (item2.BoardHashValue == item.state.BoardHashValue)
                        {
                            foreach (var item3 in item2.Actions)
                            {
                                item3.points += 0.1f;
                            }
                            contained = true;
                            break;
                        }
                    }
                    if (contained)
                    {
                        states.Add(item.state);
                    }
                }
            }
            else
            {
                foreach (BeenThrough item in statesBeenThrough)
                {
                    State state = states.Find(x => x.BoardHashValue == item.state.BoardHashValue);
                    if (state == null)
                        states.Add(item.state);
                    else
                        foreach (QMove action in state.Actions)
                        {
                            if (action.move.Equals(item.move))
                            {
                                action.points += -0.1f;
                                break;
                            }
                        }
                }
            }
            statesBeenThrough.Clear();
        }

        public void play()
        {
            if (board.FindAvailableMoves(playerNumber).Count == 0)
            {
                return;
            }
            if (WonGame(board))
            {
                List<Move> moves = board.FindAvailableMoves(playerNumber);
                Move move = moves[random.Next(0, moves.Count)];
                board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                return;
            }
            //Hvis jeg har støt på boardet før
            if (ContainsState(board))
            {
                string hash = GetRealHashCode(board);

                actionsAvailable = states.Find(x => x.BoardHashValue == hash).Actions.OrderBy(x => x.points).ToList();
                actionsAvailable.Reverse();

                State state = new State();
                Move move = null;

                List<Move> moves = board.FindAvailableMoves(playerNumber);
                foreach (var item in moves)
                {
                    state.Actions.Add(new QMove() { move = item, points = 100f });
                }
                state.BoardHashValue = GetRealHashCode(board);

                //Explore a move (Maybe)
                if (random.Next(0, 101) < explorationFactor)
                {
                    move = actionsAvailable[random.Next(0, actionsAvailable.Count)].move;
                    board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                }
                //Don't explore
                else
                {
                    move = actionsAvailable[0].move;
                    board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
                }
                statesBeenThrough.Add(new BeenThrough() { move = move, state = state });
            }
            else
            {
                //Hvis jeg ikke har set boardet før
                State state = new State();

                List<Move> moves = board.FindAvailableMoves(playerNumber);

                foreach (var item in moves)
                {
                    state.Actions.Add(new QMove() { move = item, points = 100f });
                }
                state.BoardHashValue = GetRealHashCode(board);

                Move move = moves[random.Next(0, moves.Count)];

                statesBeenThrough.Add(new BeenThrough() { move = move, state = state });
                board.MoveBrick(move.fromX, move.fromY, move.toX, move.toY);
            }
        }

        private bool WonGame(Board board)
        {
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    if (board.board[x, y] != playerNumber && board.board[x, y] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool ContainsState(Board board)
        {
            foreach (var item in states)
            {
                if (item.BoardHashValue == GetRealHashCode(board))
                {
                    return true;
                }
            }
            return false;
        }
        public string GetRealHashCode(Board board)
        {
            string s = "";
            for (int x = 0; x < board.boardSize; x++)
            {
                for (int y = 0; y < board.boardSize; y++)
                {
                    s += board.board[x, y];
                }
            }
            return s;
        }
        private class BeenThrough
        {
            public State state = null;
            public Move move = null;
        }
        private class State
        {
            public string BoardHashValue = "";
            public List<QMove> Actions = new List<QMove>();
        }
        private class QMove
        {
            public Move move = null;
            public double points = 100f;
        }
    }
}
