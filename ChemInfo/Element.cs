using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    [Flags]
    public enum ELEMENTS
    {

        // The element will be the last four bits of the flag
        WILD_CARD = 0,
        H = 1,
        He = 2,
        Li = 3,
        Be = 4,
        B = 5,
        C = 6,
        N = 7,
        O = 8,
        F = 9,
        Ne = 10,
        Na = 11,
        Mg = 12,
        Al = 13,
        Si = 14,
        P = 15,
        S = 16,
        Cl = 17,
        Ar = 18,
        K = 19,
        Ca = 20,
        Sc = 21,
        Ti = 22,
        V = 23,
        Cr = 24,
        Mn = 25,
        Fe = 26,
        Co = 27,
        Ni = 28,
        Cu = 29,
        Zn = 30,
        Ga = 31,
        Ge = 32,
        As = 33,
        Se = 34,
        Br = 35,
        Kr = 36,
        Rb = 37,
        Sr = 38,
        Y = 39,
        Zr = 40,
        Nb = 41,
        Mo = 42,
        Tc = 43,
        Ru = 44,
        Rh = 45,
        Pd = 46,
        Ag = 47,
        Cd = 48,
        In = 49,
        Sn = 50,
        Sb = 51,
        Te = 52,
        I = 53,
        Xe = 54,
        Cs = 55,
        Ba = 56,
        La = 57,
        Ce = 58,
        Pr = 59,
        Nd = 60,
        Pm = 61,
        Sm = 62,
        Eu = 63,
        Gd = 64,
        Tb = 65,
        Dy = 66,
        Ho = 67,
        Er = 68,
        Tm = 69,
        Yb = 70,
        Lu = 71,
        Hf = 72,
        Ta = 73,
        W = 74,
        Re = 75,
        Os = 76,
        Ir = 77,
        Pt = 78,
        Au = 79,
        Hg = 80,
        Tl = 81,
        Pb = 82,
        Bi = 83,
        Po = 84,
        At = 85,
        Rn = 86,
        Fr = 87,
        Ra = 88,
        Ac = 89,
        Th = 90,
        Pa = 91,
        U = 92,
        Np = 93,
        Pu = 94,
        Am = 95,
        Cm = 96,
        Bk = 97,
        Cf = 98,
        Es = 99,
        Fm = 100,
        Md = 101,
        No = 102,
        Lr = 103,
        Rf = 104,
        Db = 105,
        Sg = 106,
        Bh = 107,
        Hs = 108,
        Mt = 109,
        Ds = 110,
        Rg = 111,
        Cn = 112,
        Nh = 113,
        Fl = 114,
        Mc = 115,
        Lv = 116,
        Ts = 117,
        Og = 118,

        //Additional flags set here...
        // The lead bit will tell if the Element is aromatic.
        Aromatic = 0x8000
    };

    public static class Element
    {
        static ChemInfo.list elements;

        static Element()
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ChemInfo.list));
            System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new System.IO.StringReader(Properties.Resources.elements));
            elements = (list)serializer.Deserialize(reader);
        }

        static public bool ValidateSymbol(string symbol)
        {
            if (symbol == "*") return true;
            for (int i = 1; i < 119; i++)
            {
                if (((ELEMENTS)i).ToString() == symbol) return true;
            }
            return false;
        }

        static public ELEMENTS GetElementForSymbol(string symbol)
        {
            if (symbol == "*") return ELEMENTS.WILD_CARD;
            for (int i = 1; i < 119; i++)
            {
                if (((ELEMENTS)i).ToString() == symbol) return (ELEMENTS)i;
            }
            return (ELEMENTS)0;
        }

        static public string Symbol(ELEMENTS e)
        {
            return e.ToString();
        }

        static public string Symbol(int AtomicNumber)
        {
            return ((ELEMENTS)AtomicNumber).ToString();
        }

        static public string Name(ELEMENTS e)
        {
            listAtom atom = (listAtom)elements.atom[(int)e];
            listAtomLabel a = (listAtomLabel)((listAtom)elements.atom[(int)e]).Items[2];
            return a.value;
        }

        static public string Name(int AtomicNumber)
        {
            listAtomLabel a = (listAtomLabel)((listAtom)elements.atom[AtomicNumber]).Items[2];
            return a.value;
        }

        static public string Name(string symbol)
        {
            return Element.Name(Element.GetElementForSymbol(symbol));
        }

        static public String LookupValue(ELEMENTS e, string dictRef)
        {
            listAtom atom = (listAtom)elements.atom[(int)e];
            foreach (object item in atom.Items)
            {
                if (item.GetType() == typeof(ChemInfo.listAtomScalar))
                {
                    listAtomScalar scalar = (listAtomScalar)item;
                    if (scalar.dictRef  == "bo:mass")
                    {
                        return scalar.Value; ;
                    }
                }
            }
            listAtomLabel a = (listAtomLabel)((listAtom)elements.atom[(int)e]).Items[2];
            return string.Empty;
        }

        static public double Mass(ELEMENTS e)
        {
            return Convert.ToDouble(Element.LookupValue(e, "bo:mass"));
        }

        static public double Mass(int AtomicNumber)
        {
            return Element.Mass((ELEMENTS)AtomicNumber);
        }

        static public double Mass(string symbol)
        {
            return Element.Mass(Element.GetElementForSymbol(symbol));
        }

        static public double ExactMass(ELEMENTS e)
        {
            return Convert.ToDouble(Element.LookupValue(e, "bo:exactMass"));
        }

        static public double ExactMass(int AtomicNumber)
        {
            return Element.Mass((ELEMENTS)AtomicNumber);
        }

        static public double ExactMass(string symbol)
        {
            return Element.Mass(Element.GetElementForSymbol(symbol));
        }

        static public int Period(ELEMENTS e)
        {
            return Convert.ToInt32(Element.LookupValue(e, "bo:period"));
        }

        static public int Period(int AtomicNumber)
        {
            return Element.Period((ELEMENTS)AtomicNumber);
        }

        static public int Period(string symbol)
        {
            return Element.Period(Element.GetElementForSymbol(symbol));
        }

        static public int Group(ELEMENTS e)
        {
            return Convert.ToInt32(Element.LookupValue(e, "bo:group"));
        }

        static public int Group(int AtomicNumber)
        {
            return Element.Period((ELEMENTS)AtomicNumber);
        }

        static public int Group(string symbol)
        {
            return Element.Period(Element.GetElementForSymbol(symbol));
        }

        static public string ElecctronConfiguration(ELEMENTS e)
        {
            return Element.LookupValue(e, "bo:electronicConfiguration");
        }

        static public string ElecctronConfiguration(int AtomicNumber)
        {
            return Element.ElecctronConfiguration((ELEMENTS)AtomicNumber);
        }

        static public string ElecctronConfiguration(string symbol)
        {
            return Element.ElecctronConfiguration(Element.GetElementForSymbol(symbol));
        }



        // <atom id = "H" >
        //  < scalar dataType="xsd:Integer" dictRef="bo:atomicNumber">1</scalar>
        //  <label dictRef = "bo:symbol" value="H" />
        //  <label dictRef = "bo:name" xml:lang="en" value="Hydrogen" />
        //  <scalar dataType = "xsd:float" dictRef="bo:mass" units="units:atmass">1.008</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:exactMass" units="units:atmass">1.007825032</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:ionization" units="units:ev">13.5984</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:electronAffinity" units="units:ev" errorValue="3">0.75420375</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:electronegativityPauling" units="boUnits:paulingScaleUnit">2.20</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:nameOrigin" xml:lang="en">Greek 'hydro' and 'gennao' for 'forms water'</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:radiusCovalent" units="units:ang">0.37</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:radiusVDW" units="units:ang">1.2</scalar>
        //  <array title = "color" dictRef="bo:elementColor" size="3" dataType="xsd:float">1.00 1.00 1.00</array>
        //  <scalar dataType = "xsd:float" dictRef="bo:boilingpoint"  units="siUnits:kelvin">20.28</scalar>
        //  <scalar dataType = "xsd:float" dictRef="bo:meltingpoint"  units="siUnits:kelvin">14.01</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:periodTableBlock">s</scalar>
        //  <array dataType = "xsd:string" dictRef="bo:discoveryCountry">uk</array>
        //  <scalar dataType = "xsd:date" dictRef="bo:discoveryDate">1766</scalar>
        //  <array dataType = "xsd:string" dictRef="bo:discoverers">C.Cavendish</array>
        //  <scalar dataType = "xsd:int" dictRef="bo:period">1</scalar>
        //  <scalar dataType = "xsd:int" dictRef="bo:group">1</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:electronicConfiguration">1s1</scalar>
        //  <scalar dataType = "xsd:string" dictRef="bo:family">Non-Metal</scalar>
        //</atom>
    }




    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.xml-cml.org/schema", IsNullable = false)]
    public partial class list
    {

        private listMetadata[] metadataListField;

        private listAtom[] atomField;

        private string idField;

        private string conventionField;

        private string titleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("metadata", IsNullable = false)]
        public listMetadata[] metadataList
        {
            get
            {
                return this.metadataListField;
            }
            set
            {
                this.metadataListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("atom")]
        public listAtom[] atom
        {
            get
            {
                return this.atomField;
            }
            set
            {
                this.atomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string convention
        {
            get
            {
                return this.conventionField;
            }
            set
            {
                this.conventionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listMetadata
    {

        private string nameField;

        private string contentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtom
    {

        private object[] itemsField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("array", typeof(listAtomArray))]
        [System.Xml.Serialization.XmlElementAttribute("label", typeof(listAtomLabel))]
        [System.Xml.Serialization.XmlElementAttribute("scalar", typeof(listAtomScalar))]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtomArray
    {

        private string titleField;

        private string dictRefField;

        private byte sizeField;

        private bool sizeFieldSpecified;

        private string dataTypeField;

        private string delimiterField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictRef
        {
            get
            {
                return this.dictRefField;
            }
            set
            {
                this.dictRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sizeSpecified
        {
            get
            {
                return this.sizeFieldSpecified;
            }
            set
            {
                this.sizeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataType
        {
            get
            {
                return this.dataTypeField;
            }
            set
            {
                this.dataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string delimiter
        {
            get
            {
                return this.delimiterField;
            }
            set
            {
                this.delimiterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtomLabel
    {

        private string dictRefField;

        private string valueField;

        private string langField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictRef
        {
            get
            {
                return this.dictRefField;
            }
            set
            {
                this.dictRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.xml-cml.org/schema")]
    public partial class listAtomScalar
    {

        private string dataTypeField;

        private string dictRefField;

        private string unitsField;

        private byte errorValueField;

        private bool errorValueFieldSpecified;

        private string langField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataType
        {
            get
            {
                return this.dataTypeField;
            }
            set
            {
                this.dataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dictRef
        {
            get
            {
                return this.dictRefField;
            }
            set
            {
                this.dictRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte errorValue
        {
            get
            {
                return this.errorValueField;
            }
            set
            {
                this.errorValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool errorValueSpecified
        {
            get
            {
                return this.errorValueFieldSpecified;
            }
            set
            {
                this.errorValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
