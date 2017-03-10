using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public enum SizeDirection
    {
        Northwest,
        North,
        Northeast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        NA
    };

    public abstract class GraphicObject
    {
        protected System.Drawing.Point m_Position = new System.Drawing.Point(0, 0);
        protected System.Drawing.Size m_Size = new System.Drawing.Size(0, 0);
        protected double m_Rotation = 0;
        protected bool m_AutoSize = false;
        //GraphicObjectCollection m_Container;
        protected bool m_Selected = false;

        //Constructors
        public GraphicObject()
        {
        }

        public GraphicObject(System.Drawing.Point graphicPosition)
        {
            this.SetPosition(graphicPosition);
        }

        public GraphicObject(int posX, int posY)
        {
            this.SetPosition(new System.Drawing.Point(posX, posY));
        }

        public GraphicObject(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize)
        {
            this.SetPosition(graphicPosition);
            this.SetSize(graphicSize);
        }

        public GraphicObject(int posX, int posY, System.Drawing.Size graphicSize)
        {
            this.SetPosition(new System.Drawing.Point(posX, posY));
            this.SetSize(graphicSize);
        }

        public GraphicObject(int posX, int posY, int width, int height)
        {
            this.SetPosition(new System.Drawing.Point(posX, posY));
            this.SetSize(new System.Drawing.Size(width, height));
        }

        public GraphicObject(System.Drawing.Point graphicPosition, double Rotation)
        {
            this.SetPosition(graphicPosition);
            m_Rotation = Rotation;
        }

        public GraphicObject(int posX, int posY, double Rotation)
        {
            this.SetPosition(new System.Drawing.Point(posX, posY));
            m_Rotation = Rotation;
        }

        public GraphicObject(System.Drawing.Point graphicPosition, System.Drawing.Size graphicSize, double Rotation)
        {
            this.SetPosition(graphicPosition);
            this.SetSize(graphicSize);
            m_Rotation = Rotation;
        }

        public GraphicObject(int posX, int posY, System.Drawing.Size graphicSize, double Rotation)
        {
            this.SetPosition(new System.Drawing.Point(posX, posY));
            this.SetSize(graphicSize);
            m_Rotation = Rotation;
        }

        public GraphicObject(int posX, int posY, int width, int height, double Rotation)
        {
            this.SetPosition(new System.Drawing.Point(posX, posY));
            this.SetSize(new System.Drawing.Size(width, height));
            m_Rotation = Rotation;
        }

        // Properties 
        public object Tag { get; set; } = null;

        public virtual bool AutoSize
        {
            get
            {
                return m_AutoSize;
            }

            set
            {
                m_AutoSize = value;
            }
        }

        public virtual bool Selected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                m_Selected = value;
            }
        }

        public virtual int X
        {
            get

            {
                return m_Position.X;
            }

            set
            {
                m_Position.X = value;
            }
        }

        public virtual int Y
        {
            get
            {
                return m_Position.Y;
            }

            set
            {
                m_Position.Y = value;
            }
        }

        public virtual int Height
        {
            get
            {
                return m_Size.Height;
            }
            set
            {
                m_Size.Height = value;
            }
        }

        public virtual int Width
        {
            get
            {
                return m_Size.Width;
            }
            set
            {
                m_Size.Width = value;
            }
        }

        public virtual double Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                if (System.Math.Abs(value) < 360)
                {
                    m_Rotation = value;
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException("Rotation must be between -360.0 and 360.0");
                }
            }
        }

        //Draw method. Abstract virtual function that must be implemented by the graphic object class.
        public abstract void Draw(System.Drawing.Graphics g);


        // Graphics methods
        //chuck's new code 2/20/04
        //this method indicates whether point is along outline of graphic
        //and if so, what type of cursor should show
        public virtual void BoundaryTest(System.Drawing.Point pt, SizeDirection dir)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black, 3);

            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X - 3, this.m_Position.Y - 3, this.m_Size.Width + 6, this.m_Size.Height + 6));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF((float)this.X, (float)this.Y), System.Drawing.Drawing2D.MatrixOrder.Append);
            }

            gp.Transform(myMatrix);
            dir = SizeDirection.NA;
            if (gp.IsOutlineVisible(pt, pen))
            {
                //user has placed the mouse along the outline of the selected
                //object - change the mouse to allow for resizing
                System.Drawing.RectangleF rect = gp.GetBounds();
                if (Math.Abs((int)rect.Left - pt.X) <= 2)
                {
                    if (Math.Abs((int)rect.Top - pt.Y) <= 2)
                        dir = SizeDirection.Northwest;
                    else if (Math.Abs((int)rect.Bottom - pt.Y) <= 2)
                        dir = SizeDirection.Southwest;
                    else
                        dir = SizeDirection.West;
                }
                else if (Math.Abs((int)rect.Right - pt.X) <= 2)
                {
                    if (Math.Abs((int)rect.Top - pt.Y) <= 2)
                        dir = SizeDirection.Northeast;
                    else if (Math.Abs((int)rect.Bottom - pt.Y) <= 2)
                        dir = SizeDirection.Southeast;
                    else
                        dir = SizeDirection.East;
                }
                else if (Math.Abs((int)rect.Top - pt.Y) <= 2)
                    dir = SizeDirection.North;
                else
                    dir = SizeDirection.South;
            }
        }
        //end revised code
        public virtual bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X, this.m_Position.Y, this.m_Size.Width, this.m_Size.Height));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(this.m_Rotation), new System.Drawing.PointF((float)this.X, (float)this.Y),
                    System.Drawing.Drawing2D.MatrixOrder.Append);
            }
            gp.Transform(myMatrix);
            return gp.IsVisible(pt);
        }

        public virtual bool HitTest(System.Drawing.Rectangle rect)
        {//is this object contained within the supplied rectangle
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            gp.AddRectangle(new System.Drawing.Rectangle(this.m_Position.X, this.m_Position.Y, this.m_Size.Width, this.m_Size.Height));
            if (this.m_Rotation != 0)
            {
                myMatrix.RotateAt((float)this.m_Rotation, new System.Drawing.PointF((float)this.m_Position.X, (float)this.m_Position.Y),
                    System.Drawing.Drawing2D.MatrixOrder.Append);
            }
            gp.Transform(myMatrix);
            System.Drawing.Rectangle gpRect = System.Drawing.Rectangle.Round(gp.GetBounds());
            return rect.Contains(gpRect);
        }

        public virtual System.Drawing.Point GetPosition()
        {
            System.Drawing.Point myPosition = new System.Drawing.Point(m_Position.X, m_Position.Y);
            return myPosition;
        }

        public virtual void SetPosition(System.Drawing.Point Value)
        {
            //'any value is currently ok,
            //'but I might want to add validation later.
            m_Position = Value;
        }

        public virtual void SetSize(System.Drawing.Size Value)
        {
            m_Size = Value;
        }

        public virtual System.Drawing.Size GetSize()
        {
            System.Drawing.Size mySize = new System.Drawing.Size(m_Size.Width, m_Size.Height);
            return mySize;
        }
    }


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
            System.Drawing.Pen myPen = new System.Drawing.Pen(this.m_lineColor, (float)this.m_lineWidth+2);
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
    };

    public class GraphicBond : GraphicObjectCollection
    {

        public GraphicBond() : base() { }

        public GraphicBond(int startX, int startY, int endX, int endY, float lineWidth, System.Drawing.Color lineColor, ChemInfo.Bond bond)
        {
            //this.SetStartPosition(new System.Drawing.Point(startX, startY));
            //this.SetEndPosition(new System.Drawing.Point(endX, endY));
            this.Add(new CLineGraphic(startX, startY, endX, endY, 1, System.Drawing.Color.Black));
            //this.m_AutoSize = false;
        }

        //System.Drawing.Point GetStartPosition()
        //{
        //    return ((GraphicObject)this).GetPosition();
        //}

        //void SetStartPosition(System.Drawing.Point Value)
        //{
        //    this.SetPosition(Value);
        //}

        //System.Drawing.Point GetEndPosition()
        //{
        //    System.Drawing.Point endPosition = new System.Drawing.Point(this.m_Position.X, this.m_Position.Y);
        //    endPosition.X += this.m_Size.Width;
        //    endPosition.Y += this.m_Size.Height;
        //    return endPosition;
        //}

        //void SetEndPosition(System.Drawing.Point Value)
        //{
        //    this.Width = Value.X - m_Position.X;
        //    this.Height = Value.Y - m_Position.Y;
        //}

        //public override void Draw(System.Drawing.Graphics g)
        //{
        //    System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
        //    System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
        //    float X = (float)this.X;
        //    float Y = (float)this.Y;
        //    if (m_Rotation != 0)
        //    {
        //        myMatrix.RotateAt((float)(m_Rotation), new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
        //        g.Transform = myMatrix;
        //    }
        //    System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);

        //    g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
        //    g.EndContainer(gContainer);
        //}

        //void Draw(System.Drawing.Graphics g, System.Drawing.Drawing2D.AdjustableArrowCap customStartCap, System.Drawing.Drawing2D.AdjustableArrowCap customEndCap)
        //{
        //    System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
        //    System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;

        //    float X = (float)this.X;
        //    float Y = (float)this.Y;
        //    if (m_Rotation != 0)
        //    {
        //        myMatrix.RotateAt((float)m_Rotation, new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
        //        g.Transform = myMatrix;
        //    }
        //    System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);

        //    // put startcaps and endcaps on lines
        //    if (customStartCap != null) myPen.CustomStartCap = customStartCap;
        //    if (customEndCap != null) myPen.CustomEndCap = customEndCap;

        //    g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
        //    g.EndContainer(gContainer);
        //}
    }


    public class CTextGraphics : GraphicObject
    {
        protected System.Drawing.Font m_Font = System.Drawing.SystemFonts.DefaultFont;
        protected String m_Text = string.Empty;
        protected System.Drawing.Color m_Color = System.Drawing.Color.Black;

        public CTextGraphics() : base() { }
        public CTextGraphics(System.Drawing.Point graphicPosition, String text, System.Drawing.Font textFont, System.Drawing.Color textColor) : base(graphicPosition)
        {
            SetPosition(graphicPosition);
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        public CTextGraphics(int posX, int posY, String text, System.Drawing.Font textFont, System.Drawing.Color textColor) : base(posX, posY)
        {
            SetPosition(new System.Drawing.Point(posX, posY));
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        public CTextGraphics(System.Drawing.Point graphicPosition, String text, System.Drawing.Font textFont, System.Drawing.Color textColor, double rotation) : base(graphicPosition, rotation)
        {
            SetPosition(graphicPosition);
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        CTextGraphics(int posX, int posY, String text, System.Drawing.Font textFont, System.Drawing.Color textColor, double rotation) : base(posX, posY, rotation)
        {
            SetPosition(new System.Drawing.Point(posX, posY));
            this.m_Text = text;
            this.m_Font = textFont;
            this.m_Color = textColor;
            m_AutoSize = true;
        }

        public System.Drawing.Font Font
        {
            get
            {
                return m_Font;
            }
            set
            {
                m_Font = value;
            }
        }

        public String Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return m_Color;
            }
            set
            {
                m_Color = value;
            }
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
            if (m_AutoSize)
            {
                System.Drawing.SizeF mySize = g.MeasureString(m_Text, m_Font);
                m_Size.Width = (int)mySize.Width;
                m_Size.Height = (int)mySize.Height;
                g.DrawString(m_Text, m_Font, new System.Drawing.SolidBrush(m_Color), X, Y);
            }
            else
            {
                System.Drawing.RectangleF rect = new System.Drawing.RectangleF(X, Y, (float)m_Size.Width, (float)m_Size.Height);
                g.DrawString(m_Text, m_Font, new System.Drawing.SolidBrush(m_Color), rect);
            }
            g.EndContainer(gContainer);
        }
    }
}
