﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableChemistry
{
    public class GraphicBond :LineGraphic
    {

        public GraphicBond() : base() { }
        public GraphicBond(System.Drawing.Point startPosition, ChemInfo.BondType type) : base(startPosition)
        {
            this.SetStartPosition(startPosition);
            m_BondType = type;
        }

        public GraphicBond(int posX, int posY, ChemInfo.BondType type)
        {
            this.SetStartPosition(new System.Drawing.Point(posX, posY));
            m_BondType = type;
        }


        public GraphicBond(System.Drawing.Point startPosition, System.Drawing.Point endPosition, ChemInfo.BondType type)
        {
            this.SetStartPosition(startPosition);
            SetEndPosition(endPosition);
            this.m_AutoSize = false;
            m_BondType = type;
        }


        public GraphicBond(int startX, int startY, System.Drawing.Point endPosition, ChemInfo.BondType type)
        {
            this.SetStartPosition(new System.Drawing.Point(startX, startY));
            this.SetEndPosition(endPosition);
            m_BondType = type;
        }


        public GraphicBond(int startX, int startY, int endX, int endY, ChemInfo.BondType type)
        {
            this.SetStartPosition(new System.Drawing.Point(startX, startY));
            this.SetEndPosition(new System.Drawing.Point(endX, endY));
            this.m_AutoSize = false;
            m_BondType = type;
        }


        public GraphicBond(System.Drawing.Point startPosition, System.Drawing.Point endPosition, double lineWidth, System.Drawing.Color lineColor, ChemInfo.BondType type)
        {
            this.SetStartPosition(startPosition);
            this.SetEndPosition(endPosition);
            this.m_AutoSize = false;
            this.m_lineWidth = lineWidth;
            this.m_lineColor = lineColor;
            m_BondType = type;
        }


        public GraphicBond(int startX, int startY, int endX, int endY, float lineWidth, System.Drawing.Color lineColor, ChemInfo.BondType type)
        {
            this.SetStartPosition(new System.Drawing.Point(startX, startY));
            this.SetEndPosition(new System.Drawing.Point(endX, endY));
            this.m_AutoSize = false;
            this.m_lineWidth = lineWidth;
            this.m_lineColor = lineColor;
            m_BondType = type;
        }

        ChemInfo.BondType m_BondType = ChemInfo.BondType.Single;
        ChemInfo.BondType BondType
        {
            get
            {
                return m_BondType;
            }
            set
            {
                m_BondType = value;
            }
        }

        public override bool HitTest(System.Drawing.Point pt)
        {
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.Matrix myMatrix = new System.Drawing.Drawing2D.Matrix();
            System.Drawing.Pen myPen = new System.Drawing.Pen(this.m_lineColor, (float)this.m_lineWidth + 2);
            float X = (float)this.X;
            float Y = (float)this.Y;
            if (this.m_BondType == ChemInfo.BondType.Single) gp.AddLine(X, Y, X + m_Size.Width, Y + m_Size.Height);
            if (this.m_BondType == ChemInfo.BondType.Double)
            {
                gp.AddLine(X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                gp.AddLine(X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
            if (this.m_BondType == ChemInfo.BondType.Triple)
            {
                gp.AddLine(X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                gp.AddLine(X, Y, X + m_Size.Width, Y + m_Size.Height);
                gp.AddLine(X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
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

        float AngleToPoint(System.Drawing.Point Origin, System.Drawing.Point Target)
        {
            //'a cool little utility function, 
            //'given two points finds the angle between them....
            //'forced me to recall my highschool math, 
            //'but the task is made easier by a special overload to
            //'Atan that takes X,Y co-ordinates.
            float Angle;
            Target.X = Target.X - Origin.X;
            Target.Y = Target.Y - Origin.Y;
            Angle = (float)(Math.Atan2(Target.Y, Target.X) / (Math.PI / 180));
            return Angle;
        }

        float distance = 4;
        float m_OffsetX = 0;
        float m_OffsetY = 0;
        void DetermineOffset()
        {
            float factor = 1;
            if (this.m_BondType == ChemInfo.BondType.Triple) factor = (float)2.0;
            float angle = AngleToPoint(new System.Drawing.Point(X, Y), new System.Drawing.Point(X + m_Size.Width, Y + m_Size.Height));
            m_OffsetX = distance * factor* (float)Math.Sin((angle + 90) * (Math.PI / 180));
            m_OffsetY = distance * factor* (float)Math.Cos((angle + 90) * (Math.PI / 180));
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            System.Drawing.Drawing2D.GraphicsContainer gContainer = g.BeginContainer();
            System.Drawing.Drawing2D.Matrix myMatrix = g.Transform;
            this.DetermineOffset();
            float X = (float)this.X;
            float Y = (float)this.Y;
            if (m_Rotation != 0)
            {
                myMatrix.RotateAt((float)(m_Rotation), new System.Drawing.PointF(X, Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                g.Transform = myMatrix;
            }
            System.Drawing.Pen myPen = new System.Drawing.Pen(m_lineColor, (float)m_lineWidth);
            if (this.m_BondType == ChemInfo.BondType.Single || this.m_BondType == ChemInfo.BondType.Aromatic) g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
            if (this.m_BondType == ChemInfo.BondType.Double)
            {
                g.DrawLine(myPen, X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                g.DrawLine(myPen, X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
            if (this.m_BondType == ChemInfo.BondType.Triple)
            {
                g.DrawLine(myPen, X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
                g.DrawLine(myPen, X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
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

            if (this.m_BondType == ChemInfo.BondType.Single) g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
            if (this.m_BondType == ChemInfo.BondType.Double)
            {
                g.DrawLine(myPen, X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                g.DrawLine(myPen, X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
            if (this.m_BondType == ChemInfo.BondType.Triple)
            {
                g.DrawLine(myPen, X + m_OffsetX, Y + m_OffsetY, X + m_Size.Width + m_OffsetX, Y + m_Size.Height + m_OffsetY);
                g.DrawLine(myPen, X, Y, X + m_Size.Width, Y + m_Size.Height);
                g.DrawLine(myPen, X - m_OffsetX, Y - m_OffsetY, X + m_Size.Width - m_OffsetX, Y + m_Size.Height - m_OffsetY);
            }
            g.EndContainer(gContainer);
        }
    }
}