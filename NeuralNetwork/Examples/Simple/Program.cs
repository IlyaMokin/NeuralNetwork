using System;
using NetworkM;
using NetworkM.Networks;

namespace Simple
{
	class Program
	{
		static void Main(string[] args)
		{
			/*And*/
			var input = new[]{
				new double[]{0,0},
				new double[]{0,1},
				new double[]{1,0},
				new double[]{1,1}
			};
			var output = new[]{
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{1}
			};

			//NeuroNetwork.ActivationFunctions.Add(new ThresholdXorFunction()); //Иначе при инициализации сеть не будет знать о существовании такой функции
			//NeuroNetwork network = NeuroNetwork.Izialize("result.json");

			var network = new NeuralNetwork(false, 2, 2, 1);

			var teacher = new NetworkM.Teachers.Backpropagation.GradientDescent(network);
			teacher.Alpha = 0.3;
			double err = 0;
			do
			{
				err = teacher.RunEpoch(input, output);
				if (teacher.IterationCounter % 1000 == 0)
				{
					Console.WriteLine(err);
				}
			} while (err > 0.01);

			Console.WriteLine(err);
		}
	}
}
