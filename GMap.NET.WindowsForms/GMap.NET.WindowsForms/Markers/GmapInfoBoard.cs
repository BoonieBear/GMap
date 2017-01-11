
namespace GMap.NET.WindowsForms.Markers
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    public class GMapInfoBoard : GMapMarker
    {
        public Pen Pen;

        public readonly StringFormat Format = new StringFormat();
        public Font Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold, GraphicsUnit.Pixel);
        public Pen Stroke = new Pen(Color.FromArgb(140, Color.Aqua));
        public Brush Foreground = new SolidBrush(Color.Black);
        public Size TextPadding = new Size(10, 10);
        public string InfoText;
        public Point start;
        public Point end;
        float Radius;
        public Brush Fill = new SolidBrush(Color.FromArgb(222, Color.AntiqueWhite));
        public GMapInfoBoard(PointLatLng middle, string text)
            : base(middle)
        {
            InfoText = text;
            Radius = 10f;
#if !PocketPC
            Pen = new Pen(Brushes.Black, 1);
            this.Stroke.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
#else
         Pen = new Pen(Color.Red, 1);
#endif
            this.Offset = new Point(-6, -10);
            this.Stroke.Width = 2;

#if !PocketPC
            this.Stroke.LineJoin = LineJoin.Round;
            this.Stroke.StartCap = LineCap.RoundAnchor;
#endif

            this.Format.Alignment = StringAlignment.Center;
            this.Format.LineAlignment = StringAlignment.Center;
        }

        public override void OnRender(Graphics g)
        {
            System.Drawing.Size st = g.MeasureString(InfoText, Font).ToSize();
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y - st.Height, st.Width + TextPadding.Width, st.Height + TextPadding.Height);
            rect.Offset(Offset.X, Offset.Y);

            //g.DrawLine(Stroke, LocalPosition.X - Offset.X, LocalPosition.Y - Offset.Y, rect.X + rect.Width / 2, rect.Y + rect.Height);
            using (GraphicsPath objGP = new GraphicsPath())
            {
                objGP.AddLine(rect.X + 2 * Radius, rect.Y + rect.Height, rect.X + Radius, rect.Y + rect.Height + Radius);
                objGP.AddLine(rect.X + Radius, rect.Y + rect.Height + Radius, rect.X + Radius, rect.Y + rect.Height);
                
                
                
                
                objGP.AddArc(rect.X, rect.Y + rect.Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90);
                objGP.AddLine(rect.X, rect.Y + rect.Height - (Radius * 2), rect.X, rect.Y + Radius);
                objGP.AddArc(rect.X, rect.Y, Radius * 2, Radius * 2, 180, 90);
                objGP.AddLine(rect.X + Radius, rect.Y, rect.X + rect.Width - (Radius * 2), rect.Y);
                objGP.AddArc(rect.X + rect.Width - (Radius * 2), rect.Y, Radius * 2, Radius * 2, 270, 90);
                objGP.AddLine(rect.X + rect.Width, rect.Y + Radius, rect.X + rect.Width, rect.Y + rect.Height - (Radius * 2));
                objGP.AddArc(rect.X + rect.Width - (Radius * 2), rect.Y + rect.Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90); // Corner

                objGP.CloseFigure();

                g.FillPath(Fill, objGP);
                g.DrawPath(Stroke, objGP);
            }

            g.DrawString(InfoText, Font, Foreground, rect, Format);
        }
    }
}
