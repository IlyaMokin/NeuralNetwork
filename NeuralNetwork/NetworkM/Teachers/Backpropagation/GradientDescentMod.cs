using NetworkM.NetworkElements;
using NetworkM.Networks;
using NetworkM.SaveAndInizialize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.Teachers.Backpropagation
{
	public class GradientDescentMod
	{
		private List<List<Neuron>> _layers;
		private NeuralNetwork _network;
		private Random _rand = new Random(DateTime.Now.Millisecond);

		public GradientDescentMod(NeuralNetwork network)
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


			Error = _network.GetAbsoluteError(inputs, outputs, Threshold);
			IterationCounter += 1;
			return Error;
		}

		/*private double[] GetLayerResult(double[] input, int inputLayerId, int outputLayerId)
		{
			for(var neuronId = 0; neuronId<_layers[inputLayerId].Count;neuronId+=1)
			{
				var neuron = _layers[inputLayerId][neuronId];
				neuron.Out = input[neuronId];
			}


		}*/

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
