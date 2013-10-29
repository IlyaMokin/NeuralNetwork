﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	public class Sin : ActivationFunction
	{
		public override double F(double s)
		{
			return Math.Sin(s);
		}

		public override double DF(double s)
		{
			return Math.Cos(s);
		}
	}
}
