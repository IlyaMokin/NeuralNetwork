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

		public double W
		{
			get
			{
				return _w.Value;
			}
			set
			{
				_w.Value = value;
			}
		}
	}
}
