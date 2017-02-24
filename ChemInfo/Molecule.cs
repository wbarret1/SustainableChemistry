using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{

    //functional group based upon Kidus' fragments from TEST.
    public struct group
    {
        public string name;
        public string[] fragments;
        public string[] testFragments;
        public int[] count;
    }

    public class Molecule
    {
        List<Atom> atoms;
        List<List<Atom>> cycles;
        List<List<Atom>> fusedRings;
        bool ringsFound;

        public Molecule()
        {
            atoms = new List<Atom>();
            ringsFound = false;
        }

        public Atom[] GetAtoms()
        {
            return this.atoms.ToArray<Atom>();
        }

        public void AddAtom(Atom a)
        {
            if (this.atoms.Contains(a)) return;
            atoms.Add(a);
            ringsFound = false;
        }

        public void AddAtom(string element)
        {
            Atom a = new Atom(element);
            if (this.atoms.Contains(a)) return;
            atoms.Add(a);
            ringsFound = false;
            return;
        }

        public void AddBond(Atom atomOne, Atom atomTwo, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            if (atomTwo != null)
            {
                atomOne.AddBond(atomTwo, type);
                atomTwo.AddBond(atomOne, type);
                ringsFound = false;
            }
        }

        public Atom[][] FindRings()
        {
            if (!ringsFound)
            {
                Stack<Atom> myStack = new Stack<Atom>();
                bool[] finished = new bool[atoms.Count];
                for (int i = 0; i < atoms.Count; i++) finished[i] = false;
                cycles = new List<List<Atom>>();
                foreach (Atom a in atoms)
                {
                    depthFirstSearch(a, a, finished, myStack, cycles);
                }
                this.ExtractRings();
                this.FusedRings();
                ringsFound = true;
            }
            return this.convertToArrayArray(cycles);
        }

        void depthFirstSearch(Atom current, Atom parent, bool[] f, Stack<Atom> stack, List<List<Atom>> cycles)
        {
            stack.Push(current);
            foreach (Bond next in current.BondedAtoms)
            {
                if (next.ConnectedAtom != parent)
                {
                    if (stack.Contains(next.ConnectedAtom))
                    {
                        List<Atom> cycle = new List<Atom>();
                        foreach (Atom a in stack)
                        {
                            cycle.Add(a);
                            if (a == next.ConnectedAtom) break;
                        }
                        cycles.Add(cycle);
                        stack.Pop();
                        return;
                    }
                    else if (!f[atoms.IndexOf(next.ConnectedAtom)])
                    {
                        depthFirstSearch(next.ConnectedAtom, current, f, stack, cycles);
                    }
                }
            }
            if (stack.Count != 0) stack.Pop();
            f[atoms.IndexOf(current)] = true;
        }

        T[][] convertToArrayArray<T>(List<List<T>> lists)
        {
            T[][] retVal = new T[lists.Count][];
            for (int i = 0; i < lists.Count; i++)
            {
                retVal[i] = lists[i].ToArray<T>();
            }
            return retVal;
        }

        List<T> matches<T>(List<T> list1, List<T> list2)
        {
            List<T> retVal = new List<T>();
            foreach (T val in list1)
            {
                int location = list2.IndexOf(val);
                if (location >= 0)
                {
                    retVal.Add(val);
                }
            }
            return retVal;
        }

        void ExtractRings()
        {
            cycles.Sort(CompareListsByLength);
            for (int i = 0; i < cycles.Count; i++)
            {
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    List<Atom> m = matches(cycles[j], cycles[i]);
                    if (m.Count > 2)
                    {
                        if (m.Count == cycles[i].Count)
                        {
                            BondCollection start = m[0].BondedAtoms;
                            int first = cycles[j].IndexOf(m[0]);
                            int last = cycles[j].IndexOf(m[m.Count - 1]);
                            if (last < first)
                            {
                                cycles[j].RemoveRange(first, last - first);
                            }
                            else
                            {
                                if (cycles[j][0] == m[0])
                                {
                                    last = cycles[j].IndexOf(m[1]);
                                    int count = cycles[j].Count - last;
                                    cycles[j].RemoveRange(last + 1, count - 1);
                                }
                                else
                                {
                                    List<int> ints = new List<int>();
                                    foreach (Atom a in m)
                                    {
                                        ints.Add(atoms.IndexOf(a) + 1);
                                    }
                                    cycles[j].RemoveRange(cycles[j].IndexOf(m[1]), m.Count - 2);
                                }
                            }
                        }
                        else
                        {
                            BondCollection start = m[0].BondedAtoms;
                            BondCollection end = m[m.Count - 1].BondedAtoms;
                            Bond bridge = null;
                            foreach (Bond n in end)
                            {
                                if (start.Contains(n)) bridge = n;
                            }
                            if (bridge != null)
                            {
                                for (int k = 1; k < m.Count - 1; k++)
                                {
                                    cycles[j].Remove(m[k]);
                                }
                                cycles[j].Insert(cycles[j].IndexOf(m[0]) + 1, bridge.ConnectedAtom);
                            }
                        }
                    }
                }
            }
        }

        void FusedRings()
        {
            fusedRings = new List<List<Atom>>();
            bool[] touched = new bool[cycles.Count];
            bool[] fused = new bool[cycles.Count];
            List<int[]> touching = new List<int[]>();
            List<List<int>> groups = new List<List<int>>();
            for (int i = 0; i < cycles.Count; i++)
            {
                touched[i] = false;
                fused[i] = false;
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    touched[i] = true;
                    List<Atom> m = matches(cycles[j], cycles[i]);
                    if (m.Count > 1)
                    {
                        int[] f = new int[2];
                        f[0] = i;
                        f[1] = j;
                        touching.Add(f);
                    }
                }
            }
            foreach (int[] ar in touching)
            {
                if (!fused[ar[0]] && !fused[ar[1]])
                {
                    List<int> g = new List<int>();
                    fused[ar[0]] = true;
                    fused[ar[1]] = true;
                    g.AddRange(ar);
                    groups.Add(g);
                }
                else if (!fused[ar[0]] && fused[ar[1]])
                {
                    foreach (List<int> g in groups)
                    {
                        if (g.Contains(ar[1])) g.Add(ar[0]);
                        fused[ar[0]] = true;
                    }
                }
                else if (fused[ar[0]] && !fused[ar[1]])
                {
                    foreach (List<int> g in groups)
                    {
                        if (g.Contains(ar[0])) g.Add(ar[1]);
                        fused[ar[1]] = true;
                    }
                }
            }
            foreach (List<int> g in groups)
            {
                List<Atom> atoms = new List<Atom>();
                for (int i = 0; i < g.Count; i++)
                {
                    List<Atom> a = cycles[g[i]];
                    foreach (Atom at in a)
                    {
                        if (!atoms.Contains(at)) atoms.Add(at);
                    }
                }
                fusedRings.Add(atoms);
            }
        }

        int CompareListsByLength<T>(List<T> list1, List<T> list2)
        {
            return list1.Count - list2.Count;
        }

        void canonizerWeinginger()
        {
            // Step 1. Set Initial Invariants and go to Step 3.
            // The initial invariants are handled by the comparer.
            // Step 3. Sort the vector.
            this.atoms.Sort(atomInvariantComparerWeinginger);

            // Step 4. RankException Atomic Vector

            // Step 5. If not invariant, go to Step 2.

            // Step 6. Save partition as symmetry class.

            // Step 7. If highest rank is smaller than the number of nodes, break ties and go to Step 2.

            // Step 8. Else done
            return;

            // Step 2. 
            {

            }
        }

        int atomInvariantComparerWeinginger(Atom atom1, Atom atom2)
        {
            // Number of connections is the first test.
            if (atom1.Degree != atom2.Degree) return atom1.Degree - atom2.Degree;
            // Followed by the number of non-hydrogen bonds.
            if (atom1.numberOfBonds > atom2.numberOfBonds) return atom1.numberOfBonds - atom2.numberOfBonds;
            //Then atomic number        
            if (atom1.AtomicNumber > atom2.AtomicNumber) return atom1.AtomicNumber - atom2.AtomicNumber;
            // Sign of charge, and then charge, but why both. I'm guessing 1980s computer issues that Moore's Law solved.
            // This can be done in one sort, which the same results, especially since the comparer returns an integer.
            if (atom1.Charge > atom2.Charge) return atom1.Charge - atom2.Charge;
            // Next is number of attached hydrogens.
            if (atom1.hydrogenCount > atom2.hydrogenCount) return atom1.hydrogenCount - atom2.hydrogenCount;
            //Then simple connectivity.

            return 0;
        }


    }
}
