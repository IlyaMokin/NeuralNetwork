//Обучение сети на xor без скрытых слоев. Обучается не с первого раза!
using NetworkM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorWithoutHiddenLayer
{
	class Program
	{
		static void Main(string[] args)
		{
			/*xor*/
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
				new double[]{0}
			};

			//NeuroNetwork.ActivationFunctions.Add(new ThresholdXorFunction()); //Иначе при инициализации сеть не будет знать о существовании такой функции
			//NeuroNetwork network = NeuroNetwork.Izialize("result.json");

			var network = new NeuroNetwork(
				new LayerInfo() { CountNeuronsInLayer = 2 },
				new LayerInfo() { CountNeuronsInLayer = 1, ActivationFunction = new ThresholdXorFunction() });

			var teacher = new NetworkM.Teachers.Backpropagation.RosenblattMethod(network);
			teacher.Alpha = 1e-8;
			double err = 0;
			do
			{
				err = teacher.RunEpoch(input, output);
				if (teacher.IterationCounter % 10000 == 0)
				{
					Console.WriteLine(err);
				}
			} while (err > 0.01);

			network.Save("result.json");
		}
	}
}
