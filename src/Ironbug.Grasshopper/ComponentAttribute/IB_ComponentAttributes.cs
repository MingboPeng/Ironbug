//using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System.Drawing;

namespace Ironbug.Grasshopper.Component
{
    public class IB_ComponentAttributes : GH_ComponentAttributes
    {
        public IB_ComponentAttributes(GH_Component component) : base(component)
        {
        }
        

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                
                var mode = Ironbug_HVACComponent.DisplayMode;
                if (mode == 0) return;
                if (GH_Canvas.ZoomFadeMedium <5) return;
                
                var name = mode == 1 ? this.Owner.NickName : this.Owner.Name.Replace("Ironbug_", "");
                this.DrawComName(graphics, this.Bounds, name);
                
            }
        }
        

        private void DrawComName(Graphics graphics, RectangleF bounds, string name)
        {
            
            int size = 6;
            var y = bounds.Y - size *2.5;
            
            var smallFont = GH_FontServer.Small;
            var h = GH_FontServer.Standard.Height;
            var sz = (float)System.Math.Round(116M / h);
            Font standardFontAdjust = GH_FontServer.NewFont(GH_FontServer.Standard, sz);
            
            int fontWidth = GH_FontServer.StringWidth(name, standardFontAdjust);
            int recWidth = (int)bounds.Width - 6;

            if (recWidth< fontWidth)
            {
                standardFontAdjust = GH_FontServer.NewFont(GH_FontServer.Standard, sz* recWidth/fontWidth);
                fontWidth = GH_FontServer.StringWidth(name, standardFontAdjust);
            }

            recWidth = System.Math.Max(fontWidth + 4, recWidth);

            var rec = new Rectangle((int)(bounds.X+(bounds.Width/2-recWidth/2)), (int)bounds.Y-15, recWidth, 15);
            
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(GH_Canvas.ZoomFadeMedium,50, 50,50)),rec);
            
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(GH_Canvas.ZoomFadeMedium, 248, 248, 248));
            graphics.DrawString(name, standardFontAdjust, solidBrush, new PointF(bounds.Left + bounds.Width/2 - fontWidth/2, (float)y));

            standardFontAdjust.Dispose();
            solidBrush.Dispose();
            
        }
        
    }
    
}
