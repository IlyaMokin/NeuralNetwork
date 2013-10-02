using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	public class ExpSin : ActivationFunction
	{
		public override double F(double s)
		{
			//return Math.Exp(Math.Cos(Math.Sin(Math.Pow(s, 2))));
			return Math.Exp(-Math.Sin(Math.Exp(Math.Cos(s))));
		}

		public override double DF(double s)
		{
			return Math.Exp(-Math.Sin(Math.Exp(Math.Cos(s)))) * Math.Cos(Math.Exp(Math.Cos(s))) * Math.Exp(Math.Cos(s)) + Math.Sin(s);
		}
	}
}
