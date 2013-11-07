using NetworkM.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.SaveAndInizialize
{
	public interface ILayerInfo
	{
		int CountNeuronsInLayer { get; }
		IEnumerable<NeuronInfo> Neurons { get; }
	}
}
