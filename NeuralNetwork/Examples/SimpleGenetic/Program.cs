using System;
using NetworkM;
using NetworkM.ActivationFunctions;

namespace SimpleGenetic
{
	class Program
	{
		static void Main(string[] args)
		{
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
			//	new double[]{1}
			//};

			////NeuroNetwork.ActivationFunctions.Add(new ThresholdXorFunction()); //Иначе при инициализации сеть не будет знать о существовании такой функции
			////NeuroNetwork network = NeuroNetwork.Izialize("result.json");

			//var network = new NeuroNetwork(true,new Sigmoid(), 2, 5, 1);

			//var teacher = new NetworkM.Teachers.Genetic.Evolutionary(network, 1000, 30, 0.5);
			//double err = 0;
			//do
			//{
			//	err = teacher.RunEpoch(input, output);
			//	if (teacher.IterationCounter%10 == 0)
			//	{
			//		Console.WriteLine(err);
			//	}
			//} while (err > 0.01);

			//network.Save("result.json");
		}
	}
}
