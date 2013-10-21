using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkM;
using NetworkM.Teachers.Backpropagation;

namespace IntersectionOfTwoLines
{
	class Program
	{
		static void Main(string[] args)
		{
			#region sample
			var inp = new[]{
				new double[]{5,5},
				new double[]{5,-5},
				new double[]{3,3},
				new double[]{3,-3},
				new double[]{1,1},
				new double[]{1,-1},
				new double[]{0.5,0.5},
				new double[]{0.5,-0.5},
				new double[]{-1,-1},
				new double[]{-1,1},
				new double[]{-3,3},
				new double[]{-3,-3},
				new double[]{-5,5},
				new double[]{-5,-5},

				new double[]{5,4.9},
				new double[]{5,-4.9},
				new double[]{3,2.9},
				new double[]{3,-2.9},
				new double[]{1,0.9},
				new double[]{1,-0.9},
				new double[]{0.5,0.4},
				new double[]{0.5,-0.4},
				new double[]{-1,0.9},
				new double[]{-1,-0.9},
				new double[]{-3,2.9},
				new double[]{-3,-2.9},
				new double[]{-5,4.9},
				new double[]{-5,-4.9},

				new double[]{-5,0},
				new double[]{5,0},
				new double[]{-3,1},
				new double[]{3,1},
				new double[]{4,3},
				new double[]{-4,3},
				new double[]{-4,-3},
				new double[]{4,-3},

				new double[]{5,5.1},
				new double[]{5,-5.1},
				new double[]{3,3.1},
				new double[]{3,-3.1},
				new double[]{1,1.1},
				new double[]{1,-1.1},
				new double[]{0.5,0.6},
				new double[]{0.5,-0.6},
				new double[]{-1,1.1},
				new double[]{-1,-1.1},
				new double[]{-3,3.1},
				new double[]{-3,-3.1},
				new double[]{-5,5.1},
				new double[]{-5,-5.1}

			};
			var outp = new[]{
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},

				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},

				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},
				new double[]{1},

				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0},
				new double[]{0}
			};
			#endregion sample

			NeuroNetwork.ActivationFunctions.Add(new ReversingSigmoid());

			var network = new NeuroNetwork(
				new LayerInfo() { CountNeuronsInLayer = 2 },
				new LayerInfo() { CountNeuronsInLayer = 2, ActivationFunction = new NetworkM.ActivationFunctions.Sigmoid() },
				new LayerInfo() { CountNeuronsInLayer = 1, ActivationFunction = new ThresholdXorFunction() });

			//network = NeuroNetwork.Izialize("result.json");
			var teacher = new RosenblattMethod(network);

			var err = 0.0d;
			var e = 0d;

			teacher.Alpha = 1e-8;
			do
			{
				err = teacher.RunEpoch(inp, outp);

				if (teacher.IterationCounter % 1000== 0)
				{
					Console.WriteLine(err);
				}
			} while (err > e && teacher.IterationCounter < 500000);

			Console.WriteLine(err);
			network.Save("result.json");
		}
	}
}
