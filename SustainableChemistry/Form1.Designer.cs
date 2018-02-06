﻿namespace SustainableChemistry
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFormTESTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.enterSMILEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionalityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phosphorousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findSMARTSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testSubgraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.referencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRISFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.moleculeViewer1 = new SustainableChemistry.MoleculeViewer();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.functionalityToolStripMenuItem,
            this.testsToolStripMenuItem,
            this.referencesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1099, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFormTESTToolStripMenuItem,
            this.toolStripSeparator1,
            this.enterSMILEToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importFormTESTToolStripMenuItem
            // 
            this.importFormTESTToolStripMenuItem.Name = "importFormTESTToolStripMenuItem";
            this.importFormTESTToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.importFormTESTToolStripMenuItem.Text = "Import form TEST";
            this.importFormTESTToolStripMenuItem.Click += new System.EventHandler(this.importFormTESTToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(198, 6);
            // 
            // enterSMILEToolStripMenuItem
            // 
            this.enterSMILEToolStripMenuItem.Name = "enterSMILEToolStripMenuItem";
            this.enterSMILEToolStripMenuItem.Size = new System.Drawing.Size(201, 26);
            this.enterSMILEToolStripMenuItem.Text = "Enter SMILE";
            this.enterSMILEToolStripMenuItem.Click += new System.EventHandler(this.enterSMILEToolStripMenuItem_Click);
            // 
            // functionalityToolStripMenuItem
            // 
            this.functionalityToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.phosphorousToolStripMenuItem,
            this.findSMARTSToolStripMenuItem});
            this.functionalityToolStripMenuItem.Name = "functionalityToolStripMenuItem";
            this.functionalityToolStripMenuItem.Size = new System.Drawing.Size(105, 24);
            this.functionalityToolStripMenuItem.Text = "Functionality";
            // 
            // phosphorousToolStripMenuItem
            // 
            this.phosphorousToolStripMenuItem.Name = "phosphorousToolStripMenuItem";
            this.phosphorousToolStripMenuItem.Size = new System.Drawing.Size(171, 26);
            this.phosphorousToolStripMenuItem.Text = "Phosphorous";
            this.phosphorousToolStripMenuItem.Click += new System.EventHandler(this.phosphorousToolStripMenuItem_Click);
            // 
            // findSMARTSToolStripMenuItem
            // 
            this.findSMARTSToolStripMenuItem.Name = "findSMARTSToolStripMenuItem";
            this.findSMARTSToolStripMenuItem.Size = new System.Drawing.Size(171, 26);
            this.findSMARTSToolStripMenuItem.Text = "Find SMARTS";
            this.findSMARTSToolStripMenuItem.Click += new System.EventHandler(this.findSMARTSToolStripMenuItem_Click);
            // 
            // testsToolStripMenuItem
            // 
            this.testsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testSubgraphToolStripMenuItem});
            this.testsToolStripMenuItem.Name = "testsToolStripMenuItem";
            this.testsToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.testsToolStripMenuItem.Text = "Tests";
            // 
            // testSubgraphToolStripMenuItem
            // 
            this.testSubgraphToolStripMenuItem.Name = "testSubgraphToolStripMenuItem";
            this.testSubgraphToolStripMenuItem.Size = new System.Drawing.Size(178, 26);
            this.testSubgraphToolStripMenuItem.Text = "Test Subgraph";
            this.testSubgraphToolStripMenuItem.Click += new System.EventHandler(this.testSubgraphToolStripMenuItem_Click);
            // 
            // referencesToolStripMenuItem
            // 
            this.referencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editReferencesToolStripMenuItem,
            this.showReferencesToolStripMenuItem,
            this.importRISFileToolStripMenuItem,
            this.exportJSONToolStripMenuItem});
            this.referencesToolStripMenuItem.Name = "referencesToolStripMenuItem";
            this.referencesToolStripMenuItem.Size = new System.Drawing.Size(93, 24);
            this.referencesToolStripMenuItem.Text = "References";
            // 
            // editReferencesToolStripMenuItem
            // 
            this.editReferencesToolStripMenuItem.Name = "editReferencesToolStripMenuItem";
            this.editReferencesToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.editReferencesToolStripMenuItem.Text = "Edit References";
            this.editReferencesToolStripMenuItem.Click += new System.EventHandler(this.editReferencesToolStripMenuItem_Click);
            // 
            // showReferencesToolStripMenuItem
            // 
            this.showReferencesToolStripMenuItem.Name = "showReferencesToolStripMenuItem";
            this.showReferencesToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.showReferencesToolStripMenuItem.Text = "Show References";
            this.showReferencesToolStripMenuItem.Click += new System.EventHandler(this.showReferencesToolStripMenuItem_Click);
            // 
            // importRISFileToolStripMenuItem
            // 
            this.importRISFileToolStripMenuItem.Name = "importRISFileToolStripMenuItem";
            this.importRISFileToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.importRISFileToolStripMenuItem.Text = "Import RIS File";
            this.importRISFileToolStripMenuItem.Click += new System.EventHandler(this.importRISFileToolStripMenuItem_Click);
            // 
            // exportJSONToolStripMenuItem
            // 
            this.exportJSONToolStripMenuItem.Name = "exportJSONToolStripMenuItem";
            this.exportJSONToolStripMenuItem.Size = new System.Drawing.Size(196, 26);
            this.exportJSONToolStripMenuItem.Text = "Export JSON";
            this.exportJSONToolStripMenuItem.Click += new System.EventHandler(this.exportJSONToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 30);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1099, 636);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(1091, 607);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Atoms";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.moleculeViewer1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1083, 599);
            this.splitContainer1.SplitterDistance = 822;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.trackBar1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(257, 599);
            this.splitContainer2.SplitterDistance = 70;
            this.splitContainer2.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.Location = new System.Drawing.Point(0, 0);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trackBar1.Maximum = 250;
            this.trackBar1.Minimum = 20;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(257, 56);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Value = 20;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(257, 525);
            this.propertyGrid1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(1091, 607);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Functionalities";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(4, 4);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.listBox1);
            this.splitContainer3.Size = new System.Drawing.Size(1083, 599);
            this.splitContainer3.SplitterDistance = 258;
            this.splitContainer3.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1083, 337);
            this.listBox1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1083, 258);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // moleculeViewer1
            // 
            this.moleculeViewer1.AutoScroll = true;
            this.moleculeViewer1.AutoScrollMinSize = new System.Drawing.Size(1320, 1020);
            this.moleculeViewer1.AutoSize = true;
            this.moleculeViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.moleculeViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.moleculeViewer1.GridLineColor = System.Drawing.Color.LightBlue;
            this.moleculeViewer1.GridLineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.moleculeViewer1.GridLineWidth = 1;
            this.moleculeViewer1.GridSize = 50D;
            this.moleculeViewer1.Location = new System.Drawing.Point(0, 0);
            this.moleculeViewer1.Margin = new System.Windows.Forms.Padding(5);
            this.moleculeViewer1.MarginColor = System.Drawing.Color.Green;
            this.moleculeViewer1.MarginLineDashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.moleculeViewer1.MarginLineWidth = 1;
            this.moleculeViewer1.MaximumSize = new System.Drawing.Size(1333, 1231);
            this.moleculeViewer1.MinimumGridSize = 1D;
            this.moleculeViewer1.Name = "moleculeViewer1";
            this.moleculeViewer1.NonPrintingAreaColor = System.Drawing.Color.Gray;
            this.moleculeViewer1.PrintGrid = false;
            this.moleculeViewer1.PrintMargins = false;
            this.moleculeViewer1.SelectedObject = null;
            this.moleculeViewer1.SelectedObjects = new SustainableChemistry.GraphicObject[0];
            this.moleculeViewer1.ShowGrid = false;
            this.moleculeViewer1.ShowMargins = false;
            this.moleculeViewer1.Size = new System.Drawing.Size(822, 599);
            this.moleculeViewer1.SurfaceBounds = new System.Drawing.Rectangle(0, 0, 1100, 850);
            this.moleculeViewer1.SurfaceMargins = new System.Drawing.Rectangle(100, 100, 900, 650);
            this.moleculeViewer1.TabIndex = 0;
            this.moleculeViewer1.Zoom = 1.2D;
            this.moleculeViewer1.SelectionChanged += new SustainableChemistry.MoleculeViewer.SelectionChangedHandler(this.moleculeViewer1_SelectionChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 666);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFormTESTToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem enterSMILEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem functionalityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phosphorousToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private MoleculeViewer moleculeViewer1;
        private System.Windows.Forms.ToolStripMenuItem findSMARTSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testSubgraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem referencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showReferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editReferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importRISFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportJSONToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listBox1;
    }
}

