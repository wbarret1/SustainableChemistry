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
        List<Atom> m_Atoms;
        List<List<Atom>> cycles;
        List<Atom[]> paths;
        bool[,] touching;
        bool[,] fused;
        bool ringsFound;
        bool pathsFound;

        public Molecule()
        {
            m_Atoms = new List<Atom>();
            ringsFound = false;
            pathsFound = false;
        }

        public Atom[] GetAtoms()
        {
            return this.m_Atoms.ToArray<Atom>();
        }

        public void AddAtom(Atom a)
        {
            if (this.m_Atoms.Contains(a)) return;
            m_Atoms.Add(a);
            ringsFound = false;
            pathsFound = false;
        }

        public void AddAtom(string element)
        {
            Atom a = new Atom(element);
            if (this.m_Atoms.Contains(a)) return;
            m_Atoms.Add(a);
            ringsFound = false;
            pathsFound = false;
            return;
        }

        public void AddBond(Atom atomOne, Atom atomTwo, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            if (atomOne != null)
            {
                atomOne.AddBond(atomTwo, type);
                atomOne.AddConnectedAtom(atomTwo);
                atomTwo.AddConnectedAtom(atomOne);
                ringsFound = false;
                pathsFound = false;
                int angle = 360 / atomOne.ConnectedAtoms.Length;
                int bondAngle = 45;
                if (this.m_Atoms.IndexOf(atomOne) % 2 == 1) bondAngle = 315;
                foreach (Bond b in atomOne.BondedAtoms)
                {
                    b.Angle = bondAngle;
                    bondAngle = (bondAngle + angle) % 360;
                }
            }
        }

        public int GetBondAngle(Atom atom1, Atom atom2)
        {
            foreach (Bond b in atom1.BondedAtoms)
            {
                if (b.ConnectedAtom == atom2) return b.Angle;
            }
            foreach (Bond b in atom2.BondedAtoms)
            {
                if (b.ConnectedAtom == atom1) return (b.Angle + 180) % 360;
            }
            return -1;
        }

        public int SetBondAngle(Atom atom1, Atom atom2, int angle)
        {
            foreach (Bond b in atom1.BondedAtoms)
            {
                if (b.ConnectedAtom == atom2)
                {
                    b.Angle = angle;
                    b.SetBondededAtomLocation();
                    return angle;
                }
            }
            foreach (Bond b in atom2.BondedAtoms)
            {
                if (b.ConnectedAtom == atom1)
                {
                    b.Angle = (angle + 180) % 360;
                    b.SetParentAtomLocation();
                    return (angle + 180) % 360;
                }
            }
            return 0;
        }

        public Bond GetBond(Atom atom1, Atom atom2)
        {
            foreach (Bond b in atom1.BondedAtoms)
            {
                if (b.ConnectedAtom == atom2) return b;
            }
            foreach (Bond b in atom2.BondedAtoms)
            {
                if (b.ConnectedAtom == atom1) return b;
            }
            return null;
        }

        public Atom[][] FindRings()
        {
            if (ringsFound) return this.convertToArrayArray(cycles);
            Stack<Atom> myStack = new Stack<Atom>();
            foreach (Atom a in this.m_Atoms) a.Visited = false;
            cycles = new List<List<Atom>>();
            foreach (Atom a in m_Atoms)
            {
                FindRings(a, a, myStack);
            }
            this.ExtractRings();
            this.FusedRings();
            ringsFound = true;
            return this.convertToArrayArray(cycles);
        }

        public void FindAllPaths()
        {
            if (pathsFound) return;
            paths = new List<Atom[]>();
            Stack<Atom> stack = new Stack<Atom>();
            Atom[] ends = this.EndAtoms();
            for (int i = 0; i < ends.Length - 1; i++)
            {
                for (int j = i + 1; j < ends.Length; j++)
                {
                    stack.Clear();
                    foreach (Atom a in this.m_Atoms) a.Visited = false;
                    this.FindPath(ends[i], ends[j], null, stack);
                }
            }
            pathsFound = true;
        }

        public Atom[] EndAtoms()
        {
            List<Atom> atoms = new List<ChemInfo.Atom>();
            foreach (Atom a in this.m_Atoms) if (a.ConnectedAtoms.Length == 1) atoms.Add(a);
            return atoms.ToArray();
        }

        public void FindPath(Atom current, Atom target, Atom parent, Stack<Atom> stack)
        {
            current.Visited = true;
            stack.Push(current);
            if (current == target)
            {
                //we're done... return the path
                Atom[] a = stack.ToArray<Atom>();
                paths.Add(a.Reverse().ToArray());
                return;
            }
            else
            {
                foreach (Atom next in current.ConnectedAtoms)
                {
                    if (next != parent)
                    {
                        if (!next.Visited) FindPath(next, target, current, stack);
                        //
                    }
                }
            }
            stack.Pop();
        }

        public Atom[] FindLongestpath()
        {
            if (!pathsFound) this.FindAllPaths();
            // paths will be 0 if all atoms are in a ring...
            if (paths.Count == 0) return new Atom[0];
            paths.Sort(this.CompareArrayByCount);
            paths.Reverse();
            return paths[0];
        }

        void FindRings(Atom current, Atom parent, Stack<Atom> stack)
        {
            stack.Push(current);
            foreach (Atom next in current.ConnectedAtoms)
            {
                if (next != parent)
                {
                    if (stack.Contains(next))
                    {
                        List<Atom> cycle = new List<Atom>();
                        foreach (Atom a in stack)
                        {
                            cycle.Add(a);
                            if (a == next) break;
                        }
                        cycles.Add(cycle);
                        stack.Pop();
                        return;
                    }
                    else if (!next.Visited)
                    {
                        FindRings(next, current, stack);
                    }
                }
            }
            if (stack.Count != 0) stack.Pop();
            current.Visited = true;
        }

        void breadthFirstSearch(Atom current, Atom parent, Queue<Atom> queue)
        {
            //current.Visited = true;
            queue.Enqueue(current);
            List<Atom> path = new List<ChemInfo.Atom>();
            while (queue.Count > 0)
            {
                Atom v1 = queue.Dequeue();
                path.Add(v1);
                foreach (Atom a in v1.ConnectedAtoms)
                {
                    if (!a.Visited)
                    {
                        queue.Enqueue(a);
                        //a.Visited = true;
                    }
                }
            }
            current.Visited = true;
        }

        Atom[] depthFirstSearch(Atom current, Atom parent, Stack<Atom> stack)
        {
            current.Visited = true;
            stack.Push(current);
            if (current.ConnectedAtoms.Length == 1)
            {
                //we're done... return the path
                Atom[] a = stack.ToArray<Atom>();
                return (a.Reverse().ToArray());
            }
            else
            {
                foreach (Atom next in current.ConnectedAtoms)
                {
                    if (next != parent)
                    {
                        if (!next.Visited) depthFirstSearch(next, current, stack);
                        //
                    }
                }
            }
            stack.Pop();
            return null;
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
                                        ints.Add(m_Atoms.IndexOf(a) + 1);
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
            touching = new bool[cycles.Count, cycles.Count];
            fused = new bool[cycles.Count, cycles.Count];
            for (int i = 0; i < cycles.Count; i++)
            {
                for (int j = i + 1; j < cycles.Count; j++)
                {
                    int common = 0;
                    foreach (Atom a in cycles[j])
                    {
                        if (cycles[i].Contains(a)) common = common + 1;
                    }
                    if (common == 1) touching[i, j] = true;
                    if (common == 2) fused[i, j] = true;
                }
            }
        }

        int CompareArrayByCount<T>(T[] array1, T[] array2)
        {
            return array1.Length - array2.Length;
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
            this.m_Atoms.Sort(atomInvariantComparerWeinginger);

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

        int CompareAtomsByDegree(Atom a1, Atom a2)
        {
            return a1.Degree - a2.Degree;
        }

        public void LocateAtoms2D()
        {
            List<Atom> atoms = new List<Atom>();
            atoms.AddRange(this.m_Atoms.ToArray());
            atoms.Sort(this.CompareAtomsByDegree);
            if (atoms[atoms.Count - 1].Degree == 4)
            {
                int angle = 0;
                foreach (Atom a in atoms[atoms.Count - 1].ConnectedAtoms)
                {
                    this.SetBondAngle(atoms[atoms.Count - 1], a, angle);
                    angle = angle + 60;
                }
            }
            //foreach (Atom a in this.m_Atoms) foreach (Bond b in a.BondedAtoms) b.SetBondededAtomLocation();
        }

        public void LocateAtoms2DOld()
        {
            List<Atom> visited = new List<Atom>();
            foreach (Atom a in this.m_Atoms) a.Visited = false;
            Atom[] backbone = this.FindLongestpath();
            if (backbone.Length > 0) backbone[0].Location2D = new System.Drawing.Point(0, 0);
            else m_Atoms[0].Location2D = new System.Drawing.Point(0, 0);
            for (int i = 0; i < backbone.Length - 1; i++)
            {
                if (backbone[i].ConnectedAtoms.Length < 4)
                {
                    int angle = 45;
                    if (i % 2 == 1) angle = 135;
                    this.SetBondAngle(backbone[i], backbone[i + 1], angle);
                    foreach (Atom a in backbone[i].ConnectedAtoms) if (!backbone.Contains(a)) this.AddBranch(a, backbone[i], i % 2 == 1 ? 180 : 0);
                }
                if (backbone[i].ConnectedAtoms.Length == 4)
                {
                    int angle = 45;
                    if (i % 2 == 1) angle = 135;
                    this.SetBondAngle(backbone[i], backbone[i + 1], angle);
                    int j = 0;
                    int[] angles = { 45, 315 };
                    if (i % 2 == 1) angles = new int[] { 225, 135 };
                    foreach (Atom a in backbone[i].ConnectedAtoms) if (!backbone.Contains(a)) this.AddBranch(a, backbone[i], angles[j++]);
                }
            }
            if (cycles.Count > 0)
            {
                foreach (List<Atom> ring in cycles)
                {
                    this.LocateRing2D(ring);
                }
            }

            foreach (Atom a in this.m_Atoms)
            {
                if (!a.Visited)
                {

                }
            }
        }

        void AddBranch(Atom atom1, Atom atom2, int angle)
        {
            atom1.Visited = true;
            int newAngle = this.SetBondAngle(atom1, atom2, angle);
            foreach (List<Atom> ring in cycles)
            {
                if (ring.Contains(atom1))
                {
                    this.LocateRing2D(ring, atom1, atom2, newAngle);
                    return;
                }
            }
            if (atom1.ConnectedAtoms.Length == 2)
            {
                foreach (Atom a in atom1.ConnectedAtoms)
                {
                    if (a != atom2) AddBranch(a, atom1, (angle - 60) % 360);
                    a.Visited = true;
                }
            }
            if (atom1.ConnectedAtoms.Length == 3)
            {
                bool first = true;
                foreach (Atom a in atom1.ConnectedAtoms)
                {
                    if (a != atom2)
                    {
                        if (first) AddBranch(a, atom1, (angle - 60) % 360);
                        else AddBranch(a, atom1, (angle + 60) % 360);
                        first = false;
                    }
                    a.Visited = true;
                }
            }
        }

        void LocateRing2D(List<Atom> ring)
        {
            int ringNumber = cycles.IndexOf(ring);
            int angle = 360 / ring.Count;
            int bondAngle = angle;
            Atom start = ring[0];
            Atom current = ring[0];
            Atom next = null;
            Atom last = null;
            List<Atom> visited = new List<Atom>();
            foreach (Atom a in ring) if (a.Visited) visited.Add(a);
            if (visited.Count == 0)
            {
                while (next != start)
                {
                    foreach (Atom a in current.ConnectedAtoms)
                    {
                        if (ring.Contains(a) && !a.Visited && a != last)
                        {
                            next = a;
                        }
                    }
                    next.Visited = true;
                    last = current;
                    bondAngle = this.SetBondAngle(current, next, bondAngle);
                    bondAngle = (bondAngle - angle) % 360;
                    if (bondAngle < 0) bondAngle = bondAngle + 360;
                    current = next;
                }
            }
            if (visited.Count == 1)
            {
                start = ring[ring.IndexOf(visited[0])];
                current = start;
                while (next != start)
                {
                    foreach (Atom a in current.ConnectedAtoms)
                    {
                        if (ring.Contains(a) && !a.Visited && a != last)
                        {
                            next = a;
                        }
                    }
                    next.Visited = true;
                    last = current;
                    bondAngle = this.SetBondAngle(current, next, bondAngle);
                    bondAngle = (bondAngle + angle) % 360;
                    if (bondAngle < 0) bondAngle = bondAngle + 360;
                    current = next;
                }
                return;
            }
            if (visited.Count > 1)
            {
                start = visited[0];
                current = visited[1];
                Bond b = this.GetBond(start, current);
                if (b == null)
                {
                    return;
                }
                else
                {
                    if (b.ConnectedAtom == current)
                    {
                        start = visited[1];
                        current = visited[0];
                    }
                    foreach (Atom a in ring) a.Visited = false;
                    bondAngle = this.GetBondAngle(start, current);
                    List<Atom> notInRing = new List<Atom>();
                    foreach (Bond b1 in current.BondedAtoms) if (!ring.Contains(b1.ConnectedAtom)) notInRing.Add(b1.ConnectedAtom);
                    if (notInRing.Count == 1)
                    {
                        int otherAngle = this.GetBondAngle(notInRing[0], current);
                        if (otherAngle < bondAngle) angle = -1 * angle;
                    }
                    //Close the ring
                    start.Visited = false;
                    while (next != start)
                    {
                        foreach (Atom a in current.ConnectedAtoms) if (ring.Contains(a) && !a.Visited) next = a;
                        next.Visited = true;
                        bondAngle = (bondAngle - angle) % 360;
                        if (bondAngle < 0) bondAngle = bondAngle + 360;
                        this.SetBondAngle(current, next, bondAngle);
                        current = next;
                    }
                    return;
                }
            }
        }

        void LocateRing2D(List<Atom> ring, Atom startAtom, Atom previousAtom, int angle)
        {
            if (angle < 90) angle = 300;
            int ringNumber = cycles.IndexOf(ring);
            int factor = 1;
            for (int i = 0; i < ringNumber; i++) factor = factor * -1;
            int bondAngle = 360 / ring.Count;
            Atom start = ring[0];
            Atom current = ring[0];
            Atom next = null;
            Atom last = null;
            List<Atom> visited = new List<Atom>();
            foreach (Atom a in ring) if (a.Visited) visited.Add(a);
            if (visited.Count == 0)
            {
                int newAngle = this.SetBondAngle(start, previousAtom, angle);
                while (next != start)
                {
                    foreach (Atom a in current.ConnectedAtoms) if (ring.Contains(a) && !a.Visited) next = a;
                    next.Visited = true;
                    this.SetBondAngle(current, next, angle);
                    angle = (angle + bondAngle) % 360;
                    current = next;
                }
                return;
            }
            if (visited.Count == 1)
            {
                start = ring[ring.IndexOf(visited[0])];
                current = start;
                start.Visited = false;
                while (next != start)
                {
                    foreach (Atom a in current.ConnectedAtoms)
                    {
                        if (ring.Contains(a) && !a.Visited && a != last)
                        {
                            next = a;
                        }
                    }
                    next.Visited = true;
                    last = current;
                    bondAngle = this.SetBondAngle(current, next, bondAngle);
                    bondAngle = (bondAngle + angle) % 360;
                    if (bondAngle < 0) bondAngle = bondAngle + 360;
                    current = next;
                }
            }
            //if (visited.Count == 2)
            //{
            //    start = visited[0];
            //    current = visited[1];
            //    Bond b = this.GetBond(start, current);
            //    if (b.ConnectedAtom == current)
            //    {
            //        start = visited[1];
            //        current = visited[0];
            //    }
            //    joiningAngle = (360 - this.GetBondAngle(start, current) + (factor * bondAngle)) % 360;
            //    if (joiningAngle >= 0)
            //    {
            //        //Close the ring
            //        while (next != start)
            //        {
            //            next = start;
            //            foreach (Atom a in current.ConnectedAtoms) if (ring.Contains(a) && !a.Visited) next = a;
            //            current.Visited = true;
            //            this.SetBondAngle(current, next, joiningAngle);
            //            joiningAngle = (joiningAngle - bondAngle);
            //            current = next;
            //        }
            //        return;
            //    }
            //    else
            //    {
            //        // Bridge to one

            //        // then to the other
            //    }
            //}
        }

        public System.Drawing.Rectangle GetLocationBounds()
        {
            int top = this.m_Atoms[0].Location2D.Y;
            int bottom = this.m_Atoms[0].Location2D.Y;
            int left = this.m_Atoms[0].Location2D.X;
            int right = this.m_Atoms[0].Location2D.X;
            foreach (Atom a in this.m_Atoms)
            {
                if (top > a.Location2D.Y) top = a.Location2D.Y;
                if (bottom < a.Location2D.Y) bottom = a.Location2D.Y;
                if (left > a.Location2D.X) left = a.Location2D.X;
                if (right < a.Location2D.X) right = a.Location2D.X;
            }
            m_Location = new System.Drawing.Point(left, top);
            m_Size = new System.Drawing.Size(right - left, bottom - top);
            return new System.Drawing.Rectangle(left, top, right - left, bottom - top);
        }

        System.Drawing.Point m_Location;
        System.Drawing.Size m_Size;
        public System.Drawing.Point Location
        {
            get
            {
                return m_Location;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return m_Size;
            }
        }
    }
}
