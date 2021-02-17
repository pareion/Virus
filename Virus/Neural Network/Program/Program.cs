using System;

namespace Neural_Network
{
    public class Program
    {
        private const int LearningRate = 1; // Try playing with a different learning rate to see how fast the network gets to the right result
        private const int Iterations = 100; // Try playing with a different amount of iterations to see how fast the network gets to the right result

        static void Main()
        {
            NeuralNet net = new NeuralNet(4, new int[1] { 4 }, 1);
           
           
            double[][] input = new double[][]
            {
                new double[]{ 0, 1 },
                new double[]{ 1, 0 },
                new double[]{ 0, 0 },
                new double[]{ 1, 1 }
            };

            double[][] output = new double[][]
            {
                new double[]{ 0 },
                new double[]{ 0 },
                new double[]{ 1 },
                new double[]{ 1 }
            };

            net.Train(input, output, LearningRate, Iterations);

            net.PrepareInput(new double[] { 0, 1 });
            net.Pulse();
            Console.WriteLine("Should be less than 0.5 - " + net.OutputLayer.Neurons[0].Output);

            net.PrepareInput(new double[] { 1, 0 });
            net.Pulse();
            Console.WriteLine("Should be less than 0.5 - " + net.OutputLayer.Neurons[0].Output);

            net.PrepareInput(new double[] { 1, 1 });
            net.Pulse();
            Console.WriteLine("Should be more than 0.5 - " + net.OutputLayer.Neurons[0].Output);

            net.PrepareInput(new double[] { 0, 0 });
            net.Pulse();
            Console.WriteLine("Should be more than 0.5 - " + net.OutputLayer.Neurons[0].Output);
        }
    }
}
