﻿using System;
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
        ChemInfo.FunctionalGroupCollection m_FunctGroups;

        public FunctionalGroupViewer(ChemInfo.FunctionalGroupCollection groups)
        {
            InitializeComponent();
            m_FunctGroups = groups;
            this.comboBox1.Items.AddRange(m_FunctGroups.FunctionalGroups);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChemInfo.FunctionalGroup g = this.m_FunctGroups[this.comboBox1.SelectedItem.ToString()];
            int i = 0;
            foreach (ChemInfo.NamedReaction r in g.NamedReactions)
            {
                NamedReactionViewControl myControl = new NamedReactionViewControl();
                this.tableLayoutPanel1.Controls.Add(myControl, 0 /* Column Index */, i++ /* Row index */);
                string[] reactants = g.Reactants;
                if (reactants.Length == 2)
                {
                    ChemInfo.FunctionalGroup react0 = this.m_FunctGroups[reactants[0] + "s"];
                    ChemInfo.FunctionalGroup react1 = this.m_FunctGroups[reactants[1] + "s"];
                    myControl.Reactant1 = this.m_FunctGroups[reactants[0] + "s"].Image;
                    myControl.Reactant2 = this.m_FunctGroups[reactants[1] + "s"].Image;
                    myControl.Product = g.Image;
                    myControl.Reactant1Name = react0.Name;
                    myControl.Reactant2Name = react1.Name;
                    myControl.FunctionalGroupName = g.Name;
                    myControl.Catalyst = g.Catalyst;
                }
            }
            foreach (ChemInfo.NamedReaction r in g.NamedReactions)
            {
                NamedReactionViewControl myControl = new NamedReactionViewControl();
                this.tableLayoutPanel1.Controls.Add(myControl, 0 /* Column Index */, i++ /* Row index */);
                string[] reactants = g.Reactants;
                if (reactants.Length == 2)
                {
                    ChemInfo.FunctionalGroup react0 = this.m_FunctGroups[reactants[0] + "s"];
                    ChemInfo.FunctionalGroup react1 = this.m_FunctGroups[reactants[1] + "s"];
                    myControl.Reactant1 = this.m_FunctGroups[reactants[0] + "s"].Image;
                    myControl.Reactant2 = this.m_FunctGroups[reactants[1] + "s"].Image;
                    myControl.Product = g.Image;
                    myControl.Reactant1Name = react0.Name;
                    myControl.Reactant2Name = react1.Name;
                    myControl.FunctionalGroupName = g.Name;
                    myControl.Catalyst = g.Catalyst;
                }
            }
        }

        private void Clear()
        {
            //this.label1.Text = string.Empty;
            //this.label2.Text = string.Empty;
            //this.label3.Text = string.Empty;
            //this.label4.Text = string.Empty;
            //this.label5.Text = string.Empty;
            //this.label6.Text = string.Empty;
            //this.pictureBox1.Image = null;
            //this.pictureBox2.Image = null;
            //this.pictureBox3.Image = null;
        }
    }
}