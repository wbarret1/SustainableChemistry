using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    /*------------------------------------------------------------
    * state.h
    * Definition of an abstract class representing a state of the 
    * matching process between two ARGs.
    * See: argraph.h 
     *
    * Author: P. Foggia
    * $Id: state.h,v 1.3 1998/09/29 09:50:16 foggia Exp $
    *-----------------------------------------------------------------*/
    /*----------------------------------------------------------
    * class State
    * An abstract representation of the SSR current state.
    * NOTE: Respect to pre-2.0 version of the library, class
    *   State assumes explicitly a depth-first search. The
    *   BackTrack method has been added to allow a state 
    *   to clean up things before reverting to its parent. This
    *   can be used, for instance, for sharing resources between
    *   the parent and the child. The BackTrack implementation
    *   can safely assume that at most one AddPair has been
    *   performed on the state.
    ---------------------------------------------------------*/
    public abstract class State
    {
        public abstract State Clone();
        public abstract Molecule GetMolecule1();
        public abstract Molecule GetMolecule2();
        public abstract bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null);
        public abstract bool isFeasiblePair(Atom atom1, Atom atom2);
        public abstract void AddPair(Atom atom1, Atom atom2);
        public abstract bool IsGoal();
        public abstract bool IsDead();
        public abstract int CoreLen();
        public abstract Atom[] GetCoreSet();
        public abstract void BackTrack();
    }


    public class VF2SubState : State
    {
        Molecule m1, m2;
        Atom addedAtom1;
        bool nodesSorted;
        Stack<Atom> core_1;
        Stack<Atom> core_2;
        Stack<Atom> in_1;
        Stack<Atom> in_2;
        Stack<Atom> out_1;
        Stack<Atom> out_2;
        Stack<Atom> t_1;
        Stack<Atom> t_2;
        int t1_both, t2_both;

        public VF2SubState(Molecule molecule1, Molecule molecule2, bool SortNodes)
        {
            m1 = molecule1;
            m2 = molecule2;
            t1_both = 0;
            t2_both = 0;
            core_1 = new Stack<Atom>();
            core_2 = new Stack<Atom>();
            in_1 = new Stack<Atom>();
            in_2 = new Stack<Atom>();
            out_1 = new Stack<Atom>();
            out_2 = new Stack<Atom>();
            t_1 = new Stack<Atom>();
            t_2 = new Stack<Atom>();
            addedAtom1 = null;
            nodesSorted = SortNodes;
        }

        public VF2SubState(State s)
        {
            VF2SubState vf2 = (VF2SubState)s;
            m1 = s.GetMolecule1();
            m2 = s.GetMolecule2();
            t1_both = vf2.t1_both;
            t2_both = vf2.t2_both;
            core_1 = vf2.core_1;
            core_2 = vf2.core_2;
            in_1 = vf2.in_1;
            in_2 = vf2.in_2;
            out_1 = vf2.out_1;
            out_2 = vf2.out_2;
            t_1 = vf2.t_1;
            t_2 = vf2.t_2;
            addedAtom1 = vf2.addedAtom1;
            nodesSorted = vf2.nodesSorted;
        }

        public override State Clone()
        {
            return new VF2SubState(this);
        }

        public override Molecule GetMolecule1() { return m1; }
        public override Molecule GetMolecule2() { return m2; }
        public override bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null)
        {
            if (prevAtom1 == null) prevAtom1 = m1.GetAtoms()[0];
            if (prevAtom2 == null)
                prevAtom2 = m2.GetAtoms()[0];
            else
                prevAtom2 = m2.getNextAtom(prevAtom2);


            if (t_1.Count > core_1.Count && t_2.Count > core_1.Count)
            {
                while (prevAtom1 != null || (out_1.Peek() != m1.GetAtoms()[0]) && in_1.Peek() != m1.GetAtoms()[0])
                {
                    prevAtom1 = m1.getNextAtom(prevAtom1);
                    prevAtom2 = m2.GetAtoms()[0];
                }
            }
            else if (out_1.Count > core_1.Count && out_2.Count > core_1.Count)
            {
                while (prevAtom1 != null || (out_1.Peek() != m1.GetAtoms()[0]))
                {
                    prevAtom1 = m1.getNextAtom(prevAtom1);
                    prevAtom2 = m2.GetAtoms()[0];
                }
            }
            else if (in_1.Count > core_1.Count && in_2.Count > core_1.Count)
            {
                while (prevAtom1 != null || (in_1.Peek() != m1.GetAtoms()[0]))
                {
                    prevAtom1 = m1.getNextAtom(prevAtom1);
                    prevAtom2 = m2.GetAtoms()[0];
                }
            }
            else if (prevAtom1 == m1.GetAtoms()[0] && nodesSorted)
            {

            }
            else
            {
                while (prevAtom1 != m1.GetAtoms()[m1.GetAtoms().Length - 1])
                {
                    prevAtom1 = m1.getNextAtom(prevAtom1);
                    prevAtom2 = m2.GetAtoms()[0];
                }
            }

            if (t_1.Count > core_1.Count && t_2.Count > core_1.Count)
            {
                while (prevAtom2 != null || (out_1.Peek() != m2.GetAtoms()[0]) && in_1.Peek() != m2.GetAtoms()[0])
                {
                    prevAtom2 = m2.getNextAtom(prevAtom2);
                }
            }
            else if (out_1.Count > core_1.Count && out_2.Count > core_1.Count)
            {
                while (prevAtom2 != null || (out_1.Peek() != m2.GetAtoms()[0]))
                {
                    prevAtom2 = m2.getNextAtom(prevAtom2);
                }
            }
            else if (in_1.Count > core_1.Count && in_2.Count > core_1.Count)
            {
                while (prevAtom2 != null || (in_1.Peek() != m2.GetAtoms()[0]))
                {
                    prevAtom2 = m2.getNextAtom(prevAtom2);
                }
            }
            else
            {
                while (prevAtom2 != m2.GetAtoms()[m2.GetAtoms().Length - 1])
                {
                    prevAtom2 = m2.getNextAtom(prevAtom1);
                }
            }

            if ((prevAtom1 != m1.GetAtoms()[m1.GetAtoms().Length - 1]) && (prevAtom2 != m2.GetAtoms()[m2.GetAtoms().Length - 1]))
            {
                nextAtom1 = prevAtom1;
                nextAtom2 = prevAtom2;
                return true;
            }
            return false;
        }

        public override bool isFeasiblePair(Atom atom1, Atom atom2)
        {
            if (!this.m1.CompatibleAtom(atom1, atom2))
                return false;
            Atom[] other2 = atom2.ConnectedAtoms;
            bool[] matched = new bool[other2.Length];
            foreach (Atom other1 in atom1.ConnectedAtoms)
            {
                for (int i = 0; i < other2.Length; i++)
                {
                    if (other1.Element == other2[i].Element)
                    {
                        if (!matched[i] && atom1.GetBond(other1).BondType == atom2.GetBond(other2[i]).BondType)
                        {
                            matched[i] = true;
                        }
                    }
                }
            }
            foreach (bool m in matched)
                if (!m) return false;
            return true;
        }

        public override void AddPair(Atom atom1, Atom atom2)
        {
            if (!in_1.Contains(atom1))
            {
                in_1.Push(atom1);
                if (out_1.Contains(atom1))
                    t1_both++;
            }
            if (!out_1.Contains(atom1))
            {
                out_1.Push(atom1);
                if (in_1.Contains(atom1))
                    t1_both++;
            }
            if (!in_2.Contains(atom1))
            {
                in_2.Push(atom2);
                if (out_2.Contains(atom1))
                    t2_both++;
            }
            if (!out_2.Contains(atom1))
            {
                out_2.Push(atom2);
                if (in_2.Contains(atom1))
                    t2_both++;
            }
            if (!in_1.Contains(atom1)) core_1.Push(atom2);
            if (!in_1.Contains(atom1)) core_2.Push(atom1);
            if (!in_1.Contains(atom1)) addedAtom1 = atom1;            
        }

        public override bool IsGoal()
        {
            return core_1.Count == m1.GetAtoms().Length && core_2.Count == m2.GetAtoms().Length;
        }

        public override bool IsDead()
        {
            return m1.GetAtoms().Length > m2.GetAtoms().Length || t_1.Count > t_2.Count || out_1.Count > out_2.Count || in_1.Count > in_2.Count;
        }

        public override int CoreLen()
        {
            return core_1.Count;
        }

        public override Atom[] GetCoreSet()
        {
            return core_1.ToArray<Atom>();
        }

        public override void BackTrack()
        {
            if (addedAtom1 == null) return;
            while (core_1.Peek() != addedAtom1 && core_1.Count > 0)
            {
                in_1.Pop();
                out_1.Pop();
                in_2.Pop();
                out_2.Pop();
                core_1.Pop();
                core_2.Pop();
            }
        }
    }
}