using System;
using System.Collections.Generic;
using System.Text;

namespace Neural_Network
{
    public enum Activation
    {
        Adam,
        Sigmoid, 
        ReLu
    }

    public class ActivationFunction
    {
        public static double Activate(Activation activation, double value)
        {
            double result = 0;
            switch (activation)
            {
                case Activation.Adam:
                    break;
                case Activation.Sigmoid:
                    result = 1.0 / (1.0 + Math.Exp(-value));
                    break;
                case Activation.ReLu:
                    break;
                default:
                    break;
            }
            return result;
        }

        public static double Derivative(Activation activation, double value)
        {
            double result = 0;
            switch (activation)
            {
                case Activation.Adam:
                    break;
                case Activation.Sigmoid:
                    result = value * (1.0 - value);
                    break;
                case Activation.ReLu:
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
