using NetworkM.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.SaveAndInizialize
{
	public class StrictLayerInfo : ILayerInfo
	{
		public NeuronInfo[] Neurons = new NeuronInfo[0];

		int ILayerInfo.CountNeuronsInLayer
		{
			get { return Neurons.Length; }
		}
		IEnumerable<NeuronInfo> ILayerInfo.Neurons
		{
			get
			{
				foreach (var neuron in this.Neurons)
				{
					yield return neuron;
				}
			}
		}
	}
}
