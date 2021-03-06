﻿using NetworkM.ActivationFunctions;
using NetworkM.Networks;
using NetworkM.SaveAndInizialize;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Henon
{
	class Program
	{
		static IEnumerable<double> GetHenonList(int number)
		{
			var list = new List<double>(500);
			CultureInfo culture = new CultureInfo("en-us");
			int counter = 0;
			using (var reader = new StreamReader("HENON.TXT"))
			{
				while (!reader.EndOfStream && number > counter++)
				{
					list.Add(double.Parse(reader.ReadLine(), culture));
				}
			}
			return list;
		}

		static void Main(string[] args)
		{
			var henon = GetHenonList(4000);
			var helpValue = 0;
			var min = henon.Min();
			henon = henon.Select(x => x + Math.Abs(min));
			var max = henon.Max();

			henon = henon.Select(x => x / max);/**/

			double[][] inputR = henon
						.Select((h, index) => new { groupValue = index % 8 != 0 || index == 0 ? helpValue : ++helpValue, value = h })
						.GroupBy(v => v.groupValue, (key, values) => values.Select(x => x.value).ToArray())
						.Take(499)
						.ToArray();

			double[][] outputR = henon
						.Skip(8)
						.Where((x, index) => index % 8 == 0)
						.Select(x => new[] { x })
						.Take(499)
						.ToArray();
			
			double[][] input = inputR.Take(300).ToArray();
			double[][] output = outputR.Take(300).ToArray();

			var network = new NeuralNetwork(
				new SimpleLayerInfo { CountNeuronsInLayer = 8 },
				new StrictLayerInfo
				{
					Neurons = new[] {
						new NeuronInfo {ActivationFunction = ActivationFunctionEnum.Sin},
						new NeuronInfo {ActivationFunction = ActivationFunctionEnum.Sin},
						new NeuronInfo {ActivationFunction = ActivationFunctionEnum.GiperbalTan}
						}
				},
				new SimpleLayerInfo { CountNeuronsInLayer = 1, ActivationFunction = ActivationFunctionEnum.Liner }
			);

			var teacher = new NetworkM.Teachers.Backpropagation.GradientDescent(network);
			teacher.Alpha = 0.025;
			double err = 0;
			do
			{
				err = teacher.RunEpoch(input, output, true);
				if (teacher.IterationCounter % 100 == 0)
				{
					Console.WriteLine("{0:F6}  /  {1:F6}",err,network.GetAbsoluteError(inputR,outputR));
				}
			} while (err > 0.1);/**/
		}
	}
}
