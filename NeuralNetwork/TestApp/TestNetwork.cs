using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
	public class TestNetwork
	{
		private double[] _wRates;
		private double _tRate;
		private double _aRate;
		public double Alpha;

		public TestNetwork()
		{
			var rand = new Random(/*2/**/);
			_wRates = new object[2].Select(x => rand.NextDouble()).ToArray();
			_tRate = rand.NextDouble();
			_aRate = rand.NextDouble();
			Alpha = 0.0025;
		}

		private double ActivationFunction(double s)
		{
			var val = _aRate * s;
			return val < Math.PI && val > 0 ? Math.Sin(val) : 0;
		}

		private double DerS(double s)
		{
			var val = _aRate * s;
			return val < Math.PI && val > 0 ? s * Math.Cos(val) : 0;
		}

		private double DerA(double s)
		{
			var val = _aRate * s;
			return val < Math.PI && val > 0 ? _aRate * Math.Cos(_aRate * s) : 0;
		}

		private double GetS(double[] inp)
		{
			return inp.Zip(_wRates, (x, w) => x * w).Sum() - _tRate;
		}

		public double GetResult(double[] inp)
		{
			return ActivationFunction(GetS(inp));
		}

		public double RunEpoch(double[][] inputs, double[] outputs)
		{
			for (var i = 0; i < 2; i += 1)
			{
				var s = GetS(inputs[i]);
				var result = ActivationFunction(s);
				var error = result - outputs[i];
				_aRate -= Alpha * DerA(s) * error;
				_tRate += Alpha * error * DerS(s);
				_wRates.Select(w => w - Alpha * error * result * DerS(s));
			}
			return Math.Abs(inputs.Select((x, index) => GetResult(x) - outputs[index]).Sum());
		}
	}
}
