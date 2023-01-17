using GH_IO.Serialization;
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
        public string InstanceVersion = string.Empty;
        private bool _isOldVersion = false;

        public Ironbug_Component(string name, string nickname, string description, string category, string subCategory)
            : base(name, nickname, description, category, subCategory)
        {
            this.IconDisplayMode = DisplayMode == 0 ? GH_IconDisplayMode.application : GH_IconDisplayMode.icon;
            this.InstanceVersion = IronbugInfo.version;
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
            Menu_AppendItem(menu, "IP-Unit", ChangeUnit, true, HVAC.BaseClass.IB_ModelObject.IPUnit)
                .ToolTipText = "This will set all HVAC components with IP unit system";
            var t = new ToolStripMenuItem("Icon Display Mode");
            Menu_AppendItem(t.DropDown, "Application", SetMode0, true, DisplayMode == 0)
                .ToolTipText = "Based on Grasshopper's global setting";
            Menu_AppendItem(t.DropDown, "Icon + NickName", SetMode1, true, DisplayMode == 1);
            Menu_AppendItem(t.DropDown, "Icon + FullName", SetMode2, true, DisplayMode == 2);
            menu.Items.Add(t);
            
            Menu_AppendItem(menu, $"VER {InstanceVersion}").ToolTipText= "Source: https://github.com/MingboPeng/Ironbug";

            
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

        public override bool Write(GH_IWriter writer)
        {
            writer.SetString("InstanceVersion", InstanceVersion);
            writer.SetInt32("IconDisplayMode", DisplayMode);
            //this.IconDisplayMode = DisplayMode == 0 ? GH_IconDisplayMode.application : GH_IconDisplayMode.icon;
            return base.Write(writer);
        }
        //public override bool Obsolete => _isOldVersion;
        private bool IsVersionCheckOk()
        {
            var v1 = new Version(IronbugInfo.version); //0.0.0.13 plugin version
            var v0 = this.InstanceVersion == "[unknown version]"? new Version(): new Version(this.InstanceVersion); // component instance version

            var isOldVersion = v1.Build - v0.Build > 2;
            if (v0>v1)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"This component is from a newer version {v0}, but you have installed Ironbug {v1}, which might cause issues. \nPlease update to the most updated Ironbug");
        
            }
            return isOldVersion;
        }
        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("InstanceVersion"))
            {
                InstanceVersion = reader.GetString("InstanceVersion");
            }
            else
            {
                InstanceVersion = "[unknown version]";
            }
            this._isOldVersion = this.IsVersionCheckOk();

            if (reader.ItemExists("IconDisplayMode"))
            {
                DisplayMode = reader.GetInt32("IconDisplayMode");
            }


            return base.Read(reader);
        }
        private void ChangeUnit(object sender, EventArgs e)
        {
            HVAC.BaseClass.IB_ModelObject.IPUnit = !HVAC.BaseClass.IB_ModelObject.IPUnit;

            MessageBox.Show("This only applies to ObjParams component for unit conversions for your convenience!\rKeep in mind, the rest of world is still using SI unit, such as in setpointmanagers, and all Ladybug/Honeybee components!");
            //TODO: maybe need recompute all??
            //Only Panel
            //But is it necessary, the unit is only for representation
            var allComs = GH.Instances.ActiveCanvas.Document.Objects.Where(_ => _ is Ironbug_ObjParams);
            foreach (var item in allComs)
            {
                item.ExpireSolution(false);
            }
            GH.Instances.RedrawCanvas();
            this.ExpireSolution(true);
        }

    }
}
