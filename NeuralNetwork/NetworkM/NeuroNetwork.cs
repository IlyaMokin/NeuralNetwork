using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using NetworkM.ActivationFunctions;
using NetworkM.NetworkElements;

namespace NetworkM
{
	public class NeuroNetwork
	{
		static NeuroNetwork()
		{
			ActivationFunctions = new List<ActivationFunction>() { 
				//new Liner(), 
				new Sigmoid(), 
				new BipolarSigmoid(), 
				//new ThresholdXorFunction(),
				//new Sin(), 
				//new ExpSin(),
				new GiperbalTan()
				};
		}

		private Random rand = new Random(DateTime.Now.Millisecond);

		private LayerInfo[] _inizializeInfo;

		protected void Inizialize(params LayerInfo[] layersInfo)
		{
			_inizializeInfo = layersInfo;

			var previewLayer = new List<Neuron>();
			int count = layersInfo.Count();
			for (int index = 0; index < count; index++)
			{
				var countNeurons = layersInfo[index].CountNeuronsInLayer;
				var currentLayer = new List<Neuron>();
				for (int k = 0; k < countNeurons; k++)
				{
					Neuron core;
					if (layersInfo[index].ActivationFunction == null)
					{
						core = new Neuron(previewLayer, GetRandomActivationFunction());
					}
					else
					{
						core = new Neuron(previewLayer, layersInfo[index].ActivationFunction);
					}
					currentLayer.Add(core);
				}
				Layers.Add(currentLayer);
				previewLayer = currentLayer;
			}
		}

		public NeuroNetwork(params LayerInfo[] layersInfo)
		{
			Inizialize(layersInfo);
		}

		/// <summary>
		/// Инициализыция сети с указанной функцией инициализации
		/// </summary>
		/// <param name="neuronsInLayers">Количество значений по количеству слоев. Значения показывают количество нейронов в слое</param>
		/// <param name="actFunction">Будет задана на всех слоях, кроме выходного(в зависимости от параметра outputIsLiner)</param>
		/// <param name="outputIsLiner"></param>
		public NeuroNetwork(bool outputIsLiner, ActivationFunction actFunction, params int[] neuronsInLayers)
		{
			var neuronsInfo = neuronsInLayers.Select((x, index) =>
				(index != neuronsInLayers.Count() - 1) || (!outputIsLiner) ? new LayerInfo() { CountNeuronsInLayer = x, ActivationFunction = actFunction }
				: new LayerInfo() { CountNeuronsInLayer = x, ActivationFunction = new Liner() }
				).ToArray();
			Inizialize(neuronsInfo);
		}

		/// <summary>
		/// Инициализыция сети с рандомными функциями активации
		/// </summary>
		/// <param name="outputIsLiner">Выходной слой - линейный</param>
		/// <param name="neuronsInLayers">Количество значений по количеству слоев. Значения показывают количество нейронов в слое</param>
		public NeuroNetwork(bool outputIsLiner, params int[] neuronsInLayers)
		{
			var neuronsInfo = neuronsInLayers.Select((x, index) =>
				(index != neuronsInLayers.Count() - 1) || (!outputIsLiner) ? new LayerInfo() { CountNeuronsInLayer = x }
				: new LayerInfo() { CountNeuronsInLayer = x, ActivationFunction = new Liner() }
				).ToArray();

			Inizialize(neuronsInfo);
		}

		/// <summary>
		/// Конструктор генерирует сеть со случайными функциями активации
		/// </summary>
		/// <param name="neuronsInLayers">Количество нейронов в слое</param>
		public NeuroNetwork(params int[] neuronsInLayers)
		{
			var neuronsInfo = neuronsInLayers.Select((x, index) => new LayerInfo() { CountNeuronsInLayer = x }).ToArray();
			Inizialize(neuronsInfo);
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

		public NeuroNetwork ModifyNetwork(params int[] neuronsInLayers)
		{
			var newNetwork = new NeuroNetwork(neuronsInLayers);

			int layerIndex = 0;
			foreach (var layer in this.Layers)
			{
				int neuronIndex = 0;
				foreach (var neuron in layer)
				{
					int inpLinkIndex = 0;
					foreach (var inputLink in neuron.InputLinks)
					{
						newNetwork.Layers[layerIndex][neuronIndex].InputLinks[inpLinkIndex++].W = inputLink.W;
					}

					newNetwork.Layers[layerIndex][neuronIndex].ActivationFunc = neuron.ActivationFunc;
					newNetwork.Layers[layerIndex][neuronIndex++].T = neuron.T;
				}
				layerIndex += 1;
			}
			return newNetwork;

		}

		public List<List<Neuron>> Layers = new List<List<Neuron>>();

		public static List<ActivationFunction> ActivationFunctions;

		public static int HistorySize = 10;

		private static ActivationFunction GetFunctionByName(string name)
		{
			return ActivationFunctions.First(x => x.GetType().ToString().Equals(name));
		}

		private ActivationFunction GetRandomActivationFunction(bool withLinerFunction = false)
		{
			var flag = withLinerFunction ? 0 : 1;
			var index = rand.Next(ActivationFunctions.Count - flag) + flag;
			return ActivationFunctions[index];

		}

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

		public void Save(string path)
		{
			var js = new JavaScriptSerializer();
			var obj = new StructForSave
				{
					InizializeInfo = _inizializeInfo.Select(x => new NetworkInfo()
						{
							CountNeuronsInLayer = x.CountNeuronsInLayer
						}).ToArray()
				};

			foreach (var layer in Layers)
			{
				var neurons = new List<List<double>>();
				var t = new List<double>();
				var types = new List<string>();
				foreach (var neuron in layer)
				{
					var w = neuron.InputLinks.Select(inp => inp.W).ToList();
					neurons.Add(w);
					t.Add(neuron.T);
					types.Add(neuron.ActivationFunc.GetType().ToString());
				}
				obj.CoefficientsW.Add(neurons);
				obj.CoefficientsT.Add(t);
				obj.TypesViaIndexes.Add(types);
			}

			var str = js.Serialize(obj);
			using (var writer = new StreamWriter(path))
			{
				writer.Write(str);
				writer.Close();
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

		public static NeuroNetwork Izialize(string path)
		{
			using (var reader = new StreamReader(path))
			{
				StructForSave obj = new JavaScriptSerializer().Deserialize<StructForSave>(reader.ReadToEnd());

				var network = new NeuroNetwork(
					obj.InizializeInfo.Select(x => new LayerInfo()
					{
						CountNeuronsInLayer = x.CountNeuronsInLayer,
					}).ToArray());

				for (int layerIndex = 0; layerIndex < obj.CoefficientsT.Count; layerIndex++)
				{
					for (int neuronIndex = 0; neuronIndex < network.Layers[layerIndex].Count; neuronIndex++)
					{
						network.Layers[layerIndex][neuronIndex].T = obj.CoefficientsT[layerIndex][neuronIndex];
						network.Layers[layerIndex][neuronIndex].ActivationFunc = GetFunctionByName(obj.TypesViaIndexes[layerIndex][neuronIndex]);
						for (int wIndex = 0; wIndex < network.Layers[layerIndex][neuronIndex].InputLinks.Count; wIndex++)
						{
							network.Layers[layerIndex][neuronIndex].InputLinks[wIndex].W = obj.CoefficientsW[layerIndex][neuronIndex][wIndex];
						}
					}
				}
				network.ClearHistory();
				return network;
			}
		}
	}
}
