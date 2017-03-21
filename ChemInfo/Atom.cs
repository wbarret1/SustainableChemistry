using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{

    public enum AtomType 
    {
        NONE = 0x00,
        ORGANIC = 0x01,
        AROMATIC = 0x03
    }

    [Flags]
    public enum Chirality 
    {
        UNSPECIFIED = 0x00,
        TETRAHEDRAL_CLOCKWISE = 0x01,
        TETRAHEDRAL_COUNTER_CLOCKWISE = 0x02,
        OTHER = 0x04,
        CIP_S = 0x10,
        CIP_R = 0x20
    }

    class IntArrayTypeConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(int[])).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(int[]).IsAssignableFrom(value.GetType())))
            {
                string retVal = string.Empty;
                int[] v = (int[])value;
                if (v.Length > 1)
                {
                    for (int i = 0; i < v.Length - 1; i++)
                    {
                        retVal = retVal + v[i].ToString() + " , ";
                    }
                }
                retVal = retVal + v[v.Length - 1];
                return retVal;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    class AtomTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(string)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(Atom).IsAssignableFrom(value.GetType())))
            {
                return ((Atom)value).AtomicName;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    [System.ComponentModel.TypeConverter (typeof(AtomTypeConverter))]
    public class Atom
    {
        BondCollection m_Bonds = new ChemInfo.BondCollection();
        List<Atom> m_ConnectedAtoms = new List<Atom>();

        int degree;
        ELEMENTS e;
        int isotope;
        int charge;
        Chirality chiral;
        AtomType atomType;
        int color;
        int _x = 0;
        int _y = 0;
        int _angle = 0;
        public bool Visited { get; set; } = false;
        
        // Constructors
        public Atom(string element)
        {
            e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            degree = 0;
            isotope = 0;
            charge = 0;
            chiral = Chirality.UNSPECIFIED;
            atomType = AtomType.NONE;
            chiral = Chirality.UNSPECIFIED;
            this.SetColor(ChemInfo.Element.ElementColor(e));
        }

        public Atom(string element, AtomType type)
        {
            e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            degree = 0;
            isotope = 0;
            charge = 0;
            chiral = Chirality.UNSPECIFIED;
            atomType = type;
            chiral = Chirality.UNSPECIFIED;
            this.SetColor(ChemInfo.Element.ElementColor(e));
        }

        public Atom(string element, AtomType type, Chirality chirality)
        {
            degree = 0;
            isotope = 0;
            charge = 0;
            chiral = Chirality.UNSPECIFIED;
            atomType = AtomType.NONE;
            chiral = chirality;
            this.SetColor(ChemInfo.Element.ElementColor(e));
        }

        [System.ComponentModel.TypeConverter(typeof(Atom))]
        public Atom(string element, int isotope)
        {
            degree = 0;
            isotope = (byte)isotope;
            charge = 0;
            chiral = Chirality.UNSPECIFIED;
            atomType = AtomType.NONE;
            chiral = Chirality.UNSPECIFIED;
            this.SetColor(ChemInfo.Element.ElementColor(e));
        }

        public ELEMENTS Element { get { return e; } }

        public int AtomicNumber
        {
            get
            {
                return (int)e;
            }
        }

        public int Isotope
        {
            get
            {
                return (int)isotope;
            }
            set
            {
                isotope = (byte)value;
            }
        }

        public AtomType AtomType
        {
            get
            {
                return atomType;
            }
            set
            {
                atomType = value;
            }
        }

        public string AtomicSymbol
        {
            get
            {
                return e.ToString();
            }
        }

        public string AtomicName
        {
            get
            {
                return ChemInfo.Element.Name(e);
            }
        }

        void SetColor(int[] argb)
        {
            if (argb.Length == 3) this.color = System.Drawing.Color.FromArgb(argb[0], argb[1], argb[2]).ToArgb();
            else if (argb.Length == 4) this.color = System.Drawing.Color.FromArgb(argb[0], argb[1], argb[2], argb[3]).ToArgb();
            else color = System.Drawing.Color.Black.ToArgb();
        }

        public System.Drawing.Color Color
        {
            get
            {
                return System.Drawing.Color.FromArgb(color);
            }
            set
            {
                color = value.ToArgb();
            }
        }

        public System.Drawing.Point Location2D
        {
            get
            {
                return new System.Drawing.Point(_x, _y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
                foreach (Bond b in this.BondedAtoms)
                {
                    b.Angle = b.Angle;
                }
            }
        }

        //[System.ComponentModel.BrowsableAttribute(false)]
        //public int X_2D
        //{
        //    get
        //    {
        //        return _x;
        //    }
        //    set
        //    {
        //        _x = value;
        //    }
        //}

        //[System.ComponentModel.BrowsableAttribute(false)]
        //public int Y_2D
        //{
        //    get
        //    {
        //        return _y;
        //    }
        //    set
        //    {
        //        _y = value;
        //    }
        //}
        [System.ComponentModel.BrowsableAttribute(false)]
        public int Angle_2D
        {
            get
            {
                return _angle;
            }
            set
            {
                _angle = value;
            }
        }

        public void SetConnectedAtomLocations()
        {
            foreach (Bond b in this.BondedAtoms)
            {
                b.Angle = b.Angle;
            }
        }

        public void AddConnectedAtom(Atom a)
        {
            this.m_ConnectedAtoms.Add(a);
        }

        public Atom[] ConnectedAtoms
        {
            get
            {
                return this.m_ConnectedAtoms.ToArray();
            }
        }

        public void AddBond(Atom atom, BondType type)
        {
            //this.m_ConnectedAtoms.Add(atom);
            Bond b = new Bond(this, atom, type);
            b.StartPoint = this.Location2D;
            if (_angle == 0)
            {
                if (numberOfBonds == 0)
                {
                    atom.Angle_2D = 45;
                    b.Angle = 45;
                }
                if (numberOfBonds == 1)
                {
                    atom.Angle_2D = 315;
                    b.Angle = 315;
                }
            }
            else if (_angle < 90)
            {
                if (numberOfBonds == 0)
                {
                    atom.Angle_2D = 315;
                    b.Angle = 315;
                }
                if (numberOfBonds == 1)
                {
                    atom.Angle_2D = 45;
                    b.Angle = 45;
                }
                if (numberOfBonds == 2)
                {
                    atom.Angle_2D = 45;
                    b.Angle = 45;
                }
            }
            else if (_angle < 180)
            {
                if (numberOfBonds == 0)
                {
                    atom.Angle_2D = 45;
                    b.Angle = 45;
                }
                if (numberOfBonds == 1)
                {
                    atom.Angle_2D = 135;
                    b.Angle = 135;
                }
                if (numberOfBonds == 2)
                {
                    atom.Angle_2D = 225;
                    b.Angle = 225;
                }
            }
            else if (_angle < 270)
            {
                if (numberOfBonds == 0)
                {
                    atom.Angle_2D = 135;
                    b.Angle = 135;
                }
                if (numberOfBonds == 1)
                {
                    atom.Angle_2D = 225;
                    b.Angle = 225;
                }
                if (numberOfBonds == 2)
                {
                    atom.Angle_2D = 315;
                    b.Angle = 315;
                }
            }
            else if (_angle < 360)
            {
                if (numberOfBonds == 0)
                {
                    atom.Angle_2D = 45;
                    b.Angle = 45;
                }
                if (numberOfBonds == 1)
                {
                    atom.Angle_2D = 225;
                    b.Angle = 225;
                }
                if (numberOfBonds == 2)
                {
                    atom.Angle_2D = 315;
                    b.Angle = 315;
                }
            }
            switch (type)
            {
                case BondType.Single:
                    atomType = AtomType.ORGANIC;
                    break;
                case BondType.Aromatic:
                    atomType = AtomType.AROMATIC;
                    break;
                case BondType.Double:
                    atomType = AtomType.ORGANIC;
                    break;
                case BondType.Triple:
                    atomType = AtomType.ORGANIC;
                    break;
            }
            this.BondedAtoms.Add(b);
            degree = (byte)this.BondedAtoms.Count;
        }

        [System.ComponentModel.TypeConverter(typeof(BondCollectionTypeConverter))]
        public BondCollection BondedAtoms
        {
            get
            {
                return m_Bonds;
            }
        }

        public int Degree
        {
            get
            {
                return this.ConnectedAtoms.Length;
            }
        }

        public int numberOfBonds
        {
            get
            {
                int retVal = 0;
                foreach (Bond b in this.BondedAtoms)
                {
                    if ((b.BondType == BondType.Single) || (b.BondType == BondType.Aromatic)) retVal = retVal + 1;
                    if (b.BondType == BondType.Double) retVal = retVal + 2;
                    if (b.BondType == BondType.Triple) retVal = retVal + 3;
                }
                return retVal;
            }
        }

        [System.ComponentModel.TypeConverter(typeof(IntArrayTypeConverter))]
        public int[] possibleValences
        {
            get
            {
                switch (this.Element)
                {
                    case ELEMENTS.B:
                        return new int[] { 3 };
                    case ELEMENTS.C:
                        return new int[] { 4 };
                    case ELEMENTS.N:
                        return new int[] { 3,5 };
                    case ELEMENTS.O:
                        return new int[] { 2 };
                    case ELEMENTS.S:
                        return new int[] { 2,4,6 };
                    case ELEMENTS.P:
                        return new int[] { 3,5 };
                    default:
                        return new int[0];
                }
            }
        }


        public int NumHydrogens
        {
            get
            {
                if (this.atomType == AtomType.ORGANIC)
                {
                    return this.Valence - this.numberOfBonds;
                }
                if (this.atomType == AtomType.AROMATIC)
                {
                    switch (this.Element)
                    {
                        case ELEMENTS.B:
                            return 2;
                        case ELEMENTS.C:
                            return 3 - this.BondedAtoms.Count;
                        case ELEMENTS.N:
                            return 0;
                        case ELEMENTS.O:
                            return 0;
                        case ELEMENTS.S:
                            return 2;
                        case ELEMENTS.P:
                            return 1;
                        default:
                            return 0;
                    }
                }
                return 0;
            }
        }

        public int Charge
        {
            get
            {
                return charge;
            }
            set
            {
                charge = value;
            }
        }

        public int Valence {
            get
            {
                int[] possible = this.possibleValences;
                if (possible.Length == 1) return possible[0];
                foreach (int i in possible)
                {
                    if (this.numberOfBonds <= i) return i;
                }
                return 0;
            }
        }

        public Chirality Chirality
        {
            get
            {
                return this.chiral;
            }
            set
            {
                chiral = value;
            }
        }

        // Values from the Atom Table of a Mole File.
        [System.ComponentModel.BrowsableAttribute(false)]
        public double x { get; set; } = 0.0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public double y { get; set; } = 0.0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public double z { get; set; } = 0.0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int massDiff { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int stereoParity { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int hydrogenCount { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int stereoCareBox { get; set; } = 0;
        //public int HO;
        [System.ComponentModel.BrowsableAttribute(false)]
        public string rNotUsed { get; set; } = string.Empty;
        [System.ComponentModel.BrowsableAttribute(false)]
        public string iNotUsed { get; set; } = string.Empty;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int atomMapping { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int inversionRetension { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        public int exactChange { get; set; } = 0;
    }
}
