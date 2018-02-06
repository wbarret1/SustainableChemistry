using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class FunctionalGroup
    {
        List<Reference> m_refList;
        System.Drawing.Image m_RxnImage;

        public FunctionalGroup(string func, string directory)
        {
            Name = func;
            m_refList = new List<Reference>();
            string[] imageFile = System.IO.Directory.GetFiles(directory, "*.jpg");
            if (imageFile.Length == 1)
                m_RxnImage = System.Drawing.Image.FromFile(imageFile[0]);
            string[] references = System.IO.Directory.GetFiles(directory, "*.ris");
            foreach (string file in references)
                m_refList.Add(new Reference(func, "", System.IO.File.ReadAllText(file)));                
        }

        public string Name { get; }

        public System.Drawing.Image ReactionImage
        {
            get
            {
                return m_RxnImage;
            }
        }

        public Reference[] References
        {
            get
            {
                return m_refList.ToArray<Reference>();
            }
        }
    }
}
