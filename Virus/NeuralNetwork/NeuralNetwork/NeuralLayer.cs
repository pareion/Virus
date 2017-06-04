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

        public void Pulse()
        {
            foreach (Neuron n in neurons)
                n.Pulse();
        }
        internal void InitiateLearning()
        {
            foreach (var item in neurons)
            {
                item.InitiateLearning();
            }
        }
    }
}
