using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System.Drawing;

namespace Ironbug.Grasshopper.Component
{
    public class IB_ComponentAttributes : GH_ComponentAttributes
    {
        public IB_ComponentAttributes(Ironbug_HVACComponentBase component):base(component)
        {
        }
        

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {
                
                var mode = Ironbug_HVACComponentBase.DisplayMode;
                if (mode == 0) return;


                if (GH_Canvas.ZoomFadeMedium ==255)
                {
                    var name = mode == 1 ? this.Owner.NickName : this.Owner.Name.Replace("Ironbug_", "");
                    this.DrawComName(graphics, this.Bounds, name);
                }
                

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

            graphics.DrawString(name, standardFontAdjust, new SolidBrush(Color.Black), new PointF(bounds.Left + bounds.Width/2 - fontWidth/2, (float)y));

          

        }
        
    }
    
}
