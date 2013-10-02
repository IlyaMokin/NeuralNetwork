using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkM
{
    public class StructForSave
    {
        public NetworkInfo[] InizializeInfo;
        public List<List<List<double>>> CoefficientsW = new List<List<List<double>>>();
        public List<List<double>> CoefficientsT = new List<List<double>>();
        public List<List<string>> TypesViaIndexes = new List<List<string>>();
    }
}
