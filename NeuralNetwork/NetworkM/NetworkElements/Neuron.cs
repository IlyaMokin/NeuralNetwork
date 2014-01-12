using NetworkM.ActivationFunctions;
using NetworkM.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.NetworkElements
{
	internal class Neuron
	{
		private static Random _rand = new Random(DateTime.Now.Millisecond);
		public Neuron() { }
		public Neuron(
			List<Neuron> previewLayer, 
			ActivationFunctionEnum func, 
			string activationFunctionArguments,
			FunctionsFactory functionsFactory, 
			double? t = null, 
			double[] inputWeights = null)
		{
			int inputWIndex = 0;
			T = t ?? _rand.NextDouble();
			foreach (var previewLayerNeuron in previewLayer)
			{
				var sharedW = new SharedValue<double>(inputWeights != null ? inputWeights[inputWIndex++] : _rand.NextDouble());

				previewLayerNeuron.OutputLinks.Add(new Link(this, sharedW));
				this.InputLinks.Add(new Link(previewLayerNeuron, sharedW));
			}
			ActivationFunc = functionsFactory.GetActivationFunction(func, activationFunctionArguments, this);
		}

		public Action<double> NeuronStimulus = delegate { };

		public List<Link> InputLinks = new List<Link>();
		public List<Link> OutputLinks = new List<Link>();

		public double T {get;set;}
		public double S;
		public double Out;
		public double Error = 0;
		public ActivationFunction ActivationFunc;
	}
}
