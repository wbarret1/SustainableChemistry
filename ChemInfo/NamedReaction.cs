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

       public static class Reactants
    {
		public static string[] ReactantList
		{
			get
			{
				return new string[]
					{
						"1,3 - dibromopropane",
						"1,3 - dichloropropane",
						"1,3 Propanedithiol",
						"2 Alkenes",
						"2 Alcohol",
						"2 Azides",
						"2 Esters",
						"2 Sulfhydryl groups",
						"3 - Bromo - 1 - butene",
						"3 - bromo - 1 - propanol",
						"3 Formaldehyde",
						"ACETAL",
						"Acetone",
						"Acid Chloride",
						"Acid Halides",
						"ACYLOIN",
						"ALCOHOL",
						"ALCOHOL, ALLYLIC",
						"ALDEHYDE",
						"aliphatic amines",
						"Alkaline hydrogen peroxide",
						"alkali Metal salt thiol",
						"ALKANE",
						"ALKENE",
						"ALKENE - ALCOHOL",
						"ALKENE - ALDEHYDE",
						"ALKENE - ALKYNE",
						"ALKENE - AMINE",
						"ALKENE - CARBOXYLIC ACID",
						"ALKENE - KETONE",
						"ALKENE - THIOETHER",
						"Alkenoic acid",
						"Alkyl Diaol",
                        "Alkyl bromide",
                        "Alkyl Halide",
                        "ALKYNE",
                        "ALKYNE - ALCOHOL",
                        "ALLENE",
                        "ALLOPHANATE",
                        "Allyl Acetate",
                        "Alpha Haloester",
                        "Alpha Ketoacid",
                        "AMIDE",
                        "AMIDE - ESTER",
                        "AMIDINE",
                        "AMINE",
                        "AMINE OXIDE",
                        "AMINO - ACID",
                        "AMINO - ALCOHOL",
                        "AMINO - ALDEHYDE",
                        "AMINO - ESTER",
                        "AMINO - ETHER",
                        "AMINO - KETONE",
                        "AMINO - NITRILE",
                        "AMINO - THIOETHER",
                        "AMINO - THIOL",
                        "ammonia",
                        "Ammonium carbamate",
                        "ANHYDRIDES",
                        "Aniline",
                        "ARENES",
                        "AROMATIC",
                        "Aromatic aldehyde",
                        "Aromatic Amine",
                        "aromatic diazonium",
                        "Aryl halides",
                        "AZIDE",
                        "AZIDO - ALCOHOL",
                        "AZIDO - AMINE",
                        "AZIRIDINE",
                        "AZIRINE",
                        "AZO COMPOUND",
                        "azodicarboxylate",
                        "AZOXY COMPOUND",
                        "B - Keto ester",
                        "benzylimidates",
                        "BIARYL",
                        "BISAMIDE",
                        "BORANE",
                        "BORONIC ACID",
                        "Br2",
                        "BUNTE SALT",
                        "Calcium Cyanamide",
                        "CARBAMATES",
                        "Carbenes",
                        "Carbon dioxide",
                        "Carbon monoxide",
                        "carbon disulfide",
                        "CARBONATE",
                        "Carboxyl",
                        "CARBOXYLIC ACID",
                        "CARBOXYLIC ESTER",
                        "CBr4",
                        "Chloramine",
                        "chlorophosphate",
                        "COUMARIN",
                        "CUMULENE",
                        "CYANAMIDE",
                        "Cyanide",
                        "CYANIDES, ACYL",
                        "CYANO IMINE",
                        "CYANOHYDRIN",
                        "CYCLOBUTANE",
                        "Cyclobutanol",
                        "CYCLOBUTENE",
                        "Cyclopentene",
                        "CYCLOPROPANE",
                        "DIALDEHYDE",
                        "Dialkylsulfates",
                        "DIAMINE",
                        "DIAZO COMPOUND",
                        "DIAZONIUM COMPOUND",
                        "DICARBOXYLIC ACID",
                        "DIENE",
                        "DIESTER",
                        "DIHALIDE",
                        "DIHALIDES(GEMINAL)",
                        "DIKETONE",
                        "DIOL",
                        "DISULFIDE",
                        "DITHIOACETAL",
                        "DITHIOCARBAMATE",
                        "DITHIOKETAL",
                        "DITHIOL",
                        "DIYNE",
                        "Emamtopure 1 sulfinamides",
                        "ENAMIDE",
                        "ENAMINE",
                        "ENOL",
                        "ENOL ETHER",
                        "Enolates",
                        "EPISULFIDE",
                        "Epoxide",
                        "EPOXY - ALCOHOL",
                        "EPOXY - AMIDE",
                        "EPOXY - CARBOXYLIC ACID",
                        "EPOXY - KETONE",
                        "EPOXY - NITRILE",
                        "ESTER",
                        "ESTER - AMIDE",
                        "ESTER - SULFIDE",
                        "Ethenol",
                        "ETHER",
                        "ETHER, SILYL",
                        "ETHER - AMINE",
                        "ETHER - ESTER",
                        "ETHER - KETONE",
                        "Ethyl formate",
                        "Ethylene Carbonate",
                        "Formaldhyde",
                        "FORMAMIDE",
                        "Glutamate",
                        "GLYCIDIC ESTER",
                        "HALIDE, ACYL",
                        "HALIDE, ALKYL",
                        "HALIDE, ALLYLIC",
                        "HALIDE, SULFONYL",
                        "HALO - ALDEHYDE",
                        "HALO - ALKYNE",
                        "HALO - AMIDE",
                        "HALO - AMINE",
                        "HALO - AZIDE",
                        "HALO - CARBOXYLIC ACID",
                        "HALO - ETHER",
                        "HALOHYDRIN",
                        "HALO - KETONE",
                        "HALO - LACTAM",
                        "HALO - LACTONE",
                        "HALO - NITRO",
                        "HALO - NITROSO",
                        "HALO - SILANE",
                        "HALO - SULFONE",
                        "HALO - SULFOXIDE",
                        "HETEROCYCLE",
                        "HYDRAZIDE",
                        "HYDRAZINE",
                        "HYDRAZONE",
                        "Hydrogen",
                        "Hydrogen chloride",
                        "Hydrogen Cyanide",
                        "Hydrogen Peroxide",
                        "Hydrogen Sulfide",
                        "HYDROXAMIC ACID",
                        "HYDROXY - AMIDE",
                        "HYDROXY - AMINE",
                        "HYDROXY - AZIRIDINE",
                        "HYDROXY - CARBOXYLIC ACID",
                        "HYDROXY - ESTER",
                        "HYDROXY - KETONE",
                        "HYDROXYLAMINE",
                        "HYDROXY - NITRO",
                        "HYDROXY - PHOSPHONATE",
                        "HYDROXY - THIOCYANATE",
                        "HYDROXY - THIOETHER",
                        "IMIDE",
                        "IMINE",
                        "IMINO ESTER",
                        "ISOCYANATE",
                        "ISONITRILE",
                        "ISOTHIOCYANATE",
                        "KETENE",
                        "KETENIMINE",
                        "KETO - ALDEHYDE",
                        "KETO - AMIDE",
                        "KETO - CARBOXYLIC ACID",
                        "KETO - ESTER",
                        "KETONE",
                        "KETO - SULFONE",
                        "LACTAM",
                        "LACTONE",
                        "Methanol",
                        "Methylfomate",
                        "NITRATE",
                        "NITRILE",
                        "NITRITE",
                        "NITRO",
                        "NITRO - ALCOHOL",
                        "Nitroalkanes",
                        "Nitrobenzene",
                        "NITRONE",
                        "NITROSO - AMINE",
                        "Nitrous Acid",
                        "ORTHO ESTER",
                        "OSAZONE",
                        "OXIME",
                        "Oxazolones",
                        "PEROXIDE",
                        "Phenol",
                        "Phenylboronic acid",
                        "PHOSPHINE",
                        "Phosphine Oxide",
                        "PHOSPHONATE ESTER",
                        "Phosphonic Acid",
                        "PHOSPHORANE",
                        "Potassium Thiocyanate",
                        "Primary Alcohol",
                        "Primary amines",
                        "Propynyl bromide",
                        "Propyne",
                        "QUINONE",
                        "RSO2I",
                        "RX",
                        "Secondary Amine",
                        "SELENIDE",
                        "SELENOCARBONATE",
                        "SELENOETHER - ALDEHYDE",
                        "SELENOETHER - KETONE",
                        "SELENOXIDE",
                        "SILANE",
                        "Silyl chloride",
                        "Silyl Enol Ethers",
                        "Sodium",
                        "Sodium Azide",
                        "Sodium thiosulfate",
                        "Sulfides",
                        "SULFINIC ACID",
                        "SULFONAMIDE",
                        "SULFONATE ESTER",
                        "SULFONE",
                        "Sulfonyl chloride",
                        "SULFONYL IMINE",
                        "SULFOXIDE",
                        "sulfur",
                        "TELLURIDE",
                        "Tertiary Amine",
                        "THIIRANE(EPISULFIDE)",
                        "THIOCARBAMATE",
                        "THIOCARBOXYLIC ACID",
                        "Thiocyanate",
                        "THIOESTER",
                        "THIOETHER(SULFIDE)",
                        "THIOETHER - ALDEHYDE",
                        "THIOETHER - KETONE",
                        "THIOKETONE",
                        "THIOL",
                        "THIOLACTAM",
                        "THIOUREA",
                        "Toulene",
                        "TRIAZOLE",
                        "TRIAZOLINE",
                        "TRIHALIDE",
                        "Trihaloethanol",
                        "TRIOXANE",
                        "UREA",
                        "urethane",
                        "vinyl azides",
                        "VINYL ETHER",
                        "Vinyl Ketone",
                        "Vinyl Halides",
                        "VINYL PHOSPHINE",
                        "VINYL SILANE",
                        "VINYL SULFIDE",
                        "VINYL SULFONE",
                        "Vinyl triflates",
                        "Water",
                        "water + CO2",
                        "X",
                        "X2",
                        "XANTHATE"
                    };
            }
		}
	}

    [Serializable]
    public class NamedReaction
    {
        References m_refList;
        List<System.Drawing.Image> m_RxnImage;
        FunctionalGroup m_FunctionalGroup;

        public NamedReaction(FunctionalGroup functGroup, string name, string url, string reactA, string reactB, string product, string catalyst, string solvent, string byPrduct)
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
            URL = url;
            ReactantA = reactA;
            ReactantB = reactB;
            Product = product;
            Catalyst = catalyst;
            this.SetAcidBase(catalyst);
            this.SetSolvent(solvent);
            ByProduct = byPrduct;
        }

        public string Name { get; set; }
        public string URL { get; set; }
        //public string[] Reactants { get; set; }
        public string ReactantA { get; set; }
        public string ReactantB { get; set; }
        public string Catalyst { get; set; }
        public SOLVENT Solvent { get; set; }
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
            this.Solvent = SOLVENT.NONE;
            if (solvent.ToLower() == "aceton") this.Solvent = SOLVENT.ACETONE;
            if (solvent.ToLower() == "acetonitrile ") this.Solvent = SOLVENT.ACETONITRILE;
            if (solvent.ToLower() == "aqueous ammonia/ the treated with lead nitrate") this.Solvent = SOLVENT.AQUEOUS_AMMONIA;
            if (solvent.ToLower() == "benzoic acid /toluene") this.Solvent = SOLVENT.BENZOIC_ACID_TOLUENE;
            if (solvent.ToLower() == "dcm") this.Solvent = SOLVENT.DCM;
            if (solvent.ToLower() == "dmc") this.Solvent = SOLVENT.DMC;
            if (solvent.ToLower() == "dmf") this.Solvent = SOLVENT.DMF;
            if (solvent.ToLower() == "dmso") this.Solvent = SOLVENT.DMSO;
            if (solvent.ToLower() == "ethanol") this.Solvent = SOLVENT.ETHANOL;
            if (solvent.ToLower() == "halo ketones") this.Solvent = SOLVENT.HALO_KETONE;
            if (solvent.ToLower() == "methanol") this.Solvent = SOLVENT.METHANOL;
            if (solvent.ToLower() == "methanol/ triethylamine") this.Solvent = SOLVENT.METHANOL_TRIETHYLAMINE;
            if (solvent.ToLower() == "nitrene") this.Solvent = SOLVENT.NITRENE;
            if (solvent.ToLower() == "nitrites") this.Solvent = SOLVENT.NITRITES;
            if (solvent.ToLower() == "thf") this.Solvent = SOLVENT.THF;
            if (solvent.ToLower() == "toluene") this.Solvent = SOLVENT.TOLUENE;
            if (solvent.ToLower() == "water") this.Solvent = SOLVENT.WATER;
        }
    }
}