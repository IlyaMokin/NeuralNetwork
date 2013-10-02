using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM.ActivationFunctions
{
    public class Sigmoid: ActivationFunction
    {
        public override double F(double s)
        {
            return 1d / (1d + Math.Exp(-s));
        }

        public override double DF(double s)
        {
            return Math.Exp(-s) / Math.Pow(1 + Math.Exp(-s), 2);
        }
    }
}
