using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.NetworkElements
{
	public class SharedValue<T>
	{
		public T Value;
		public SharedValue(T value)
		{
			Value = value;
		}
		public SharedValue(){}
	}
}
