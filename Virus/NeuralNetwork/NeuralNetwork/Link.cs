using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Link
    {
        public Neuron Input;
        public double Weight;
        public double DeltaWeight;
        public double LastDeltaWeight;
        public Link(Neuron Input, double Weight)
        {
            this.Input = Input;
            this.Weight = Weight;
            DeltaWeight = 0;
            LastDeltaWeight = 0;
        }

    }
}
