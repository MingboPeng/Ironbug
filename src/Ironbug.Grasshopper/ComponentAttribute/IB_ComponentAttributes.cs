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
                var pStateMsg = (this.Owner as Ironbug_HVACComponentBase).PuppetableStateMsg;
                if (string.IsNullOrEmpty(pStateMsg)) return;

                if (this.Selected)
                {
                    //show detail puppet counts info
                    this.DrawPuppetIcon(graphics, this.Bounds, pStateMsg);
                }
                else
                {
                    //only show dots to indicate
                    this.DrawPuppetIcon(graphics, this.Bounds, string.Empty);
                }

            }
        }
        

        private void DrawPuppetIcon(Graphics graphics, RectangleF bounds, string message)
        {
            
            int size = 6;
            var y = bounds.Y - size - 3;

            
            //draw message
            var smallFont = GH_FontServer.Small;
            var sz = (float)System.Math.Round(66M / smallFont.Height);
            Font standardFontAdjust = GH_FontServer.NewFont(smallFont, sz);
            int fontWidth = GH_FontServer.StringWidth(message, standardFontAdjust);

            graphics.DrawString(message, standardFontAdjust, new SolidBrush(Color.Black), new PointF(bounds.Right - fontWidth, y-3));

            //draw dots
            for (int i = 1; i <= 3; i++)
            {
                var px = bounds.Right - fontWidth - i * size;
                var py = y;
                var center = new PointF( px - 2*i , py);
                var pBound = new RectangleF(center, new SizeF(size, size));
                graphics.FillEllipse(Brushes.Aquamarine, pBound);
                
            }

        }
        
    }
    
}
