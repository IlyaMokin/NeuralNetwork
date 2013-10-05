﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.Teachers.Backpropagation
{
	public class GradientDescent
	{
		private List<List<Neuron>> layers;
		private NeuroNetwork network;

		public GradientDescent(NeuroNetwork network)
		{
			layers = network.Layers;
			this.network = network;
		}

		private void CalculateError(double[] output)
		{
			for (int i = layers.Count - 1; i >= 0; i--)
			{
				for (int k = 0; k < layers[i].Count; k++)
				{
					if (i == layers.Count - 1)
					{
						layers[i][k].Error = layers[i][k].Out - output[k];
					}
					else
					{
						layers[i][k].Error =
							layers[i][k].OutputLinks.Sum(x =>
								x.Neuron.Error * x.W * x.Neuron.ActivationFunc.DF(x.Neuron.S));
					}
				}
			}
		}

		public double Alpha = 0.15;
		public int IterationCounter = 0;
		public double Threshold = 0.0;
		private double Error = 0;

		public double RunEpoch(IEnumerable<double[]> inputs, IEnumerable<double[]> outputs)
		{
			for (int i = 0; i < inputs.Count(); i++)
			{
				double[] result = network.GetResult(inputs.ElementAt(i));
				if (GetErrorForElement(result, outputs.ElementAt(i)) > Threshold)
				{
					CalculateError(outputs.ElementAt(i));
					//layerIndex > -1 - для рекурентной сети
					for (int layerIndex = layers.Count - 1; layerIndex > -1; layerIndex--)
					{
						foreach (var neuron in layers[layerIndex])
						{
							neuron.T += Alpha * neuron.Error * neuron.ActivationFunc.DF(neuron.S);
							neuron.InputLinks.ForEach(inpLink =>
							{
								inpLink.W -= Alpha * neuron.Error * neuron.ActivationFunc.DF(neuron.S) * inpLink.Neuron.Out;
							});
						}
					}
				}
			}
			IterationCounter += 1;
			Error = network.GetAbsoluteError(inputs, outputs, Threshold);
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