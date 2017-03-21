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
        [System.ComponentModel.Category("Layout")]
        System.Drawing.Rectangle m_SurfaceBounds = new System.Drawing.Rectangle(0, 0, 1100, 850);
        GraphicObjectCollection m_DrawingObjects = new GraphicObjectCollection();
        bool Modified { get; set; } = false;// Modified is set to false indictaing no changes are made to a newly opened flowsheet
        bool m_DrawingLine = false;

        // Selection Rectangle Properties...
        System.Drawing.Rectangle m_SelectionRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
        bool m_SelectionDragging = false;// As Boolean = False
        System.Drawing.Point dragOffset = new System.Drawing.Point(0, 0);


        //Grid Properties...
        [System.ComponentModel.Category("Appearance")]
        public bool ShowGrid { get; set; } = true;
        [System.ComponentModel.Category("Appearance")]
        public System.Drawing.Color GridLineColor { get; set; } = System.Drawing.Color.LightBlue;
        [System.ComponentModel.Category("Appearance")]
        public System.Drawing.Drawing2D.DashStyle GridLineDashStyle { get; set; } = System.Drawing.Drawing2D.DashStyle.Solid;
        [System.ComponentModel.Category("Appearance")]
        public int GridLineWidth { get; set; } = 1;
        [System.ComponentModel.Category("Appearance")]
        public double GridSize { get; set; } = 50;
        [System.ComponentModel.Category("Appearance")]
        public double MinimumGridSize { get; set; } = 1;

        //Margins Properties...
        [System.ComponentModel.Category("Appearance")]
        public bool ShowMargins { get; set; } = true;
        [System.ComponentModel.Category("Appearance")]
        public System.Drawing.Drawing2D.DashStyle MarginLineDashStyle { get; set; } = System.Drawing.Drawing2D.DashStyle.Solid;
        [System.ComponentModel.Category("Appearance")]
        public System.Drawing.Color MarginColor { get; set; } = System.Drawing.Color.Green;
        [System.ComponentModel.Category("Appearance")]
        public int MarginLineWidth { get; set; } = 1;
        [System.ComponentModel.Category("Layout")]
        public System.Drawing.Rectangle SurfaceMargins { get; set; } = new System.Drawing.Rectangle(100, 100, 900, 650);

        System.Drawing.Rectangle m_MoleculeRectangle = new System.Drawing.Rectangle(0, 0, 1100, 850);
        int m_HorizRes = 300;
        int m_VertRes = 300;
        double m_Zoom = 1.0;

        //Printing Properties...
        [System.ComponentModel.Category("Appearance")]
        public System.Drawing.Color NonPrintingAreaColor { get; set; } = System.Drawing.Color.Gray;
        [System.ComponentModel.Category("Behavior")]
        public bool PrintGrid { get; set; } = false;
        [System.ComponentModel.Category("Behavior")]
        public bool PrintMargins { get; set; } = false;

        System.Drawing.Point m_Location = new System.Drawing.Point(0, 0);


        bool m_DraggingSelectedObject = false;
        bool m_RotatingSelectedObject = false;
        double startingRotation = 0;
        double originalRotation = 0;
        List<Point> m_Points;
        int sel_corner;
        bool resizing = false;
        SizeDirection sz_direct;
        int sel_line;


        public MoleculeViewer()
        {
            InitializeComponent();
          //  m_MoleculeRectangle = this.ClientRectangle;
        }

        [System.ComponentModel.Category("Property Changed")]
        public delegate void SelectionChangedHandler(Object sender, SelectionChangedEventArgs args);
        public delegate void StatusUpdateHandler(Object sender, StatusUpdateEventArgs args);
        public delegate void GraphicObjectsChangedHandler(Object sender, GraphicObjectsChangedEventsArgs args);

        ////Public Events
        public event SelectionChangedHandler SelectionChanged;
        public event StatusUpdateHandler StatusUpdate;
        public event GraphicObjectsChangedHandler GraphicObjectsChanged;


        public ChemInfo.Molecule Molecule
        {
            set
            {
                m_DrawingObjects.Clear();
                this.SelectedObject = null;                
                ChemInfo.Molecule m = value;
                m.LocateAtoms2D();
                m_MoleculeRectangle = m.GetLocationBounds();

                // Select Offset to senter the molecule rectangle in the bounds.
                int deltaX = (this.Bounds.Width - m_MoleculeRectangle.Width) / 2;
                int deltaY = (this.Bounds.Height - m_MoleculeRectangle.Height) / 2;

                foreach (ChemInfo.Atom a in m.GetAtoms())
                {
                    System.Drawing.Point atomLocation = a.Location2D;
                    atomLocation.Offset(-1 * m.Location.X + deltaX, -1 * m.Location.Y + deltaY);
                    TextGraphics text = new TextGraphics(atomLocation, a.AtomicSymbol, this.Font, a.Color);
                    text.Tag = a;
                    m_DrawingObjects.Add(text);
                    foreach (ChemInfo.Bond b in a.BondedAtoms)
                    {
                        System.Drawing.Point bondedAtomLocation = b.ConnectedAtom.Location2D;
                        bondedAtomLocation.Offset(-1*m.Location.X + deltaX, -1*m.Location.Y+ deltaY);
                        //LineGraphic line = new LineGraphic(atomLocation, bondedAtomLocation, 1, Color.Black);
                        //line.Tag = bond;
                        //m_DrawingObjects.Add(line);
                        GraphicBond bond = new GraphicBond(atomLocation, bondedAtomLocation, 1.5, Color.Black, b.BondType);
                        bond.Tag = b;
                        m_DrawingObjects.Add(bond);
                    }
                }
                double zoom1 = this.GetFitWidthZoom();
                double zoom2 = this.GetFitWidthZoom();
                if (zoom1 > zoom2) m_Zoom = zoom2;
                else m_Zoom = zoom1;
            }
        }

        public GraphicObjectCollection DrawingObjects
        {
            get
            {
                return m_DrawingObjects;
            }
            set
            {
                m_DrawingObjects = value;
            }
        }

        int m_SelectedIndex = -1;
        public GraphicObject SelectedObject
        {
            get
            {
                if (m_SelectedIndex >= 0) return m_DrawingObjects[m_SelectedIndex];
                return null;
            }

            set
            {
                m_SelectedIndex = m_DrawingObjects.IndexOf(value);
                if (value == null) m_SelectedIndex = -1;
                if (SelectionChanged != null) SelectionChanged(this, new SelectionChangedEventArgs(value));
                if (StatusUpdate != null) StatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.SelectionChanged, value,
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
                if (SelectionChanged!=null) SelectionChanged(this, new SelectionChangedEventArgs(this.SelectedObjects));
                Point location = new System.Drawing.Point(0, 0);
                if (this.SelectedObjects.Length > 0)
                {
                    location = value[0].GetPosition();
                }
                if (StatusUpdate!=null) StatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.SelectionChanged, value,
                    "Selected Object Changed", location, 0.0));
            }
        }

        public System.Drawing.Rectangle SurfaceBounds { get; set; } = new System.Drawing.Rectangle(0, 0, 1100, 850);


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
                if (StatusUpdate!=null) StatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.SurfaceZoomChanged, 
                    this.SelectedObjects, String.Format("Zoom set to {0}%", (this.m_Zoom * 100)), 
                    new System.Drawing.Point (0,0), this.m_Zoom));
                this.Invalidate();
            }
        }

        // Public Methods
        double GetFitWidthZoom()
        {
            System.Drawing.Rectangle bounds = ConvertToPixels(this.Bounds);
            System.Drawing.Size display = this.Size;
            double fitWidth = (double)display.Width / (double)bounds.Width;
            int newHeight = (int)(fitWidth * bounds.Height);
            if (display.Height < newHeight)
            {
                this.AutoScrollMargin = new System.Drawing.Size(this.AutoScrollMargin.Height, 18);
                fitWidth = (double)(display.Width - 17) / (double)bounds.Width;
            }
            return fitWidth;
        }

        double GetFitHeightZoom()
        {
            System.Drawing.Rectangle bounds = ConvertToPixels(this.Bounds);
            System.Drawing.Size display = this.Size;
            double fitHeight = (double)display.Height / (double)bounds.Height;
            int newWidth = (int)(fitHeight * bounds.Width);
            if (display.Width < newWidth)
            {
                this.AutoScrollMargin = new System.Drawing.Size(18, this.AutoScrollMargin.Width);
                fitHeight = (double)(display.Height - 17) / (double)bounds.Height;
            }
            return fitHeight;
        }

        // Private Methods
        double ConvertToHPixels(double value)
        {
            return (value * m_HorizRes);
        }

        double ConvertToVPixels(double value)
        {
            return (value * m_VertRes);
        }

        System.Drawing.Rectangle ConvertDSToInches(System.Drawing.Rectangle rect)
        {
            return new System.Drawing.Rectangle((int)((double)rect.X / (double)m_HorizRes * 100),
                (int)(((double)rect.Y) / ((double)m_VertRes) * 100),
                (int)((double)(rect.Width) / ((double)(m_HorizRes) * 100)),
                (int)((double)(rect.Height) / ((double)(m_VertRes) * 100)));
        }

        System.Drawing.Rectangle ConvertToPixels(System.Drawing.Rectangle rect)
        {
            //'convert from 100ths of an inch to pixels
            return new System.Drawing.Rectangle((int)(ConvertToHPixels(rect.X)) / 100,
                (int)(ConvertToVPixels(rect.Y)) / 100,
                (int)(ConvertToHPixels(rect.Width)) / 100,
                (int)(ConvertToVPixels(rect.Height)) / 100);
        }

        System.Drawing.Rectangle ZoomRectangle(System.Drawing.Rectangle originalRect)
        {
            return new System.Drawing.Rectangle((int)(originalRect.X * m_Zoom),
                (int)(originalRect.Y * m_Zoom),
                (int)(originalRect.Width * m_Zoom),
                (int)(originalRect.Height * m_Zoom));

        }

        System.Drawing.Rectangle DeZoomRectangle(System.Drawing.Rectangle originalRect)
        {
            return new System.Drawing.Rectangle((int)(originalRect.X / m_Zoom),
                (int)(originalRect.Y / m_Zoom),
                (int)(originalRect.Width / m_Zoom),
                (int)(originalRect.Height / m_Zoom));
        }

        void DrawGrid(System.Drawing.Graphics g)
        {
            double horizGridSize = (ConvertToHPixels(this.GridSize / 100 * m_Zoom));
            double vertGridSize = (ConvertToVPixels(this.GridSize / 100 * m_Zoom));
            System.Drawing.Rectangle bounds = this.ConvertToPixels(this.SurfaceBounds);
            bounds = ZoomRectangle(bounds);

            System.Drawing.Pen gridPen = new System.Drawing.Pen(this.GridLineColor, (float)this.GridLineWidth);
            gridPen.DashStyle = this.GridLineDashStyle;

            for (int i = (int)vertGridSize; i < bounds.Height - 1; i = i + (int)vertGridSize)
            {
                g.DrawLine(gridPen, 0, i, bounds.Width, i);
            }
            for (int i = (int)horizGridSize; i < bounds.Width - 1; i = i + (int)horizGridSize)
            {
                g.DrawLine(gridPen, i, 0, i, bounds.Height);
            }
        }

        void DrawMargins(System.Drawing.Graphics g)
        {
            System.Drawing.Rectangle margins = ZoomRectangle(ConvertToPixels(this.SurfaceMargins));
            System.Drawing.Pen marginPen = new System.Drawing.Pen(this.MarginColor);
            marginPen.DashStyle = this.MarginLineDashStyle;
            marginPen.Width = (float)this.MarginLineWidth;
            g.DrawRectangle(marginPen, margins);
        }

        void DrawSelectionRectangle(System.Drawing.Graphics g)
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

        void MoleculeViewer_Paint(object sender, PaintEventArgs e)
        {
            //sender;
            //	try{

            System.Drawing.Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //get the dpi settings of the graphics context,
            //for example; 96dpi on screen, 600dpi for the printer
            //used to adjust grid and margin sizing.
            m_HorizRes = (int)(g.DpiX);
            m_VertRes = (int)(g.DpiY);

            //handle the possibility that the viewport is scrolled,
            //adjust my origin coordintates to compensate
            Point pt = this.AutoScrollPosition;
            g.TranslateTransform((float)(pt.X), (float)(pt.Y));

            System.Drawing.Rectangle bounds = this.ConvertToPixels(this.SurfaceBounds);
            bounds = ZoomRectangle(bounds);
            if (AutoScrollMinSize.Height != bounds.Height &&
                AutoScrollMinSize.Width != bounds.Width)
            {
                AutoScrollMinSize = new System.Drawing.Size(bounds.Width, bounds.Height);
            }
            g.Clear(this.NonPrintingAreaColor);
            g.FillRectangle(new SolidBrush(this.BackColor), bounds);


            //Draw dashed line margin indicators, over top of objects
            if (this.ShowGrid)
                DrawGrid(g);
            if (this.ShowMargins)
                DrawMargins(g);
            //draw the actual objects onto the page, on top of the grid
            //pass the graphics resolution onto the objects
            //so that images and other objects can be sized
            //correct taking the dpi into consideration.
            m_DrawingObjects.HorizontalResolution = (int)(g.DpiX);
            m_DrawingObjects.VerticalResolution = (int)(g.DpiY);
            m_DrawingObjects.DrawObjects(g, m_Zoom);

            //doesn't really draw the selected object, but instead the
            //selection indicator, a dotted outline around the selected object
            m_DrawingObjects.DrawSelectedObject(g, this.SelectedObject, this.m_Zoom);

            //draw selection rectangle (click and drag to select interface)
            //on top of everything else, but transparent
            if (m_SelectionDragging)
            {
                DrawSelectionRectangle(g);
            }
        }

        System.Drawing.Point gscTogoc(System.Drawing.Point gsPT)
        {
            return new System.Drawing.Point((int)((gsPT.X - this.AutoScrollPosition.X) / this.Zoom), (int)((gsPT.Y - this.AutoScrollPosition.Y) / this.Zoom));
        }

        System.Drawing.Point gscTogoc(int X, int Y)
        {
            return new System.Drawing.Point((int)((X - this.AutoScrollPosition.X) / this.Zoom), (int)((Y - this.AutoScrollPosition.Y) / this.Zoom));
        }

        System.Drawing.Point gocTogsc(System.Drawing.Point goPT)
        {
            return new System.Drawing.Point((int)((goPT.X) * this.m_Zoom), (int)((goPT.Y) * this.m_Zoom));
        }

        System.Drawing.Point gocTogsc(int X, int Y)
        {
            return new System.Drawing.Point((int)(X * m_Zoom), (int)(Y * m_Zoom));
        }

        double AngleToPoint(System.Drawing.Point Origin, System.Drawing.Point Target)
        {
            //'a cool little utility function, 
            //'given two points finds the angle between them....
            //'forced me to recall my highschool math, 
            //'but the task is made easier by a special overload to
            //'Atan that takes X,Y co-ordinates.
            double Angle;
            Target.X = Target.X - Origin.X;
            Target.Y = Target.Y - Origin.Y;
            Angle = Math.Atan2(Target.Y, Target.X) / (Math.PI / 180);
            return Angle;
        }

        private void MoleculeViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Cursor == System.Windows.Forms.Cursors.Cross) return;
            System.Drawing.Point mousePT = gscTogoc(e.X, e.Y);
            this.Invalidate();

            if (this.Cursor == System.Windows.Forms.Cursors.Arrow)
            {
                this.SelectedObject = m_DrawingObjects.FindObjectAtPoint(mousePT);
                if (this.SelectedObject != null)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        m_RotatingSelectedObject = true;
                        startingRotation = AngleToPoint(this.SelectedObject.GetPosition(), mousePT);
                        originalRotation = this.SelectedObject.Rotation;
                    }
                    else
                    {
                        //m_DraggingSelectedObject = true;
                        //dragOffset.X = this.SelectedObject.X - mousePT.X;
                        //dragOffset.Y = this.SelectedObject.Y - mousePT.Y;
                    }
                }
                else
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        m_SelectionDragging = true;
                        m_SelectionRectangle.X = e.X;
                        m_SelectionRectangle.Y = e.Y;
                        m_SelectionRectangle.Height = 0;
                        m_SelectionRectangle.Width = 0;
                    }
                }
            }
            else if (this.Cursor == System.Windows.Forms.Cursors.SizeNWSE ||
                this.Cursor == System.Windows.Forms.Cursors.SizeNESW ||
                this.Cursor == System.Windows.Forms.Cursors.SizeWE ||
                this.Cursor == System.Windows.Forms.Cursors.SizeNS)
            {
                dragOffset.X = this.SelectedObject.X - mousePT.X;
                dragOffset.Y = this.SelectedObject.Y - mousePT.Y;
                resizing = true;
            }
        }

        private void MoleculeViewer_MouseMove(object sender, MouseEventArgs e)
        {
            System.Drawing.Point dragPoint = gscTogoc(e.X, e.Y);
            System.Drawing.Point MousePoint = gscTogoc(e.X, e.Y);
            dragPoint.Offset(dragOffset.X, dragOffset.Y);
            System.Drawing.Size minSize = new System.Drawing.Size(16, 16);
            if (this.SelectedObject != null)
            {
                if (m_DraggingSelectedObject)
                {
                    Rectangle rect = new System.Drawing.Rectangle(this.SelectedObject.GetPosition().X, this.SelectedObject.GetPosition().Y, this.SelectedObject.Width, this.SelectedObject.Height);
                    this.SelectedObject.SetPosition(dragPoint);
                    if (StatusUpdate!=null) StatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.ObjectMoved,
                        this.SelectedObject, String.Format("Object Moved to {0}, {1}", dragPoint.X, dragPoint.Y),
                        dragPoint, 0));
                    this.Invalidate();
                }
                else if (m_RotatingSelectedObject)
                {
                    float currentRotation;
                    currentRotation = (float)AngleToPoint(this.SelectedObject.GetPosition(), dragPoint);
                    currentRotation = (float)((int)(currentRotation - startingRotation + originalRotation) % 360);
                    this.SelectedObject.Rotation = currentRotation;
                    if (StatusUpdate!=null) StatusUpdate(this, new StatusUpdateEventArgs(StatusUpdateType.ObjectRotated,
                        this.SelectedObject, String.Format("Object Rotated to {0} degrees", currentRotation),
                        new System.Drawing.Point(0, 0), currentRotation));
                    this.Invalidate();
                }
                else if (resizing)
                { //combine code because you will need to move ports in both cases
                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(this.SelectedObject.X, this.SelectedObject.Y, this.SelectedObject.Width, this.SelectedObject.Height);
                    System.Drawing.Size sz = new System.Drawing.Size(0, 0);
                    System.Drawing.Point fixedPT = new System.Drawing.Point(0, 0);
                    System.Drawing.Point dragPT = new System.Drawing.Point(0, 0);
                    this.SelectedObject.AutoSize = false;
                    switch (sz_direct)
                    {
                        case SizeDirection.Northwest:  //changing all
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                System.Drawing.Point LRHC = new System.Drawing.Point(0, 0);
                                sz = this.SelectedObject.GetSize();
                                LRHC.X = ULHC.X + sz.Width;
                                LRHC.Y = ULHC.Y + sz.Height;
                                sz.Width = LRHC.X - MousePoint.X;
                                sz.Height = LRHC.Y - MousePoint.Y;
                                if (sz.Width < 16)
                                {
                                    sz.Width = 16;
                                }
                                if (sz.Height < 16)
                                {
                                    sz.Height = 16;
                                }
                                ULHC.X = LRHC.X - sz.Width;
                                ULHC.Y = LRHC.Y - sz.Height;
                                this.SelectedObject.SetPosition(ULHC);
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.North://changing top, and height
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                System.Drawing.Point LLHC = new System.Drawing.Point(0, 0);
                                LLHC.X = ULHC.X;
                                sz = this.SelectedObject.GetSize();
                                LLHC.Y = ULHC.Y + sz.Height;
                                sz.Height = LLHC.Y - MousePoint.Y;
                                if (sz.Height < 16)
                                {
                                    sz.Height = 16;
                                }
                                ULHC.Y = LLHC.Y - sz.Height;
                                this.SelectedObject.SetPosition(ULHC);
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.Northeast://changing top, width, and height
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                System.Drawing.Point LLHC = new System.Drawing.Point(0, 0);
                                sz = this.SelectedObject.GetSize();
                                LLHC.X = ULHC.X;
                                LLHC.Y = ULHC.Y + sz.Height;
                                sz.Width = MousePoint.X - LLHC.X;
                                sz.Height = LLHC.Y - MousePoint.Y;
                                if (sz.Width < 16)
                                {
                                    sz.Width = 16;
                                }
                                if (sz.Height < 16)
                                {
                                    sz.Height = 16;
                                }
                                ULHC.X = LLHC.X;
                                ULHC.Y = LLHC.Y - sz.Height;
                                this.SelectedObject.SetPosition(ULHC);
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.East: //changing width
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                sz = this.SelectedObject.GetSize();
                                sz.Width = MousePoint.X - ULHC.X;
                                if (sz.Width < 16)
                                {
                                    sz.Width = 16;
                                }
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.Southeast:  //changing height, width
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                sz = this.SelectedObject.GetSize();
                                sz.Width = MousePoint.X - ULHC.X;
                                sz.Height = MousePoint.Y - ULHC.Y;
                                if (sz.Width < 16)
                                {
                                    sz.Width = 16;
                                }
                                if (sz.Height < 16)
                                {
                                    sz.Height = 16;
                                }
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.South://changing height
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                sz = this.SelectedObject.GetSize();
                                sz.Height = MousePoint.Y - ULHC.Y;
                                if (sz.Height < 16)
                                {
                                    sz.Height = 16;
                                }
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.Southwest:  //changing left,height, and width
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                System.Drawing.Point URHC = new System.Drawing.Point(0, 0);
                                sz = this.SelectedObject.GetSize();
                                URHC.Y = ULHC.Y;
                                URHC.X = ULHC.X + sz.Width;
                                sz.Width = URHC.X - MousePoint.X;
                                sz.Height = MousePoint.Y - URHC.Y;
                                if (sz.Width < 16)
                                {
                                    sz.Width = 16;
                                }
                                if (sz.Height < 16)
                                {
                                    sz.Height = 16;
                                }
                                ULHC.X = URHC.X - sz.Width;
                                this.SelectedObject.SetPosition(ULHC);
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        case SizeDirection.West://changing left and width
                            {
                                System.Drawing.Point ULHC = this.SelectedObject.GetPosition();
                                System.Drawing.Point URHC = new System.Drawing.Point(0, 0);
                                sz = this.SelectedObject.GetSize();
                                URHC.Y = ULHC.Y;
                                URHC.X = ULHC.X + sz.Width;
                                sz.Width = URHC.X - MousePoint.X;
                                if (sz.Width < 16)
                                {
                                    sz.Width = 16;
                                }
                                ULHC.X = URHC.X - sz.Width;
                                this.SelectedObject.SetPosition(ULHC);
                                this.SelectedObject.SetSize(sz);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            //else if (this.GetDrawingLine())
            //{
            //    //this.Cursor = Cursors.Cross;
            //    System.Drawing.Point points[] = this.GetDrawingLinePoints();
            //    points[points.Length - 1] = gscTogoc(e.X, e.Y);
            //    this.SetDrawingLinePoints(points);
            //    this.Invalidate();
            //}
            else if (m_SelectionDragging)
            {
                m_SelectionRectangle.Width = e.X - m_SelectionRectangle.X;
                m_SelectionRectangle.Height = e.Y - m_SelectionRectangle.Y;
            }
            this.Invalidate();
        }

        private void MoleculeViewer_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_DrawingLine) return;
            m_DraggingSelectedObject = false;
            m_RotatingSelectedObject = false;
            Point mousePT = gscTogoc(e.X, e.Y);
            if (m_SelectionDragging)
            {
                //'TODO: Rewrite to handle multiple selections
                //'really just need to change from this.SelectedObject to a collection
                //'add each found object in this loop, removing the Exit For
                System.Drawing.Rectangle zoomedSelection = DeZoomRectangle(m_SelectionRectangle);
                int numObj = m_DrawingObjects.Count;
                foreach (GraphicObject graphicObj in m_DrawingObjects)
                {
                    if (graphicObj.HitTest(zoomedSelection))
                    {
                        graphicObj.Selected = true;
                    }
                }
                m_SelectionDragging = false;
                if (SelectionChanged != null)
                {
                    if (numObj == 1) SelectionChanged(this, new SelectionChangedEventArgs(this.SelectedObject));
                    else SelectionChanged(this, new SelectionChangedEventArgs(this.SelectedObjects));
                }
            }
            else if (resizing)
            {
                resizing = false;
                sz_direct = SizeDirection.NA;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;
            }
            this.Invalidate();
        }

        private void MoleculeViewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == System.Windows.Forms.Keys.Delete)
            {
                if (this.SelectedObjects != null)
                {
                    for (int i = 0; i < this.SelectedObjects.Length; i++)
                    {
                        m_DrawingObjects.Remove(this.SelectedObjects[i]);
                    }
                    if (GraphicObjectsChanged!=null) GraphicObjectsChanged(this, new GraphicObjectsChangedEventsArgs(this.SelectedObjects, GraphicObjectsChangedType.Deleted));
                }
            }
            this.Invalidate();
        }

        private void MoleculeViewer_MouseClick(object sender, MouseEventArgs e)
        {
            System.Drawing.Point clickPT = System.Windows.Forms.Control.MousePosition;
            clickPT = PointToClient(clickPT);
            System.Drawing.Point mousePT = gscTogoc(clickPT);
            if (System.Windows.Forms.Cursor.Current == System.Windows.Forms.Cursors.Arrow)
            {
                this.SelectedObject = m_DrawingObjects.FindObjectAtPoint(mousePT);
            }
        }
    }
}

