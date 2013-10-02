using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.Teachers.Genetic
{
	public class Evolutionary
	{
		private NeuroNetwork _network;
		private List<List<Neuron>> _layers;
		private Random rand = new Random(DateTime.Now.Millisecond);
		private int randRate = 1000;
		private int _sizeOfPopulation;
		private int _leadOfPopulation;
		private double _probabilityOfMutation;
		private int _ratesWCount;
		private int _ratesTCount;
		private List<Population> _populations;
		public int IterationCounter = 0;

		private double GenerateNewW()
		{
			var t = 1d / (rand.Next(randRate) + 1d);
			return t;
		}

		private double GenerateNewT()
		{
			var t = 100d / (rand.Next(randRate) + 1d);
			return t;
		}

		private IEnumerable<Population> GenerateNewPopulation()
		{
			return new object[_sizeOfPopulation].Select(
				x => new Population()
				{
					W = new object[_ratesWCount].Select(y => GenerateNewW()).ToArray(),
					T = new object[_ratesTCount].Select(y => GenerateNewT()).ToArray()
				});
		}

		public Evolutionary(NeuroNetwork network, int sizeOfPopulation, int leadOfPopulation, double probabilityOfMutation)
		{
			_network = network;
			_sizeOfPopulation = sizeOfPopulation;
			_leadOfPopulation = leadOfPopulation;
			_probabilityOfMutation = probabilityOfMutation;
			_layers = network.Layers;
			var ratesW = _layers.SelectMany((layer, index) => index != 0 ? layer.SelectMany(neuron => neuron.InputLinks.Select(link => link.W)) : new List<double>());
			var ratesT = _layers.SelectMany((layer, index) => index != 0 ? layer.Select(neuron => neuron.T) : new List<double>());

			_ratesTCount = ratesT.Count();
			_ratesWCount = ratesW.Count();

			_populations = GenerateNewPopulation().ToList();
			_populations.Add(new Population() { T = ratesT.ToArray(), W = ratesW.ToArray() });
		}

		public double Threshold = 0.0000;


		public double RunEpoch(IEnumerable<double[]> inputs, IEnumerable<double[]> outputs)
		{
			var lastPopulation = GetNetworkPopulation();
			var lastErr = _network.GetAbsoluteError(inputs, outputs, Threshold);
			if (IterationCounter == 0)
			{
				_populations = _populations.OrderBy(x => SetRangesInNetwork(x).GetAbsoluteError(inputs, outputs, Threshold)).ToList();
			}
			_populations = CrossPopulations(_populations.Take(_leadOfPopulation).Concat(GenerateNewPopulation()).ToList()).OrderBy(x => SetRangesInNetwork(x).GetAbsoluteError(inputs, outputs, Threshold)).ToList();
			IterationCounter += 1;
			double err = SetRangesInNetwork(_populations[0]).GetAbsoluteError(inputs, outputs, Threshold);	
			
			if (lastErr < err)
			{
				SetRangesInNetwork(lastPopulation);
				return lastErr;
			}
			return err;
		}

		private IEnumerable<Population> CrossPopulations(IList<Population> currentPopulations)
		{
			var newPopulations = new List<Population>();
			newPopulations.Add(currentPopulations[0]);
			for (int i = 0; i < 1000; i++)
			{
				int firstIndex = rand.Next(currentPopulations.Count);
				int secondIndex = rand.Next(currentPopulations.Count);
				var tIndexes = new int[2].Select(x => rand.Next(_ratesTCount)).OrderBy(x => x).ToArray();
				var wIndexes = new int[2].Select(x => rand.Next(_ratesWCount)).OrderBy(x => x).ToArray();

				newPopulations.Add(
					new Population()
					{
						T = new double[0].Concat(
							currentPopulations[firstIndex].T.Take(tIndexes[0])
							.Concat(currentPopulations[secondIndex].T.Skip(tIndexes[0]).Take(tIndexes[1] - tIndexes[0]))
							.Concat(currentPopulations[firstIndex].T.Skip(tIndexes[1]).Take(int.MaxValue))).ToArray(),

						W = new double[0].Concat(
						currentPopulations[firstIndex].W.Take(wIndexes[0])
						.Concat(currentPopulations[secondIndex].W.Skip(wIndexes[0]).Take(wIndexes[1] - wIndexes[0]))
						.Concat(currentPopulations[firstIndex].W.Skip(wIndexes[1]).Take(int.MaxValue))).ToArray()
					});
			}

			//Мутация
			for (int i = 0; i < newPopulations.Count; i++)
			{
				if (rand.Next(100) < _probabilityOfMutation * 100)
				{
					newPopulations[rand.Next(newPopulations.Count)].W[rand.Next(_ratesWCount)] = GenerateNewW();
					newPopulations[rand.Next(newPopulations.Count)].T[rand.Next(_ratesTCount)] = GenerateNewT();
				}
			}

			return newPopulations;
		}

		private NeuroNetwork SetRangesInNetwork(Population population)
		{
			int wCounter = 0;
			int tCounter = 0;
			foreach (var layer in _layers.Skip(1))
			{
				foreach (var neuron in layer)
				{
					neuron.T = population.T[tCounter++];
					foreach (var link in neuron.InputLinks)
					{
						link.W = population.W[wCounter++];
					}
				}
			}

			return _network;
		}

		private Population GetNetworkPopulation()
		{
			var population = new Population();
			var t = new List<double>();
			var w = new List<double>();

			foreach (var layer in _layers.Skip(1))
			{
				foreach (var neuron in layer)
				{
					t.Add(neuron.T);
					foreach (var link in neuron.InputLinks)
					{
						w.Add(link.W);
					}
				}
			}

			population.T = t.ToArray();
			population.W = w.ToArray();

			return population;
		}
	}
}
