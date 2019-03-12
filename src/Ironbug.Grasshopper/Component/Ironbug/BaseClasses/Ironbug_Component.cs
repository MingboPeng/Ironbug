using Grasshopper.Kernel;
using System;
using System.Linq;
using System.Windows.Forms;
using GH = Grasshopper;

namespace Ironbug.Grasshopper.Component
{
    public abstract class Ironbug_Component: GH_Component
    {

        public static int DisplayMode = 1;

        public Ironbug_Component(string name, string nickname, string description, string category, string subCategory)
            : base(name, nickname, description, category, subCategory)
        {
            this.IconDisplayMode = DisplayMode == 0 ? GH_IconDisplayMode.application : GH_IconDisplayMode.icon;
        }
        public override void CreateAttributes()
        {
            var newAttri = new IB_ComponentAttributes(this);
            this.m_attributes = newAttri;

        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            menu.Items.RemoveAt(1); // remove Preview
            menu.Items.RemoveAt(2); // remove Bake

            var t = new ToolStripMenuItem("Icon Display Mode");
            Menu_AppendItem(t.DropDown, "Application", SetMode0, true, DisplayMode == 0)
                .ToolTipText = "Based on Grasshopper's global setting";
            Menu_AppendItem(t.DropDown, "Icon + NickName", SetMode1, true, DisplayMode == 1);
            Menu_AppendItem(t.DropDown, "Icon + FullName", SetMode2, true, DisplayMode == 2);
            menu.Items.Add(t);

            Menu_AppendItem(menu, "v 0.0.8");
        }

        private void SetMode0(object sender, EventArgs e)
        {
            DisplayMode = 0;
            UpdateAttribute();
        }
        private void SetMode1(object sender, EventArgs e)
        {
            DisplayMode = 1;
            UpdateAttribute();
        }
        private void SetMode2(object sender, EventArgs e)
        {
            DisplayMode = 2;
            UpdateAttribute();
        }

        private void UpdateAttribute()
        {
            var allComs = GH.Instances.ActiveCanvas.Document.Objects.Where(_ => _ is Ironbug_Component);
            var mode = DisplayMode == 0 ? GH_IconDisplayMode.application : GH_IconDisplayMode.icon;
            foreach (var item in allComs)
            {
                item.IconDisplayMode = mode;
                item.Attributes.ExpireLayout();
            }
            GH.Instances.RedrawCanvas();


        }
    }
}
