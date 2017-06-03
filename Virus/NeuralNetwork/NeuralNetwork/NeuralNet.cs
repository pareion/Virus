using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class NeuralNet
    {
        public NeuralLayer inputLayer;
        public NeuralLayer hiddenLayer;
        public NeuralLayer outputLayer;
        public ActivationFunction activation;
        public double fitness;
        public int seed;
        public NeuralNet(ActivationFunction activation)
        {
            this.activation = activation;
        }
        public void Pulse()
        {
            lock (this)
            {
                hiddenLayer.Pulse(this);
                outputLayer.Pulse(this);
            }
        }

        public void Init(int inputNeurons, int hiddenNeurons, int outputNeurons)
        {
            inputLayer = new NeuralLayer();
            hiddenLayer = new NeuralLayer();
            outputLayer = new NeuralLayer();
            fitness = 100;
            Random rand = new Random();
            Random rand2 = new Random();
            Random rand3 = new Random();
            Random rand4 = new Random();

            double bias = 0.5;
            int i, j;
            for (i = 0; i < inputNeurons; i++)
                inputLayer.neurons.Add(new Neuron(0, activation));

            for (i = 0; i < hiddenNeurons; i++)
                hiddenLayer.neurons.Add(new Neuron(rand.NextDouble(), activation));

            for (i = 0; i < outputNeurons; i++)
                outputLayer.neurons.Add(new Neuron(rand2.NextDouble(), activation));
            
            //Wire input together with the hidden layer
            for (i = 0; i < hiddenLayer.neurons.Count; i++)
            {
                for (j = 0; j < inputLayer.neurons.Count; j++)
                {
                    hiddenLayer.neurons[i].Input.Add(new Link(inputLayer.neurons[j], rand3.NextDouble() - bias));
                }
            }


            //Wire output together with hidden layer
            for (i = 0; i < outputLayer.neurons.Count; i++)
            {
                for (j = 0; j < hiddenLayer.neurons.Count; j++)
                {
                    outputLayer.neurons[i].Input.Add(new Link(hiddenLayer.neurons[j], rand4.NextDouble() - bias));
                }
            }
        }
        int i, a, b;
        public void Train(double[][] Input, double[][] ExpectedOutput, double learningrate, int iterations)
        {
            lock (this)
            {
                for (i = 0; i < iterations; i++)
                {
                    for (a = 0; a < iterations; a++)
                    {
                        InitiateLearning();
                        for (b = 0; b < Input.Count(); b++)
                        {
                            BackPropogation_TrainingSession(this, Input[b], ExpectedOutput[b], learningrate);
                        }
                    }
                }
            }
        }

        private void BackPropogation_TrainingSession(NeuralNet neuralNet, double[] input, double[] expected, double learningrate)
        {
            PreparePerceptionLayerForPulse(neuralNet, input);
            neuralNet.Pulse();
            BackPropagation(neuralNet, expected, learningrate);
            
            /*
            CalculateErrors(neuralNet, expected);
            AdjuestNet(neuralNet, learningrate);*/
        }

        private void PreparePerceptionLayerForPulse(NeuralNet neuralNet, double[] input)
        {
            for (i = 0; i < inputLayer.neurons.Count; i++)
            {
                neuralNet.inputLayer.neurons[i].Output = input[i];
            }
        }

        private void InitiateLearning()
        {
            lock (this)
            {
                hiddenLayer.InitiateLearning();
                outputLayer.InitiateLearning();
            }
        }
        Neuron node, output;
        private void AdjuestNet(NeuralNet neuralNet, double learningrate)
        {
            for (int i = 0; i < neuralNet.hiddenLayer.neurons.Count; i++)
            {
                node = neuralNet.hiddenLayer.neurons[i];

                for (int i2 = 0; i2 < neuralNet.outputLayer.neurons.Count; i2++)
                {
                    output = neuralNet.outputLayer.neurons[i2];

                    output.Input[i].Weight += learningrate * output.error * node.Output;
                }
            }

            for (int i = 0; i < neuralNet.inputLayer.neurons.Count; i++)
            {
                node = neuralNet.inputLayer.neurons[i];

                for (int i2 = 0; i2 < neuralNet.hiddenLayer.neurons.Count; i2++)
                {
                    output = neuralNet.hiddenLayer.neurons[i2];

                    output.Input[i].Weight += learningrate * output.error * node.Output;
                }
            }
        }

        public void CalculateErrors(NeuralNet neuralNet, double[] expectedOutput)
        {
            //Calculate outputlayer errors
            for (int i = 0; i < neuralNet.outputLayer.neurons.Count; i++)
            {
                double temp = neuralNet.outputLayer.neurons[i].Output;
                neuralNet.outputLayer.neurons[i].error = (expectedOutput[i] - temp) * Utility.SigmoidDerivative(temp);
            }

            //Calculate hiddenlayer errors
            for (int i = 0; i < neuralNet.hiddenLayer.neurons.Count; i++)
            {
                double error = 0;
                Neuron temp = neuralNet.hiddenLayer.neurons[i];
                for (int a = 0; a < neuralNet.outputLayer.neurons.Count; a++)
                {
                    Neuron outputNode = neuralNet.outputLayer.neurons[a];

                    error += outputNode.error * (outputNode.Input[i].Weight * Utility.SigmoidDerivative(temp.Output));
                }
                neuralNet.hiddenLayer.neurons[i].error = error;
            }
        }
        public void BackPropagation(NeuralNet net, double[] expectedOutput, double learningRate)
        {
            for (int i = 0; i < net.outputLayer.neurons.Count; i++)
            {
                if (expectedOutput[i] == 1)
                {
                    Neuron temp = net.outputLayer.neurons[i];
                    temp.error = (expectedOutput[i] - temp.Output) * Utility.SigmoidDerivative(temp.Output);
                }
                else
                {
                    Neuron temp = net.outputLayer.neurons[i];
                    temp.error = 0;
                }
            }

            for (int i = 0; i < net.hiddenLayer.neurons.Count; i++)
            {
                Neuron temp = net.hiddenLayer.neurons[i];

                foreach (var item in net.outputLayer.neurons)
                {
                    temp.error += item.error * (item.Input[i].Weight * Utility.SigmoidDerivative(temp.Output));
                }
            }
            AdjuestNet(net, learningRate);
        }
    }
}
