//Обучение сети на xor c одним скрытым слоем
using NetworkM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorWithOneHiddenLayer
{
	class Program
	{
		static void Main(string[] args)
		{
			///*xor*/
			//var input = new[]{
			//	new double[]{0,0},
			//	new double[]{0,1},
			//	new double[]{1,0},
			//	new double[]{1,1}
			//};
			//var output = new[]{
			//	new double[]{0},
			//	new double[]{1},
			//	new double[]{1},
			//	new double[]{0}
			//};

			////NeuroNetwork.ActivationFunctions.Add(new NetworkM.ActivationFunctions.Sin()); //Иначе при инициализации сеть не будет знать о существовании такой функции
			////NeuroNetwork network = NeuroNetwork.Izialize("result.json");

			//var network = new NeuroNetwork(
			//	new LayerInfo() { CountNeuronsInLayer = 2 },
			//	new LayerInfo() { CountNeuronsInLayer = 2, ActivationFunction = new NetworkM.ActivationFunctions.Sigmoid() },
			//	new LayerInfo() { CountNeuronsInLayer = 1, ActivationFunction = new NetworkM.ActivationFunctions.Sin() });

			//var teacher = new NetworkM.Teachers.Backpropagation.GradientDescent(network);
			//teacher.Alpha = 0.15;
			//double err = 0;
			//do
			//{
			//	err = teacher.RunEpoch(input, output);
			//	if (teacher.IterationCounter % 1000 == 0)
			//	{
			//		Console.WriteLine(err);
			//	}
			//} while (err > 0.01);

			//network.Save("result.json");
		}
	}
}
