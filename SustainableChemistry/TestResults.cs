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
    public partial class TestResults : Form
    {
        public TestResults()
        {
            InitializeComponent();
            this.listView1.Columns.Add("Name", -2, HorizontalAlignment.Left);
            string[] directories = System.IO.Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ToxRuns", "StructureData", System.IO.SearchOption.AllDirectories);
            int i = 0;
            foreach (string directory in directories)
            {
                System.Drawing.Image image = System.Drawing.Image.FromFile(directory + "\\structure.png");
                imageList1.Images.Add(image);
                System.IO.DirectoryInfo info = System.IO.Directory.GetParent(directory);
                ListViewItem item = new ListViewItem(info.Name) { ImageIndex = i++ };
                this.listView1.Items.Add(item);
            }
        }

        public string FilePath { get; set; }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ToxRuns\\" + this.listView1.SelectedItems[0].Text;
            string[] fileNames = System.IO.Directory.GetFiles(directory + "\\StructureData", "*.mol");
            this.FilePath = fileNames[0]; ;
            this.Close();
        }
    }
}
