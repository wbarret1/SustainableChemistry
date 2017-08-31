using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    public class smilesParser
    {
        string m_smile;

        public smilesParser()
        {
            m_smile = string.Empty;
        }

        public smilesParser(string smile)
        {
            m_smile = smile;
        }

        public string SMILE
        {
            get
            {
                return m_smile;
            }
            set
            {
                m_smile = value;
            }
        }

        public Molecule Parse()
        {
            if (m_smile == string.Empty)
            {
                // we didn't make changes in the last loop, so something is wrong.
                //
                return null;
                //throw new Exception("No SMILES string has been provided");
            }
            Molecule m = new Molecule();
            Atom a = null;
            this.Parse(m_smile, m, ref a, BondStereo.NotStereoOrUseXYZ);
            //m.LocateAtoms2D();
            //m.GetLocationBounds();
            return m;
        }


        string Parse(string smile, Molecule molecule, ref Atom last, BondStereo stereo)
        {
            string retVal = string.Empty;
            BondType nextBond = BondType.Single;
            Atom current = null;
            string smileLeftToParse = smile.Trim();
            string atomRegExPattern = @"^\[(\d*)([A-Z][a-z]?)([H]\d*)?((\+)*\d*)?((\-)*\d*)?(:\d+)?\]";
            string organicsRegExPattern = @"^[BCNOPSFI][lr]?";
            string aromaticAtomsRegExPattern = "^[bcnops]";
            string branchRegExPattern = @"^(((?'Open'\()[^\(^\)]*)+((?'Close-Open'\))[\S+)]*)+)*(?(Open)(?!))$";
            // A NOTE ABOUT RINGS... Rings use a one digit number, or a two digit nomber preceded by a % sign. The problem is that %0d
            // is the same as d. That is a one digit number (d) matches %0d. To handle this, the ring match string includes a non-matching
            // group containing %0. If a percent sign is there, followed by a zero, the 
            //string ringRegExPattern = @"^(((?'Open'[%d]*\d)[^\(^\)]*)+((?'Close-Open'[%d]*\d)[\S+)]*)+)*(?(Open)(?!))$";
            //string ringRegExPattern = @"^([%\d]*\d)((?'Open'[%\d]*\d)[^\d]*)+((?'Close-Open'([%\d]*\d)[\S+]*)+)*(?(Open)(?!))$";
            //string ringRegExPattern = @"^(?:%0)?(\d)(\S+?)(?:%0)?(\1)";
            string ringRegExPattern = @"^([%\d]?\d)(\S+?)(\1)(\S*)";
            string bondRegExPattern = @"^[-=#$:/\.]";
            string chiralRegExPattern = @"^\[(C)(@@?)\]";
            int smileLeftLength = 0;
            while (smileLeftToParse.Length > 0)
            {
                if (smileLeftToParse.Length == smileLeftLength)
                {
                    // we didn't make changes in the last loop, so something is wrong.
                    //
                    //System.Windows.Forms.MessageBox.Show("Remaining SMILES string is: " + smileLeftToParse, "Error in SMILES string!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    last = current;
                    return string.Empty;
                }
                smileLeftLength = smileLeftToParse.Length;
                if (smileLeftToParse.IndexOf("*") == 0)
                {
                    current = new Atom("*");
                    molecule.AddAtom(current);
                    molecule.AddBond(current, last, nextBond, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.Unmarked);
                    nextBond = BondType.Single;
                    last = current;
                    smileLeftToParse = smileLeftToParse.Remove(0, 1);
                }
                if (smileLeftToParse.IndexOf("[*]") == 0)
                {
                    current = new Atom("*");
                    molecule.AddAtom(current);
                    molecule.AddBond(current, last, BondType.SingleOrAromatic, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.Unmarked);
                    nextBond = BondType.Single;
                    last = current;
                    smileLeftToParse = smileLeftToParse.Remove(0, 3);
                }
                //System.Text.RegularExpressions.Match organicsRingMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, organicsRegExPattern);
                //if (organicsRingMatch.Length > 0)
                //{
                //    if (organicsRingMatch.Value.Length == 2 && !((organicsRingMatch.Value == "Cl") || (organicsRingMatch.Value == "Br")))
                //    {
                //        System.Windows.Forms.MessageBox.Show(organicsRingMatch + " is not a valid organic element." + System.Environment.NewLine + "Remaining SMILES string is: " + smileLeftToParse, "Error in SMILES string!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        this.textBox1.Text = string.Empty;
                //        return string.Empty;
                //    }
                //    current = new TreeNode(organicsRingMatch.Value);
                //    nodes.Add(current);
                //    smileLeftToParse = smileLeftToParse.Remove(0, organicsRingMatch.Value.Length);
                //    current.Nodes.Add(new TreeNode("Organic Default"));
                //}
                System.Text.RegularExpressions.Match atomMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, atomRegExPattern);
                if (atomMatch.Length > 0)
                {
                    int isotope = 0;
                    if (atomMatch.Groups[1].Value.Length > 0) isotope = Convert.ToInt32(atomMatch.Groups[1].Value);
                    if (atomMatch.Value.Contains("HH"))
                    {
                        //System.Windows.Forms.MessageBox.Show("Hydrogens can not be bonded to hydrogens.", "Invalid SMILES string!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //this.textBox1.Text = string.Empty;
                        return string.Empty;
                    }
                    int charge = 0;
                    int hydrogens = 0;
                    current = new Atom(atomMatch.Groups[2].Value);
                    if (atomMatch.Groups[2].Value == "H") hydrogens = 0;
                    if (atomMatch.Groups[3].Value.Length > 0)
                    {
                        if (atomMatch.Groups[3].Value.Length == 1) hydrogens = 1;
                        else hydrogens = Convert.ToInt32(atomMatch.Groups[3].Value.Remove(0, 1));
                    }
                    if (atomMatch.Groups[4].Value.Length > 0)
                    {
                        try
                        {
                            charge = Convert.ToInt32(atomMatch.Groups[4].Value);
                        }
                        catch (FormatException ex)
                        {
                            charge = atomMatch.Groups[4].Value.Length;
                        }
                    }
                    if (atomMatch.Groups[6].Value.Length > 0)
                    {
                        try
                        {
                            charge = Convert.ToInt32(atomMatch.Groups[6].Value);
                        }
                        catch (FormatException ex)
                        {
                            charge = -1 * atomMatch.Groups[6].Value.Length;
                        }
                    }
                    int atomClass = 0;
                    if (atomMatch.Groups[8].Value.Length > 0) atomClass = Convert.ToInt32(atomMatch.Groups[8].Value.Remove(0, 1));
                    molecule.AddAtom(current);
                    molecule.AddBond(last, current, BondType.SingleOrAromatic, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.Unmarked);
                    nextBond = BondType.Single;
                    last = current;
                    //current.Nodes.Add("Charge = " + charge.ToString());
                    //current.Nodes.Add("Number Of Hydrogens = " + hydrogens.ToString());
                    //current.Nodes.Add("Isotope = " + (isotope > 0 ? isotope.ToString() : "No Isotope Specified"));
                    //current.Nodes.Add("Atom Class = " + atomClass);
                    smileLeftToParse = smileLeftToParse.Remove(0, atomMatch.Value.Length);
                }
                System.Text.RegularExpressions.Match organicsMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, organicsRegExPattern);
                if (organicsMatch.Length > 0)
                {
                    if (organicsMatch.Value.Length == 2 && !((organicsMatch.Value == "Cl") || (organicsMatch.Value == "Br")))
                    {
                        //System.Windows.Forms.MessageBox.Show(organicsRingMatch + " is not a valid organic element." + System.Environment.NewLine + "Remaining SMILES string is: " + smileLeftToParse, "Error in SMILES string!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //this.textBox1.Text = string.Empty;
                        return string.Empty;
                    }
                    current = new Atom(organicsMatch.Value, AtomType.ORGANIC);
                    molecule.AddAtom(current);
                    molecule.AddBond(last, current, nextBond, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.Unmarked);
                    nextBond = BondType.Single;
                    last = current;
                    smileLeftToParse = smileLeftToParse.Remove(0, organicsMatch.Value.Length);
                    //current.Nodes.Add(new TreeNode("Organic Default"));
                }
                System.Text.RegularExpressions.Match chiralMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, chiralRegExPattern);
                if (chiralMatch.Length > 0)
                {
                    Chirality c = Chirality.TETRAHEDRAL_CLOCKWISE;
                    if (chiralMatch.Groups[2].Value == "@@") c = Chirality.TETRAHEDRAL_COUNTER_CLOCKWISE;
                    current = new Atom(chiralMatch.Groups[1].Value, AtomType.ORGANIC, c);
                    molecule.AddAtom(current);
                    molecule.AddBond(last, current, nextBond, BondStereo.NotStereoOrUseXYZ, BondTopology.Chain, BondReactingCenterStatus.Unmarked);
                    nextBond = BondType.Single;
                    last = current;
                    smileLeftToParse = smileLeftToParse.Remove(0, chiralMatch.Value.Length);
                    //current.Nodes.Add(new TreeNode("Organic Default"));
                }
                System.Text.RegularExpressions.Match aromatic = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, aromaticAtomsRegExPattern);
                if (aromatic.Length > 0)
                {
                    current = new Atom(aromatic.Value.ToUpper(), AtomType.AROMATIC);
                    nextBond = BondType.Single;
                    molecule.AddAtom(current);
                    if (last.AtomType != AtomType.AROMATIC)
                    {
                        molecule.AddBond(last, current, BondType.Single, BondStereo.NotStereoOrUseXYZ, BondTopology.Ring, BondReactingCenterStatus.Unmarked);
                    }
                    else molecule.AddBond(last, current, BondType.Aromatic, BondStereo.NotStereoOrUseXYZ, BondTopology.Ring, BondReactingCenterStatus.Unmarked);
                    last = current;
                    smileLeftToParse = smileLeftToParse.Remove(0, 1);
                }
                System.Text.RegularExpressions.Match bondMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, bondRegExPattern);
                if (bondMatch.Length > 0)
                {
                    if (bondMatch.Value == "-") nextBond = BondType.Single;
                    if (bondMatch.Value == "=") nextBond = BondType.Double;
                    if (bondMatch.Value == "#") nextBond = BondType.Triple;
                    smileLeftToParse = smileLeftToParse.Remove(0, bondMatch.Value.Length);
                }
                System.Text.RegularExpressions.Match branchMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, branchRegExPattern);
                if (branchMatch.Length > 0)
                {
                    Atom temp = last;
                    
                    smileLeftToParse = Parse(branchMatch.Groups[5].Value, molecule, ref last, stereo) + branchMatch.Groups[3].Value;
                    if (smileLeftToParse.Length > 1) smileLeftToParse = smileLeftToParse.Remove(0, 1);
                    last = temp;
                }
                else if (smileLeftToParse.IndexOf('(') == 0)
                {
                    smileLeftToParse = smileLeftToParse.Remove(0, 1);
                    retVal = retVal + "(";
                }
                System.Text.RegularExpressions.Match ringMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, ringRegExPattern);
                System.Text.RegularExpressions.Match moveCharMatch = System.Text.RegularExpressions.Regex.Match(smileLeftToParse, @"^([%\d]?\d)");
                if (ringMatch.Length > 0)
                {
                    Atom temp = last;
                    smileLeftToParse = Parse(ringMatch.Groups[2].Value, molecule, ref last, stereo) + ringMatch.Groups[4].Value;
                    molecule.AddBond(temp, last, nextBond, BondStereo.NotStereoOrUseXYZ, BondTopology.Ring, BondReactingCenterStatus.Unmarked);
                    nextBond = BondType.Single;
                    last = temp;
                }
                else if (moveCharMatch.Length > 0)
                {
                    if (smileLeftToParse.Length == 1)
                    {
                        retVal = string.Empty;
                        smileLeftToParse = string.Empty;
                        System.Windows.Forms.MessageBox.Show("Ring label: " + moveCharMatch.Value, "Ignoring unmatched ring.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    }
                    else
                    {
                        smileLeftToParse = smileLeftToParse.Remove(0, moveCharMatch.Length);
                        retVal = retVal + moveCharMatch;
                    }
                }
                if (smileLeftToParse.IndexOf(')') == 0)
                {
                    System.Windows.Forms.MessageBox.Show(smileLeftToParse, "Ignoring unmatched closed parentheses.", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    smileLeftToParse = smileLeftToParse.Remove(0, 1);
                }
            }
            last = current;
            return retVal;
        }
    }
}
