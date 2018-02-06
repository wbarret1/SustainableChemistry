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
        FunctionalGroup m_FunctGroup;

        public FunctionalGroupViewer(FunctionalGroup group)
        {
            InitializeComponent();
            m_FunctGroup = group;
            this.Text = group.Name;
            this.pictureBox1.Image = group.ReactionImage;
            this.dataGridView1.DataSource = group.References;
        }
    }
}
