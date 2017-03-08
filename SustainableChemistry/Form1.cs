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
            if (!string.IsNullOrEmpty(filepath))
            {
                ChemInfo.MoleFileReader reader = new ChemInfo.MoleFileReader(test.FilePath);
                molecule = reader.ReadMoleFile();
            }
            molecule.FindRings();
            this.listBox1.Items.Clear();
            this.moleculeViewer1.Molecule = molecule;
        }

        private void enterSMILEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smilesInput smiles = new smilesInput();
            smiles.ShowDialog();
            ChemInfo.smilesParser parser = new ChemInfo.smilesParser();
            parser.SMILE = smiles.SMILES;
            molecule = parser.Parse();
            this.listBox1.Items.Clear();
            if (molecule == null) return;
            molecule.FindRings();
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
            if (args.SelectedObject != null) this.propertyGrid1.SelectedObject = ((GraphicObject)(args.SelectedObject)).Tag;
        }
    }
}
