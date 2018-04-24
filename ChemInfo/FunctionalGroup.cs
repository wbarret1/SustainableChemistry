﻿using System;
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
        [NonSerialized]
        System.Drawing.Image m_FunctGroupImage;
        [NonSerialized] List<string> m_ByProducts;

        public FunctionalGroup(string str)
        {   
            string[] parts = str.Split('\t');
            Name = parts[0].Trim();
            Smart = parts[1].Trim();
            string ReactionName = parts[2].Trim();
            string URL = parts[3].Trim();
            string ReactantA = parts[4].Trim();
            string ReactantB = parts[5].Trim();
            string ReactantC = parts[5].Trim();
            string Catalyst = parts[6].Trim();
            string Solvent = parts[7].Trim();
            string Product = parts[8].Trim();
            string[] ByProducts = new string[]{ parts[9].Trim()};
            m_Reactions = new NamedReactionCollection();
            m_Reactions.Add(new NamedReaction(this, ReactionName, URL, ReactantA, ReactantB, ReactantC, Product, Catalyst, Solvent, ByProducts));
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
        //public string URL { get; set; }
        public string Smart { get; set; }
//        public string ReactionName { get; set; }
//        public string[] Reactants { get; set; }
//        public string ReactantB { get; set; }
//        public string ReactantA { get; set; }
//        public string Catalyst { get; set; }
//        public string Solvent { get; set; }
//        public string Product { get; set; }
//        public string[] ByProducts
//        {
//            get
//            {
//                return m_ByProducts.ToArray<string>();
//            }
//            set
//            {
//                m_ByProducts.Clear();
//                m_ByProducts.AddRange(value);
//            }
//        }

//        public System.Drawing.Image ReactionImage
//        {
//            get
//            {
//                return m_FunctGroupImage;
//            }
//        }

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
