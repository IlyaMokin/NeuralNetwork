using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			/*xor*/
			var input = new[]{
				new double[]{0,0},
				new double[]{0,1},
				new double[]{1,0},
				new double[]{1,1}
			};
			var output = new double[] { 0, 1, 1, 0 };

			var network = new TestNetwork();
			double err;
			int counter = 0;
			do
			{
				err = network.RunEpoch(input, output);
				if(counter++%10000 == 0){
					Console.WriteLine(err);
				}
			} while (err > 0);
			Console.WriteLine(counter);
		}
	}
}
