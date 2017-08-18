using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    //<<<<<<< HEAD
    //=======

    //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f
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
        //<<<<<<< HEAD
        public abstract Atom[] GetCoreSet();
        //=======
        //        public abstract void GetCoreSet(Atom a1, Atom a2);
        //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f

        public abstract void BackTrack();
    }

    //<<<<<<< HEAD
    //public class VF2SubState//: State
    //{
    //    int core_len, orig_core_len;
    //    int added_node1;
    //    int t1both_len, t2both_len, t1in_len, t1out_len,
    //        t2in_len, t2out_len; // Core nodes are also counted by these...
    //    int[] core_1;
    //    int[] core_2;
    //    int[] in_1;
    //    int[] in_2;
    //    int[] out_1;
    //    int[] out_2;

    //    int[] order;
    //    int NULL_NODE = -1;

    //    Molecule g1, g2;
    //    int n1, n2;

    //    int share_count;
    //    /*----------------------------------------------------------
    //      * Methods of the class VF2SubState
    //      ---------------------------------------------------------*/

    //    /*----------------------------------------------------------
    //     * VF2SubState::VF2SubState(g1, g2, sortNodes)
    //     * Constructor. Makes an empty state.
    //     * If sortNodes is true, computes an initial ordering
    //     * for the nodes based on the frequency of their valence.
    //     ---------------------------------------------------------*/
    //    public VF2SubState(Molecule ag1, Molecule ag2, bool sortNodes)
    //    {
    //        g1 = ag1;
    //        g2 = ag2;
    //        n1 = g1.GetAtoms().Length;
    //        n2 = g2.GetAtoms().Length;

    //        if (sortNodes)
    //            n1 = n1;
    //        //order = SortNodesByFrequency(ag1);
    //        else
    //            order = null;

    //        core_len = orig_core_len = 0;
    //        t1both_len = t1in_len = t1out_len = 0;
    //        t2both_len = t2in_len = t2out_len = 0;

    //        added_node1 = NULL_NODE;

    //        core_1 = new int[n1];
    //        core_2 = new int[n2];
    //        in_1 = new int[n1];
    //        in_2 = new int[n2];
    //        out_1 = new int[n1];
    //        out_2 = new int[n2];
    //        share_count = 1;
    //        //if (!core_1 || !core_2 || !in_1 || !in_2 
    //        // || !out_1 || !out_2 || !share_count)
    //        //  error("Out of memory");

    //        int i;
    //        for (i = 0; i < n1; i++)
    //        {
    //            core_1[i] = NULL_NODE;
    //            in_1[i] = 0;
    //            out_1[i] = 0;
    //        }
    //        for (i = 0; i < n2; i++)
    //        {
    //            core_2[i] = NULL_NODE;
    //            in_2[i] = 0;
    //            out_2[i] = 0;
    //        }


    //        share_count = 1;
    //    }


    //    /*----------------------------------------------------------
    //     * VF2SubState::VF2SubState(state)
    //     * Copy constructor. 
    //     ---------------------------------------------------------*/
    //    //VF2SubState::VF2SubState(const VF2SubState &state)
    //    //  { g1=state.g1;
    //    //    g2=state.g2;
    //    //    n1=state.n1;
    //    //    n2=state.n2;

    //    //    order=state.order;

    //    //    core_len=orig_core_len=state.core_len;
    //    //    t1in_len=state.t1in_len;
    //    //    t1out_len=state.t1out_len;
    //    //    t1both_len=state.t1both_len;
    //    //    t2in_len=state.t2in_len;
    //    //    t2out_len=state.t2out_len;
    //    //    t2both_len=state.t2both_len;

    //    //	added_node1=NULL_NODE;

    //    //    core_1=state.core_1;
    //    //    core_2=state.core_2;
    //    //    in_1=state.in_1;
    //    //    in_2=state.in_2;
    //    //    out_1=state.out_1;
    //    //    out_2=state.out_2;
    //    //    share_count=state.share_count;

    //    //	++ * share_count;

    //    //  }


    //    /*---------------------------------------------------------------
    //     * VF2SubState::~VF2SubState()
    //     * Destructor.
    //     --------------------------------------------------------------*/
    //    //////VF2SubState::~VF2SubState()
    //    //////{
    //    //////    if (--*share_count == 0)
    //    //////    {
    //    //////        delete[] core_1;
    //    //////        delete[] core_2;
    //    //////        delete[] in_1;
    //    //////        delete[] out_1;
    //    //////        delete[] in_2;
    //    //////        delete[] out_2;
    //    //////        delete share_count;
    //    //////        delete[] order;
    //    //////    }
    //    //////}


    //    /*--------------------------------------------------------------------------
    //     * bool VF2SubState::NextPair(pn1, pn2, prev_n1, prev_n2)
    //     * Puts in *pn1, *pn2 the next pair of nodes to be tried.
    //     * prev_n1 and prev_n2 must be the last nodes, or NULL_NODE (default)
    //     * to start from the first pair.
    //     * Returns false if no more pairs are available.
    //     -------------------------------------------------------------------------*/
    //    public bool NextPair(ref int pn1, ref int pn2,
    //                  int prev_n1, int prev_n2)
    //    {
    //        if (prev_n1 == NULL_NODE)
    //            prev_n1 = 0;
    //        if (prev_n2 == NULL_NODE)
    //            prev_n2 = 0;
    //        else
    //            prev_n2++;

    //        if (t1both_len > core_len && t2both_len > core_len)
    //        {
    //            while (prev_n1 < n1 &&
    //               (core_1[prev_n1] != NULL_NODE || out_1[prev_n1] == 0
    //                        || in_1[prev_n1] == 0))
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }
    //        else if (t1out_len > core_len && t2out_len > core_len)
    //        {
    //            while (prev_n1 < n1 &&
    //               (core_1[prev_n1] != NULL_NODE || out_1[prev_n1] == 0))
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }
    //        else if (t1in_len > core_len && t2in_len > core_len)
    //        {
    //            while (prev_n1 < n1 &&
    //               (core_1[prev_n1] != NULL_NODE || in_1[prev_n1] == 0))
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }
    //        else if (prev_n1 == 0 && order != null)
    //        {
    //            int i = 0;
    //            while (i < n1 && core_1[prev_n1 = order[i]] != NULL_NODE)
    //                i++;
    //            if (i == n1)
    //                prev_n1 = n1;
    //        }
    //        else
    //        {
    //            while (prev_n1 < n1 && core_1[prev_n1] != NULL_NODE)
    //            {
    //                prev_n1++;
    //                prev_n2 = 0;
    //            }
    //        }



    //        if (t1both_len > core_len && t2both_len > core_len)
    //        {
    //            while (prev_n2 < n2 &&
    //               (core_2[prev_n2] != NULL_NODE || out_2[prev_n2] == 0
    //                        || in_2[prev_n2] == 0))
    //            {
    //                prev_n2++;
    //            }
    //        }
    //        else if (t1out_len > core_len && t2out_len > core_len)
    //        {
    //            while (prev_n2 < n2 &&
    //               (core_2[prev_n2] != NULL_NODE || out_2[prev_n2] == 0))
    //            {
    //                prev_n2++;
    //            }
    //        }
    //        else if (t1in_len > core_len && t2in_len > core_len)
    //        {
    //            while (prev_n2 < n2 &&
    //               (core_2[prev_n2] != NULL_NODE || in_2[prev_n2] == 0))
    //            {
    //                prev_n2++;
    //            }
    //        }
    //        else
    //        {
    //            while (prev_n2 < n2 && core_2[prev_n2] != NULL_NODE)
    //            {
    //                prev_n2++;
    //            }
    //        }


    //        if (prev_n1 < n1 && prev_n2 < n2)
    //        {
    //            pn1 = prev_n1;
    //            pn2 = prev_n2;
    //            return true;
    //        }

    //        return false;
    //    }



    //    /*---------------------------------------------------------------
    //     * bool VF2SubState::IsFeasiblePair(node1, node2)
    //     * Returns true if (node1, node2) can be added to the state
    //     * NOTE: 
    //     *   The attribute compatibility check (methods CompatibleNode
    //     *   and CompatibleEdge of ARGraph) is always performed
    //     *   applying the method to g1, and passing the attribute of
    //     *   g1 as first argument, and the attribute of g2 as second
    //     *   argument. This may be important if the compatibility
    //     *   criterion is not symmetric.
    //     --------------------------------------------------------------*/
    //    public bool IsFeasiblePair(int node1, int node2)
    //    {
    //        //assert(node1 < n1);
    //        //assert(node2 < n2);
    //        //assert(core_1[node1] == NULL_NODE);
    //        //assert(core_2[node2] == NULL_NODE);

    //        //if (!g1.CompatibleNode(g1.GetNodeAttr(node1), g2.GetNodeAttr(node2)))
    //        //    return false;
    //        if (!g1.CompatibleAtom(g1.GetAtoms()[node1], g2.GetAtoms()[node2]))
    //            return false;

    //        int i;
    //        int other1, other2;
    //        void* attr1;
    //        int termout1 = 0, termout2 = 0, termin1 = 0, termin2 = 0, new1 = 0, new2 = 0;

    //        // Check the 'out' edges of node1
    //        //for (i = 0; i < g1.OutEdgeCount(node1); i++)
    //        for (i = 0; i < g1.GetAtoms()[node1].Degree; i++)
    //            {
    //            other1 = g1.GetOutEdge(node1, i, &attr1);
    //            if (core_1[other1] != NULL_NODE)
    //            {
    //                other2 = core_1[other1];
    //                if (!g2.HasEdge(node2, other2) ||
    //                    !g1.CompatibleEdge(attr1, g2.GetEdgeAttr(node2, other2)))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_1[other1] != 0)
    //                    termin1++;
    //                if (out_1[other1] != 0)
    //                    termout1++;
    //                if (in_1[other1] == 0 && out_1[other1] == 0)
    //                    new1++;
    //            }
    //        }

    //        // Check the 'in' edges of node1
    //        for (i = 0; i < g1.InEdgeCount(node1); i++)
    //        {
    //            other1 = g1.GetInEdge(node1, i, &attr1);
    //            if (core_1[other1] != NULL_NODE)
    //            {
    //                other2 = core_1[other1];
    //                if (!g2.HasEdge(other2, node2) ||
    //                    !g1.CompatibleEdge(attr1, g2.GetEdgeAttr(other2, node2)))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_1[other1] != 0)
    //                    termin1++;
    //                if (out_1[other1] != 0)
    //                    termout1++;
    //                if (in_1[other1] == 0 && out_1[other1] == 0)
    //                    new1++;
    //            }
    //        }


    //        // Check the 'out' edges of node2
    //        for (i = 0; i < g2.OutEdgeCount(node2); i++)
    //        {
    //            other2 = g2.GetOutEdge(node2, i);
    //            if (core_2[other2] != NULL_NODE)
    //            {
    //                other1 = core_2[other2];
    //                if (!g1.HasEdge(node1, other1))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_2[other2] != 0)
    //                    termin2++;
    //                if (out_2[other2] != 0)
    //                    termout2++;
    //                if (in_2[other2] == 0 && out_2[other2] == 0)
    //                    new2++;
    //            }
    //        }

    //        // Check the 'in' edges of node2
    //        for (i = 0; i < g2.InEdgeCount(node2); i++)
    //        {
    //            other2 = g2.GetInEdge(node2, i);
    //            if (core_2[other2] != NULL_NODE)
    //            {
    //                other1 = core_2[other2];
    //                if (!g1.HasEdge(other1, node1))
    //                    return false;
    //            }
    //            else
    //            {
    //                if (in_2[other2] != 0)
    //                    termin2++;
    //                if (out_2[other2] != 0)
    //                    termout2++;
    //                if (in_2[other2] == 0 && out_2[other2] == 0)
    //                    new2++;
    //            }
    //        }

    //        return termin1 <= termin2 && termout1 <= termout2 && new1 <= new2;
    //    }



    //    /*--------------------------------------------------------------
    //     * void VF2SubState::AddPair(node1, node2)
    //     * Adds a pair to the Core set of the state.
    //     * Precondition: the pair must be feasible
    //     -------------------------------------------------------------*/
    //    public void AddPair(int node1, int node2)
    //    {
    //        //assert(node1 < n1);
    //        //assert(node2 < n2);
    //        //assert(core_len < n1);
    //        //assert(core_len < n2);

    //        core_len++;
    //        added_node1 = node1;

    //        if (in_1[node1] == 0)
    //        {
    //            in_1[node1] = core_len;
    //            t1in_len++;
    //            if (out_1[node1] != 0)
    //                t1both_len++;
    //        }
    //        if (out_1[node1] == 0)
    //        {
    //            out_1[node1] = core_len;
    //            t1out_len++;
    //            if (in_1[node1] != 0)
    //                t1both_len++;
    //        }

    //        if (in_2[node2] == 0)
    //        {
    //            in_2[node2] = core_len;
    //            t2in_len++;
    //            if (out_2[node2] != 0)
    //                t2both_len++;
    //        }
    //        if (out_2[node2] == 0)
    //        {
    //            out_2[node2] = core_len;
    //            t2out_len++;
    //            if (in_2[node2] != 0)
    //                t2both_len++;
    //        }

    //        core_1[node1] = node2;
    //        core_2[node2] = node1;


    //        int i, other;
    //        for (i = 0; i < g1.InEdgeCount(node1); i++)
    //        {
    //            other = g1.GetInEdge(node1, i);
    //            if (in_1[other] == 0)
    //            {
    //                in_1[other] = core_len;
    //                t1in_len++;
    //                if (out_1[other] != 0)
    //                    t1both_len++;
    //            }
    //        }

    //        for (i = 0; i < g1.OutEdgeCount(node1); i++)
    //        {
    //            other = g1.GetOutEdge(node1, i);
    //            if (out_1[other] == 0)
    //            {
    //                out_1[other] = core_len;
    //                t1out_len++;
    //                if (in_1[other] != 0)
    //                    t1both_len++;
    //            }
    //        }

    //        for (i = 0; i < g2.InEdgeCount(node2); i++)
    //        {
    //            other = g2.GetInEdge(node2, i);
    //            if (in_2[other] == 0)
    //            {
    //                in_2[other] = core_len;
    //                t2in_len++;
    //                if (out_2[other] != 0)
    //                    t2both_len++;
    //            }
    //        }

    //        for (i = 0; i < g2.OutEdgeCount(node2); i++)
    //        {
    //            other = g2.GetOutEdge(node2, i);
    //            if (out_2[other] == 0)
    //            {
    //                out_2[other] = core_len;
    //                t2out_len++;
    //                if (in_2[other] != 0)
    //                    t2both_len++;
    //            }
    //        }

    //    }



    //    /*--------------------------------------------------------------
    //     * void VF2SubState::GetCoreSet(c1, c2)
    //     * Reads the core set of the state into the arrays c1 and c2.
    //     * The i-th pair of the mapping is (c1[i], c2[i])
    //     --------------------------------------------------------------*/
    //    public void GetCoreSet(int[] c1, int[] c2)
    //    {
    //        int i, j;
    //        for (i = 0, j = 0; i < n1; i++)
    //            if (core_1[i] != NULL_NODE)
    //            {
    //                c1[j] = i;
    //                c2[j] = core_1[i];
    //                j++;
    //            }
    //    }


    //    /*----------------------------------------------------------------
    //     * Clones a VF2SubState, allocating with new the clone.
    //     --------------------------------------------------------------*/
    //    //State* VF2SubState::Clone()
    //    //{
    //    //    return new VF2SubState(*this);
    //    //}

    //    /*----------------------------------------------------------------
    //     * Undoes the changes to the shared vectors made by the 
    //     * current state. Assumes that at most one AddPair has been
    //     * performed.
    //     ----------------------------------------------------------------*/
    //    public void BackTrack()
    //    {
    //        //assert(core_len - orig_core_len <= 1);
    //        //assert(added_node1 != NULL_NODE);

    //        if (orig_core_len < core_len)
    //        {
    //            int i, node2;

    //            if (in_1[added_node1] == core_len)
    //                in_1[added_node1] = 0;
    //            for (i = 0; i < g1.InEdgeCount(added_node1); i++)
    //            {
    //                int other = g1.GetInEdge(added_node1, i);
    //                if (in_1[other] == core_len)
    //                    in_1[other] = 0;
    //            }

    //            if (out_1[added_node1] == core_len)
    //                out_1[added_node1] = 0;
    //            for (i = 0; i < g1.OutEdgeCount(added_node1); i++)
    //            {
    //                int other = g1.GetOutEdge(added_node1, i);
    //                if (out_1[other] == core_len)
    //                    out_1[other] = 0;
    //            }

    //            node2 = core_1[added_node1];

    //            if (in_2[node2] == core_len)
    //                in_2[node2] = 0;
    //            for (i = 0; i < g2.InEdgeCount(node2); i++)
    //            {
    //                int other = g2.GetInEdge(node2, i);
    //                if (in_2[other] == core_len)
    //                    in_2[other] = 0;
    //            }

    //            if (out_2[node2] == core_len)
    //                out_2[node2] = 0;
    //            for (i = 0; i < g2.OutEdgeCount(node2); i++)
    //            {
    //                int other = g2.GetOutEdge(node2, i);
    //                if (out_2[other] == core_len)
    //                    out_2[other] = 0;
    //            }

    //            core_1[added_node1] = NULL_NODE;
    //            core_2[node2] = NULL_NODE;

    //            core_len = orig_core_len;
    //            added_node1 = NULL_NODE;
    //        }

    //    }
    //}

    //    public class VF2SubStateStack : State
    //=======
    public class VF2SubState : State
    //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f
    {
        Molecule m1, m2;

        Atom addedAtom1;
        //<<<<<<< HEAD
        bool nodesSorted;
        //=======
        //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f

        Stack<Atom> core_1;
        Stack<Atom> core_2;
        Stack<Atom> in_1;
        Stack<Atom> in_2;
        Stack<Atom> out_1;
        Stack<Atom> out_2;
        Stack<Atom> t_1;
        Stack<Atom> t_2;

        //<<<<<<< HEAD
        int t1_both, t2_both;



        VF2SubState(Molecule molecule1, Molecule molecule2, bool SortNodes)
        {
            m1 = molecule1;
            m2 = molecule2;
            t1_both = 0;
            t2_both = 0;
            //=======


            //        VF2SubState(Molecule molecule1, Molecule molecule2, bool SortNodes)
            //        {
            //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f
            core_1 = new Stack<Atom>();
            core_2 = new Stack<Atom>();
            in_1 = new Stack<Atom>();
            in_2 = new Stack<Atom>();
            out_1 = new Stack<Atom>();
            out_2 = new Stack<Atom>();
            t_1 = new Stack<Atom>();
            t_2 = new Stack<Atom>();
            addedAtom1 = null;
            //<<<<<<< HEAD
            nodesSorted = SortNodes;
            //=======
            //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f
        }

        public override Molecule GetMolecule1() { return m1; }
        public override Molecule GetMolecule2() { return m2; }
        public override bool NextPair(ref Atom nextAtom1, ref Atom nextAtom2, Atom prevAtom1 = null, Atom prevAtom2 = null)
        {
            if (prevAtom1 == null) prevAtom1 = m1.GetAtoms()[0];
            if (prevAtom2 == null)
                prevAtom2 = m2.GetAtoms()[0];
            else
                //<<<<<<< HEAD
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
                //=======
                //            {
                //                prevAtom2 = m2.getNextAtom(prevAtom2);
                //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f
            }

            if (t_1.Count > core_1.Count && t_2.Count > core_1.Count)
            {
                //<<<<<<< HEAD
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
            //if (!in_1.Contains(atom1))
            //{
            //    in_1.Push(atom1);
            //}
            //out_1.Push(atom1);
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
    //=======
    //                while ()
    //            }

    //            return false;

    //        }
    //        public override bool isFeasiblePair(Atom atom1, Atom atom2)
    //        {

    //        }
    //        public override void AddPair(Atom atom1, Atom atom2)
    //        {
    //            core_1.Push(atom2);
    //            core_2.Push(atom1);
    //            addedAtom1 = atom1;
    //            if (!in_1.Contains(atom1)){
    //                in_1.Add(atom1);
    //            }

    //            out_1.Add(atom1);


    //        }
    //        public override bool IsGoal() { return core_1.Count == m1.GetAtoms().Length && core_2.Count == m2.GetAtoms().Length; }
    //        public override bool IsDead()
    //        {

    //        }
    //        public override int CoreLen() { return core_1.Count; }
    //        public override void GetCoreSet(Atom a1, Atom a2)
    //        {

    //        }

    //        public abstract void BackTrack()
    //        {

    //        }

    //    }


    //>>>>>>> 449cf2214a0cac14d9ffa36c08a61722e8eafa9f
}