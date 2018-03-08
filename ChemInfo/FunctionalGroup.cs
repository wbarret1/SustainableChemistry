using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    [Serializable]
    public class FunctionalGroup
    {
        NamedReactionCollection m_Reactions;
        List<Reference> m_refList;
        System.Drawing.Image m_FunctGroupImage;

        public FunctionalGroup(string str)
        {   
            string[] parts = str.Split('\t');
            Name = parts[0].Trim();
            Smart = parts[1].Trim();
            ReactionName = parts[2].Trim();
            ReactantA = parts[3].Trim();
            ReactantB = parts[4].Trim();
            Catalyst = parts[5].Trim();
            Solvent = parts[6].Trim();
            Product = parts[7].Trim();
            ByProduct = parts[8].Trim();
            m_Reactions = new NamedReactionCollection();
            m_Reactions.Add(new NamedReaction(parts[2].Trim(), this, parts[3].Trim(), parts[4].Trim(), parts[7].Trim(), parts[5].Trim(), parts[6].Trim(), parts[8].Trim()));
            string fileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\USEPA\\SustainableChemistry\\Images\\" + Name + ".jpg";
            if (System.IO.File.Exists(fileName)) m_FunctGroupImage = System.Drawing.Image.FromFile(fileName);
        }

        public FunctionalGroup(string func, string directory)
        {
            Name = func;
            m_refList = new List<Reference>();
            string[] imageFile = System.IO.Directory.GetFiles(directory, "*.jpg");
            if (imageFile.Length == 1)
                m_FunctGroupImage = System.Drawing.Image.FromFile(imageFile[0]);
            string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
            foreach (string file in references)
                m_refList.Add(new Reference(this.Name, "", System.IO.File.ReadAllText(file)));
            m_Reactions = new NamedReactionCollection();
        }

        public void AddNamedReaction(NamedReaction reaction)
        {
            m_Reactions.Add(reaction);
        }

        public string Name { get; set; }
        public System.Drawing.Image Image { get; set; }
        public string Smart { get; set; }
        public string ReactionName { get; set; }
//        public string[] Reactants { get; set; }
        public string ReactantB { get; set; }
        public string ReactantA { get; set; }
        public string Catalyst { get; set; }
        public string Solvent { get; set; }
        public string Product { get; set; }
        public string ByProduct { get; set; }

        public System.Drawing.Image ReactionImage
        {
            get
            {
                return m_FunctGroupImage;
            }
        }

        public NamedReactionCollection NamedReactions
        {
            get
            {
                return this.m_Reactions;
            }
        }

        //public Reference[] References
        //{
        //    get
        //    {
        //        return m_refList.ToArray<Reference>();
        //    }
        //}
    }
}
