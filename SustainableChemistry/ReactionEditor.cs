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
    public partial class ReactionEditor : Form
    {

        ChemInfo.FunctionalGroupCollection m_FunctionalGroups;

        public ReactionEditor(ChemInfo.FunctionalGroupCollection fGroups)
        {
            InitializeComponent();
            this.m_FunctionalGroups = fGroups;
        }

 
    }
}
