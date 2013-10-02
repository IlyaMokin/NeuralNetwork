using NetworkM.ActivationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM
{
    public class LayerInfo
    {
        public int CountNeuronsInLayer;
        public ActivationFunction ActivationFunction;
        public int RecurrentConnectionWithLayer = -1;
    }
}
