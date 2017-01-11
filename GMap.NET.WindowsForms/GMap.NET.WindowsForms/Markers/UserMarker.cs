namespace GMap.NET.WindowsForms.Markers
{
    using System.Drawing;

#if !PocketPC
    using System.Windows.Forms.Properties;
#else
   using GMap.NET.WindowsMobile.Properties;
#endif

    public class UserMarker : GMapMarker
    {
        public float? Bearing;

        static readonly System.Drawing.Size SizeSt = new System.Drawing.Size(Resources.boat.Width, Resources.boat.Height);

        public UserMarker(PointLatLng p)
            : base(p)
        {
            Size = SizeSt;
            Offset = new Point(-15, -29);
        }

        static readonly Point[] Arrow = new Point[] { new Point(-7, 7), new Point(0, -22), new Point(7, 7), new Point(0, 2) };

        public override void OnRender(Graphics g)
        {
#if !PocketPC
            if (!Bearing.HasValue)
            {
                g.DrawImageUnscaled(Resources.shadow50, LocalPosition.X+5, LocalPosition.Y-4);
            }
            g.TranslateTransform(ToolTipPosition.X, ToolTipPosition.Y);

            if (Bearing.HasValue)
            {
                g.RotateTransform(Bearing.Value - Overlay.Control.Bearing);
                g.FillPolygon(Brushes.Lime, Arrow);
            }

            g.ResetTransform();

            if (!Bearing.HasValue)
            {
                g.DrawImageUnscaled(Resources.boat, LocalPosition.X, LocalPosition.Y);
            }
#else
            DrawImageUnscaled(g, Resources.shadow50, LocalPosition.X, LocalPosition.Y);
            DrawImageUnscaled(g, Resources.marker, LocalPosition.X, LocalPosition.Y);
#endif
        }
    }
}