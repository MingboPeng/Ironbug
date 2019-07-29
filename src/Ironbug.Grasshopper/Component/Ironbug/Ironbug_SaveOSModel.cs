using System;
using Grasshopper.Kernel;
using System.IO;
using System.Windows.Forms;
using GH_IO.Serialization;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SaveOSModel : Ironbug_Component
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.saveHVAC; 
        public override Guid ComponentGuid => new Guid("{2B473359-4DFC-4DE7-BD3E-79C119C64250}");

        bool _overrideMode = true;
        public Ironbug_SaveOSModel()
          : base("Ironbug_SaveToFile", "SaveToFile",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("NewFilePath", "_OSMpath", "New OSM file path. This file will be deleted first if it is existed", GH_ParamAccess.item);
            pManager.AddGenericParameter("HVACSystem", "_HVAC", "A HVAC system from Ironbug_HVACSystem", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Write", "_write", "Write the OpenStudio file.", GH_ParamAccess.item, false);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.Message = this._overrideMode ? "Override" : "";
            string filepath = string.Empty;
            HVAC.IB_HVACSystem hvac = null;
            bool write = false;

            DA.GetData(0, ref filepath);
            DA.GetData(1, ref hvac);
            DA.GetData(2, ref write);

            if (!write) return;
            
            if (string.IsNullOrEmpty(filepath)) return;
            if (File.Exists(filepath) && this._overrideMode)
            {
                File.Delete(filepath);
            } 
            var saved = hvac.SaveHVAC(filepath);

            if (saved)
            {
                DA.SetData(0, filepath);
            }
            
            
        }


        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            
            Menu_AppendItem(menu, "Override", ChangeOverrideModel, true, _overrideMode)
               .ToolTipText = "This will remove the osm file first if exists.";
            Menu_AppendSeparator(menu);

            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void ChangeOverrideModel(object sender, EventArgs e)
        {
            this._overrideMode = !_overrideMode;
            this.ExpireSolution(true);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean ("OverrideMode", this._overrideMode);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {

            if (reader.ItemExists("OverrideMode"))
            {
                this._overrideMode = reader.GetBoolean("OverrideMode");
            }

            return base.Read(reader);
        }
    }
}