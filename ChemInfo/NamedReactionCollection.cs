﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemInfo
{
    class NamedReactionCollectionTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if ((typeof(NamedReactionCollection)).IsAssignableFrom(destinationType))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override Object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value, System.Type destinationType)
        {
            if ((typeof(System.String)).IsAssignableFrom(destinationType) && (typeof(NamedReactionCollection).IsAssignableFrom(value.GetType())))
            {
                return string.Empty;
                //return ((FunctionalGroupCollection)value).AtomList;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    };

    [Serializable]
    [System.ComponentModel.TypeConverter(typeof(NamedReactionCollectionTypeConverter))]
    public sealed class NamedReactionCollection : System.ComponentModel.BindingList<NamedReaction>,
        System.ComponentModel.ICustomTypeDescriptor
    {
        public NamedReactionCollection()
        {

        }

        public string[] NamedReaction
        {
            get
            {
                string[] retVal = new string[this.Count];
                for (int i = 0; i < this.Count; i++)
                    retVal[i] = this[i].Name;
                return retVal;
            }
        }

        //public System.Drawing.Image Image(string groupName)
        //{
        //    foreach (NamedReaction g in this)
        //    {
        //        if (g.Name == groupName)
        //        {
        //            return g.Image;
        //        }
        //    }
        //    return null;
        //}

        public NamedReaction this[string name]
        {
            get
            {
                foreach (NamedReaction g in this)
                {
                    if (g.Name.ToLower() == name.ToLower())
                    {
                        return g;
                    }
                }
                return null;
            }
        }

        //[System.ComponentModel.Browsable(false)]
        //public string AtomList
        //{
        //    get
        //    {
        //        string retVal = string.Empty;
        //        if (this.Count > 1)
        //        {
        //            for (int i = 0; i < this.Count - 2; i++)
        //            {
        //                retVal = retVal + this[i].ConnectedAtom.AtomicSymbol + ", ";
        //            }
        //        }
        //        if (this.Count < 1) return retVal;
        //        return retVal + this[this.Count - 1].ConnectedAtom.AtomicSymbol;
        //    }
        //}

        // Implementation of ICustomTypeDescriptor: 

        String System.ComponentModel.ICustomTypeDescriptor.GetClassName()
        {
            return System.ComponentModel.TypeDescriptor.GetClassName(this, true);
        }

        System.ComponentModel.AttributeCollection System.ComponentModel.ICustomTypeDescriptor.GetAttributes()
        {
            return System.ComponentModel.TypeDescriptor.GetAttributes(this, true);
        }

        String System.ComponentModel.ICustomTypeDescriptor.GetComponentName()
        {
            return System.ComponentModel.TypeDescriptor.GetComponentName(this, true);
        }

        System.ComponentModel.TypeConverter System.ComponentModel.ICustomTypeDescriptor.GetConverter()
        {
            return System.ComponentModel.TypeDescriptor.GetConverter(this, true);
        }

        System.ComponentModel.EventDescriptor System.ComponentModel.ICustomTypeDescriptor.GetDefaultEvent()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultEvent(this, true);
        }

        System.ComponentModel.PropertyDescriptor System.ComponentModel.ICustomTypeDescriptor.GetDefaultProperty()
        {
            return System.ComponentModel.TypeDescriptor.GetDefaultProperty(this, true);
        }

        Object System.ComponentModel.ICustomTypeDescriptor.GetEditor(Type editorBaseType)
        {
            return System.ComponentModel.TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        System.ComponentModel.EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, attributes, true);
        }

        System.ComponentModel.EventDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetEvents()
        {
            return System.ComponentModel.TypeDescriptor.GetEvents(this, true);
        }

        Object System.ComponentModel.ICustomTypeDescriptor.GetPropertyOwner(System.ComponentModel.PropertyDescriptor pd)
        {
            return this;
        }

        System.ComponentModel.PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
        {
            return ((System.ComponentModel.ICustomTypeDescriptor)this).GetProperties();
        }

        System.ComponentModel.PropertyDescriptorCollection System.ComponentModel.ICustomTypeDescriptor.GetProperties()
        {
            // Create a new collection object PropertyDescriptorCollection
            System.ComponentModel.PropertyDescriptorCollection pds = new System.ComponentModel.PropertyDescriptorCollection(null);
            // Iterate the list of parameters
            for (int i = 0; i < this.Items.Count; i++)
            {
                // For each parameter create a property descriptor 
                // and add it to the PropertyDescriptorCollection instance
                NamedReactionCollectionPropertyDescriptor pd = new NamedReactionCollectionPropertyDescriptor(this, i);
                pds.Add(pd);
            }
            return pds;
        }
    }

    /// <summary>
    /// Summary description for CollectionpublicDescriptor.
    /// </summary>
    [System.Runtime.InteropServices.ComVisibleAttribute(false)]
    class NamedReactionCollectionPropertyDescriptor : System.ComponentModel.PropertyDescriptor
    {
        private NamedReactionCollection collection;
        private int index;

        public NamedReactionCollectionPropertyDescriptor(NamedReactionCollection coll, int idx) :
            base("#" + idx.ToString(), null)
        {
            this.collection = coll;
            this.index = idx;
        }

        public override System.ComponentModel.AttributeCollection Attributes
        {
            get
            {
                return new System.ComponentModel.AttributeCollection(null);
            }
        }

        public override bool CanResetValue(Object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return this.collection.GetType();
            }
        }

        public override String DisplayName
        {
            get
            {
                return ((NamedReaction)this.collection[index]).Name;
            }
        }

        public override String Description
        {
            get
            {
                return ((NamedReaction)this.collection[index]).Name;
            }
        }

        public override Object GetValue(Object component)
        {
            return this.collection[index];
        }

        public override bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public override String Name
        {
            get
            {
                return String.Concat("#", index.ToString());
            }
        }

        public override Type PropertyType
        {
            get
            {
                return this.collection[index].GetType();
            }
        }

        public override void ResetValue(Object component)
        {

        }

        public override bool ShouldSerializeValue(Object component)
        {
            return true;
        }

        public override void SetValue(Object component, Object value)
        {
            //this.collection[index] = value;
        }
    };
}
