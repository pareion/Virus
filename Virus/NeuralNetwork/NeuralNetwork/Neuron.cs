using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    public class Neuron
    {
        public double bias;
        public double weight;
        public double Output;
        public double error;
        public double delta;
        public double lastdelta;
        public List<Link> Input = new List<Link>();
        public ActivationFunction activation;

        public Neuron(double bias, ActivationFunction activation)
        {
            this.bias = bias;
            delta = 0;
            lastdelta = 0;
            this.activation = activation;
        }

        internal void Pulse(NeuralLayer neuralLayer)
        {
            Output = 0;

            foreach (var item in Input)
            {
                Output += item.Input.Output * item.Weight;
            }

            Output += bias;

            Output = ActivateFunction(Output);

        }

        internal void InitiateLearning(NeuralLayer neuralLayer)
        {
            foreach (var item in Input)
            {
                item.LastDeltaWeight = 0;
                item.DeltaWeight = 0;
            }
            delta = 0;
            lastdelta = 0;
        }

        internal void ApplyLearning(NeuralLayer neuralLayer, double learningRate)
        {
            foreach (var item in Input)
            {
                item.ApplyLearning(learningRate);
            }
            lastdelta = delta;
            weight += delta * learningRate;
        }

        public double GetOutput()
        {
            return Output;
        }
        public void SetOutput(double output)
        {
            Output = output;
        }
        public double ActivateFunction(double value)
        {
            switch (activation)
            {
                case ActivationFunction.SigmoidDerivative:
                    return Utility.SigmoidDerivative(value);
                case ActivationFunction.SoftSigmoid:
                    return Utility.SoftSigmoid(value);
            }
            return 0;
        }

    }
}