using NetworkM.NetworkElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	internal static class FunctionsFactory
	{
		private static Random _random = new Random(DateTime.Now.Millisecond);
		private static IEnumerable<ActivationFunctionEnum> _randomIgnoreList = new[] { 
			ActivationFunctionEnum.Gauss,
			ActivationFunctionEnum.ExpSin, 
			ActivationFunctionEnum.Bithreshold, 
			ActivationFunctionEnum.Threshold
		};

		public static ActivationFunction GetActivationFunction(ActivationFunctionEnum activationFunction, string ActivationFunctionArguments, Neuron neuron)
		{
			switch (activationFunction)
			{
				case ActivationFunctionEnum.BipolarSigmoid:
					return new BipolarSigmoid();
				case ActivationFunctionEnum.GiperbalTan:
					return new GiperbalTan();
				case ActivationFunctionEnum.Sigmoid:
					return new Sigmoid();
				case ActivationFunctionEnum.Sin:
					return new Sin(neuron, ActivationFunctionArguments);
				case ActivationFunctionEnum.Liner:
					return new Liner();
				case ActivationFunctionEnum.Random:
					var functions = Enum.GetValues(typeof(ActivationFunctionEnum)).Cast<ActivationFunctionEnum>()
						.Where(f => !_randomIgnoreList.Contains(f));
					var functionsCount = functions.Count();
					return GetActivationFunction(functions.ElementAt(_random.Next(functionsCount)), ActivationFunctionArguments, neuron);
				case ActivationFunctionEnum.Gauss:
					return new Gauss(neuron, ActivationFunctionArguments);
				case ActivationFunctionEnum.ExpSin:
					return new ExpSin();
				case ActivationFunctionEnum.Bithreshold:
					return new Bithreshold();
				case ActivationFunctionEnum.Threshold:
					return new Threshold();
				default:
					return null;
			}
		}

		public static ActivationFunctionEnum GetActivationFunctionName(string activationFunction)
		{
			return Enum.GetValues(typeof(ActivationFunctionEnum)).Cast<ActivationFunctionEnum>()
				.First(f => f.ToString().Equals(activationFunction.Split('.').Last(), StringComparison.InvariantCultureIgnoreCase)); ;
		}
	}
}
