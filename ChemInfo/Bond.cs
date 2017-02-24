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
        public Bond(Atom connectedAtom, BondType type)
        {
            m_connectedAtom = connectedAtom;
            m_bondType = type;
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
    }
}
