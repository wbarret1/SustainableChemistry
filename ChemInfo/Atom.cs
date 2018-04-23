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

    static class WeiningerInitialInvariant
    {
        public static long ToInt64(Atom a)
        {
            byte[] retval = new byte[8];
            retval[0] = 0;
            retval[1] = 0;
            retval[2] = 0;
            retval[3] = BitConverter.GetBytes(a.Degree)[3];
            retval[4] = BitConverter.GetBytes(a.numberOfBonds)[3];
            retval[5] = BitConverter.GetBytes(a.AtomicNumber)[3];
            retval[6] = BitConverter.GetBytes(a.Charge)[3];
            retval[7] = BitConverter.GetBytes(a.NumHydrogens)[3];
            return BitConverter.ToInt64(retval, 0);
        }
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
                if (v != null && v.Length != 0)
                {
                    for (int i = 0; i < v.Length - 1; i++)
                    {
                        retVal = retVal + v[i].ToString() + " , ";
                    }
                    retVal = retVal + v[v.Length - 1];
                }
                //else return v[0].
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

    public class WeiningerInvariant : Comparer<WeiningerInvariant>
    {
        Atom m_Atom;
        public int NumberOfConnections { get; set; } = 0;
        public int NumberOfNonHydrogenBonds { get; set; } = 0;
        public int AtomicNumber { get; set; } = 0;
        public int SignOfCharge { get; set; } = 0;
        public int Charge { get; set; } = 0;
        public int NumberOfAttachedHydrogens { get; set; } = 0;

        public WeiningerInvariant(Atom a)
        {
            m_Atom = a;
            this.Reset();
        }

        public void Reset()
        {
            this.NumberOfConnections = m_Atom.Degree;
            this.NumberOfNonHydrogenBonds = m_Atom.NumberOfNonHydrogens;
            this.AtomicNumber = m_Atom.AtomicNumber;
            this.SignOfCharge = m_Atom.Charge;
            this.Charge = m_Atom.Charge;
            this.NumberOfAttachedHydrogens = m_Atom.NumHydrogens;
        }

        public void AddExtendedConnectivity(WeiningerInvariant y)
        {
            this.NumberOfConnections = y.NumberOfConnections;
            this.NumberOfNonHydrogenBonds = y.NumberOfNonHydrogenBonds;
            this.AtomicNumber = y.AtomicNumber;
            this.SignOfCharge = y.SignOfCharge;
            this.Charge = y.Charge;
            this.NumberOfAttachedHydrogens = y.NumberOfAttachedHydrogens;
        }

        public void AddExtendedConnectivity(Atom a)
        {
            this.NumberOfConnections = a.Degree;
            this.NumberOfNonHydrogenBonds = a.NumberOfNonHydrogens;
            this.AtomicNumber = a.AtomicNumber;
            this.SignOfCharge = a.Charge;
            this.Charge = a.Charge;
            this.NumberOfAttachedHydrogens = a.NumHydrogens;
        }

        public static bool operator >(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if (w1.NumberOfConnections > w2.NumberOfConnections) return true;
            // Followed by the number of non-hydrogen bonds.
            if (w1.NumberOfNonHydrogenBonds > w2.NumberOfNonHydrogenBonds) return true;
            //Then atomic number        
            if (w1.AtomicNumber > w2.AtomicNumber) return true;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (w1.SignOfCharge > w2.SignOfCharge) return true;
            // Next is number of attached hydrogens.
            if (w1.NumberOfAttachedHydrogens > w2.NumberOfAttachedHydrogens) return true;
            return false;
        }

        public static bool operator <(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if (w1.NumberOfConnections < w2.NumberOfConnections) return true;
            // Followed by the number of non-hydrogen bonds.
            if (w1.NumberOfNonHydrogenBonds < w2.NumberOfNonHydrogenBonds) return true;
            //Then atomic number        
            if (w1.AtomicNumber < w2.AtomicNumber) return true;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (w1.SignOfCharge < w2.SignOfCharge) return true;
            // Next is number of attached hydrogens.
            if (w1.NumberOfAttachedHydrogens < w2.NumberOfAttachedHydrogens) return true;
            return false;
        }

        public static bool operator ==(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if (w1.NumberOfConnections != w2.NumberOfConnections) return false;
            // Followed by the number of non-hydrogen bonds.
            if (w1.NumberOfNonHydrogenBonds != w2.NumberOfNonHydrogenBonds) return false;
            //Then atomic number        
            if (w1.AtomicNumber != w2.AtomicNumber) return false;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (w1.SignOfCharge != w2.SignOfCharge) return false;
            // Next is number of attached hydrogens.
            if (w1.NumberOfAttachedHydrogens != w2.NumberOfAttachedHydrogens) return false;
            return true;
        }

        public static bool operator !=(WeiningerInvariant w1, WeiningerInvariant w2)
        {
            // Number of connections is the first test.
            if ((w1.NumberOfConnections == w2.NumberOfConnections) &&
            // Followed by the number of non-hydrogen bonds.
                (w1.NumberOfNonHydrogenBonds == w2.NumberOfNonHydrogenBonds) &&
            //Then atomic number        
                (w1.AtomicNumber == w2.AtomicNumber) &&
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
                (w1.SignOfCharge == w2.SignOfCharge) &&
                // Next is number of attached hydrogens.
                (w1.NumberOfAttachedHydrogens == w2.NumberOfAttachedHydrogens)) return false;
            return true;
        }

        public override int Compare(WeiningerInvariant x, WeiningerInvariant y)
        {
            // Number of connections is the first test.
            if (x.NumberOfConnections != y.NumberOfConnections) return x.NumberOfConnections - y.NumberOfConnections;
            // Followed by the number of non-hydrogen bonds.
            if (x.NumberOfNonHydrogenBonds != y.NumberOfNonHydrogenBonds) return x.NumberOfNonHydrogenBonds - y.NumberOfNonHydrogenBonds;
            //Then atomic number        
            if (x.AtomicNumber != y.AtomicNumber) return x.AtomicNumber - y.AtomicNumber;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (x.Charge != y.Charge) return x.Charge - y.Charge;
            // Next is number of attached hydrogens.
            if (x.NumberOfAttachedHydrogens != y.NumberOfAttachedHydrogens) return x.NumberOfAttachedHydrogens - y.NumberOfAttachedHydrogens;
            return 0;
        }
    }

    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(AtomTypeConverter))]
    public class Atom
    {
        BondCollection m_Bonds = new ChemInfo.BondCollection();
        List<Atom> m_ConnectedAtoms = new List<Atom>();
        WeiningerInvariant m_WeiningerInvariant;

        //int degree;
        ELEMENTS e;
        int m_Isotope;
        int m_Charge;
        int m_ExplicitHydrogens;
        Chirality m_Chiral;
        AtomType atomType;
        int color;
        int _x = 0;
        int _y = 0;
        public bool Visited { get; set; } = false;
        System.Random random = new Random();

        // Constructors
        public Atom(string element)
        {
            if (element == "*") e = ELEMENTS.WILD_CARD;
            else e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            //degree = 0;
            m_Isotope = 0;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            atomType = AtomType.NONE;
            m_Chiral = Chirality.UNSPECIFIED;
            this.SetColor(ChemInfo.Element.ElementColor(e));
            this.m_AtomicMass = ChemInfo.Element.ExactMass(e);
            this.m_CovalentRadius = ChemInfo.Element.CovalentRadius(e);
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, AtomType type)
        {
            if (element == "*") e = ELEMENTS.WILD_CARD;
            else e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            m_Isotope = 0;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            atomType = type;
            m_Chiral = Chirality.UNSPECIFIED;
            this.SetColor(ChemInfo.Element.ElementColor(e));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, AtomType type, Chirality chirality)
        {
            //degree = 0;
            if (element == "*") e = ELEMENTS.WILD_CARD;
            else e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            m_Isotope = 0;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            atomType = AtomType.NONE;
            m_Chiral = chirality;
            this.SetColor(ChemInfo.Element.ElementColor(e));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, int isotope)
        {
            //degree = 0;
            if (element == "*") e = ELEMENTS.WILD_CARD;
            else e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = 0;
            isotope = (byte)isotope;
            m_Charge = 0;
            m_Chiral = Chirality.UNSPECIFIED;
            atomType = AtomType.NONE;
            m_Chiral = Chirality.UNSPECIFIED;
            this.SetColor(ChemInfo.Element.ElementColor(e));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        public Atom(string element, AtomType type, int isotope, Chirality chirality, int hCount, int charge, int atomClass)
        {
            //degree = 0;
            if (element == "*") e = ELEMENTS.WILD_CARD;
            else e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            m_ExplicitHydrogens = hCount;
            m_Isotope = isotope;
            m_Charge = charge;
            m_Chiral = chirality;
            atomType = type;
            this.SetColor(ChemInfo.Element.ElementColor(e));
            _x = (int)(random.NextDouble() * 100);
            _y = (int)(random.NextDouble() * 100);
            m_WeiningerInvariant = new WeiningerInvariant(this);
        }

        // Operator Overloads

        //public override bool Equals(System.Object obj)
        //{
        //    // If parameter cannot be cast to ThreeDPoint return false:
        //    Atom p = obj as Atom;
        //    if ((object)p == null)
        //    {
        //        return false;
        //    }

        //    // Return true if the fields match:
        //    return base.Equals(obj) && this.Element == p.Element && this.Degree == p.Degree;
        //}

        //public bool Equals(Atom p)
        //{
        //    // Return true if the fields match:
        //    return base.Equals((Atom)p) && this.Element == p.Element && this.Degree == p.Degree;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode() ^ this.GetHashCode();
        //}

        //public static bool operator ==(Atom a, Atom b)
        //{
        //    // If both are null, or both are same instance, return true.
        //    if (System.Object.ReferenceEquals(a, b))
        //    {
        //        return true;
        //    }

        //    // If one is null, but not both, return false.
        //    if (((object)a == null) || ((object)b == null))
        //    {
        //        return false;
        //    }

        //    // Return true if the fields match:
        //    return a.Element == b.Element && a.Degree == b.Degree;
        //}

        //public static bool operator !=(Atom a, Atom b)
        //{
        //    return !(a == b);
        //}



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
                return (int)m_Isotope;
            }
            set
            {
                m_Isotope = (byte)value;
            }
        }

        public int ExplicitHydrogens
        {
            get
            {
                return m_ExplicitHydrogens;
            }
            set
            {
               m_ExplicitHydrogens = value;
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

        public int WeiningerRank { get; set; } = 0;

        public long WeiningerInitialInvariant
        {
            get
            {
                byte[] retval = new byte[8];
                retval[0] = 0;
                retval[1] = 0;
                retval[2] = BitConverter.GetBytes(this.Degree)[0];
                retval[3] = BitConverter.GetBytes(this.numberOfBonds)[0];
                retval[4] = BitConverter.GetBytes(this.AtomicNumber)[0];
                retval[5] = this.m_Charge > 0 ? (byte)1 : (byte)0;
                retval[6] = BitConverter.GetBytes(this.Charge)[0];
                retval[7] = BitConverter.GetBytes(this.NumHydrogens)[0];
                if (BitConverter.IsLittleEndian) Array.Reverse(retval);
                return BitConverter.ToInt64(retval, 0);
            }
        }


        public Atom[] SetHydrogens()
        {
            List<Atom> hydrogens = new List<Atom>();
            for (int i = this.numberOfBonds; i < this.Valence; i++)
            {
                Atom h = new Atom("H");
                this.AddBond(h, BondType.Single, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.notACenter);
                this.m_ConnectedAtoms.Add(h);
                h.Color = System.Drawing.Color.Fuchsia;
                hydrogens.Add(h);
            }
            return hydrogens.ToArray();
        }

        public Atom RemoveOneHydrogen()
        {
            foreach (Atom a in this.ConnectedAtoms)
            {
                if (a.Element == ELEMENTS.H)
                {
                    Bond bondToRemove = null;
                    foreach (Bond b in this.m_Bonds)
                    {
                        if (b.ConnectedAtom == a) bondToRemove = b;
                    }
                    if (bondToRemove != null)
                    {
                        this.m_Bonds.Remove(bondToRemove);
                        this.m_ConnectedAtoms.Remove(a);
                        return a;
                    }
                }
            }
            return null;
        }

        public Atom[] RemoveHydrogens()
        {
            List<Atom> hydrogens = new List<Atom>();
            foreach (Atom a in this.ConnectedAtoms)
            {
                if (a.Element == ELEMENTS.H)
                {
                    Bond bondToRemove = null;
                    foreach (Bond b in this.m_Bonds)
                    {
                        if (b.ConnectedAtom == a) bondToRemove = b;
                    }
                    if (bondToRemove != null)
                    {
                        this.m_Bonds.Remove(bondToRemove);
                        this.m_ConnectedAtoms.Remove(a);
                        hydrogens.Add(a);
                    }
                }
            }
            return hydrogens.ToArray();
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
                //foreach (Bond b in this.BondedAtoms)
                //{
                //    b.Angle = b.Angle;
                //}
            }
        }

        public double deltaX { get; set; } = 0;
        public double deltaY { get; set; } = 0;

        double m_AtomicMass;
        public double AtomicMass
        {
            get
            {
                return m_AtomicMass;
            }
        }

        double m_CovalentRadius;
        public double CovalentRadius
        {
            get
            {
                return m_CovalentRadius;
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
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public int Angle_2D
        //{
        //    get
        //    {
        //        return _angle;
        //    }
        //    set
        //    {
        //        _angle = value;
        //    }
        //}

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

        public Bond AddBond(Atom atom, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            Bond b = new Bond(this, atom, type, BondStereo.NotStereoOrUseXYZ, BondTopology.Either, BondReactingCenterStatus.notACenter);
            this.BondedAtoms.Add(b);
            return b;
        }

        [System.ComponentModel.TypeConverter(typeof(BondCollectionTypeConverter))]
        public BondCollection BondedAtoms
        {
            get
            {
                return m_Bonds;
            }
        }

        public Bond GetBond(Atom a)
        {
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom == a)
                    return b;
            }
            return null;
        }

        public Bond[] GetBondsToAtomByElement(Atom a)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom.Element == a.Element)
                    bonds.Add(b);
            }
            return bonds.ToArray<Bond>();
        }

        public Bond[] GetBondsToElement(ELEMENTS element)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom.Element == element)
                    bonds.Add(b);
            }
            return bonds.ToArray<Bond>();
        }

        public Bond[] GetBondsToElementSymbol(String element)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
            {
                if (b.ConnectedAtom.Element.ToString() == element)
                    bonds.Add(b);
            }
            return bonds.ToArray<Bond>();
        }

        public Bond[] GetCompatibileBonds(Bond bondToCompare)
        {
            List<Bond> bonds = new List<Bond>();
            foreach (Bond b in m_Bonds)
                if (b.CompareTo(bondToCompare)) bonds.Add(b);
            return bonds.ToArray<Bond>();
        }

        public int Degree
        {
            get
            {
                if(this.ExplicitHydrogens > 0)
                {

                }
                return this.ConnectedAtoms.Length;
            }
        }

        public int numberOfBonds
        {
            get
            {
                int retVal = 0;
                foreach (Atom a in this.ConnectedAtoms)
                {
                    Bond b = null;
                    foreach (Bond testBond in this.BondedAtoms)
                    {
                        if (testBond.ConnectedAtom == a) b = testBond;
                    }
                    if (b == null)
                    {
                        foreach (Bond testBond in a.BondedAtoms)
                        {
                            if (testBond.ConnectedAtom == this) b = testBond;
                        }
                    }
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
                        return new int[] { 3, 5 };
                    case ELEMENTS.O:
                        return new int[] { 2 };
                    case ELEMENTS.S:
                        return new int[] { 2, 4, 6 };
                    case ELEMENTS.P:
                        return new int[] { 3, 5 };
                    case ELEMENTS.F:
                    case ELEMENTS.Cl:
                    case ELEMENTS.I:
                    case ELEMENTS.Br:
                        return new int[] { 1 };
                    default: // handles Chlorine
                        return new int[0];
                }
            }
        }

        public int NumberOfNonHydrogens
        {
            get
            {
                int retVal = 0;
                foreach (Atom a in this.m_ConnectedAtoms)
                {
                    if (a.Element != ELEMENTS.H) retVal++;
                }
                return retVal;
            }
        }


        public int NumHydrogens
        {
            get
            {
                if (hydrogenCount != 0) return hydrogenCount;
                //if (m_ExplicitHydrogens != 0) return m_ExplicitHydrogens;
                if (this.atomType == AtomType.ORGANIC || this.AtomType == AtomType.NONE)
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
                            return 3 - this.ConnectedAtoms.Length;
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
                return m_Charge;
            }
            set
            {
                m_Charge = value;
            }
        }

        public int Valence
        {
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
                return this.m_Chiral;
            }
            set
            {
                m_Chiral = value;
            }
        }

        public WeiningerInvariant WeiningerInvariant
        {
            get
            {
                return this.m_WeiningerInvariant;
            }
        }

        public int WeiningerSymmetryClass { get; set; } = 0;
        public int WeiningerProductOfPrimes
        {
            get
            {
                int[] primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71 };
                int retVal = 1;
                foreach (Atom a in m_ConnectedAtoms)
                {
                    retVal = retVal * a.WeiningerRank;
                }
                return retVal;
            }
        }

        // Values from the Atom Table of a Mole File.
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public double x { get; set; } = 0.0;
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public double y { get; set; } = 0.0;
        //[System.ComponentModel.BrowsableAttribute(false)]
        //public double z { get; set; } = 0.0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int massDiff { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int stereoParity { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int hydrogenCount { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int stereoCareBox { get; set; } = 0;
        //public int HO;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal string rNotUsed { get; set; } = string.Empty;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal string iNotUsed { get; set; } = string.Empty;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int atomMapping { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int inversionRetension { get; set; } = 0;
        [System.ComponentModel.BrowsableAttribute(false)]
        internal int exactChange { get; set; } = 0;
    }
}