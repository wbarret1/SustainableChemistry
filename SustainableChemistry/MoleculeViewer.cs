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
    public delegate void SelectionChanged(Object sender, SelectionChangedEventArgs args);
    public delegate void StatusUpdate(Object sender, StatusUpdateEventArgs args);
    public delegate void GraphicObjectsChanged(Object sender, GraphicObjectsChangedEventsArgs args);

    public class SelectionChangedEventArgs : System.EventArgs
    {

        GraphicObject m_SelectedObject;
        GraphicObject[] m_SelectedObjects;

        public SelectionChangedEventArgs(GraphicObject selectedObject)
        {
            m_SelectedObject = selectedObject;
            m_SelectedObjects = new GraphicObject[0];
        }


        public SelectionChangedEventArgs(GraphicObject[] selectedObjects)
        {
            m_SelectedObject = null;
            if (selectedObjects.Length > 0) m_SelectedObject = selectedObjects[0];
            m_SelectedObjects = selectedObjects;
        }

        public Object SelectedObject
        {
            get
            {
                return m_SelectedObject;
            }
        }

        public Object SelectedObjects
        {
            get
            {
                return m_SelectedObjects;
            }
        }
    };

    public enum StatusUpdateType
    {
        ObjectRotated = 0,
        ObjectMoved = 1,
        ObjectDeleted = 2,
        SurfaceZoomChanged = 3,
        FileLoaded = 4,
        FileSaved = 5,
        SelectionChanged = 6
    };

    public class StatusUpdateEventArgs : System.EventArgs
    {
        StatusUpdateType m_UpdateType;
        GraphicObject m_SelectedObject;
        GraphicObject[] m_SelectedObjects;
        String m_Message;
        Point m_Coord;
        double m_Amount;

        public StatusUpdateEventArgs(StatusUpdateType UpdateType, GraphicObject Selection, String StatusMessage, Point Coord, double Amt)
        {
            m_UpdateType = UpdateType;
            m_SelectedObject = Selection;
            m_SelectedObjects = new GraphicObject[1];
            m_SelectedObjects[0] = Selection;
            m_Message = StatusMessage;
            m_Coord = Coord;
            m_Amount = Amt;
        }


        public StatusUpdateEventArgs(StatusUpdateType UpdateType, GraphicObject[] Selection, String StatusMessage, Point Coord, double Amt)
        {
            m_UpdateType = UpdateType;
            m_SelectedObject = null;
            if (Selection.Length > 0) m_SelectedObject = Selection[0];
            m_SelectedObjects = Selection;
            m_Message = StatusMessage;
            m_Coord = Coord;
            m_Amount = Amt;
        }

        public StatusUpdateType Type
        {
            get
            {
                return m_UpdateType;
            }
        }

        public GraphicObject SelectedObject
        {
            get
            {
                return m_SelectedObject;
            }
        }

        public GraphicObject[] SelectedObjects
        {
            get
            {
                return m_SelectedObjects;
            }
        }

        public String Message
        {
            get
            {
                return m_Message;
            }
        }
        public System.Drawing.Point Coordinates
        {
            get
            {
                return m_Coord;
            }
        }

        public double Amount
        {
            get
            {
                return m_Amount;
            }
        }
    };

    public enum GraphicObjectsChangedType
    {
        Edited = 0,
        Added = 1,
        Cut = 2,
        Copied = 3,
        Pasted = 4,
        Deleted = 5,
        DeleteAll = 6
    };


    public class GraphicObjectsChangedEventsArgs : System.EventArgs
    {
        Object m_ChangedObject;
        GraphicObjectsChangedType m_ChangeType;

        public GraphicObjectsChangedEventsArgs(Object newObject, GraphicObjectsChangedType type)
        {
            m_ChangedObject = newObject;
            m_ChangeType = type;
        }

        public Object ChangedObject
        {
            get
            {
                return m_ChangedObject;
            }
        }

        public GraphicObjectsChangedType ChangeType
        {
            get
            {
                return m_ChangeType;
            }
        }
    };

    public partial class MoleculeViewer : UserControl
    {
        // Member variables...
        //Properties...
        //      bool m_AddingStream;
        //      bool m_DrawingLine;
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
        System.Drawing.Rectangle m_SelectionRectangle = new System.Drawing.Rectangle(0, 0, 850, 1100);
        bool m_SelectionDragging = false;// As Boolean = False
        //      bool m_ShowGrid;// As Boolean = True
        //      bool m_ShowMargins;// As Boolean = True
        //System.Drawing.Rectangle m_SurfaceBounds;// AsNew Rectangle(0, 0, 850, 1100)
        //      System.Drawing.Rectangle m_SurfaceMargins;

        int m_HorizRes;// As Integer = 300
        int m_VertRes;// As Integer = 300
        double m_Zoom;// As Single = 0.5
        System.Drawing.Point m_Location;


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
            m_Zoom = 1.0;
        }

        //Public Events
        event SelectionChanged OnSelectionChanged;
        event StatusUpdate OnStatusUpdate;
        event GraphicObjectsChanged OnGraphicObjectsChanged;

        public ChemInfo.Molecule Molecule
        {
            set
            {
                this.m_DrawingObjects.Clear();
                ChemInfo.Molecule m = value;
                foreach (ChemInfo.Atom a in m.GetAtoms())
                {
                    m_DrawingObjects.Add(new CTextGraphics(a.X_2D - m.Location.X, a.Y_2D - m.Location.Y, a.AtomicSymbol, this.Font, a.Color));
                    foreach (ChemInfo.Bond bond in a.BondedAtoms)
                        m_DrawingObjects.Add(new CLineGraphic(a.X_2D - m.Location.X, a.Y_2D - m.Location.Y, bond.ConnectedAtom.X_2D - m.Location.X, bond.ConnectedAtom.Y_2D - m.Location.Y, 1, Color.Black));
                }
                m_Location = m.Location;
                m_Zoom = this.Size.Height / m.Size.Height;
                double test = this.Size.Width / m.Size.Width;
                if (test > m_Zoom) m_Zoom = test;
                m_Zoom = 0.75;

            }
        }

        public GraphicObject SelectedObject
        {
            get
            {
                for (int i = 0; i < m_DrawingObjects.Count; i++)
                {
                    if (((GraphicObject)(m_DrawingObjects[i])).Selected) return (GraphicObject)m_DrawingObjects[i];
                }
                return null;
            }

            set
            {
                for (int i = 0; i < m_DrawingObjects.Count; i++)
                {
                    ((GraphicObject)m_DrawingObjects[i]).Selected = false;
                    if ((GraphicObject)m_DrawingObjects[i] == value) ((GraphicObject)m_DrawingObjects[i]).Selected = true;
                }
                if (value == null)
                {
                    GraphicObject gObj = null;
                    OnStatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.SelectionChanged, gObj,
                        "No Object Selected", new System.Drawing.Point(0, 0), 0.0));
                    OnSelectionChanged(this, new SelectionChangedEventArgs(gObj));
                    return;
                }
                OnSelectionChanged(this, new SelectionChangedEventArgs(value));
                OnStatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.SelectionChanged, value,
                    "Selected Object Changed", value.GetPosition(), 0.0));
            }
        }

        public GraphicObject[] SelectedObjects
        {
            get
            {
                if (m_DrawingObjects.Count == 0) return new GraphicObject[0];
                int selectedCount = 0;
                for (int i = 0; i < m_DrawingObjects.Count; i++)
                {
                    if (((GraphicObject)m_DrawingObjects[i]).Selected) selectedCount++;
                }
                GraphicObject[] retVal = new GraphicObject[selectedCount];
                selectedCount = 0;
                for (int j = 0; j < m_DrawingObjects.Count; j++)
                {
                    if (((GraphicObject)m_DrawingObjects[j]).Selected)
                    {
                        retVal[selectedCount] = (GraphicObject)m_DrawingObjects[j];
                        selectedCount++;
                    }
                }
                return retVal;
            }
            set
            {
                for (int i = 0; i < m_DrawingObjects.Count; i++)
                {
                    ((GraphicObject)m_DrawingObjects[i]).Selected = false;
                    for (int j = 0; j < value.Length; i++)
                    {
                        if ((GraphicObject)m_DrawingObjects[i] == value[j])
                            ((GraphicObject)m_DrawingObjects[i]).Selected = true;
                    }
                }
                OnSelectionChanged(this, new SelectionChangedEventArgs(this.SelectedObjects));
                Point location = new System.Drawing.Point(0, 0);
                if (this.SelectedObjects.Length > 0)
                {
                    location = value[0].GetPosition();
                }
                OnStatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.SelectionChanged, value,
                    "Selected Object Changed", location, 0.0));
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

        private void MoleculeViewer_Paint(object sender, PaintEventArgs e)
        {
            //sender;
            //	try{

            System.Drawing.Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //get the dpi settings of the graphics context,
            //for example; 96dpi on screen, 600dpi for the printer
            //used to adjust grid and margin sizing.
            this.m_HorizRes = (int)(g.DpiX);
            this.m_VertRes = (int)(g.DpiY);

            //handle the possibility that the viewport is scrolled,
            //adjust my origin coordintates to compensate
            Point pt = this.AutoScrollPosition;
            g.TranslateTransform((float)(pt.X), (float)(pt.Y));


            ////Draw dashed line margin indicators, over top of objects
            //if (m_ShowGrid)
            //    DrawGrid(g);
            //if (m_ShowMargins)
            //    DrawMargins(g);
            //draw the actual objects onto the page, on top of the grid
            //pass the graphics resolution onto the objects
            //so that images and other objects can be sized
            //correct taking the dpi into consideration.
            this.m_DrawingObjects.HorizontalResolution = (int)(g.DpiX);
            this.m_DrawingObjects.VerticalResolution = (int)(g.DpiY);
            this.m_DrawingObjects.DrawObjects(g, m_Zoom);
            //doesn't really draw the selected object, but instead the
            //selection indicator, a dotted outline around the selected object
            this.m_DrawingObjects.DrawSelectedObject(g, this.SelectedObject, this.m_Zoom);

            //draw selection rectangle (click and drag to select interface)
            //on top of everything else, but transparent
            if (m_SelectionDragging)
            {
                DrawSelectionRectangle(g);
            }
        }

        void DrawSelectionRectangle(Graphics g)
        {
            SolidBrush selectionBrush = new SolidBrush(Color.FromArgb(75, Color.Gray));
            System.Drawing.Rectangle normalizedRectangle = new System.Drawing.Rectangle();

            //make sure the rectangle's upper left point is
            //up and to the left relative to the other points of the rectangle by
            //ensuring that it has a positive width and height.
            normalizedRectangle.Size = m_SelectionRectangle.Size;
            if (m_SelectionRectangle.Width < 0)
            {
                normalizedRectangle.X = m_SelectionRectangle.X - normalizedRectangle.Width;
            }
            else
            {
                normalizedRectangle.X = m_SelectionRectangle.X;
            }

            if (m_SelectionRectangle.Height < 0)
            {
                normalizedRectangle.Y = m_SelectionRectangle.Y - normalizedRectangle.Height;
            }
            else
            {
                normalizedRectangle.Y = m_SelectionRectangle.Y;
            }

            g.FillRectangle(selectionBrush, normalizedRectangle);
        }
    }
}
