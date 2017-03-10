using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public class CLineGraphic : CShapeGraphic
    {
        //Constructors

        public CLineGraphic() : base() { }
        public CLineGraphic(System.Drawing.Point startPosition) : base(startPosition)
        {
            this.SetStartPosition(startPosition);
        }

        public CLineGraphic(int posX, int posY)
        {
            this.SetStartPosition(new System.Drawing.Point(posX, posY));
        }


        public CLineGraphic(System.Drawing.Point startPosition, System.Drawing.Point endPosition)
        {
            this.SetStartPosition(startPosition);
            SetEndPosition(endPosition);
            this.m_AutoSize = false;
        }


        public CLineGraphic(int startX, int startY, System.Drawing.Point endPosition)
        {
            this.SetStartPosition(new System.Drawing.Point(startX, startY));
            this.SetEndPosition(endPosition);
        }


        public CLineGraphic(int startX, int startY, int endX, int endY)
        {
            this.SetStartPosition(new System.Drawing.Point(startX, startY));
            this.SetEndPosition(new System.Drawing.Point(endX, endY));
            this.m_AutoSize = false;
        }


        public CLineGraphic(System.Drawing.Point startPosition, System.Drawing.Point endPosition, double lineWidth, System.Drawing.Color lineColor)
        {
            this.SetStartPosition(startPosition);
            this.SetEndPosition(endPosition);
            this.m_AutoSize = false;
            this.m_lineWidth = lineWidth;
            this.m_lineColor = lineColor;
        }


        public CLineGraphic(int startX, int startY, int endX, int endY, float lineWidth, System.Drawing.Color lineColor)
        {
            this.SetStartPosition(new System.Drawing.Point(startX, startY));
            this.SetEndPosition(new System.Drawing.Point(endX, endY));
            this.m_AutoSize = false;
            this.m_lineWidth = lineWidth;
            this.m_lineColor = lineColor;
        }

        public override bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Pen myPen = new System.Drawing.Pen(this.m_lineColor, (float)this.m_lineWidth + 2);
            float X = (float)this.X;
            float Y = (float)this.Y;
            gp.AddLine(X, Y, X + m_Size.Width, Y + m_Size.Height);
            myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
            gp.Transform(myMatrix);
            return gp.IsOutlineVisible(pt, myPen);
        }

        System.Drawing.Point GetStartPosition()
        {
            return ((GraphicObject)this).GetPosition();
        }

        void SetStartPosition(System.Drawing.Point Value)
        {
            this.SetPosition(Value);
        }

        System.Drawing.Point GetEndPosition()
        {
            System.Drawing.Point endPosition = new System.Drawing.Point(this.m_Position.X, this.m_Position.Y);
            endPosition.X += this.m_Size.Width;
            endPosition.Y += this.m_Size.Height;
            return endPosition;
        }

        void SetEndPosition(System.Drawing.Point Value)
        {
            this.Width = Value.X - m_Position.X;
            this.Height = Value.Y - m_Position.Y;
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
            float X = (float)this.X;
            float Y = (float)this.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(m_Rotation), new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);

            g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
            g.EndContainer(gContainer);
        }

        void Draw(System.Drawing.Graphics g, System.Drawing.Drawing2D.AdjustableArrowCap customStartCap, System.Drawing.Drawing2D.AdjustableArrowCap customEndCap)
        {
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;

            float X = (float)this.X;
            float Y = (float)this.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)m_Rotation, new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);

            // put startcaps and endcaps on lines
            if (customStartCap != null) myPen.CustomStartCap = customStartCap;
            if (customEndCap != null) myPen.CustomEndCap = customEndCap;

            g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
            g.EndContainer(gContainer);
        }
    }
}
