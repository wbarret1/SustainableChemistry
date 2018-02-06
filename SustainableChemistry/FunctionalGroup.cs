using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class FunctionalGroup
    {
        public FunctionalGroup(string str)
        {
            string[] parts = str.Split('\t');
            Name = parts[0];
            Smart = parts[2];
            ReactionName = parts[3];
            string[] reacts = parts[4].Split('+');
            for (int i = 0; i < reacts.Length; i++) reacts[i] = reacts[i].Trim();
            Reactants = reacts;
            Catalyst = parts[5];
            Solvent = parts[6];
            Product = parts[7];
            ByProduct = parts[8];
        }

        public string Name { get; set; }
        public System.Drawing.Image Image { get; set; }
        public string Smart { get; set; }
        public string ReactionName { get; set; }
        public string[] Reactants { get; set; }
        public string Catalyst { get; set; }
        public string Solvent { get; set; }
        public string Product { get; set; }
        public string ByProduct { get; set; }
    }
}
