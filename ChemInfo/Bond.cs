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
        public Bond(Atom parent, Atom connectedAtom, BondType type)
        {
            m_ParentAtom = parent;
            m_StaringPoint = new System.Drawing.Point(parent.X_2D, parent.Y_2D);
            m_connectedAtom = connectedAtom;
            m_bondType = type;
        }

        double m_length = 100;
        int m_Angle = 100;
        System.Drawing.Point m_StaringPoint;
        System.Drawing.Point m_EndingPoint;
        bool newStart = false;
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

        public BondStereo bondStereo;
        public string xNotUsed;
        public BondTopology bondTopology;
        public BondReactingCenterStatus reactingCenter;
        public double length
        {
            get
            {
                return m_length;
            }
            set
            {
                m_length = value;
                this.SetConnectedAtomLocation();
            }
        }

        public int Angle
        {
            get
            {
                return m_Angle;
            }
            set
            {
                m_Angle = value;
                this.SetConnectedAtomLocation();
            }
        }

        public System.Drawing.Point StartPoint
        {
            get
            {
                return m_StaringPoint;
            }
            set
            {
                this.m_ParentAtom.X_2D = value.X;
                this.m_ParentAtom.Y_2D = value.Y;
                this.m_StaringPoint = value;
                this.SetConnectedAtomLocation();
            }
        }

        public void SetConnectedAtomLocation()
        {
            int endX = this.m_ParentAtom.X_2D + (int)(this.m_length * Math.Sin(Math.PI*180/m_Angle));
            int endY = this.m_ParentAtom.Y_2D + (int)(this.m_length * Math.Cos(Math.PI * 180 / m_Angle));
            this.m_connectedAtom.X_2D = endX;
            this.m_connectedAtom.Y_2D = endY;
            this.m_EndingPoint = new System.Drawing.Point(endX, endY);
        }

        public System.Drawing.Point EndPoint
        {
            get
            {
                return m_EndingPoint;
            }
            //set
            //{                
            //    this.m_EndingPoint = value;
            //    this.m_connectedAtom.X_2D 
            //}
        }
    }
}
