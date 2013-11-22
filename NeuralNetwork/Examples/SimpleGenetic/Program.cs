using System;
using NetworkM;
using NetworkM.ActivationFunctions;
using NetworkM.SaveAndInizialize;
using NetworkM.Networks;

namespace SimpleGenetic
{
	class Program
	{
		static void Main(string[] args)
		{
			var input = new[]{
				new double[]{0,0},
				new double[]{0,1},
				new double[]{1,0},
				new double[]{1,1}
			};
			var output = new[]{
				new double[]{0},
				new double[]{1},
				new double[]{1},
				new double[]{1}
			};


			var network = new NeuralNetwork(
				new SimpleLayerInfo() { CountNeuronsInLayer = 2 },
				new SimpleLayerInfo() { CountNeuronsInLayer = 5, ActivationFunction = ActivationFunctionEnum.Sigmoid },
				new SimpleLayerInfo() { CountNeuronsInLayer = 1, ActivationFunction = ActivationFunctionEnum.Sigmoid }
			);

			var teacher = new NetworkM.Teachers.Genetic.Evolutionary(
				network, 
				sizeOfPopulation: 1000, 
				leadOfPopulation: 30, 
				probabilityOfMutation: 0.5);

			double err = 0;
			do
			{
				err = teacher.RunEpoch(input, output);
				if (teacher.IterationCounter % 10 == 0)
				{
					Console.WriteLine(err);
				}
			} while (err > 0.01);
		}
	}
}
