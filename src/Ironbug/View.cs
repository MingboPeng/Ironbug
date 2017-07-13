using System;
using System.Collections.Generic;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.IO;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Ironbug.Properties;
using System.Windows.Forms;
using System.Drawing;
using Grasshopper.GUI;
using GH_IO.Serialization;

namespace Ironbug
{
    public class View : GH_Component
    {
        
        public Bitmap DisplayImage;
        public double Scale = 1;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public View()
          : base("ViewData", "ViewData",
              "Description",
              "Ironbug", "Ironbug")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Image Path", "imagePath", "File Path", GH_ParamAccess.item);
            //pManager.AddNumberParameter("Viewport Scale", "scale", "Set this image viewport scale.", GH_ParamAccess.item,1);
            pManager.AddPointParameter("Viewport Scale", "coords", "Set this image viewport scale.", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "imagePath", "Converted file path", GH_ParamAccess.item);
            //pManager.AddTextParameter("Color Values", "Values", "Color infomation that extracted from the input image.", GH_ParamAccess.list);
            pManager.AddTextParameter("Color Values", "Colors", "Color infomation that extracted from the input image.", GH_ParamAccess.list);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        //protected override void BeforeSolveInstance()
        //{
        //    base.BeforeSolveInstance();
        //    var pathParam = this.Params.Input[0];
        //    if (pathParam.SourceCount>0)
        //    {
        //        GH_Structure<GH_String> filePath = (GH_Structure<GH_String>)pathParam.VolatileData;

        //        this.FilePath = CheckImg(filePath.get_DataItem(0).Value);
        //    }
            
        //}

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = string.Empty;
            this.DisplayImage = null;

            if (!DA.GetData(0, ref filePath))
            {
                //((ImageFromPathAttrib)m_attributes).imgPath = string.Empty;
                return;
            }

            filePath = CheckImg(filePath);

            if (string.IsNullOrEmpty(filePath))
            {
                
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Path must be a valid location");
                return;
            }
            this.DisplayImage = new Bitmap(filePath);

            //get colors from input coordinates
            var imgCoordinates = new List<Point3d>();
            if (DA.GetDataList(1, imgCoordinates))
            {
                var colors = GetColors(imgCoordinates, this.DisplayImage);
                DA.SetDataList(1, colors);
            }
            

            //((ImageFromPathAttrib)m_attributes).imgPath = FilePath;
            //((ImageFromPathAttrib)m_attributes).Scale = this.Scale;
            //GH.Instances.InvalidateCanvas();
            //GH.Instances.ActiveCanvas.Update();

            DA.SetData(0, filePath);
            
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return Resources.Ladybug_Viewer_24;
                //return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a5359d3f-336b-443b-98b7-7903b6cc0c47"); }
        }


        public override void CreateAttributes()
        {
            var newAttri = new ImageFromPathAttrib(this);
            //newAttri.mouseDownEvent += OnMouseDownEvent;
            
            //TODO: add two way event for click
            m_attributes = newAttri;
            
        }
        
        private string CheckImg(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }

            string tiffFile = string.Empty;

            if (File.Exists(filePath) && Path.GetExtension(filePath).ToUpper() == ".HDR")
            {
                tiffFile = filePath.Substring(0, filePath.Length - 3) + "TIF";
                var isNewHDR = true;

                if (File.Exists(tiffFile))
                {
                    var hdrTimeStamp = File.GetLastWriteTime(filePath);
                    var tifTimeStamp = File.GetLastWriteTime(tiffFile);

                    //if ==1: hdrTimeStamp is later than tifTimeStamp
                    //isNewHDR = true, to convert to a new tiff
                    isNewHDR = DateTime.Compare(hdrTimeStamp, tifTimeStamp) == 1;
                }

                if (isNewHDR)
                {
                    //convert the hdr to tiff
                    string cmdStr1 = @"ra_tiff " + filePath + " " + tiffFile;
                    var cmdStrings = new List<string>();
                    cmdStrings.Add(@"SET RAYPATH=.;C:\Radiance\lib&PATH=C:\Radiance\bin;$PATH");
                    cmdStrings.Add(cmdStr1);
                    CMD.Execute(cmdStrings);
                }

            }
            else if (File.Exists(filePath))
            {
                tiffFile = filePath;
            }
            else
            {
                
                return string.Empty;

            }

            return tiffFile;
        }

        private double checkScale (string scale)
        {

            double scaler;
            try
            {
                scaler = Convert.ToDouble(scale);
                //scaler = 1;
                if (scaler > 10)
                {
                    scaler = 10;
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 10x. I've set your input to this!");
                }
                else if (scaler < 0.5)
                {
                    scaler = 0.5;
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 0.5x. I've set your input to this!");
                }
            }
            catch
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Scale must be a number. Set to 1.0");
                scaler = 1.0;
            }

            return scaler;
            
        }
        
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Clear values", ClearValues);
            
            //TODO:
            Menu_AppendItem(menu, "Disable clickable image");
            
            var menuItemScale = Menu_AppendItem(menu, "Viewport scale");

            Menu_AppendTextItem(menuItemScale.DropDown, Scale.ToString(), keydownEventHandler, textChanged, true);
            
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("scale"))
            {
                this.Scale = reader.GetDouble("scale");
            }
            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetDouble("scale", this.Scale);
            return base.Write(writer);  
        }

        private void textChanged(GH_MenuTextBox sender, string text)
        {
            //throw new NotImplementedException();
        }

        private void keydownEventHandler(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Scale = checkScale(sender.Text);
                

                this.m_attributes.ExpireLayout();
                //this.OnDisplayExpired(true);
                this.ExpireSolution(true);
               
            }
            
        }

        private void ClearValues(object sender, EventArgs e)
        {
            this.Params.Output[1].ExpireSolution(false);
            this.Params.Output[1].ClearData();
            GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }

        private List<Color> GetColors(List<Point3d> imgCoordinates, Bitmap inBitmap)
        {
            var colors = new List<Color>();
           
            foreach (var item in imgCoordinates)
            {
                int x = (int)item.X;
                int y = (int)item.Y;
                bool isValidX = x >= 0 && x <= inBitmap.Width;
                bool isValidY = y >= 0 && y <= inBitmap.Height;

                if (isValidX && isValidY)
                {
                    colors.Add(inBitmap.GetPixel(x, y));
                    //((ImageFromPathAttrib)m_attributes).displayCoordinates(new System.Drawing.Point(x,y));
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "{ "+item.ToString() + "} is not a valid coordinate.\nIt should be no bigger than current image's size " + inBitmap.Size);
                }
                
            }
            
            return colors;

        }
    }
}
