using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Microsoft.Scripting.Utils;
using System;
using System.Drawing;

namespace Ironbug.Grasshopper.Component
{
    public class IB_LoopComponentAttributes : GH_ComponentAttributes
    {
        public IB_LoopComponentAttributes(GH_Component component) : base(component)
        {
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel == GH_CanvasChannel.Wires)
            {
                var sources = Owner.Params.Input[0].Sources;
                //sources.AddRange(Owner.Params.Input[1].Sources);
                if (sources.Count>1)
                {
                    for (int i = 1; i < sources.Count; i++)
                    {
                        var obj1 = sources[i-1].Attributes.GetTopLevel.DocObject;
                        var obj2 = sources[i].Attributes.GetTopLevel.DocObject;
                        var color = Color.FromArgb(50, 240, 248, 255); //airloop
                        var colorSel = Color.FromArgb(200, 240, 248, 255); //airloop
                       
                        var pen = this.Selected? new Pen(colorSel, 2.5f): new Pen(color, 1.5f);
                        pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        DrawLoopFlow(canvas, graphics, pen, obj1, obj2);
                    }
                }
                
            }
            

            void DrawLoopFlow(GH_Canvas cvs, Graphics grph, Pen pen, IGH_DocumentObject p1, IGH_DocumentObject p2)
            {
                var b1 = p1.Attributes.Bounds;
                var p10 = new PointF((b1.Left + b1.Right) / 2, b1.Bottom);

                var b2 = p2.Attributes.Bounds;
                var p20 = new PointF((b2.Left + b2.Right) / 2, b2.Top);

                if (!cvs.Painter.ConnectionVisible(p10, p20)) return;

                float dy = Math.Abs(p10.Y - p20.Y) * 0.5f;
                var p11 = new PointF(p10.X, p10.Y + dy);
                var p21 = new PointF(p20.X, p20.Y - dy);

                var p2L = new PointF(p20.X - 3, p20.Y - 4);
                var p2R = new PointF(p20.X + 3, p20.Y - 4);

                
                //graphics.DrawEllipse(pen, new RectangleF(p11, new SizeF(1, 1)));
                //graphics.DrawEllipse(pen, new RectangleF(p21, new SizeF(1, 1)));
                //graphics.DrawPath(pen, wire);
                //graphics.DrawCurve(pen,new PointF[] { p10, p11, p21, p20 },1);
                grph.DrawBezier(pen, p10, p11, p21, p20);
                //grph.DrawLines(pen, new PointF[3] { p2L, p20, p2R });
                pen.Dispose();
            }
        }
        
    }
}
