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
        public abstract Molecule GetMolecule1();
        public abstract Molecule GetMolecule2();
        public abstract bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null);
        public abstract bool isFeasiblePair(Atom atom1, Atom atom2);
        public abstract void AddPair(Atom atom1, Atom atom2);
        public abstract bool IsGoal();
        public abstract bool IsDead();
        public abstract int CoreLen();
        public abstract void GetCoreSet(Atom a1, Atom a2);

        public abstract void BackTrack();
    }

    public class VF2SubState : State
    {
        Molecule m1, m2;
        bool ordered;
        Atom addedAtom1;

        Stack<Atom> core_1;
        Stack<Atom> core_2;
        Stack<Atom> in_1;
        Stack<Atom> in_2;
        Stack<Atom> out_1;
        Stack<Atom> out_2;
        Stack<Atom> t_1;
        Stack<Atom> t_2;



        VF2SubState(Molecule molecule1, Molecule molecule2, bool SortNodes)
        {
            core_1 = new Stack<Atom>();
            core_2 = new Stack<Atom>();
            in_1 = new Stack<Atom>();
            in_2 = new Stack<Atom>();
            out_1 = new Stack<Atom>();
            out_2 = new Stack<Atom>();
            t_1 = new Stack<Atom>();
            t_2 = new Stack<Atom>();
            addedAtom1 = null;
        }

        public override Molecule GetMolecule1() { return m1; }
        public override Molecule GetMolecule2() { return m2; }
        public override bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null)
        {
            if (prevAtom1 == null) prevAtom1 = m1.GetAtoms()[0];
            if (prevAtom2 == null)
                prevAtom2 = m2.GetAtoms()[0];
            else
            {
                prevAtom2 = m2.getNextAtom(prevAtom2);
            }

            if (t_1.Count > core_1.Count && t_2.Count > core_1.Count)
            {
                while ()
            }

            return false;

        }
        public override bool isFeasiblePair(Atom atom1, Atom atom2)
        {

        }
        public override void AddPair(Atom atom1, Atom atom2)
        {
            core_1.Push(atom2);
            core_2.Push(atom1);
            addedAtom1 = atom1;
            if (!in_1.Contains(atom1)){
                in_1.Add(atom1);
            }

            out_1.Add(atom1);


        }
        public override bool IsGoal() { return core_1.Count == m1.GetAtoms().Length && core_2.Count == m2.GetAtoms().Length; }
        public override bool IsDead()
        {

        }
        public override int CoreLen() { return core_1.Count; }
        public override void GetCoreSet(Atom a1, Atom a2)
        {

        }

        public abstract void BackTrack()
        {

        }

    }


}
