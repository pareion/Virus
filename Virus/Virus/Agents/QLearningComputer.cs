using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Agents
{
    public class QLearningComputer : IVirusPlayer
    {
        private int playerNumber;
        private int learningRate;
        private int discountFactor;

        public QLearningComputer(int playerNumber, int learningRate, int discountFactor)
        {
            this.playerNumber = playerNumber;
            this.learningRate = learningRate;
            this.discountFactor = discountFactor;
        }
        public void play()
        {
            throw new NotImplementedException();
        }
    }
}
