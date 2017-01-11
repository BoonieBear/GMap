
namespace Demo.WindowsForms
{
   using System.Windows.Forms;
   using GMap.NET.WindowsForms;
   using System.Drawing;

   /// <summary>
   /// custom map of GMapControl
   /// </summary>
   public class Map : GMapControl
   {
      public long ElapsedMilliseconds;
      public string MapName;
      readonly Font DebugFont = new Font(FontFamily.GenericSansSerif, 36, FontStyle.Regular);


      /// <summary>
      /// any custom drawing here
      /// </summary>
      /// <param name="drawingContext"></param>
      protected override void OnPaintEtc(System.Drawing.Graphics g)
      {
         base.OnPaintEtc(g);


         g.DrawString(MapName, DebugFont, Brushes.Blue, 36, 36);

      }
   }
}
