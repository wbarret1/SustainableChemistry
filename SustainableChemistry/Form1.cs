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
    // CCOP(=O)(N(C)C)N(C)C
    // CCN(CC)P(OC)OC
    // COP(=O)(OC)OC


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
        //List<ChemInfo.FunctionalGroup> functionalGroups;
        //List<ChemInfo.FunctionalGroup> m_FGroups;
        static Encoding enc8 = Encoding.UTF8;
        ChemInfo.References m_References;
        string documentPath;
        ChemInfo.FunctionalGroupCollection fGroups;
        ChemInfo.NamedReactionCollection m_NamedReactions;

        public Form1()
        {
            InitializeComponent();
            molecule = new ChemInfo.Molecule();
            fGroups = new ChemInfo.FunctionalGroupCollection();
            m_NamedReactions = fGroups.NamedReactions;
            this.trackBar1.Value = (int)(this.moleculeViewer1.Zoom * 100);
            documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            documentPath = documentPath + "\\USEPA\\SustainableChemistry";

            System.IO.FileStream fs = new System.IO.FileStream(documentPath + "\\references.dat", System.IO.FileMode.Open);
            
            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                m_References = (ChemInfo.References)formatter.Deserialize(fs);
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            // Initial code to load references from text RIS resource files. Can be deleted when done.
            
            //m_References = new ChemInfo.References();
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Diisoproprylethyamine Solvent", enc8.GetString(Properties.Resources.S0040403900813763)));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Diisoproprylethyamine Solvent", enc8.GetString(Properties.Resources.S0040403900942163)));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Diisoproprylethyamine Solvent", enc8.GetString(Properties.Resources.S0040403901904617)));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Diisoproprylethyamine Solvent", enc8.GetString(Properties.Resources.achs_jacsat105_661)));
            //m_References.Add(new ChemInfo.Reference("phosphate", "No Name", enc8.GetString(Properties.Resources.S1001841712003142)));
            //m_References.Add(new ChemInfo.Reference("phosphate", "Catalyst", Properties.Resources._10_1002_2Fchin_199605199));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Catalyst Solvent", enc8.GetString(Properties.Resources.europepmc)));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Catalyst Solvent", enc8.GetString(Properties.Resources.europepmc1)));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Catalyst Solvent", enc8.GetString(Properties.Resources.achs_oprdfk4_175)));
            //m_References.Add(new ChemInfo.Reference("phosphoramidite", "Catalyst Solvent", enc8.GetString(Properties.Resources.BIB)));


            // Reads functional Groups from tab-delimited text file.
            string[] functionalGroupStrs = SustainableChemistry.Properties.Resources.Full_Functional_Group_List_20180301.Split('\n');

            // This next line creates a list of strings that don't have images. Can be commented out!
            List<string> missingImages = new List<string>();

            // Creates the collection of functional groups.
            foreach (string line in functionalGroupStrs)
            {                
                ChemInfo.FunctionalGroup temp = fGroups.Add(line);
                string filename = documentPath + "\\Images\\" + temp.Name.ToLower() + ".jpg";
                if (System.IO.File.Exists(filename)) temp.Image = System.Drawing.Image.FromFile(filename);

                //this line adds the missing image to the list of missing images. Can be commented out.
                else missingImages.Add(temp.Name);
            }

            // Writes the missing images to a file.

            // Write the string array to a new file named "WriteLines.txt".
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(documentPath + @"\MissingImages.txt"))
            {
                foreach (string line in missingImages)
                    outputFile.WriteLine(line);
            }

            string[] imageFiles = System.IO.Directory.GetFiles(documentPath + "\\Images\\");
            string[] groupNames = fGroups.FunctionalGroups;
            List<string> extraImages = new List<string>();
            foreach(string name in imageFiles)
            {
                string temp = name.Replace(documentPath + "\\Images\\", string.Empty);
                temp = temp.Replace(".jpg", string.Empty);
                bool add = true;
                foreach (string gName in groupNames)
                {
                    if (temp.ToUpper() == gName.ToUpper()) add = false;
                }
                if (add) extraImages.Add(temp);
            }

            // Write the string array to a new file named "WriteLines.txt".
            using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(documentPath + @"\ExtraImages.txt"))
            {
                foreach (string line in extraImages)
                    outputFile.WriteLine(line);
            }


            //Reads in phosphate groups from text file.

            //string[] directories = System.IO.Directory.GetDirectories(documentPath);
            //foreach(string dir in directories)
            //{
            //    string groupName = dir.Remove(0, documentPath.Length + 1);
            //    fGroups.Add(new ChemInfo.FunctionalGroup(groupName, dir));
            //}

            //Reads in functional groups from JSON file. This should be used after Excel file is completed.
            //var json = new System.Web.Script.Serialization.JavaScriptSerializer();
            //functionalGroups = (List<ChemInfo.FunctionalGroup>)json.Deserialize(ChemInfo.Functionalities.AvailableFunctionalGroups(), typeof(List<ChemInfo.FunctionalGroup>));            // string text = ChemInfo.Functionalities.FunctionalGroups("P(c1ccccc1)(c1ccccc1)(N)=O", "json");

            // Test code for generating a results file from a functional group name.
            //Results res = new Results("phosphoramidite", m_References);
            //string results = json.Serialize(res);
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
            molecule = new ChemInfo.Molecule(smiles.SMILES);
            this.listBox1.Items.Clear();
            if (molecule == null) return;
            molecule.FindRings();
            molecule.FindAllPaths();
            this.moleculeViewer1.Molecule = molecule;
            this.propertyGrid1.SelectedObject = molecule;
        }        

        private void phosphorousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] phos = ChemInfo.Functionalities.PhosphorousFunctionality(molecule);
            int i = 0;
            List<ChemInfo.FunctionalGroup> groups = new List<ChemInfo.FunctionalGroup>();
            foreach (string p in phos)
            {
                foreach (ChemInfo.FunctionalGroup group in fGroups)
                {
                    if (group.Name.ToLower() == p)
                    {
                        groups.Add(group);
                        i++;
                    }
                }
                //listBox1.Items.Add(p);
                //listBox1.Items.Add(string.Empty);
                //var refs = m_References.GetReferences(p);
                //foreach (Reference r in refs) listBox1.Items.Add(r.ToString());
                //listBox1.Items.Add(string.Empty);
                //listBox1.Items.Add(string.Empty);

            }
            if (i== 1)
            {
                this.pictureBox1.Image = groups[0].ReactionImage;
                //foreach(ChemInfo.Reference r in groups[0].References)
                //{
                //    this.listBox1.Items.Add(r.ToString());
                //}
            }
            //listBox1.Items.AddRange(phos);
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
            this.phosphorousToolStripMenuItem_Click(sender, e);
        }

        private void findSMARTSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smilesInput smiles = new smilesInput();
            smiles.Text = "Enter SMARTS String";
            smiles.ShowDialog();
            if (string.IsNullOrEmpty(smiles.SMILES)) return;
            int[] atoms = null;
            if (!this.molecule.FindSmarts(smiles.SMILES, ref atoms)) MessageBox.Show("SMARTS not found in molecule.");
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
                "P(c1ccccc1)(c1ccccc1)(c1ccccc1)=O",
                "CCOP(=O)(N)OCC",
                "CCOP(=O)(N(C)C)N(C)C",
                "CN(C)P(=O)(N(C)C)N(C)C",
                "CCCc1ccccc1NP(=O)(C)Oc1ccccc1CC",
                "P(c1ccccc1)(c1ccccc1)(N)=O",
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
            DateTime start = DateTime.Now;
            ChemInfo.Molecule m = null;
            int[] indices = null;
            foreach (string molecule in molecules)
            {
                m = new ChemInfo.Molecule(molecule);
                bool found = false;
                int numFound = 0;
                foreach (string smart in groups)
                {
                    if (m.FindFunctionalGroup(smart, ref indices))
                    {
                        found = true;
                        numFound++;
                    }
                    //if (m.FindSmarts2(smart, ref temp)) found = true;
                }
                if (!found || numFound >1)
                {
                    MessageBox.Show(molecule);
                }
            }
            MessageBox.Show("Time Required is: " + (double)DateTime.Now.Subtract(start).Milliseconds + " milliseconds", "Test Completed Successfully");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.IO.Directory.CreateDirectory(documentPath);
            System.IO.FileStream fs = new System.IO.FileStream(documentPath + "\\references.dat", System.IO.FileMode.Create);
            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            try
            {
                formatter.Serialize(fs, m_References);
            }
            catch (System.Runtime.Serialization.SerializationException ex)
            {
                Console.WriteLine("Failed to serialize. Reason: " + ex.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }

            var json = new System.Web.Script.Serialization.JavaScriptSerializer();
            System.IO.File.WriteAllText(documentPath + "\\FunctionalGroups.json", json.Serialize(fGroups));
        }

        private void showReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> references = new List<string>();
            string[] funcs = ChemInfo.Functionalities.PhosphorousFunctionality(molecule);
            foreach (string f in funcs)
            {
                var refs = m_References.GetReferences(f);
                foreach (ChemInfo.Reference r in refs) references.Add(r.ToString());
            }
            ReferenceList form = new ReferenceList();
            form.References = m_References;
            form.ShowDialog();
        }

        private void editReferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void importRISFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("THIS NEEDS FIXED", "THIS NEEDS FIXED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //AddNewReference form = new AddNewReference();
            //if (form.ShowDialog() == DialogResult.OK)
            //    m_References.Add(new ChemInfo.Reference(form.FunctionalGroup, form.ReactionName, form.Data));
        }

        private void exportJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ChemInfo.Reference> refs = new List<ChemInfo.Reference>();
            string[] phos = ChemInfo.Functionalities.PhosphorousFunctionality(molecule);
            foreach (string p in phos)
            {
                var pRefs = m_References.GetReferences(p);
                foreach (ChemInfo.Reference r in pRefs) refs.Add(r);
            }
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            System.IO.StreamWriter writer = new System.IO.StreamWriter(documentPath + "\\output.json");
            writer.Write(serializer.Serialize(refs));
            writer.Close();
          //  System.Diagnostics.Process.Start(documentPath + "\\output.json");
        }

        private void functionalGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FunctionalGroupViewer viewer = new FunctionalGroupViewer(this.fGroups);
            viewer.ShowDialog();
        }

        private void reactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReactionEditor editor = new ReactionEditor(fGroups);
            System.Windows.Forms.DialogResult result = editor.ShowDialog();
            ChemInfo.NamedReaction rxn = editor.SelectedNamedReaction;
            if (rxn != null)
            {
                rxn.Solvent = editor.Solvent;
                rxn.AcidBase = editor.AcidBase;
                rxn.Catalyst = editor.Catalyst;
            }
        }

        private void functionalGroupsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FunctionalGroupEditor editor = new FunctionalGroupEditor(fGroups);
            editor.ShowDialog();
        }
    }
}
