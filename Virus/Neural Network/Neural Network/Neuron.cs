namespace Neural_Network
{
    using System;
    using System.Collections.Generic;

    public class Neuron
    {
        public List<Connection> Connections;
        public double Error;
        public double BiasError;
        public double Output;
        public Activation Activation;
        public double Bias;

        public Neuron(double bias, Activation activation)
        {
            Connections = new List<Connection>();
            Bias = bias;
            Activation = activation;
        }

        internal void StartLearning()
        {
            Error = 0;
            BiasError = 0;
        }

        internal void Pulse()
        {
            double output = 0;
            foreach (Connection conn in Connections)
                output += conn.Input.Output * conn.Weight;

            output += Bias;

            Output = ActivationFunction.Activate(Activation, output);
        }
    }
}
