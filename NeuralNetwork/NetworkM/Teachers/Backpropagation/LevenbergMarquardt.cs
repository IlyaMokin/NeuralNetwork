using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace NetworkM.Teachers.Backpropagation
{
	public class LevenbergMarquardt
	{
		private List<List<Neuron>> layers;
		private NeuroNetwork network;

		public LevenbergMarquardt(NeuroNetwork network,
			double startAlpha = 5,
			double upperLimitForAlpha = 1e20,
			double lowerLimitForAlpha = 1e-20,
			double multiplier = 5,
			double divider = 7)
		{
			layers = network.Layers;
			this.network = network;
			this.Alpha = startAlpha;
			this.UpperLimitForAlpha = upperLimitForAlpha;
			this.LowerLimitForAlpha = lowerLimitForAlpha;
			this.Divider = divider;
			this.Multiplier = multiplier;
		}

		private void CalculateError(double[] output)
		{
			for (int i = layers.Count - 1; i >= 0; i--)
			{
				for (int k = 0; k < layers[i].Count; k++)
				{
					if (i == layers.Count - 1)
					{
						layers[i][k].Error = layers[i][k].Out - output[k];
					}
					else
					{
						layers[i][k].Error =
							layers[i][k].OutputLinks.Sum(x =>
								x.Neuron.Error * x.W * x.Neuron.ActivationFunc.DF(x.Neuron.S));
					}
				}
			}
		}

		public double Alpha;
		public double UpperLimitForAlpha;
		public double LowerLimitForAlpha;
		public double Multiplier;
		public double Divider;
		public int IterationCounter = 0;
		public double Threshold = 0.0000;
		private double error = 0;
		private double lastError = 0;

		private void GetLMParametres(
			IEnumerable<double[]> inputs,
			IEnumerable<double[]> outputs,
			out List<List<double>> jacobianList,
			out List<double> errList,
			out List<double> rateList)
		{
			jacobianList = new List<List<double>>();
			errList = new List<double>();
			rateList = new List<double>();
			bool ratesIsNotFilled = true;
			for (int i = 0; i < inputs.Count(); i++)
			{
				double[] result = network.GetResult(inputs.ElementAt(i));
				double curElementError = Math.Sqrt(result.Zip(outputs.ElementAt(i), (x1, x2) => Math.Pow(x1 - x2, 2)).Sum());

				var jacobianRow = new List<double>();
				CalculateError(outputs.ElementAt(i));
				errList.Add(layers.Last().Sum(x => x.Error));
				int addedElement = 0;
				foreach (var layer in layers)
				{
					foreach (var neuron in layer)
					{
						jacobianRow.Add(-neuron.ActivationFunc.DF(neuron.S));
						if (ratesIsNotFilled)
						{
							rateList.Add(neuron.T);
						}
						addedElement += 1;


						foreach (var inputLink in neuron.InputLinks)
						{

							jacobianRow.Add(neuron.ActivationFunc.DF(neuron.S) * inputLink.Neuron.Out);

							if (ratesIsNotFilled)
							{
								rateList.Add(inputLink.W);
							}
							addedElement += 1;
						}
					}
				}
				jacobianList.Add(jacobianRow);
				ratesIsNotFilled = false;
			}
		}

		private void SetRatesToNetwork(Matrix<double> rates)
		{
			int ind = 0;
			foreach (var layer in layers)
			{
				foreach (var neuron in layer)
				{
					neuron.T = rates[ind++, 0];
					foreach (var inputLink in neuron.InputLinks)
					{
						inputLink.W = rates[ind++, 0];
					}
				}
			}
		}

		public double RunEpoch(IEnumerable<double[]> inputs, IEnumerable<double[]> outputs)
		{
			bool isRepeatBigCicle = false;
			bool isRepeatMinCicle = false;
			double curAlpha = Alpha;
			if (IterationCounter == 0)
			{
				lastError = network.GetAbsoluteError(inputs, outputs, Threshold);
			}

			do
			{
				List<List<double>> jacobianList;
				List<double> errList;
				List<double> rateList;
				GetLMParametres(inputs, outputs, out jacobianList, out errList, out rateList);

				var jacobian = DenseMatrix.OfRows(jacobianList.Count, jacobianList[0].Count, jacobianList);
				var errorVector = DenseVector.OfEnumerable(errList).ToColumnMatrix();
				var hesseMatrix = jacobian.TransposeThisAndMultiply(jacobian);
				var diagonalMatrix = DenseMatrix.Identity(hesseMatrix.ColumnCount);
				do
				{
					var ratesVector = DenseVector.OfEnumerable(rateList).ToColumnMatrix();

					//perfomance otimize-> //ratesVector = ratesVector - (hesseMatrix + Alpha * diagonalMatrix).Inverse() * jacobian.TransposeThisAndMultiply(errorVector);
					var t1 = diagonalMatrix.Multiply(Alpha);
					hesseMatrix.Add(t1, t1);
					var t2 = jacobian.TransposeThisAndMultiply(errorVector);
					t1.Inverse().Multiply(t2, t2);
					ratesVector.Subtract(t2,ratesVector);/**/
					//-<perfomance optimize

					SetRatesToNetwork(ratesVector);

					error = network.GetAbsoluteError(inputs, outputs, Threshold);

					if (error >= lastError)
					{
						if (Alpha < UpperLimitForAlpha)
						{
							network.Undertow();
							isRepeatMinCicle = true;
							Alpha *= Multiplier;

						}
						/*else
						{
							if (network.Undertow(2))
							{
								lastError = network.GetAbsoluteError(inputs, outputs, Threshold);
								isRepeatMinCicle = false;
								isRepeatBigCicle = true;
							}
							else
							{
								isRepeatMinCicle = false;
								isRepeatBigCicle = false;
								network.Undertow();
								ratesVector = DenseVector.OfEnumerable(rateList).ToColumnMatrix();
								ratesVector = ratesVector - (hesseMatrix + curAlpha * diagonalMatrix).Inverse() * jacobian.TransposeThisAndMultiply(errorVector);
								SetRatesToNetwork(ratesVector);
							}
						}*/
					}
					else
					{
						if (Alpha > LowerLimitForAlpha)
						{
							Alpha /= Divider;
						}
						isRepeatMinCicle = false;
						/*if (error == lastError)
						{
							if (network.Undertow(2))
							{
								lastError = network.GetAbsoluteError(inputs, outputs, Threshold);
								isRepeatMinCicle = false;
								isRepeatBigCicle = true;
							}
							else
							{
								isRepeatMinCicle = false;
								isRepeatBigCicle = false;
								network.Undertow();
								ratesVector = DenseVector.OfEnumerable(rateList).ToColumnMatrix();
								ratesVector = ratesVector - (hesseMatrix + curAlpha * diagonalMatrix).Inverse() * jacobian.TransposeThisAndMultiply(errorVector);
								SetRatesToNetwork(ratesVector);
							}
						}
						else*/
							isRepeatBigCicle = false;
					}
				} while (isRepeatMinCicle);


			} while (isRepeatBigCicle);
			IterationCounter += 1;
			lastError = error;
			return error;
		}
	}
}
