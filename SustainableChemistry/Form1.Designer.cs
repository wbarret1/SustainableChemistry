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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.functionalityToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(544, 28);
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
            this.phosphorousToolStripMenuItem});
            this.functionalityToolStripMenuItem.Name = "functionalityToolStripMenuItem";
            this.functionalityToolStripMenuItem.Size = new System.Drawing.Size(105, 24);
            this.functionalityToolStripMenuItem.Text = "Functionality";
            // 
            // phosphorousToolStripMenuItem
            // 
            this.phosphorousToolStripMenuItem.Name = "phosphorousToolStripMenuItem";
            this.phosphorousToolStripMenuItem.Size = new System.Drawing.Size(169, 26);
            this.phosphorousToolStripMenuItem.Text = "Phosphorous";
            this.phosphorousToolStripMenuItem.Click += new System.EventHandler(this.phosphorousToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(544, 500);
            this.treeView1.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(41, 63);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(316, 260);
            this.listBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 500);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.treeView1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importFormTESTToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem enterSMILEToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem functionalityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phosphorousToolStripMenuItem;
        private System.Windows.Forms.ListBox listBox1;
    }
}

