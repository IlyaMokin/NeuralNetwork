using NetworkM.NetworkElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	internal class Sin : ActivationFunction
	{
		private double _aRate = 1;
		private double _minStep = 1e-16;
		public Sin(Neuron neuron, string aRate)
		{
			if (aRate!=null){
				_aRate = double.Parse(aRate);
			}
			neuron.NeuronStimulus += (alpha) =>
			{
				_aRate -= alpha * DFA(neuron.S) * neuron.Error;
			};
		}
		public override double F(double s)
		{
			var val = _aRate * s;
			return
				val < Math.PI && val > 0 ? Math.Sin(val)
				: val < 0 ? -_minStep
				: _minStep;
		}

		public override double DF(double s)
		{
			var val = _aRate * s;
			return
				val < Math.PI && val > 0 ? s * Math.Cos(val)
				: val < 0 ? 1
				: -1;
		}

		private double DFA(double s)
		{
			var val = _aRate * s;
			return
				val < Math.PI && val > 0 ? _aRate * Math.Cos(_aRate * s)
				: val < 0                ? 1
				:                         -1;
		}

		public override string SerializeParams()
		{
			return _aRate.ToString();
		}
	}
}
