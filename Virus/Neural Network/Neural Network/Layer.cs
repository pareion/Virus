using System;
using System.Collections.Generic;
using System.Text;

namespace Neural_Network
{
    public class Layer
    {
        public List<Neuron> Neurons;

        public Layer()
        {
            Neurons = new List<Neuron>();
        }

        internal void StartLearning()
        {
            foreach (Neuron neuron in Neurons)
                neuron.StartLearning();
        }

        internal void Pulse()
        {
            foreach (Neuron neuron in Neurons)
                neuron.Pulse();
        }
    }
}
