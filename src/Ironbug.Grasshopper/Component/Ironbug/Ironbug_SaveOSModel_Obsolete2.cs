using System;
using Grasshopper.Kernel;
using System.IO;
using System.Windows.Forms;
using GH_IO.Serialization;
using System.Diagnostics;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SaveOSModel_Obsolete2 : Ironbug_Component
    {
        public override bool Obsolete => true;
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.saveHVAC; 
        public override Guid ComponentGuid => new Guid("{2B473359-4DFC-4DE7-BD3E-79C119C64250}");

        bool _overrideMode = false;
        int _writeMode = 0;
        public Ironbug_SaveOSModel_Obsolete2()
          : base("IB_SaveToFile", "SaveToFile",
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
           
            if (File.Exists(filepath))
            {
                if (this._overrideMode)
                {
                    File.Delete(filepath);
                }
                else
                {
                    var newfilepath = filepath.Replace(".osm", "_IB.osm");
                    File.Copy(filepath, newfilepath, true);
                    filepath = newfilepath;
                }
            }

            var saved = hvac.SaveHVAC(filepath);

            if (saved)
            {
                DA.SetData(0, filepath);
            }

            if (this._writeMode == 1)
            {
                try
                {
                    OpenOPS(filepath);
                }
                catch (Exception)
                {

                    throw;
                }
               
            }

            void OpenOPS(string FilePath)
            {
                var OpsPath = OpenStudio.OpenStudioUtilitiesCore.getOpenStudioModuleDirectory().__str__().Remove(20) + @"bin";
                var appPath = Path.Combine(OpsPath, "OpenStudioApp.exe");
                if (File.Exists(appPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.Arguments = " \"" + FilePath + "\"";
                    startInfo.FileName = appPath;
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    startInfo.UseShellExecute = false;
                    Process.Start(startInfo);
                }
                else
                {
                    Process.Start(FilePath);
                }
               
            }

        }


        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            
            Menu_AppendItem(menu, "Override", ChangeOverrideModel, true, _overrideMode)
               .ToolTipText = "This will remove the osm file first if exists.";
            Menu_AppendItem(menu, "Write&Open", ChangeWriteMode, true, _writeMode == 1)
              .ToolTipText = "Open the OpenStudio App after saved the osm file.";
            //Menu_AppendItem(menu, "Write&Run", ChangeOverrideModel, true, _overrideMode)
            //  .ToolTipText = "This will remove the osm file first if exists.";
            Menu_AppendSeparator(menu);

            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void ChangeWriteMode(object sender, EventArgs e)
        {
            var writeParm = this.Params.Input[2];
            if (this._writeMode != 1)
            {
                this._writeMode = 1;
                writeParm.NickName = "_write&Open";
            }
            else
            {
                this._writeMode = 0;
                writeParm.NickName = "_write";
            }
            this.ExpireSolution(true);

        }

        private void ChangeOverrideModel(object sender, EventArgs e)
        {
            this._overrideMode = !_overrideMode;
            this.ExpireSolution(true);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean ("OverrideMode", this._overrideMode);
            writer.SetInt32("_writeMode", this._writeMode);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {

            if (reader.ItemExists("OverrideMode"))
            {
                this._overrideMode = reader.GetBoolean("OverrideMode");
            }
            if (reader.ItemExists("_writeMode"))
            {
                this._writeMode = reader.GetInt32("_writeMode");
            }
            return base.Read(reader);
        }
    }
}