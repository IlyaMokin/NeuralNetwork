﻿using NetworkM.NetworkElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	internal class Threshold : ActivationFunction
	{
		public override double F(double s)
		{
			return s > 0 ? 1 : 0;
		}

		public override double DF(double s)
		{
			return 1;
		}
	}
}
