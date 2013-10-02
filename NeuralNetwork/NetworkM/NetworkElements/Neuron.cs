using NetworkM.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM
{
	public class Neuron
	{
		static Random rand = new Random(DateTime.Now.Millisecond);
		public Neuron() { }
		public Neuron(List<Neuron> previewLayer, ActivationFunction func)
		{
			foreach (var previewLayerNeuron in previewLayer)
			{
				double w = 1d / (rand.Next(10) + 1d);
				var setter = new Action<double>((double x) => { w = x; });
				var getter = new Func<double>(() => { return w; });

				previewLayerNeuron.OutputLinks.Add(new Link(this, getter, setter));
				this.InputLinks.Add(new Link(previewLayerNeuron, getter, setter));
			}
			ActivationFunc = func;
		}
		public List<Link> InputLinks = new List<Link>();
		public List<Link> OutputLinks = new List<Link>();
		private double _t = 0.5d / (rand.Next(10) + 1d);

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
				if (_tHistory.Count > NeuroNetwork.HistorySize)
					_tHistory.RemoveFirst();
				_t = value;
			}
		}
		public double S;
		public double Out;
		public double Error = 0;
		public ActivationFunction ActivationFunc;
		public void ClearHistory(){
			_tHistory = new LinkedList<double>();
			foreach(var link in InputLinks){
				link.ClearHistory();
			}
		}
	}
}
