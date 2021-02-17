using System;
using System.Collections.Generic;
using System.Text;

namespace Neural_Network
{
    public class Connection
    {
        public Neuron Input;
        public double Weight;
        public double DeltaWeight;

        public Connection(Neuron input, double weight)
        {
            Input = input;
            Weight = weight;
        }
    }
}
