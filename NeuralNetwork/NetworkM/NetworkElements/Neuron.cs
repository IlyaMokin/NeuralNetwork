using NetworkM.ActivationFunctions;
using NetworkM.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.NetworkElements
{
	internal class Neuron
	{
		private static Random _rand = new Random(DateTime.Now.Millisecond);
		public Neuron() { }
		public Neuron(List<Neuron> previewLayer, ActivationFunctionEnum func, string ActivationFunctionArguments, double? t = null, double[] InputWeights = null)
		{
			int inputWIndex = 0;
			_t = t ?? _rand.NextDouble();
			foreach (var previewLayerNeuron in previewLayer)
			{
				var sharedW = new SharedValue<double>(InputWeights != null ? InputWeights[inputWIndex++] : _rand.NextDouble());

				previewLayerNeuron.OutputLinks.Add(new Link(this, sharedW));
				this.InputLinks.Add(new Link(previewLayerNeuron, sharedW));
			}
			ActivationFunc = FunctionsFactory.GetActivationFunction(func, ActivationFunctionArguments, this);
		}

		public Action<double> NeuronStimulus = delegate { };

		public List<Link> InputLinks = new List<Link>();
		public List<Link> OutputLinks = new List<Link>();

		private double _t;

		public bool Undertow(int on)
		{
			int undertowOn = _tHistory.Count - on;
			bool isUndertow = undertowOn > -1;
			if (isUndertow)
			{
				_t = _tHistory.ElementAt(undertowOn);
				for (int i = 0; i < on; i++)
					_tHistory.RemoveLast();
			}

			foreach (var inp in this.InputLinks)
				inp.Undertow(on);

			return isUndertow;
		}
		private LinkedList<double> _tHistory = new LinkedList<double>();

		public IEnumerable<double> THistory
		{
			get
			{
				return _tHistory;
			}
		}
		public double T
		{
			get
			{
				return _t;
			}
			set
			{
				_tHistory.AddLast(_t);
				if (_tHistory.Count > NeuralNetwork.HistorySize)
					_tHistory.RemoveFirst();
				_t = value;
			}
		}
		public double S;
		public double Out;
		public double Error = 0;
		public ActivationFunction ActivationFunc;

		public void ClearHistory()
		{
			_tHistory = new LinkedList<double>();
			foreach (var link in InputLinks)
			{
				link.ClearHistory();
			}
		}
	}
}
