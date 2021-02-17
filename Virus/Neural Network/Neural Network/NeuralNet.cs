namespace Neural_Network
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The neural net class contains all the layers, neurons and everything related to the neural net
    /// </summary>
    public class NeuralNet
    {
        public Layer InputLayer;
        public Layer OutputLayer;
        public List<Layer> HiddenLayers;

        /// <param name="inputNeurons">
        /// The number of neurons in the input layer
        /// </param>
        /// <param name="layers">
        /// The number of hidden layers
        /// </param>
        /// <param name="outputNeurons">
        /// The number of neurons in the output layer
        /// </param>
        public NeuralNet(int inputNeurons, int[] hiddenNeurons, int outputNeurons)
        {
            InputLayer = new Layer();
            OutputLayer = new Layer();
            HiddenLayers = new List<Layer>();

            for (int i = 0; i < hiddenNeurons.Length; i++)
                HiddenLayers.Add(new Layer());

            Random random = new Random();

            if (inputNeurons <= 0)
                throw new Exception("Input neurons must be more than 0");
            if (outputNeurons <= 0)
                throw new Exception("Output neurons must be more than 0");
            if (hiddenNeurons.Length <= 0)
                throw new Exception("There must be atleat 1 hidden layer");

            // Put neurons into the layers along with the activation function for each neuron
            for (int i = 0; i < inputNeurons; i++)
                InputLayer.Neurons.Add(new Neuron(0, Activation.Sigmoid));

            for (int i = 0; i < HiddenLayers.Count; i++)
                for (int j = 0; j < hiddenNeurons[i]; j++)
                    HiddenLayers[i].Neurons.Add(new Neuron(random.NextDouble(), Activation.Sigmoid));

            for (int i = 0; i < outputNeurons; i++)
                OutputLayer.Neurons.Add(new Neuron(random.NextDouble(), Activation.Sigmoid));

            // Wire together all the layers
            // input -> hidden -> output
            // Wire the input layer with the first hidden layer
            for (int i = 0; i < HiddenLayers[0].Neurons.Count; i++)
                for (int j = 0; j < InputLayer.Neurons.Count; j++)
                    HiddenLayers[0].Neurons[i].Connections.Add(
                       new Connection(InputLayer.Neurons[j], random.NextDouble()));

            // Wire together all the hidden layers if there is more than one
            if (hiddenNeurons.Length > 1)
                for (int i = hiddenNeurons.Length - 1; i > 0; i--)
                    for (int j = 0; j < HiddenLayers[i].Neurons.Count; j++)
                        for (int k = 0; k < HiddenLayers[i - 1].Neurons.Count; k++)
                            HiddenLayers[i].Neurons[j].Connections.Add(
                                new Connection(HiddenLayers[i - 1].Neurons[k], random.NextDouble()));

            // Wire together the last hidden layer with the output layer
            for (int i = 0; i < OutputLayer.Neurons.Count; i++)
                for (int j = 0; j < HiddenLayers[HiddenLayers.Count - 1].Neurons.Count; j++)
                    OutputLayer.Neurons[i].Connections.Add(
                        new Connection(HiddenLayers[HiddenLayers.Count - 1].Neurons[j], random.NextDouble()));
        }

        public void Train(double[][] input, double[][] expectedOutput, double learningRate, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                StartLearning();
                for (int j = 0; j < input.Length; j++)
                    StartTraining(input[j], expectedOutput[j], learningRate);
            }
        }

        private void StartTraining(double[] input, double[] output, double learningRate)
        {
            // Prepare the network 
            PrepareInput(input);

            // Pulse through the network
            Pulse();

            // Calculate the errors
            CalculateErrors(output);

            // Adjust the network using the errors
            AdjustNetwork(learningRate);
        }

        public void PrepareInput(double[] input)
        {
            for (int i = 0; i < input.Length; i++)
                InputLayer.Neurons[i].Output = input[i];
        }

        // Go through the network backwards and correct the weights using the new errors
        private void AdjustNetwork(double learningRate)
        {
            for (int i = 0; i < HiddenLayers[HiddenLayers.Count - 1].Neurons.Count; i++)
            {
                var from = HiddenLayers[HiddenLayers.Count - 1].Neurons[i];

                for (int k = 0; k < OutputLayer.Neurons.Count; k++)
                {
                    var outputNeuron = OutputLayer.Neurons[k];
                    outputNeuron.Connections[i].Weight += outputNeuron.Error * from.Output;
                    outputNeuron.Bias += outputNeuron.BiasError * from.Output;
                }

            }

            for (int i = HiddenLayers.Count - 1; i > 0; i--)
            {
                for (int j = 0; j < HiddenLayers[i].Neurons.Count; j++)
                {
                    var currentNeuron = HiddenLayers[i].Neurons[j];

                    for (int k = 0; k < HiddenLayers[i - 1].Neurons.Count; k++)
                    {
                        var outputNeuron = HiddenLayers[i - 1].Neurons[k];
                        currentNeuron.Connections[k].Weight += learningRate * outputNeuron.Error * currentNeuron.Output;
                        outputNeuron.Bias += outputNeuron.BiasError * currentNeuron.Output;
                    }
                }
            }

            for (int i = 0; i < InputLayer.Neurons.Count; i++)
            {
                var currentNeuron = InputLayer.Neurons[i];
                for (int j = 0; j < HiddenLayers[0].Neurons.Count; j++)
                {
                    var outputNeuron = HiddenLayers[0].Neurons[j];
                    outputNeuron.Connections[i].Weight += outputNeuron.Error * currentNeuron.Output;
                    outputNeuron.Bias += outputNeuron.BiasError * currentNeuron.Output;
                }
            }
        }

        // Calculate the errors in the last layer and then go through the network backwards
        private void CalculateErrors(double[] expected)
        {
            for (int i = 0; i < OutputLayer.Neurons.Count; i++)
            {
                double output = OutputLayer.Neurons[i].Output;
                OutputLayer.Neurons[i].Error = (expected[i] - output);
                OutputLayer.Neurons[i].BiasError = (expected[i] - output);
            }

            for (int i = HiddenLayers.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < HiddenLayers[i].Neurons.Count; j++)
                {
                    double error = 0;
                    double biasError = 0;
                    if (i == HiddenLayers.Count - 1)
                    {
                        for (int k = 0; k < OutputLayer.Neurons.Count; k++)
                        {
                            var neuron = OutputLayer.Neurons[k];
                            error += neuron.Error * neuron.Connections[j].Weight;
                            biasError += neuron.BiasError * neuron.Bias;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < HiddenLayers[i + 1].Neurons.Count; k++)
                        {
                            var neuron = HiddenLayers[i + 1].Neurons[k];
                            error += neuron.Error * neuron.Connections[j].Weight;
                            biasError += neuron.BiasError * neuron.Bias;
                        }
                    }
                    HiddenLayers[i].Neurons[j].Error = error * ActivationFunction.Derivative(HiddenLayers[i].Neurons[j].Activation, HiddenLayers[i].Neurons[j].Output);
                    HiddenLayers[i].Neurons[j].Bias = biasError * ActivationFunction.Derivative(HiddenLayers[i].Neurons[j].Activation, HiddenLayers[i].Neurons[j].Output);
                }
            }
        }

        public void Pulse()
        {
            for (int i = 0; i < HiddenLayers.Count; i++)
                HiddenLayers[i].Pulse();
            OutputLayer.Pulse();
        }

        private void StartLearning()
        {
            foreach (Layer layer in HiddenLayers)
                layer.StartLearning();
            OutputLayer.StartLearning();
        }
    }
}
