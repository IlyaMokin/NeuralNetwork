using NetworkM.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.NetworkElements
{
	internal class Link
	{
		private SharedValue<double> _w;
		public Link(Neuron neuron, SharedValue<double> sharedValue)
		{
			_w = sharedValue;
			Neuron = neuron;
		}

		public Neuron Neuron;

		public bool Undertow(int on)
		{
			int undertowOn = _wHistory.Count - on;
			bool isUndertow = undertowOn > -1;
			if (isUndertow)
			{
				_w.Value = _wHistory.ElementAt(undertowOn);
				for (int i = 0; i < on; i++)
					_wHistory.RemoveLast();
			}
			return isUndertow;
		}

		private LinkedList<double> _wHistory = new LinkedList<double>();
		public IEnumerable<double> WHistory
		{
			get
			{
				return _wHistory;
			}
		}
		public double W
		{
			get
			{
				return _w.Value;
			}
			set
			{
				_wHistory.AddLast(_w.Value);
				if (_wHistory.Count > NeuralNetwork.HistorySize)
					_wHistory.RemoveFirst();

				_w.Value = value;
			}
		}

		public void ClearHistory()
		{
			_wHistory = new LinkedList<double>();
		}
	}
}
