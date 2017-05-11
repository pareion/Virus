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
            return 1.0 / (1.0 + Math.Exp(-value));
        }
        public static double SoftSigmoid(double value)
        {
            return value / (1 + Math.Abs(value));
        }
    }
    public enum ActivationFunction { SigmoidDerivative, SoftSigmoid }
}
