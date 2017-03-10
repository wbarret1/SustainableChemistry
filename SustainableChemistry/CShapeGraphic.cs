using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public abstract class CShapeGraphic : GraphicObject
    {
        protected double m_lineWidth = 1;
        protected System.Drawing.Color m_lineColor = System.Drawing.Color.Black;
        protected System.Drawing.Color m_fillColor = System.Drawing.Color.White;
        protected bool m_fill = false;

        public CShapeGraphic() : base() { }
        public CShapeGraphic(System.Drawing.Point graphicPosition) : base(graphicPosition) { }
        public CShapeGraphic(int posX, int posY) : base(posX, posY) { }
        public CShapeGraphic(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize) : base(graphicPosition, graphicSize) { }
        public CShapeGraphic(int posX, int posY, System.Drawing.Size graphicSize) : base(posX, posY, graphicSize) { }
        public CShapeGraphic(int posX, int posY, int width, int height) : base(posX, posY, width, height) { }
        public CShapeGraphic(System.Drawing.Point graphicPosition, double Rotation) : base(graphicPosition, Rotation) { }
        public CShapeGraphic(int posX, int posY, double Rotation) : base(posX, posY, Rotation) { }
        public CShapeGraphic(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize, double Rotation) : base(graphicPosition, graphicSize, Rotation) { }
        public CShapeGraphic(int posX, int posY, System.Drawing.Size graphicSize, double Rotation) : base(posX, posY, graphicSize, Rotation) { }
        public CShapeGraphic(int posX, int posY, int width, int height, double Rotation) : base(posX, posY, width, height, Rotation) { }

        public float LineWidth
        {
            get
            {
                return (float)m_lineWidth;
            }
            set
            {
                m_lineWidth = value;
            }
        }

        public System.Drawing.Color LineColor
        {

            get
            {
                return m_lineColor;
            }
            set
            {
                m_lineColor = value;
            }
        }

        public bool Fill
        {
            get
            {
                return m_fill;
            }
            set
            {
                m_fill = value;
            }
        }

        System.Drawing.Color FillColor
        {
            get
            {
                return m_fillColor;
            }
            set
            {
                m_fillColor = value;
            }
        }
    }
}
