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
        ChemInfo.FunctionalGroup m_FunctionalGroup;

        public ReactionEditor(ChemInfo.FunctionalGroupCollection fGroups)
        {
            InitializeComponent();
            this.m_FunctionalGroups = fGroups;
            this.productComboBox.Items.AddRange(m_FunctionalGroups.FunctionalGroups);
            this.reactantAComboBox.Items.AddRange(m_FunctionalGroups.FunctionalGroups);
            this.reactantBComboBox.Items.AddRange(m_FunctionalGroups.FunctionalGroups);
        }

        private void ReactionEditor_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void ReactionNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChemInfo.NamedReaction r = m_FunctionalGroup.NamedReactions[ReactionNameComboBox.SelectedItem.ToString()];
            reactantAComboBox.SelectedItem = r.ReactantA.ToUpper();
            reactantBComboBox.SelectedItem = r.ReactantB.ToUpper();
            this.Solvent = r.Solvent;
            this.AcidBase = r.AcidBase;
        }

        private void productComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReactionNameComboBox.Items.Clear();
            m_FunctionalGroup = m_FunctionalGroups[productComboBox.SelectedItem.ToString()];
            foreach (ChemInfo.NamedReaction r in m_FunctionalGroup.NamedReactions)
            {
                this.ReactionNameComboBox.Items.Add(r.Name);
            }
            reactantAComboBox.Text = string.Empty;
            reactantBComboBox.Text = string.Empty;
            pictureBox1.Image = m_FunctionalGroup.Image;
            ReactionNameComboBox.SelectedIndex = 0;
        } 

        public ChemInfo.Solvent Solvent
        {
            get
            {
                if (this.acetoneButton.Checked == true) return ChemInfo.Solvent.ACETONE;
                if (this.AcetonitrileButton.Checked == true) return ChemInfo.Solvent.ACETONITRILE;
                if (this.AmmoniaButton.Checked == true) return ChemInfo.Solvent.AQUEOUS_AMMONIA;
                if (this.BenzoicAcidButton.Checked == true) return ChemInfo.Solvent.BENZOIC_ACID_TOLUENE;
                if (this.DichloromethaneButton.Checked == true) return ChemInfo.Solvent.DCM;
                if (this.DimethylCarbonateButton.Checked == true) return ChemInfo.Solvent.DMC;
                if (this.DimethylformamideButton.Checked == true) return ChemInfo.Solvent.DMF;
                if (this.DimethylSulfoxideSolventButton.Checked == true) return ChemInfo.Solvent.DMSO;
                if (this.EthanolSolventButton.Checked == true) return ChemInfo.Solvent.ETHANOL;
                if (this.HaloKetoneSolventButton.Checked == true) return ChemInfo.Solvent.HALO_KETONE;
                if (this.MethanolSolventButton.Checked == true) return ChemInfo.Solvent.METHANOL;
                if (this.TriethylamineSolventButton.Checked == true) return ChemInfo.Solvent.METHANOL_TRIETHYLAMINE;
                if (this.NitreneSolventButton.Checked == true) return ChemInfo.Solvent.NITRENE;
                if (this.NitritesSolventButton.Checked == true) return ChemInfo.Solvent.NITRITES;
                if (this.TetrahydrofuranSolventButton.Checked == true) return ChemInfo.Solvent.THF;
                if (this.TolueneSolventButton.Checked == true) return ChemInfo.Solvent.TOLUENE;
                if (this.WaterSolventButton.Checked == true) return ChemInfo.Solvent.WATER;
                return ChemInfo.Solvent.NONE;
            }
            set
            {
                this.NoSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.ACETONE) this.acetoneButton.Checked = true;
                if (value == ChemInfo.Solvent.ACETONITRILE) this.AcetonitrileButton.Checked = true;
                if (value == ChemInfo.Solvent.AQUEOUS_AMMONIA) this.AmmoniaButton.Checked = true;
                if (value == ChemInfo.Solvent.BENZOIC_ACID_TOLUENE) this.BenzoicAcidButton.Checked = true;
                if (value == ChemInfo.Solvent.DCM) this.DichloromethaneButton.Checked = true;
                if (value == ChemInfo.Solvent.DMC) this.DimethylCarbonateButton.Checked = true;
                if (value == ChemInfo.Solvent.DMF) this.DimethylformamideButton.Checked = true;
                if (value == ChemInfo.Solvent.DMSO) this.DimethylSulfoxideSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.ETHANOL) this.EthanolSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.HALO_KETONE) this.HaloKetoneSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.METHANOL) this.MethanolSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.METHANOL_TRIETHYLAMINE) this.TriethylamineSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.NITRENE) this.NitreneSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.NITRITES) this.NitritesSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.THF) this.TetrahydrofuranSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.TOLUENE) this.TolueneSolventButton.Checked = true;
                if (value == ChemInfo.Solvent.WATER) this.WaterSolventButton.Checked = true;
            }
        }
        public string Catalyst { get; set; }
        public ChemInfo.AcidBase AcidBase
        {
            get
            {
                if (AcidButton.Checked) return ChemInfo.AcidBase.ACID;
                if (BasicButton.Checked) return ChemInfo.AcidBase.BASE;
                if (AcidBaseButton.Checked) return ChemInfo.AcidBase.ACID_BASE;
                return ChemInfo.AcidBase.NONE;
            }
            set
            {
                this.NotAcidBaseButton.Checked = true;
                if (value == ChemInfo.AcidBase.ACID) AcidButton.Checked = true;
                if (value == ChemInfo.AcidBase.ACID_BASE) AcidBaseButton.Checked = true;
                if (value == ChemInfo.AcidBase.BASE) BasicButton.Checked = true;
            }
        }

        ChemInfo.AcidBase CheckAcidBase(string acidBase)
        {
            if (acidBase.ToLower() == "acid") return ChemInfo.AcidBase.ACID;
            if (acidBase.ToLower() == "base") return ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/heat") return ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/acid") return ChemInfo.AcidBase.ACID_BASE;
            return ChemInfo.AcidBase.NONE;
        }

        void SetAcidBase(string acidBase)
        {
            if (acidBase.ToLower() == "acid") this.AcidBase = ChemInfo.AcidBase.ACID;
            if (acidBase.ToLower() == "base") this.AcidBase = ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/heat") this.AcidBase = ChemInfo.AcidBase.BASE;
            if (acidBase.ToLower() == "base/acid") this.AcidBase = ChemInfo.AcidBase.ACID_BASE;
            if (acidBase.ToLower() == string.Empty) this.AcidBase = ChemInfo.AcidBase.NONE;
        }

        //void SetSolvent(String solvent)
        //{
        //    if (solvent.ToLower() == "aceton") this.acetoneButton.Checked = true;
        //    if (solvent.ToLower() == "acetonitrile ") this.AcetonitrileButton.Checked = true;
        //    if (solvent.ToLower() == "aqueous ammonia/ the treated with lead nitrate") this.AmmoniaButton.Checked = true;
        //    if (solvent.ToLower() == "benzoic acid /toluene") this.BenzoicAcidButton.Checked = true;
        //    if (solvent.ToLower() == "dcm") this.DichloromethaneButton.Checked = true;
        //    if (solvent.ToLower() == "dmc") this.DimethylCarbonateButton.Checked = true;
        //    if (solvent.ToLower() == "dmf") this.DimethylformamideButton.Checked = true;
        //    if (solvent.ToLower() == "dmso") this.DimethylSulfoxideSolventButton.Checked = true;
        //    if (solvent.ToLower() == "ethanol") this.EthanolSolventButton.Checked = true;
        //    if (solvent.ToLower() == "halo ketones") this.HaloKetoneSolventButton.Checked = true;
        //    if (solvent.ToLower() == "methanol") this.MethanolSolventButton.Checked = true;
        //    if (solvent.ToLower() == "methanol/ triethylamine") this.TriethylamineSolventButton.Checked = true;
        //    if (solvent.ToLower() == "nitrene") this.NitreneSolventButton.Checked = true;
        //    if (solvent.ToLower() == "nitrites") this.NitritesSolventButton.Checked = true;
        //    if (solvent.ToLower() == "thf") this.TetrahydrofuranSolventButton.Checked = true;
        //    if (solvent.ToLower() == "toluene") this.TolueneSolventButton.Checked = true;
        //    if (solvent.ToLower() == "water") this.WaterSolventButton.Checked = true;
        //}
    }
}
