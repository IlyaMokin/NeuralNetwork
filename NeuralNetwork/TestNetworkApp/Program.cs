using NetworkM;
using NetworkM.Teachers.Backpropagation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNetworkApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var inputs = new List<double[]>{
				new double[]{0,0},
				new double[]{1,0},
				new double[]{0,1},
				new double[]{1,1}
			};
			var outputs = new List<double[]>{
				new double[]{0},
				new double[]{1},
				new double[]{1},
				new double[]{0}
			};
			NeuroNetwork network = new NeuroNetwork(true,new BipolarSigmoid(), 2, 2, 1);
			LevenbergMarquardt teacher = new LevenbergMarquardt(network, 10, 1e20, 1e-20, 5, 7);
			//GradientDescent teacher = new GradientDescent(network); teacher.Alpha = 0.15;
			double err = 0;
			var timer = DateTime.Now;
			do
			{
				teacher.RunEpoch(inputs, outputs);
				err = network.GetAbsoluteError(inputs, outputs, 0.4);
			} while (err > 0.000 && teacher.IterationCounter < 500);

			var interval = (DateTime.Now - timer).Milliseconds;
			Console.WriteLine(teacher.IterationCounter);
			Console.WriteLine(network.GetAbsoluteError(inputs, outputs, 0.4));
			Console.WriteLine(interval);
		}
	}
}
