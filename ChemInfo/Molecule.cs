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
        }

        public void AddBond(Atom atomOne, Atom atomTwo, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            if (atomOne != null)
            {
                atomOne.AddBond(atomTwo, type, stereo, topology, rcStatus);
                atomOne.AddConnectedAtom(atomTwo);
                atomTwo.AddConnectedAtom(atomOne);
                ringsFound = false;
                pathsFound = false;
            }
        }

        //public int GetBondAngle(Atom atom1, Atom atom2)
        //{
        //    foreach (Bond b in atom1.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom2) return b.Angle;
        //    }
        //    foreach (Bond b in atom2.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom1) return (b.Angle + 180) % 360;
        //    }
        //    return -1;
        //}

        //public int SetBondAngle(Atom atom1, Atom atom2, int angle)
        //{
        //    foreach (Bond b in atom1.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom2)
        //        {
        //            b.Angle = angle;
        //            //b.SetBondededAtomLocation();
        //            return angle;
        //        }
        //    }
        //    foreach (Bond b in atom2.BondedAtoms)
        //    {
        //        if (b.ConnectedAtom == atom1)
        //        {
        //            b.Angle = (angle + 180) % 360;
        //            //b.SetParentAtomLocation();
        //            return (angle + 180) % 360;
        //        }
        //    }
        //    return 0;
        //}

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

        // Molecule Boundaries for drawing algorithm

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
        System.Drawing.Size m_Size = new System.Drawing.Size(1100, 850);
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


        // Force Directed Graph Atom Locations

        int GetArea()
        {
            return this.Size.Height * this.Size.Width;
        }

        double ForceConstant
        {
            get
            {
                //double retVal = 0.2*Math.Sqrt((double)this.GetArea() / (double)this.m_Atoms.Count);
                return 50.0;
            }
        }

        double Temperature
        {
            get;
            set;
        }

        public double InitialTemperature
        {
            get
            {
               return Math.Min(this.Size.Height, this.Size.Width) / 10;
            }
        }

        double RepulsiveMiutiplier { get; set; } = 1.0;
        double AttractiveMultiplier { get; set; } = 1.0;
        double ConstantOfRepulsion = 0;
        private void CalculateConstantOfRepulsion()
        {
            ConstantOfRepulsion = Math.Pow(this.ForceConstant * this.RepulsiveMiutiplier, 2);
            //NotifyPropertyChanged("ConstantOfRepulsion");
        }
        double ConstantOfAttraction = 0;
        private void CalculateConstantOfAttraction()
        {
            ConstantOfAttraction = this.ForceConstant * this.AttractiveMultiplier;
            //NotifyPropertyChanged("ConstantOfAttraction");
        }

        double DistanceBetweenAtomsSquared(Atom a1, Atom a2)
        {
            int delX = a1.Location2D.X - a2.Location2D.X;
            int delY = a1.Location2D.Y - a2.Location2D.Y;
            return Math.Pow(delX, 2) + Math.Pow(delY, 2);
        }
        double DistanceBetweenAtoms(Atom a1, Atom a2)
        {
            return Math.Sqrt(this.DistanceBetweenAtomsSquared(a1, a2));
        }

        public void ForceDirectedGraph()
        {
            this.RandomLocateAtoms();
            this.Temperature = this.InitialTemperature;
            int maxIter = 200;
            for (int i = 0; i < maxIter; i++)
            {
                this.IterateFGD();
                this.Temperature *= (1.0 - (double)i / (double)maxIter);
            }
        }

        void RandomLocateAtoms()
        {
            System.Random random = new Random();
            foreach(Atom a in this.m_Atoms)
            {
                a.Location2D = new System.Drawing.Point(random.Next(this.Size.Width), random.Next(this.Size.Height));
            }
        }

        protected void IterateFGD()
        {
            this.CalculateConstantOfAttraction();
            this.CalculateConstantOfRepulsion();
            foreach (Atom v in this.m_Atoms)
            {
                v.deltaX = 0.0;
                v.deltaY = 0.0;
            }
            foreach (Atom v in this.m_Atoms)
            {
                foreach (Atom u in this.m_Atoms)
                {
                    if (u != v)
                    {
                        double distance = this.DistanceBetweenAtoms(u, v);
                        if (distance < 1) distance = 1;
                        double delX = v.Location2D.X - u.Location2D.X;
                        double delY = v.Location2D.Y - u.Location2D.Y;
                        v.deltaX = v.deltaX + ((delX / distance) * this.ConstantOfRepulsion/distance);
                        v.deltaY = v.deltaY + ((delY / distance) * this.ConstantOfRepulsion/distance);
                    }
                }
            }

            foreach (Atom v in this.m_Atoms)
            {
                foreach (Atom u in this.m_Atoms)
                {
                    Bond b = this.GetBond(u, v);
                    if (b != null)
                    {
                        double distance = this.DistanceBetweenAtoms(u, v);
                        if (distance < 10) distance = 10;
                        double delX = v.Location2D.X - u.Location2D.X;
                        double delY = v.Location2D.Y - u.Location2D.Y;
                        //double force = AttractiveForce(this.ForceConstant, distance);
                        double deltaX = ((double)delX / (double)distance) * Math.Pow(distance, 2) / this.ConstantOfAttraction;
                        double deltaY = ((double)delY / (double)distance) * Math.Pow(distance, 2) / this.ConstantOfAttraction;
                        v.deltaX = v.deltaX - deltaX;
                        v.deltaY = v.deltaY - deltaY;
                        u.deltaX = u.deltaX + deltaX;
                        u.deltaY = u.deltaY + deltaY;
                    }
                }
            }

            // This add the Angle force to fix bond angles from Fraczek 2016
            foreach (Atom v in this.m_Atoms)
            {
                foreach (Atom u in this.m_Atoms)
                {
                    if (u.ConnectedAtoms.Length < 5)
                    {
                        foreach(Atom a in u.ConnectedAtoms)
                        {
                            if (a != u)
                            {
                                double distance = this.DistanceBetweenAtoms(u, v);
                                if (distance < 10) distance = 10;
                                double delX = v.Location2D.X - u.Location2D.X;
                                double delY = v.Location2D.Y - u.Location2D.Y;
                                //double force = AttractiveForce(this.ForceConstant, distance);
                                double deltaX = 0.1*((double)delX / (double)distance) * Math.Pow(distance, 2) / this.ConstantOfAttraction;
                                double deltaY = 0.1*((double)delY / (double)distance) * Math.Pow(distance, 2) / this.ConstantOfAttraction;
                                v.deltaX = v.deltaX - deltaX;
                                v.deltaY = v.deltaY - deltaY;
                                u.deltaX = u.deltaX + deltaX;
                                u.deltaY = u.deltaY + deltaY;
                            }
                        }
                    }



                    //Bond b = this.GetBond(u, v);
                    //if (b != null)
                    //{
                    //    double distance = this.DistanceBetweenAtoms(u, v);
                    //    if (distance < 10) distance = 10;
                    //    double delX = v.Location2D.X - u.Location2D.X;
                    //    double delY = v.Location2D.Y - u.Location2D.Y;
                    //    //double force = AttractiveForce(this.ForceConstant, distance);
                    //    double deltaX = ((double)delX / (double)distance) * Math.Pow(distance, 2) / this.ConstantOfAttraction;
                    //    double deltaY = ((double)delY / (double)distance) * Math.Pow(distance, 2) / this.ConstantOfAttraction;
                    //    v.deltaX = v.deltaX - deltaX;
                    //    v.deltaY = v.deltaY - deltaY;
                    //    u.deltaX = u.deltaX + deltaX;
                    //    u.deltaY = u.deltaY + deltaY;
                    //}
                }
            }
            foreach (Atom v in this.m_Atoms)
            {
                double displacement = Math.Sqrt((v.deltaX * v.deltaX) + (v.deltaY * v.deltaY));
                double x = v.Location2D.X;
                x = x + ((v.deltaX / displacement) * Math.Min(displacement, this.Temperature));
                x = Math.Min(this.Size.Width, Math.Max(0, x));
                double y = v.Location2D.Y;
                y = y + ((v.deltaY / displacement) * Math.Min(displacement, this.Temperature));
                y = Math.Min(this.Size.Height, Math.Max(0, y));
                v.Location2D = new System.Drawing.Point((int)x, (int)y);
            }
        }
    }
}
