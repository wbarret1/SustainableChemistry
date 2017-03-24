using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    public enum BondType
    {
        Single = 1,
        Double = 2,
        Triple = 3,
        Aromatic = 4,
        SingleOrDouble = 5,
        SingleOrAromatic = 6,
        DoubleOrAromatic = 7,
        Any = 8
    }

    public enum BondStereo
    {
        NotStereoOrUseXYZ = 0,
        Up = 1,
        cisOrTrans = 3,
        Down = 4,
        Either = 6,
    }

    public enum BondTopology
    {
        Either = 0,
        Ring = 1,
        Chain = 3,
    }

    [Flags]
    public enum BondReactingCenterStatus
    {
        notACenter = -1,
        Unmarked = 0,
        aCenter = 1,
        noChange = 2,
        bondMadeOrBroken = 4,
        bondOrderChanges = 8
    }

    class BondTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if (typeof(Bond).IsAssignableFrom(destinationType))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if (typeof(System.String).IsAssignableFrom(destinationType) && typeof(Bond).IsAssignableFrom(value.GetType()))
            {
                return ((Bond)value).ConnectedAtom.AtomicName;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    };


    [System.ComponentModel.TypeConverter(typeof(BondTypeConverter))]
    public class Bond
    {
        public Bond(Atom parent, Atom connectedAtom, BondType type, BondStereo stereo, BondTopology topology, BondReactingCenterStatus rcStatus)
        {
            m_ParentAtom = parent;
            m_connectedAtom = connectedAtom;
            m_bondType = type;
            m_bondStereo = stereo;
            m_BondTopology = topology;
            m_bondReactingCenter = rcStatus;
        }

        Atom m_ParentAtom;
        public Atom ParentAtom
        {
            get
            {
                return m_ParentAtom;
            }
        }

        Atom m_connectedAtom;
        public Atom ConnectedAtom
        {
            get
            {
                return m_connectedAtom;
            }
        }

        BondType m_bondType;
        public BondType BondType
        {
            get
            {
                return m_bondType;
            }
        }

        BondStereo m_bondStereo;
        public BondStereo Stereo
        {
            get
            {
                return m_bondStereo;
            }
        }

        public string xNotUsed;
        BondTopology m_BondTopology;
        public BondTopology Topology
        {
            get
            {
                return m_BondTopology;
            }
        }

        BondReactingCenterStatus m_bondReactingCenter;
        public BondReactingCenterStatus ReactingCenter
        {
            get
            {
                return m_bondReactingCenter;
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(this.LengthSquared);
            }

        }

        public double LengthSquared
        {
            get
            {
                return Math.Pow((this.ParentAtom.Location2D.X - this.ConnectedAtom.Location2D.X), 2) + Math.Pow((this.ParentAtom.Location2D.Y - this.ConnectedAtom.Location2D.Y), 2);
            }

        }

        //public int Angle
        //{
        //    get
        //    {
        //        return m_Angle;
        //    }
        //    set
        //    {
        //        m_Angle = value;
        //        this.ParentAtom.Angle_2D = value;
        //    }
        //}

        public System.Drawing.Point StartPoint
        {
            get
            {
                return this.ParentAtom.Location2D;
            }
            //set
            //{
            //    this.m_ParentAtom.Location2D = new System.Drawing.Point(value.X, value.Y);
            //    this.m_StaringPoint = value;
            //}
        }

        //bool m_SettingLocation = false;
        //public void SetParentAtomLocation()
        //{
        //    if (m_SettingLocation) return;
        //    m_SettingLocation = true;
        //    int startX = this.m_connectedAtom.Location2D.X + (int)(this.m_length * Math.Cos(2 * Math.PI * (float)m_Angle / 360.0));
        //    int startY = this.m_connectedAtom.Location2D.Y + (int)(this.m_length * Math.Sin(2 * Math.PI * (float)m_Angle / 360.0));
        //    this.m_StaringPoint = new System.Drawing.Point(startX, startX);
        //    this.m_ParentAtom.Location2D = new System.Drawing.Point(startX, startY);
        //    m_SettingLocation = false;
        //}

        //public void SetBondededAtomLocation()
        //{
        //    if (m_SettingLocation) return;
        //    m_SettingLocation = true;
        //    int endX = this.m_ParentAtom.Location2D.X + (int)(this.m_length * Math.Cos(2 * Math.PI * (float)m_Angle / 360.0));
        //    int endY = this.m_ParentAtom.Location2D.Y + (int)(this.m_length * Math.Sin(2 * Math.PI * (float)m_Angle / 360.0));
        //    this.m_EndingPoint = new System.Drawing.Point(endX, endY);
        //    this.m_connectedAtom.Location2D = new System.Drawing.Point(endX, endY);
        //    m_SettingLocation = false;
        //}

        public System.Drawing.Point EndPoint
        {
            get
            {
                return m_connectedAtom.Location2D;
            }
            //set
            //{
            //    this.m_EndingPoint = value;
            //    //this.m_connectedAtom.Location.X
            //}
        }
    }
}
