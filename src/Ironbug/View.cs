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
using Drawing = System.Drawing;
using Grasshopper.GUI;
using GH_IO.Serialization;

namespace Ironbug
{
    public class View : GH_Component
    {
        
        public Bitmap DisplayImage;
        public double Scale = 1;
        public List<Drawing.Point> ExtrCoordinates = new List<Drawing.Point>();
        public bool DisableClickable = true;
        public bool SaveImgWithCoords = false;
        public string newFilePath = string.Empty;
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
            pManager.AddPointParameter("Viewport Scale", "coordinates", "Set this image viewport scale.", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "imagePath", "New image marked with coordinates.\nClicked coordinates will not be ploted, please extract all coordinates first if you want to keep them.", GH_ParamAccess.item);
            //pManager.AddTextParameter("Color Values", "Values", "Color infomation that extracted from the input image.", GH_ParamAccess.list);
            pManager.AddTextParameter("Color Values", "Colors", "Color infomation that extracted from the input image.", GH_ParamAccess.list);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();

            //var downStreamParams = this.Params.Output[1].DirectConnectedComponents();
            //foreach (var item in downStreamParams)
            //{
            //    item.ExpireSolution(false);
            //}

            //var pathParam = this.Params.Input[0];
            //if (pathParam.SourceCount > 0)
            //{
            //    GH_Structure<GH_String> filePath = (GH_Structure<GH_String>)pathParam.VolatileData;

            //    this.FilePath = CheckImg(filePath.get_DataItem(0).Value);
            //}

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = string.Empty;
            this.DisplayImage = null;
            this.Message = null;
            this.ExtrCoordinates = new List<Drawing.Point>();

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
            
            if (this.SaveImgWithCoords)
            {
                string imgPathWithCoord = filePath.Insert(filePath.Length - 4, "_v");
                this.newFilePath = SaveImg(imgPathWithCoord);
            }
            
            if (File.Exists(this.newFilePath))
            {
                DA.SetData(0, this.newFilePath);
            }
            
            
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
            newAttri.mouseDownEvent += OnMouseDownEvent;
            
            //TODO: add two way event for click
            m_attributes = newAttri;
            
        }

        private void OnMouseDownEvent(object sender)
        {
            //MessageBox.Show(this.ExtrCoordinates.Count.ToString());
            string savedFile = SaveImg(this.newFilePath);
            var colors = GetColors(this.ExtrCoordinates, this.DisplayImage);

            if (File.Exists(savedFile))
            {
                this.Params.Output[0].ExpireSolution(false);
                this.Params.Output[0].AddVolatileData(new GH_Path(0), 0, savedFile);
            }
            
            //update color outputs
            this.Params.Output[1].ExpireSolution(false);
            this.Params.Output[1].ClearData();
            this.Params.Output[1].AddVolatileDataList(new GH_Path(0), colors);
            //GH_Structure<GH_String> outparam1 = (GH_Structure<GH_String>)this.Params.Output[1].VolatileData;
            //var currentValues = outparam1.AllData(true).Select(_ => _.ToString()).ToList();
            //currentValues.Add(clickedColor.ToString());
            //owner.Params.Output[1].ExpireSolution(false);
            ////this.Owner.Params.Output[1].AddVolatileData(new GH_Path(0), 0, currentValues);
            //owner.Params.Output[1].AddVolatileDataList(new GH_Path(0), currentValues);
            GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }

        //protected override void AfterSolveInstance()
        //{
        //    base.AfterSolveInstance();

        //}

        private string CheckImg(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }

