using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class ReferenceAddedEventArgs : EventArgs
    {
        private Reference[] reference;

        public ReferenceAddedEventArgs(Reference[] reference)
        {
            this.reference = reference;
        }

        public Reference[] AddedReferences { get; }
    }

    [Serializable]
    public class References
    {
        List<Reference> m_References;
        public event EventHandler ReferenceAdded;

        protected virtual void OnReferenceAdded(EventArgs e)
        {
            ReferenceAdded?.Invoke(this, e);
        }

        public References()
        {
            m_References = new List<Reference>();
        }

        public void Clear()
        {
            m_References.Clear();
        }

        public Reference this[int i]
        {
            get { return m_References[i]; }
            set { m_References[i] = value; }
        }

        public Reference[] Items
        {
            get
            {
                return m_References.ToArray();
            }
            set
            {
                this.m_References.AddRange(Items);
            }
        }

        public void AddReference(Reference newRef)
        {
            this.m_References.Add(newRef);
            OnReferenceAdded(new ReferenceAddedEventArgs(new Reference[]{ newRef}));
        }

        public void AddReferences(Reference[] newRef)
        {
            this.m_References.AddRange(newRef);
            OnReferenceAdded(new ReferenceAddedEventArgs(newRef));
        }

        public Reference[] GetReferences(string functionalGroup)
        {
            var results = from reference in m_References where reference.FunctionalGroup == functionalGroup select reference;

            List<Reference> retVal = new List<Reference>();
            foreach (Reference r in results)
            {
                retVal.Add(r);
            }

            return retVal.ToArray<Reference>();
        }


    }
}
