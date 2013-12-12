using NetworkM.ActivationFunctions;
using NetworkM.Networks;
using NetworkM.SaveAndInizialize;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterRecognition
{
	class Program
	{
		static double[] ConvertBitmapToVector(string bmpPath)
		{
			var bmp = new Bitmap(bmpPath);
			var vector = new List<double>();
			for (var x = 0; x < bmp.Width; x++)
			{
				for (var y = 0; y < bmp.Height; y++)
				{
					var c = bmp.GetPixel(x, y);
					vector.Add((c.R * .3) + (c.G * .59) + (c.B * .11));
				}
			}
			return vector.ToArray();
		}
		static void Main(string[] args)
		{
			var inputs = new[]{
				new double[]{
					1,0,0,1,
					1,0,1,1,
					1,1,0,1,
					1,0,0,1
				},
				new double[]{
					0,1,1,0,
					1,0,0,1,
					1,1,1,1,
					1,0,0,1
				}
			};

			var outputs = new[]{
				new[]{1d},
				new[]{0d}
			};

			var network = new NeuralNetwork(
				new SimpleLayerInfo() { CountNeuronsInLayer = 16 },
				new SimpleLayerInfo() { CountNeuronsInLayer = 5, ActivationFunction = ActivationFunctionEnum.Sigmoid },
				new SimpleLayerInfo() { CountNeuronsInLayer = 1, ActivationFunction = ActivationFunctionEnum.Sigmoid });

			var teacher = new NetworkM.Teachers.Backpropagation.GradientDescent(network);
			teacher.Alpha = 0.15;
			teacher.Threshold = 0.3;

			double err = 0;
			do
			{
				err = teacher.RunEpoch(inputs, outputs);
				if (teacher.IterationCounter % 1000 == 0)
				{
					Console.WriteLine(err);
				}
			} while (err > 0.01);
			
			Console.WriteLine();
		}
	}
}
