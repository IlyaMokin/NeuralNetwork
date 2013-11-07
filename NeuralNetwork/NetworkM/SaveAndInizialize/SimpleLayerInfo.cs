using NetworkM.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.SaveAndInizialize
{
	public class SimpleLayerInfo : ILayerInfo
	{
		public int CountNeuronsInLayer { get; set; }
		public ActivationFunctionEnum ActivationFunction = ActivationFunctionEnum.None;

		IEnumerable<NeuronInfo> ILayerInfo.Neurons
		{
			get
			{
				for (int i = 0; i < CountNeuronsInLayer; i += 1)
				{
					yield return new NeuronInfo() { ActivationFunction = this.ActivationFunction };
				}
			}
		}
	}
}
