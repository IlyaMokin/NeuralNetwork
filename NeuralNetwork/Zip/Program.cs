using NetworkM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zip
{
	class Program
	{
		static void Main(string[] args)
		{
			var rand = new Random(10);
			
			IEnumerable<double[]> inputs = new object[1].Select(x => 
				new double[100].Select(y => (double)rand.Next(255)
				).ToArray()).ToArray();
			 
			//NetworkM.NeuroNetwork.ActivationFunctions.Add(new  NetworkM.ActivationFunctions.ExpSin());
			var network = new NetworkM.NeuroNetwork(
				new LayerInfo() { CountNeuronsInLayer = 100 },
				new LayerInfo() { CountNeuronsInLayer = 1 },
				new LayerInfo() { CountNeuronsInLayer = 100, ActivationFunction = new NetworkM.ActivationFunctions.Liner() }
			);
			var teacher = new NetworkM.Teachers.Backpropagation.GradientDescent(network);
			teacher.Alpha = 0.01;
			teacher.Threshold = 0.2;

			double err = 0, e = 0;
			do
			{
				err = teacher.RunEpoch(inputs, inputs);
				if (teacher.IterationCounter % 100 == 0)
				{
					Console.WriteLine(err);
				}
			} while (err > e);
		}
	}
}
