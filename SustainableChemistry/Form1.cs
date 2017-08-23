using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SustainableChemistry
{

    public struct descriptor
    {
        public string name;
        public double value;
    }

    public struct Fragment
    {
        public int atomNumber;
        public string element;
        public string fragment;
    }

    public partial class Form1 : Form
    {

        ChemInfo.Molecule molecule;


        public Form1()
        {
            InitializeComponent();
            molecule = new ChemInfo.Molecule();
            this.trackBar1.Value = (int)(this.moleculeViewer1.Zoom * 100);
            //moleculeViewer1.s
        }

        private void importFormTESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestResults test = null;
            try
            {
                test = new TestResults();
            }
            catch (Exception)
            {
                return;
            }
            test.ShowDialog();
            string filepath = test.FilePath;
            if (string.IsNullOrEmpty(filepath)) return;
            ChemInfo.MoleFileReader reader = new ChemInfo.MoleFileReader(test.FilePath);
            molecule = reader.ReadMoleFile();
            molecule.FindRings();
            this.listBox1.Items.Clear();
            this.moleculeViewer1.Molecule = molecule;
        }

        private void enterSMILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smilesInput smiles = new smilesInput();
            smiles.ShowDialog();
            if (string.IsNullOrEmpty(smiles.SMILES)) return;
            ChemInfo.smilesParser parser = new ChemInfo.smilesParser();
            parser.SMILE = smiles.SMILES;
            molecule = parser.Parse();
            this.listBox1.Items.Clear();
            if (molecule == null) return;
            molecule.FindRings();
            molecule.FindAllPaths();
            //TreeNodeCollection nodes = treeView1.Nodes;
            //nodes.Clear();
            //foreach (ChemInfo.Atom a in molecule.GetAtoms())
            //{
            //    TreeNode node = new TreeNode(a.AtomicSymbol);
            //    node.Tag = a;
            //    nodes.Add(node);
            //}
            this.moleculeViewer1.Molecule = molecule;
        }

        private void phosphorousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] phos = ChemInfo.Functionalities.PhosphorousFunctionality(molecule);
            listBox1.Items.AddRange(phos);
        }

        private void moleculeViewer1_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            this.propertyGrid1.SelectedObject = null;
            if (args.SelectedObject != null) this.propertyGrid1.SelectedObject = ((GraphicObject)(args.SelectedObject)).Tag;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.moleculeViewer1.Zoom = (double)(this.trackBar1.Value) / 100.0;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] phos = ChemInfo.Functionalities.PhosphorousFunctionality(molecule);
            listBox1.Items.AddRange(phos);
        }

        private void findSMARTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int[] atoms = null;
            this.molecule.FindSmarts("NP(=O)(C)C", ref atoms);
        }

        private void testSubgraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] molecules = {"COP(OC)OC",
                "CCOP(C)OCC",
                "COP(C1=CC=CC=C1)C2=CC=CC=C2",
                "P(c1ccccc1)(c1ccccc1)c1ccccc1",
                "CCN(CC)P(OC)OC",
                "CC(C)N(C(C)C)P(N(C(C)C)C(C)C)OCCC#N",
                "O(P(N(C)C)C)C",
                "C(COP(O)S)NC(=O)CS",
                "CCOP(SC)SC(C)C",
                "COP(=O)(OC)OC",
                "COP(=O)(C)OC",
                "CCOP(=O)(C1=CC=CC=C1)C2=CC=CC=C2",
                "C1=CC=C(C=C1)P(=O)(C2=CC=CC=C2)C3=CC=CC=C3",
                "CCOP(=O)(N)OCC",
                "CCOP(=O)(N(C)C)N(C)C",
                "CN(C)P(=O)(N(C)C)N(C)C",
                "CCCOC1=CC=C(C=C1)NP(=O)(C)OC2=CC=C(C=C2)CC",
                "C1=CC=C(C=C1)P(=O)(C2=CC=CC=C2)N",
                "CCOP(=S)(OCC)OCC",
                "CCOP(=S)(OCC)SCC",
                "CN(C)P(=O)(C)N(C)C"};
            string[] groups = {"OP(O)O",
                "OP(C)O",
                "OP(C)C",
                "CP(C)C",
                "NP(O)O",
                "NP(N)O",
                "OP(N)C",
                "OP(O)S",
                "OP(S)S",
                "OP(=O)(O)O",
                "OP(=O)(O)C",
                "OP(=O)(C)C",
                "CP(=O)(C)C",
                "OP(=O)(N)O",
                "OP(=O)(N)N",
                "NP(=O)(N)N",
                "NP(=O)(C)O",
                "NP(=O)(C)C",
                "OP(=S)(O)O",
                "OP(=S)(O)S",
                "NP(=O)(C)N"};
            ChemInfo.Molecule m = null;
            ChemInfo.smilesParser parser = new ChemInfo.smilesParser();
            int[] temp = null;
            foreach (string molecule in molecules)
            {
                parser.SMILE = molecule;
                m = parser.Parse();
                bool found = false;
                foreach (string smart in groups)
                {
                    if (m.FindSmarts(smart, ref temp)) found = true;
                }
                if (!found)
                {
                    MessageBox.Show(molecule);
                }
            }
        }
    }
}
