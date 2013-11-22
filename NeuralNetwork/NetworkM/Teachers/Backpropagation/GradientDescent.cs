using NetworkM.NetworkElements;
using NetworkM.Networks;
using NetworkM.SaveAndInizialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.Teachers.Backpropagation
{
	public class GradientDescent
	{
		private List<List<Neuron>> _layers;
		private NeuralNetwork _network;
		private Random _rand = new Random(DateTime.Now.Millisecond);

		public GradientDescent(NeuralNetwork network)
		{
			_layers = network.Layers;
			this._network = network;
		}

		private void CalculateError(double[] output)
		{
			for (int i = _layers.Count - 1; i > 0; i--)
			{
				for (int k = 0; k < _layers[i].Count; k++)
				{
					if (i == _layers.Count - 1)
					{
						_layers[i][k].Error = _layers[i][k].Out - output[k];
					}
					else
					{
						_layers[i][k].Error =
							_layers[i][k].OutputLinks.Sum(x =>
								x.Neuron.Error * x.W * x.Neuron.ActivationFunc.DF(x.Neuron.S));
					}
				}
			}
		}

		public double Alpha = 0.15;
		public int IterationCounter = 0;
		public double Threshold = 0.0;
		private double Error = 0;

		public double RunEpoch(IEnumerable<double[]> inputs, IEnumerable<double[]> outputs, bool optimize = false)
		{

			for (int i = 0; i < inputs.Count(); i++)
			{
				double[] result = _network.GetResult(inputs.ElementAt(i));
				if (GetErrorForElement(result, outputs.ElementAt(i)) > Threshold)
				{
					CalculateError(outputs.ElementAt(i));
					for (int layerIndex = _layers.Count - 1; layerIndex > 0; layerIndex--)
					{
						foreach (var neuron in _layers[layerIndex])
						{
							neuron.T += Alpha * neuron.Error * neuron.ActivationFunc.DF(neuron.S);
							neuron.InputLinks.ForEach(inpLink =>
							{
								inpLink.W -= Alpha * neuron.Error * neuron.ActivationFunc.DF(neuron.S) * inpLink.Neuron.Out;
							});
							neuron.NeuronStimulus(Alpha);
						}
						if (optimize)
						{
							result = _network.GetResult(inputs.ElementAt(i));
							CalculateError(outputs.ElementAt(i));
						}
					}
				}
			}
			Error = _network.GetAbsoluteError(inputs, outputs, Threshold);
			IterationCounter += 1;
			return Error;
		}

		private double GetErrorForElement(double[] res, double[] output)
		{
			return Math.Sqrt(res.Zip(output, (x, y) =>
			{
				double val = x - y;
				return Math.Abs(val) > Threshold ? Math.Pow(val, 2) : 0;
			}).Sum());
		}
	}
}
