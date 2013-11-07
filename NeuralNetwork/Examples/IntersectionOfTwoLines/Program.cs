using System;
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
		}
	}
}
