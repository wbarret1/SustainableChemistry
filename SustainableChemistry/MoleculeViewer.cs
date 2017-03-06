using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SustainableChemistry
{


    public partial class MoleculeViewer : UserControl
    {
        // Member variables...
        //Properties...
        //      bool m_AddingStream;
        //      bool m_DrawingLine;
        //      //GraphicObjectCollection^ m_DrawingObjects;// As New GraphicObjectCollection()
        //Color m_GridLineColor;// As Color = Color.LightBlue
        //      System.Drawing.Drawing2D.DashStyle m_GridLineDashStyle;
        //      int m_GridLineWidth;// As Integer = 1
        //      double m_GridSize;// A Single = 50
        //      Color m_MarginColor;// AsColor = Color.Green
        //      System.Drawing.Drawing2D.DashStyle m_MarginLineDashStyle;
        //      int m_MarginLineWidth;// As Integer = 1
        //      double m_MinimumGridSize;
        //      bool m_Modified;// Modified is set to false indictaing no changes are made to a newly opened flowsheet
        //      Color m_NonPrintingAreaColor;// As Color = Color.Gray
        //      bool m_PrintGrid;// As Boolean = True
        //      bool m_PrintMargins;// As Boolean = True
        //      System.Drawing.Rectangle m_SelectionRectangle;// AsNew Rectangle(0, 0, 850, 1100)
        //      bool m_SelectionDragging;// As Boolean = False
        //      bool m_ShowGrid;// As Boolean = True
        //      bool m_ShowMargins;// As Boolean = True
        //System.Drawing.Rectangle m_SurfaceBounds;// AsNew Rectangle(0, 0, 850, 1100)
        //      System.Drawing.Rectangle m_SurfaceMargins;

        int m_HorizRes;// As Integer = 300
        int m_VertRes;// As Integer = 300
        double m_Zoom;// As Single = 0.5


        //      bool m_DraggingSelectedObject;// As Boolean = False
        //      bool m_RotatingSelectedObject;// As Boolean = False
        //      double startingRotation;// As Single = 0
        //      double originalRotation;// As Single = 0
        //      List<Point> m_Points;
        //int sel_corner;
        //      bool resizing;
        //      Point dragOffset;
        //      SizeDirection sz_direct;
        //      int sel_line;


        GraphicObjectCollection m_DrawingObjects;// As New GraphicObjectCollection()
        public MoleculeViewer()
        {
            InitializeComponent();
            this.m_DrawingObjects = new GraphicObjectCollection();
            m_Zoom = 0.5;

        }



        public ChemInfo.Molecule Molecule
        {
            set
            {
                this.m_DrawingObjects.Clear();
                ChemInfo.Molecule m = value;
                foreach (ChemInfo.Atom a in m.GetAtoms())
                {
                    m_DrawingObjects.Add(new CTextGraphics(a.X_2D, a.Y_2D, a.AtomicSymbol, this.Font, a.Color));
                    foreach (ChemInfo.Bond bond in a.BondedAtoms)
                        m_DrawingObjects.Add(new CLineGraphic(a.X_2D, a.Y_2D, bond.ConnectedAtom.X_2D, bond.ConnectedAtom.Y_2D, 1, Color.Black));
                }
            }
        }

        public double Zoom
        {
            get
            {
                return m_Zoom;
            }
            set
            {
                if (value > 0.05)
                    m_Zoom = value;
                else m_Zoom = 0.05;
                /*OnStatusUpdate(this, gcnew StatusUpdateEventArgs(StatusUpdateType::SurfaceZoomChanged, 
                    this->SelectedObjects, String::Format("Zoom set to {0}", (this->m_Zoom * 100)), 
                    Point (0,0), this->m_Zoom));*/
                this.Invalidate();
            }
        }


        //public delegate void PaintEventHandler(object sender, PaintEventArgs e);
        private void MoleculeViewer_Paint(object sender, PaintEventArgs e)
        {
            //sender;
            //	try{

            System.Drawing.Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            ////get the dpi settings of the graphics context,
            ////for example; 96dpi on screen, 600dpi for the printer
            ////used to adjust grid and margin sizing.
            //this->m_HorizRes = static_cast<int>(g->DpiX);
            //this->m_VertRes = static_cast<int>(g->DpiY);

            //handle the possibility that the viewport is scrolled,
            //adjust my origin coordintates to compensate
            Point pt = this.AutoScrollPosition;
            g.TranslateTransform((float)(pt.X), (float)(pt.Y));


            ////Draw dashed line margin indicators, over top of objects
            //if (m_ShowGrid)
            //    DrawGrid(g);
            //if (m_ShowMargins)
            //    DrawMargins(g);
            ////draw the actual objects onto the page, on top of the grid
            ////pass the graphics resolution onto the objects
            ////so that images and other objects can be sized
            ////correct taking the dpi into consideration.
            this.m_DrawingObjects.HorizontalResolution = (int)(g.DpiX);
            this.m_DrawingObjects.VerticalResolution = (int)(g.DpiY);
            this.m_DrawingObjects.DrawObjects(g, this.Zoom);
            ////doesn't really draw the selected object, but instead the
            ////selection indicator, a dotted outline around the selected object
            ////this->m_DrawingObjects->DrawSelectedObject(g, this->SelectedObject, this->m_Zoom);


            ////draw selection rectangle (click and drag to select interface)
            ////on top of everything else, but transparent
            //if (SelectionDragging)
            //{
            //    DrawSelectionRectangle(g);
            //}

            ////if user is currently inserting a stream,draw line
            //if (m_DrawingLine == true && m_Points->Count > 0)
            //{
            //    //Pen^ pen = gcnew Pen((gcnew SolidBrush(Color::Red),3);
            //    Pen ^ pen = gcnew Pen(Color::Red, 3);

            //    //see if user is manually inserting stream lines
            //    //if so, draw them
            //    //if(stream.lines->Count>=1)
            //    //{
            //    //	for(int n=0;n<stream.lines->Count;n++)
            //    //	{
            //    //		CLineGraphic^ ln = dynamic_cast<CLineGraphic^>(stream.lines->get_Item(n));
            //    //		ln->Draw(g);
            //    //	}

            //    //}
            //    array<Point> ^ ptTemp = gcnew array<Point>(m_Points->Count);
            //    for (int i = 0; i < m_Points->Count; i++)
            //    {
            //        ptTemp[i] = static_cast<Point>(m_Points[i]);
            //    }
            //    g->DrawLines(pen, ptTemp);
            //    delete ptTemp;


            //}
        }
    }
}
