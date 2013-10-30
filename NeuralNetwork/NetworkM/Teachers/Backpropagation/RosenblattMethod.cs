using NetworkM.NetworkElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.Teachers.Backpropagation
{
	public class RosenblattMethod
	{
		private List<List<Neuron>> _layers;
		private NeuroNetwork _network;

		public RosenblattMethod(NeuroNetwork network)
		{
			_layers = network.Layers;
			_network = network;
		}

		private void CalculateError(double[] output)
		{
			for (int i = _layers.Count - 1; i >= 0; i--)
			{
				for (int k = 0; k < _layers[i].Count; k++)
				{
					if (i == _layers.Count - 1)
					{
						_layers[i][k].Error = _layers[i][k].Out - output[k];
					}
				}
			}
		}

		public double Alpha = 0.12;
		public int IterationCounter = 0;
		private double _error = 0;

		public double RunEpoch(IEnumerable<double[]> inputs, IEnumerable<double[]> outputs)
		{
			for (int i = 0; i < inputs.Count(); i++)
			{
				CalculateError(outputs.ElementAt(i));
				for (int layerIndex = _layers.Count - 1; layerIndex > 0; layerIndex--)
				{
					foreach (var neuron in _layers[layerIndex])
					{
						neuron.T += Alpha * neuron.Error;

						neuron.InputLinks.ForEach(inpLink =>
						{
							inpLink.W -= Alpha * neuron.Error * inpLink.Neuron.Out;
						});
					}
				}

			}
			IterationCounter += 1;
			_error = _network.GetAbsoluteError(inputs, outputs, 0);

			return _error;
		}
	}
}
