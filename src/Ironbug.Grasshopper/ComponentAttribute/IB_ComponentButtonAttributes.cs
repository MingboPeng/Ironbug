using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using System;
using System.Drawing;
using System.Windows.Forms;

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
        public Action<object> MouseDownEvent;
        public string ButtonText = string.Empty;

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Capsule button = GH_Capsule.CreateTextCapsule(ButtonBounds, ButtonBounds, GH_Palette.Black, ButtonText , 2, 0);
                button.Render(graphics, Selected,false, false);
                button.Dispose();
            }
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left && this.MouseDownEvent != null)
            {
                RectangleF rec = ButtonBounds;
                if (rec.Contains(e.CanvasLocation))
                {
                    var aa = sender.Location ;
                    this.MouseDownEvent(sender);
                    return GH_ObjectResponse.Handled;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

    }
    
}
