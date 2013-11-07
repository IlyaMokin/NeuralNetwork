using NetworkM.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.SaveAndInizialize
{
	[Serializable]
	public class NeuronInfo
	{
		public ActivationFunctionEnum ActivationFunction = ActivationFunctionEnum.None;

		public string ActivationFunctionArguments;
		public double? T = null;
		public double[] InputWeights = null;
	}
}
