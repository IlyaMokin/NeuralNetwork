using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectionOfTwoLines
{
	class ReversingSigmoid : NetworkM.ActivationFunctions.Sigmoid
	{
		public override double F(double s)
		{
			return -base.F(s);
		}

		public override double DF(double s)
		{
			return -base.DF(s);
		}
	}
}
