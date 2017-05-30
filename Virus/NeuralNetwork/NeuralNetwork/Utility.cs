using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Utility
    {
        public static double Sigmoid(double value)
        {
            return 1.0 / (1.0 + Math.Exp(-value));
        }
        public static double SigmoidDerivative(double value)
        {
            return value * (1.0 - value);
        }
    }
    public enum ActivationFunction { Sigmoid }
}
