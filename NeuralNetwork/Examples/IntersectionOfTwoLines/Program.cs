﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkM;
using NetworkM.Teachers.Backpropagation;
using NetworkM.SaveAndInizialize;
using NetworkM.Networks;
using NetworkM.ActivationFunctions;

namespace IntersectionOfTwoLines
{
	class Program
	{
		static void Main(string[] args)
		{
			var network = new NeuralNetwork(
				new SimpleLayerInfo() { CountNeuronsInLayer = 2 },
				new StrictLayerInfo()
				{
					Neurons = new[] { 
						new NeuronInfo() { ActivationFunction = ActivationFunctionEnum.Threshold,T=0,InputWeights = new double[]{-1, 1 } },
						new NeuronInfo() { ActivationFunction = ActivationFunctionEnum.Threshold,T=0,InputWeights = new double[]{1, 1 } } 
						}
				},
				new StrictLayerInfo()
				{
					Neurons = new[] { 
						new NeuronInfo() { ActivationFunction = ActivationFunctionEnum.Bithreshold,T=0.5,InputWeights = new double[]{1, 1 }}}
				}
			);

			var rand = new Random();
			var input = new List<double[]>();
			var output = new List<double[]>();
			for (var i = 0; i < 50; i += 1)
			{
				var inp = new[] { rand.NextDouble()*10 * 2 - 10, rand.NextDouble()*10 * 2 - 10 };
				input.Add(inp);
				output.Add(network.GetResult(inp));
			}


			/*network = new NeuralNetwork(
				new SimpleLayerInfo() { CountNeuronsInLayer = 2 },
				new SimpleLayerInfo() { CountNeuronsInLayer = 6 , ActivationFunction = ActivationFunctionEnum.GiperbalTan },
				new SimpleLayerInfo() { CountNeuronsInLayer = 1, ActivationFunction = ActivationFunctionEnum.Sin }
			);*/

			network = NeuralNetwork.Inizialize("output.txt");
			var teacher = new NetworkM.Teachers.Backpropagation.GradientDescent(network);
			teacher.Alpha = 0.0025;
			double err = 0;
			do
			{
				err = teacher.RunEpoch(input, output);
				if (teacher.IterationCounter % 100 == 0)
				{
					Console.WriteLine(err);
				}
			} while (err > 0.1);/**/

			Console.WriteLine(network.GetAbsoluteError(input,output));
			network.Serialize("output.txt");
		}
	}
}
