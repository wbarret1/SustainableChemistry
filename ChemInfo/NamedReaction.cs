using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    [Serializable]
    public class NamedReaction
    {
        References m_refList;
        List<System.Drawing.Image> m_RxnImage;
        FunctionalGroup m_FunctionalGroup;

        public NamedReaction(string name, FunctionalGroup functGroup, string[] reactants, string product, string catalyst, string solvent, string byPrduct)
        {
            Name = name;
            m_FunctionalGroup = functGroup;
            m_refList = new References();
            m_RxnImage = new List<System.Drawing.Image>();
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry\\" + functGroup;
            if (System.IO.Directory.Exists(directory))
            {
                string[] imageFiles = System.IO.Directory.GetFiles(directory, "*.jpg");
                foreach (string file in imageFiles)
                    m_RxnImage.Add(System.Drawing.Image.FromFile(file));
                string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
                foreach (string file in references)
                    m_refList.Add(new Reference(functGroup, name, System.IO.File.ReadAllText(file)));
            }
            Reactants = reactants;
            Product = product;
            Catalyst = catalyst;
            Solvent = solvent;
            ByProduct = byPrduct;
        }

        public string Name { get; set; }
        public string[] Reactants { get; set; }
        public string Catalyst { get; set; }
        public string Solvent { get; set; }
        public string Product { get; set; }
        public string ByProduct { get; set; }

        public System.Drawing.Image[] ReactionImage
        {
            get
            {
                return m_RxnImage.ToArray<System.Drawing.Image>();
            }
        }

        public References References
        {
            get
            {
                return m_refList;
            }
        }
    }
}
