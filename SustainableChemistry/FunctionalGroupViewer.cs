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
    public partial class FunctionalGroupViewer : Form
    {
        FunctionalGroupCollection m_FunctGroups;

        public FunctionalGroupViewer(FunctionalGroupCollection groups)
        {
            InitializeComponent();
            m_FunctGroups = groups;
            this.comboBox1.Items.AddRange(m_FunctGroups.FunctionalGroups);
            this.label1.Text = string.Empty;
            this.label2.Text = string.Empty;
            this.label3.Text = string.Empty;
            this.label4.Text = string.Empty;
            this.label5.Text = string.Empty;
            this.label6.Text = string.Empty;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunctionalGroup g = this.m_FunctGroups[this.comboBox1.SelectedItem.ToString()];
            string[] reactants = g.Reactants;
            this.imageList1.Images.Clear();
            FunctionalGroup react0 = this.m_FunctGroups[reactants[0] + "s"];
            FunctionalGroup react1 = this.m_FunctGroups[reactants[1] + "s"];
            this.pictureBox1.Image = this.m_FunctGroups[reactants[0] + "s"].Image;
            this.pictureBox2.Image = this.m_FunctGroups[reactants[1] + "s"].Image;
            this.pictureBox3.Image = g.Image;
            this.label1.Text = react0.Name;
            this.label2.Text = react1.Name;
            this.label3.Text = g.Name;
            this.label4.Text = "+";
            this.label5.Text = "=>";
            this.label6.Text = g.ReactionName;
        }
    }
}
