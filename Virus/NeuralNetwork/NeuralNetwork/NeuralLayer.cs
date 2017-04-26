using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralLayer
    {
        public List<Neuron> neurons = new List<Neuron>();

        public void Pulse(NeuralNet net)
        {
            foreach (Neuron n in neurons)
                n.Pulse(this);
        }

        internal void ApplyLearning(double learningRate)
        {
            foreach (var item in neurons)
            {
                item.ApplyLearning(this, learningRate);
            }
        }
        internal void InitiateLearning()
        {
            foreach (var item in neurons)
            {
                item.InitiateLearning(this);
            }
        }
    }
}
