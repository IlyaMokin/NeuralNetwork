using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntersectionOfTwoLines
{
	/// <summary>
	////Пример реализации своей функции активации
	/// </summary>
	public class ThresholdFunction : NetworkM.ActivationFunctions.ActivationFunction
	{
		public override double F(double s)
		{
			return
				s < 0 ? 0 : 1;
		}
	}
}
