using Grasshopper.Kernel;
using System;
using GH = Grasshopper;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HVACTemplate : Ironbug_Component
    {
        List<string> folderList = new List<string>();
        List<List<string>> filesList = new List<List<string>>();
        public Ironbug_HVACTemplate()
          : base("IB_HVACTemplate", "HVACTemplate",
              "Description",
              "Ironbug", "HVAC")
        {
        }
        public override Guid ComponentGuid => new Guid("F11CEDF5-6613-4B0B-A6C3-C0FCCF1454FD");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.HVACTemplate;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Directory", "_dir", "Additional folder path to import the HVAC template.", GH_ParamAccess.list);
            pManager[0].Optional = true;
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Templates", "out", "HVAC templates found from folders", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.folderList = new List<string>();
            this.filesList = new List<List<string>>();
            var root = System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location);
            var templateDir = System.IO.Path.Combine(root, "HVACTemplates");
            var dirs = new List<string>() { templateDir };
            DA.GetDataList(0, dirs);

            dirs =  dirs.Where(_ => Directory.Exists(_)).ToList();

            foreach (var dir in dirs)
            {
                var fs = Directory.GetFiles(dir, "*.gh*", SearchOption.AllDirectories).ToList();
                if (fs.Any())
                {
                    this.folderList.Add(Path.GetDirectoryName(Path.Combine(dir, "test.txt")));
                    this.filesList.Add(fs);

                }
            }
            DA.SetDataList(0, this.filesList.SelectMany(_=>_));

            this.templateMenu = GetHVACMenu();
        }

        private Size GetMoveVector(PointF FromLocation)
        {
            var moveX = this.Attributes.Bounds.Left - 80 - FromLocation.X;
            var moveY = this.Attributes.Bounds.Y + 180 - FromLocation.Y;
            var loc = new Point(Convert.ToInt32(moveX), Convert.ToInt32(moveY));
            
            return new Size(loc);
        }

        private void CreateTemplateFromXMLString(string FilePath, ref bool Run)
        {
            var canvasCurrent = GH.Instances.ActiveCanvas;
            var f = canvasCurrent.Focused;
            var isFileExist = File.Exists(FilePath);

            if (Run && f && isFileExist)
            {
                var io = new GH_DocumentIO();

                var success = io.Open(FilePath);

                if (!success)
                {
                    MessageBox.Show("Failed to add template.");
                    return;
                }
                var docTemp = io.Document;

                docTemp.SelectAll();
                docTemp.MutateAllIds();

                //move to where this component is...
                var box = docTemp.BoundingBox(false);
                var vec = GetMoveVector(box.Location);
                docTemp.TranslateObjects(vec ,true);

                docTemp.ExpireSolution();
                
                var docCurrent = canvasCurrent.Document;
                docCurrent.DeselectAll();
                docCurrent.MergeDocument(docTemp);
            }
        }


        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            //var newMenu = menu;
            //newMenu.Items.Clear();

            //newMenu.Items.AddRange(GetHVACMenu().Items);
            //templateMenu = newMenu;

        }

        private ToolStripDropDownMenu GetHVACMenu()
        {
            var menu = new ToolStripDropDownMenu();

            var count = 0;
            foreach (var filesPerFolder in this.filesList)
            {
                var menuItem = addFromFolder(this.folderList[count], filesPerFolder);
                menu.Items.Add(menuItem);
                count++;
            }
            return menu;
        }

        ToolStripDropDownMenu templateMenu = new ToolStripDropDownMenu();
        private ToolStripMenuItem addFromFolder(string rootFolder , List<string> filesPerFolder)
        {
            var folderName = new DirectoryInfo(rootFolder).Name;
            var t = new ToolStripMenuItem(folderName);

            foreach (var item in filesPerFolder)
            {
                var p = Path.GetDirectoryName(item);
                var name = Path.GetFileNameWithoutExtension(item);
                var showName = p.Length > rootFolder.Length ? p.Replace(rootFolder+"\\", "") + "\\" + name : name;

                EventHandler ev = (object sender, EventArgs e) =>
                {
                    var a = sender as ToolStripDropDownItem;
                    var r = true;
                    CreateTemplateFromXMLString(a.Tag.ToString(), ref r);
                    this.ExpireSolution(true);

                };

                Menu_AppendItem(t.DropDown, showName, ev, null, item);
            }
           
            return t;
        }

        public override void CreateAttributes()
        {
            var att = new IB_ComponentButtonAttributes(this);
            att.ButtonText = "Pick a system";

            att.MouseDownEvent += (object loc) => this.templateMenu.Show((GH.GUI.Canvas.GH_Canvas)loc,(loc as GH.GUI.Canvas.GH_Canvas).CursorControlPosition);
            this.Attributes = att;

        }
        
    }
    
}
