using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Utility
    {
        public static double SigmoidDerivative(double value)
        {
            return value * (1.0F - value);
        }
    }
}
