﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
	internal class Liner : ActivationFunction
	{
		public override double F(double s)
		{
			return s;
		}

		public override double DF(double s)
		{
			return 1;
		}
	}
}
