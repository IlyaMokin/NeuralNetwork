using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM
{
	public class Link
	{
		private Func<double> _WGet;
		private Action<double> _WSet;
		public Link(Neuron neuron, Func<double> wGet, Action<double> wSet)
		{
			_WGet = wGet;
			_WSet = wSet;
			Neuron = neuron;
		}

		public Neuron Neuron;

		public bool Undertow(int on)
		{
			int undertowOn = _wHistory.Count - on;
			bool isUndertow = undertowOn > -1;
			if (isUndertow)
			{
				_WSet(_wHistory.ElementAt(undertowOn));
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
				return _WGet();
			}
			set
			{
				_wHistory.AddLast(_WGet());
				if (_wHistory.Count > NeuroNetwork.HistorySize)
					_wHistory.RemoveFirst();

				_WSet(value);
			}
		}

		public void ClearHistory()
		{
			_wHistory = new LinkedList<double>();
		}
	}
}
