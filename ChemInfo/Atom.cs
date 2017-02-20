﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    public enum AtomType
    {
        NONE = 0,
        ORGANIC = 1,
        AROMATIC = 2
    }

    public enum Chirality
    {
        NONE = 0,
        CLOCKWISE = 1,
        COUNTER_CLOCKWISE = 2
    }


    public class Atom
    {
        List<Bond> bondedAtoms;
        AtomType atomType;
        Chirality chiral;

        public Atom(string element)
        {
            bondedAtoms = new List<Bond>();
            this.SetElement(element);
            atomType = AtomType.NONE;
            chiral = Chirality.NONE;
        }

        public Atom(string element, AtomType type)
        {
            bondedAtoms = new List<Bond>();
            this.SetElement(element);
            atomType = type;
            chiral = Chirality.NONE;
        }

        public Atom(string element, AtomType type, Chirality chirality)
        {
            bondedAtoms = new List<Bond>();
            this.SetElement(element);
            atomType = type;
            chiral = chirality;
        }

        ELEMENTS e;
        void SetElement(string element)
        {
            e = (ELEMENTS)Enum.Parse(typeof(ELEMENTS), element);
            string name = ChemInfo.Element.Name(e);
        }

        public ELEMENTS Element { get { return e; } }
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

        public double x { get; set; } = 0.0;
        public double y { get; set; } = 0.0;
        public double z { get; set; } = 0.0;
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
        public int massDiff { get; set; } = 0;
        public int charge { get; set; } = 0;
        public int stereoParity { get; set; } = 0;
        public int hydrogenCount { get; set; } = 0;
        public int stereoCareBox { get; set; } = 0;
        public int valence { get; set; } = 0;
        //public int HO;
        public string rNotUsed { get; set; } = string.Empty;
        public string iNotUsed { get; set; } = string.Empty;
        public int atomMapping { get; set; } = 0;
        public int inversionRetension { get; set; } = 0;
        public int exactChange { get; set; } = 0;

        public void AddBond(Atom atom, BondType type)
        {
            Bond b = new Bond();
            b.connectedAtom = atom;
            b.bondType = type;
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
            this.bondedAtoms.Add(b);
        }

        public Atom[] BondedAtoms
        {
            get
            {
                List<Atom> atoms = new List<Atom>();
                foreach (Bond bond in bondedAtoms)
                    atoms.Add(bond.connectedAtom);
                return atoms.ToArray<Atom>();
            }
        }

        int NumHydrogens
        {
            get
            {
                int retVal = 4;
                switch (this.Element)
                {
                    case ELEMENTS.B:
                        {
                            if (this.atomType == AtomType.AROMATIC)
                            {
                                return 3 - this.BondedAtoms.Length;
                            }
                            foreach (Bond b in this.bondedAtoms)
                            {
                                retVal = retVal - (int)b.bondType;
                            }
                            return retVal;
                        }
                    case ELEMENTS.C:
                        {
                            if (this.atomType == AtomType.AROMATIC)
                            {
                                return 3 - this.BondedAtoms.Length;
                            }
                            foreach (Bond b in this.bondedAtoms)
                            {
                                retVal = retVal - (int)b.bondType;
                            }
                            return retVal;
                        }
                    case ELEMENTS.N:
                        {
                            retVal = 3;
                            if (this.atomType == AtomType.AROMATIC)
                            {
                                return 0;
                            }
                            foreach (Bond b in this.bondedAtoms)
                            {
                                retVal = retVal - (int)b.bondType;
                                return retVal;
                            }
                        }
                        return retVal;
                    case ELEMENTS.O:
                        {
                            retVal = 2;
                            if (this.atomType == AtomType.AROMATIC)
                            {
                                return 0;
                            }
                            foreach (Bond b in this.bondedAtoms)
                            {
                                retVal = retVal - (int)b.bondType;
                                return retVal;
                            }
                        }
                        return retVal;
                    case ELEMENTS.S:
                        {
                            retVal = 2;
                            if (this.atomType == AtomType.AROMATIC)
                            {
                                return 0;
                            }
                            foreach (Bond b in this.bondedAtoms)
                            {
                                retVal = retVal - (int)b.bondType;
                                return retVal;
                            }
                        }
                        return retVal;
                    case ELEMENTS.P:
                        {
                            retVal = 2;
                            if (this.atomType == AtomType.AROMATIC)
                            {
                                return 0;
                            }
                            foreach (Bond b in this.bondedAtoms)
                            {
                                retVal = retVal - (int)b.bondType;
                                return retVal;
                            }
                        }
                        return retVal;
                    default:
                        return 1 - BondedAtoms.Length;
                }
            }
        }
    }
}
