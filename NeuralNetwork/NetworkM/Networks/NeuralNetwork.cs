﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using NetworkM.ActivationFunctions;
using NetworkM.NetworkElements;
using NetworkM.SaveAndInizialize;

namespace NetworkM.Networks
{
	public class NeuralNetwork
	{
		public NeuralNetwork(params ILayerInfo[] layersInfo)
		{
			var previewLayer = new List<Neuron>();
			int count = layersInfo.Count();
			for (int index = 0; index < count; index++)
			{
				var countNeurons = layersInfo[index].CountNeuronsInLayer;
				var currentLayer = new List<Neuron>();
				for (int k = 0; k < countNeurons; k++)
				{
					var neuron = layersInfo[index].Neurons.ElementAt(k);

					var core = new Neuron(previewLayer, neuron.ActivationFunction, neuron.ActivationFunctionArguments, neuron.T, neuron.InputWeights);
					currentLayer.Add(core);
				}
				Layers.Add(currentLayer);
				previewLayer = currentLayer;
			}
		}

		public bool Undertow(int on = 1)
		{
			bool isUndertow = true;
			foreach (var layer in Layers)
				foreach (var neuron in layer)
				{
					isUndertow = isUndertow && neuron.Undertow(on);
				}

			return isUndertow;
		}

		internal List<List<Neuron>> Layers = new List<List<Neuron>>();

		public static int HistorySize = 10;

		public double GetAbsoluteError(IEnumerable<double[]> inputs, IEnumerable<double[]> outputs, double threshold = 0)
		{
			double sum = 0.0;
			var outp = outputs.ToList();
			for (int i = 0; i < outputs.Count(); i++)
			{
				double[] res = GetResult(inputs.ElementAt(i));

				if (double.NaN.Equals(res[0]))
				{
					throw new ArgumentOutOfRangeException();
				}

				sum += res.Zip(outp[i], (x, y) =>
				{
					double val = Math.Abs(x - y);
					return val > threshold ? val : 0;
				}).Sum();
			}
			return sum;
		}

		public double[] GetResult(double[] inputs)
		{
			for (int i = 0; i < inputs.Length; i++)
			{
				Layers[0][i].Out = inputs[i];
			}
			Calculate();
			return Layers.Last().Select(x => x.Out).ToArray();
		}

		private void Calculate()
		{
			for (int i = 1; i < Layers.Count; i++)
			{
				foreach (var neuron in Layers[i])
				{
					neuron.S = neuron.InputLinks.Sum(x => x.W * x.Neuron.Out) - neuron.T;
					neuron.Out = neuron.ActivationFunc.F(neuron.S);
				}
			}
		}

		public void ClearHistory()
		{
			foreach (var layer in Layers)
			{
				foreach (var neuron in layer)
				{
					neuron.ClearHistory();
				}
			}
		}

		public void Serialize(string path)
		{
			var obj = new List<StrictLayerInfo>();
			foreach (var layer in Layers)
			{
				var layerNeurons = new List<NeuronInfo>();
				foreach (var neuron in layer)
				{
					var inputWeights = neuron.InputLinks.Select(w => w.W).ToArray();
					layerNeurons.Add(new NeuronInfo()
					{
						T = neuron.T,
						ActivationFunction = neuron.ActivationFunc != null 
							? FunctionsFactory.GetActivationFunctionName(neuron.ActivationFunc.GetType().ToString()) 
							: ActivationFunctionEnum.None,
						ActivationFunctionArguments = neuron.ActivationFunc != null
							? neuron.ActivationFunc.SerializeParams()
							: null,
						InputWeights = inputWeights
					});
				}
				obj.Add(new StrictLayerInfo() { Neurons = layerNeurons.ToArray() });
			}

			using (var writer = new StreamWriter(path, false, Encoding.UTF8))
			{
				writer.WriteLine(new JavaScriptSerializer().Serialize(obj));
			}
		}

		public static NeuralNetwork Inizialize(string path)
		{
			using (var reader = new StreamReader(path, Encoding.UTF8))
			{
				var inizializeInfo = new JavaScriptSerializer().Deserialize <List<StrictLayerInfo>>(reader.ReadToEnd()).ToArray();
				return new NeuralNetwork(inizializeInfo);
			}
		}
	}
}