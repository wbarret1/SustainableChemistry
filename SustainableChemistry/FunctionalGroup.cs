using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class FunctionalGroup
    {
        List<Reference> m_refList;
        System.Drawing.Image m_RxnImage;

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

        public FunctionalGroup(string func, string directory)
        {
            Name = func;
            m_refList = new List<Reference>();
            string[] imageFile = System.IO.Directory.GetFiles(directory, "*.jpg");
            if (imageFile.Length == 1)
                m_RxnImage = System.Drawing.Image.FromFile(imageFile[0]);
            string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
            foreach (string file in references)
                m_refList.Add(new Reference(func, "", System.IO.File.ReadAllText(file)));
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

        public System.Drawing.Image ReactionImage
        {
            get
            {
                return m_RxnImage;
            }
        }

        public Reference[] References
        {
            get
            {
                return m_refList.ToArray<Reference>();
            }
        }
    }
}
