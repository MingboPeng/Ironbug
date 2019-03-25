using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System.Drawing;

namespace Ironbug.Grasshopper.Component
{
    public class IB_ComponentButtonAttributes : IB_ComponentAttributes
    {
        public IB_ComponentButtonAttributes(GH_Component component) : base(component)
        {
        }

        private Rectangle ButtonBounds { get; set; }

        protected override void Layout()
        {
            base.Layout();
            Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
            rec0.Height += 22;

            Rectangle rec1 = rec0;
            rec1.Y = rec1.Bottom - 22;
            rec1.Height = 22;
            rec1.Inflate(-2, -2);

            Bounds = rec0;
            this.ButtonBounds = rec1;
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule button = GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Black, "Right click", 2, 0);
                button.Render(graphics, Selected,false, false);
                button.Dispose();
            }
        }
        
    }
    
}
