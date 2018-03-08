using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{

    public enum AcidBase
    {
        NONE = 0,
        ACID = 1,
        BASE = 2,
        ACID_BASE = 3
    }

    public enum Solvent
    {
        NONE = 0,
        ACETONE = 1,
        ACETONITRILE = 2,
        AQUEOUS_AMMONIA = 3,
        BENZOIC_ACID_TOLUENE = 4,
        DCM = 5,
        DMC = 6,
        DMF = 7,
        DMSO = 8,
        ETHANOL = 9,
        HALO_KETONE = 10,
        METHANOL = 11,
        METHANOL_TRIETHYLAMINE = 12,
        NITRENE = 12,
        NITRITES = 13,
        THF = 14,
        TOLUENE = 15,
        WATER = 16
    }

    [Serializable]
    public class NamedReaction
    {
        References m_refList;
        List<System.Drawing.Image> m_RxnImage;
        FunctionalGroup m_FunctionalGroup;

        public NamedReaction(string name, FunctionalGroup functGroup, string reactA, string reactB, string product, string catalyst, string solvent, string byPrduct)
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
                    m_refList.Add(new Reference(functGroup.Name, name, System.IO.File.ReadAllText(file)));
            }
            //Reactants = reactants;
            ReactantA = reactA;
            ReactantB = reactB;
            Product = product;
            Catalyst = catalyst;
            this.SetAcidBase(catalyst);
            this.SetSolvent(solvent);
            ByProduct = byPrduct;
        }

        public string Name { get; set; }
        //public string[] Reactants { get; set; }
        public string ReactantA { get; set; }
        public string ReactantB { get; set; }
        public string Catalyst { get; set; }
        public Solvent Solvent { get; set; }
        public string Product { get; set; }
        public string ByProduct { get; set; }
        public AcidBase AcidBase { get; set; }

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

        void SetAcidBase(string acidBase)
        {
            this.AcidBase = ChemInfo.AcidBase.NONE;
            if (acidBase.ToLower() == "acid") this.AcidBase = ChemInfo.AcidBase.ACID;
            if (acidBase.ToLower() == "base") this.AcidBase = ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/heat") this.AcidBase = ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/acid") this.AcidBase = ChemInfo.AcidBase.ACID_BASE;
        }

        void SetSolvent(String solvent)
        {
            this.Solvent = Solvent.NONE;
            if (solvent.ToLower() == "aceton") this.Solvent = Solvent.ACETONE;
            if (solvent.ToLower() == "acetonitrile ") this.Solvent = Solvent.ACETONITRILE;
            if (solvent.ToLower() == "aqueous ammonia/ the treated with lead nitrate") this.Solvent = Solvent.AQUEOUS_AMMONIA;
            if (solvent.ToLower() == "benzoic acid /toluene") this.Solvent = Solvent.BENZOIC_ACID_TOLUENE;
            if (solvent.ToLower() == "dcm") this.Solvent = Solvent.DCM;
            if (solvent.ToLower() == "dmc") this.Solvent = Solvent.DMC;
            if (solvent.ToLower() == "dmf") this.Solvent = Solvent.DMF;
            if (solvent.ToLower() == "dmso") this.Solvent = Solvent.DMSO;
            if (solvent.ToLower() == "ethanol") this.Solvent = Solvent.ETHANOL;
            if (solvent.ToLower() == "halo ketones") this.Solvent = Solvent.HALO_KETONE;
            if (solvent.ToLower() == "methanol") this.Solvent = Solvent.METHANOL;
            if (solvent.ToLower() == "methanol/ triethylamine") this.Solvent = Solvent.METHANOL_TRIETHYLAMINE;
            if (solvent.ToLower() == "nitrene") this.Solvent = Solvent.NITRENE;
            if (solvent.ToLower() == "nitrites") this.Solvent = Solvent.NITRITES;
            if (solvent.ToLower() == "thf") this.Solvent = Solvent.THF;
            if (solvent.ToLower() == "toluene") this.Solvent = Solvent.TOLUENE;
            if (solvent.ToLower() == "water") this.Solvent = Solvent.WATER;
        }
    }
}