            string tiffFile = string.Empty;
            var isHDR = Path.GetExtension(filePath).ToUpper() == ".HDR";
            if (File.Exists(filePath) && isHDR)
            {
                tiffFile = filePath.Substring(0, filePath.Length - 4) + "_LB.TIF";
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
                    //this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 10x. I've set your input to this!");
                    
                }
                else if (scaler < 0.5)
                {
                    scaler = 0.5;
                    //AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 0.5x. I've set your input to this!");
                    
                }
            }
            catch
            {
                //AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Scale must be a number. Set to 1.0");
                scaler = 1.0;
            }

            return scaler;
            
        }
        
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Clear clicked pixel coordinates", ClearValues);
            
            //TODO:
            Menu_AppendItem(menu, "Extract all pixel coordinates",OnExtractCoordinatesToGhPoints);
            Menu_AppendItem(menu, "Disable clickable image", OnDisableClick, true, this.DisableClickable);
            Menu_AppendItem(menu, "Save image with extracted coordinates", OnSaveImgWithCoords, true, this.SaveImgWithCoords);
            var menuItemScale = Menu_AppendItem(menu, "Viewport scale (0.5-10)");

            Menu_AppendTextItem(menuItemScale.DropDown, Scale.ToString(), OnKeydownEventHandler_Scale, OnTextChanged_Scale, true);
            
        }

        private void OnSaveImgWithCoords(object sender, EventArgs e)
        {
            this.SaveImgWithCoords = !this.SaveImgWithCoords;
            this.ExpireSolution(true);
        }

        private void OnExtractCoordinatesToGhPoints(object sender, EventArgs e)
        {
            var GHPoints = new List<GH_Point>();
            foreach (var item in this.ExtrCoordinates)
            {
                var pt = new Point3d(item.X, item.Y, 0);
                GHPoints.Add(new GH_Point(pt));
            }
            var thisParamAttr = this.Params.Input[1].Attributes;
            var paramPt = new Grasshopper.Kernel.Parameters.Param_Point();
            paramPt.CreateAttributes();
            paramPt.PersistentData.AppendRange(GHPoints);
            paramPt.Attributes.Pivot = new PointF(thisParamAttr.Bounds.Left - 80, thisParamAttr.Bounds.Y + 10);
            GH.Instances.ActiveCanvas.Document.AddObject(paramPt, false);
            paramPt.ExpireSolution(false);

            //create a group
            var GhGroup = new Grasshopper.Kernel.Special.GH_Group();
            GhGroup.CreateAttributes();
            GhGroup.Colour = Color.White;
            GhGroup.NickName = "Extracted Coordinates";
            GhGroup.AddObject(paramPt.InstanceGuid);
            GH.Instances.ActiveCanvas.Document.AddObject(GhGroup, false);
            GhGroup.ExpireSolution(false);

            GH.Instances.ActiveCanvas.Document.NewSolution(false);


        }

        private void OnDisableClick(object sender, EventArgs e)
        {
            this.DisableClickable = !this.DisableClickable;
        }

        //Read
        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("scale"))
            {
                this.Scale = reader.GetDouble("scale");
            }

            if (reader.ItemExists("DisableClickable"))
            {
                this.DisableClickable = reader.GetBoolean("DisableClickable");
            }
            return base.Read(reader);
        }

        //Write
        public override bool Write(GH_IWriter writer)
        {
            writer.SetDouble("scale", this.Scale);
            writer.SetBoolean("DisableClickable", this.DisableClickable);
            return base.Write(writer);  
        }

        private void OnTextChanged_Scale(GH_MenuTextBox sender, string text)
        {
            //throw new NotImplementedException();
        }

        private void OnKeydownEventHandler_Scale(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Scale = checkScale(sender.Text);
                
                //this.m_attributes.ExpireLayout();
                //this.OnDisplayExpired(true);
                this.ExpireSolution(true);
                
               
            }
            
        }

        private void ClearValues(object sender, EventArgs e)
        {
            
            var takeCount = this.Params.Input[1].VolatileDataCount;

            this.ExtrCoordinates.Take(takeCount);
            
            GH_Structure<GH_String> inputCoordinates = (GH_Structure<GH_String>)this.Params.Output[1].VolatileData;
            var keptValues = inputCoordinates.AllData(true).Select(_ => _.ToString()).ToList().Take(takeCount);

            this.Params.Output[1].ExpireSolution(false);
            this.Params.Output[1].ClearData();
            this.Params.Output[1].AddVolatileDataList(new GH_Path(0, 0), keptValues);

            this.Message = null;
            this.ExpireSolution(true);
            //GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }

        private List<Color> GetColors(List<Drawing.Point> imgCoordinates, Bitmap inBitmap)
        {
            var colors = new List<Color>();
            var coordinates = new List<Drawing.Point>();
           
            foreach (var item in imgCoordinates)
            {
                int x = item.X;
                int y = item.Y;
                bool isValidX = x >= 0 && x <= inBitmap.Width;
                bool isValidY = y >= 0 && y <= inBitmap.Height;

                if (isValidX && isValidY)
                {
                    coordinates.Add(new Drawing.Point(x, y));
                    colors.Add(inBitmap.GetPixel(x, y));
                    //((ImageFromPathAttrib)m_attributes).displayCoordinates(new System.Drawing.Point(x,y));
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "{ "+item.ToString() + "} is not a valid coordinate.\nIt should be no bigger than current image's size " + inBitmap.Size);
                }
                
            }

            this.ExtrCoordinates = coordinates;
            return colors;

        }

        private List<Color> GetColors(List<Point3d> imgCoordinates, Bitmap inBitmap)
        {

            var points = new List<Drawing.Point>();
            foreach (var item in imgCoordinates)
            {
                points.Add(new Drawing.Point((int)item.X, (int)item.Y));
            }

            var colors = GetColors(points,inBitmap);
            return colors;
        }

        //Save Image to file
        public string SaveImg(string filePath)
        {
            Bitmap bmp = new Bitmap(this.DisplayImage);
            var saveToFile = string.Empty;

            if (this.ExtrCoordinates.Count>0)
            {
                foreach (var item in this.ExtrCoordinates)
                {
                    var circlePixels = Drawcircle(item.X, item.Y, 3);
                    var validPixels = new List<Drawing.Point>();
                    foreach (var pixel in circlePixels)
                    {
                        int x = pixel.X;
                        int y = pixel.Y;
                        bool isValidX = x >= 0 && x <= bmp.Width;
                        bool isValidY = y >= 0 && y <= bmp.Height;
                        if (isValidX && isValidY)
                        {
                            validPixels.Add(pixel);
                        }
                    }

                    foreach (var pixel in validPixels)
                    {
                        bmp.SetPixel(pixel.X, pixel.Y, Color.White);
                    }
                }
                saveToFile = filePath.Insert(filePath.Length - 4, "d");
                
            }
            else
            {
                saveToFile = filePath;
                
            }


            try
            {
                if (File.Exists(saveToFile))
                {
                    File.Delete(saveToFile);
                }
                //var saveToFileTEMP = filePath.Insert(filePath.Length - 4, "TEMP");
                //bmp.Save(saveToFile);
                
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("cannot access the file"))
                //{
                //    saveToFile = filePath.Insert(filePath.Length - 4, "TEMP");
                //    //MessageBox.Show("Test");
                //    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "ssss");
                //}
                
                throw ex;
            }
            finally
            {
                bmp.Save(saveToFile);
            }
            
            return saveToFile;


        }

        private List<Drawing.Point> Drawcircle(int x0, int y0, int radius)
        {
            int x = radius - 1;
            int y = 0;
            int dx = 1;
            int dy = 1;
            int err = dx - (radius << 1);
            var pixelCoord = new List<Drawing.Point>();

            while (x >= y)
            {
                pixelCoord.Add(new Drawing.Point(x0 + x, y0 + y));
                pixelCoord.Add(new Drawing.Point(x0 + y, y0 + x));
                pixelCoord.Add(new Drawing.Point(x0 - y, y0 + x));
                pixelCoord.Add(new Drawing.Point(x0 - x, y0 + y));
                pixelCoord.Add(new Drawing.Point(x0 - x, y0 - y));
                pixelCoord.Add(new Drawing.Point(x0 - y, y0 - x));
                pixelCoord.Add(new Drawing.Point(x0 + y, y0 - x));
                pixelCoord.Add(new Drawing.Point(x0 + x, y0 - y));

                if (err <= 0)
                {
                    y++;
                    err += dy;
                    dy += 2;
                }
                else
                {
                    x--;
                    dx += 2;
                    err += (-radius << 1) + dx;
                }
            }



            return pixelCoord;
        }

        

    }
}
