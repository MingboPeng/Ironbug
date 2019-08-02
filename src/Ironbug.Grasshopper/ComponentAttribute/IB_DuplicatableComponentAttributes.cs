using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class IB_DuplicatableComponentAttributes : IB_ComponentAttributes
    {
        public IB_DuplicatableComponentAttributes(GH_Component component) : base(component)
        {
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);
            if (channel == GH_CanvasChannel.Objects)
            {

                if (GH_Canvas.ZoomFadeMedium < 5) return;

                var sz = 3;
                
                SolidBrush solidBrush = new SolidBrush(Color.FromArgb(GH_Canvas.ZoomFadeMedium, Color.Gray));
                for (int i = 0; i < 3; i++)
                {
                    var rec = new RectangleF((this.Bounds.X + 8 +5*i), (float)(this.Bounds.Bottom -sz-1.5), sz, sz);
                    graphics.FillEllipse(solidBrush, rec);
                }
               
                solidBrush.Dispose();

            }
        }
    }
}
