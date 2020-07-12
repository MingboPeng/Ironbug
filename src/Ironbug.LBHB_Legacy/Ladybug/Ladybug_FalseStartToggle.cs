using System;
using System.Drawing;
using GH_IO.Serialization;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace Ironbug.LBHB_Legacy
{
    public class Ladybug_FalseStartToggle : GH_BooleanToggle
    {
        /// <summary>
        /// Initializes a new instance of the FalseStartToggle class.
        /// </summary>
        public Ladybug_FalseStartToggle()
        {
            this.Name = "Ladybug_FalseStartToggle";
            this.NickName = "FalseStart";
            this.Category = "LB-Legacy";
            this.SubCategory = "5 | Extra";
            this.Description = "This is the Boolean Toggle that will be automatically reset to False when copied or file open.\n\nPlease find the source code from:\nhttps://github.com/MingboPeng/Ironbug";
            
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        public override bool Read(GH_IReader reader)
        {
            var r = base.Read(reader);
            this.Value = false;
            return r;   
        }


        public override Guid ComponentGuid => new Guid("d447e41c-575f-47bb-b688-a157a9a86028");

        public override void CreateAttributes()
        {
            this.Attributes = new FalseStartToggleAttributes(this);
        }
    }

    public class FalseStartToggleAttributes : GH_BooleanToggleAttributes
    {
        public FalseStartToggleAttributes(Ladybug_FalseStartToggle owner) :base(owner)
        {
        }

        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            if (channel != GH_CanvasChannel.Objects)
                return;

            var btnBounds = Bounds;
            btnBounds.Width = Bounds.Width - 50;
            var btn = GH_Capsule.CreateTextCapsule(btnBounds, btnBounds, GH_Palette.Blue, Owner.NickName);
            btn.Render(graphics, Selected, Owner.Locked, true);
            btn.Dispose();
        }
    }
}