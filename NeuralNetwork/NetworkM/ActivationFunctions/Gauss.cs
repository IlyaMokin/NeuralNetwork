using NetworkM.NetworkElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	internal class Gauss : ActivationFunction
	{
		private double _rate1 = 0.4;
		private double _rate2 = 1;

		public Gauss(Neuron neuron, string args)
		{
			if (args!=null)
			{
				var doubleArgs = args.Split(';').Select(a=>double.Parse(a));
				_rate1 = doubleArgs.ElementAt(0);
				_rate2 = doubleArgs.ElementAt(1);
			}
			neuron.NeuronStimulus += (alpha) =>
			{
				_rate1 -= alpha * DF_rate1(neuron.S) * neuron.Error;
				_rate2 -= alpha * DF_rate2(neuron.S) * neuron.Error;
			};
		}

		private double f(double s, double _rate1, double _rate2)
		{
			return 1 / (_rate1 * Math.Sqrt(2 * Math.PI)) * Math.Exp(-Math.Pow(s - _rate2, 2) / Math.Pow(_rate1, 2));
		}

		public override double F(double s)
		{
			return f(s, _rate1, _rate2);
		}

		public virtual double DF_rate1(double s)
		{
			double x1 = f(1, _rate1 + _h, 1);
			double x2 = f(1, _rate1, 1);
			double res = (x1 - x2) / _h;
			return res;
		}

		public virtual double DF_rate2(double s)
		{
			double x1 = f(1, 1, _rate2 + _h);
			double x2 = f(1, 1, _rate2);
			double res = (x1 - x2) / _h;
			return res;
		}

		public override string SerializeParams()
		{
			return string.Join(";",_rate1,_rate2);
		}
	}
}
