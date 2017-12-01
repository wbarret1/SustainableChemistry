using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    [Serializable]
    public class Results
    {

        public Results(string functionalGroup, References references)
        {
            FunctionalGroup = functionalGroup;
            References = references.GetReferences(functionalGroup);
        }

        public string FunctionalGroup;
        public Reference[] References { get; } 
    }
}
