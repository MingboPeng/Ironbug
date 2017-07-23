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
using Grasshopper.Kernel.Special;

namespace Ironbug
{
    public class View : GH_Component
    {
        public List<string> FilePaths = new List<string>();
        public List<Bitmap> Bitmaps = new List<Bitmap>();
        public int currentBitmapIndex = 0;

        public Bitmap Bitmap;


        public double Scale = 1;
        public List<Drawing.Point> ExtrCoordinates = new List<Drawing.Point>();
        private List<Drawing.Point> TempExtrCoordinates = new List<Drawing.Point>();
        private List<Drawing.Point> TempExtrCoordinates2 = new List<Drawing.Point>();
        public List<Color> ExtrColors = new List<Color>();

        public bool DisableClickable = true;
        public bool SaveImgWithCoords = false;

        private bool isScaleChanged = false;
        //public string newFilePath = string.Empty;
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public View()
          : base("Ladybug_ImageViewer", "Viewer",
              "Preview image files",
              "Ladybug", "5 | Extra")
        {
            this.Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }


        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Image Path", "imagePath_", "Image file path", GH_ParamAccess.list);
            pManager.AddNumberParameter("Viewport Scale", "_scale_", "Set this image viewport scale.", GH_ParamAccess.item, 1);
            pManager.AddPointParameter("Pixel Coordinates", "coordinates_", "A list of points for extracting colors from the source image.", GH_ParamAccess.list);
            
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "imagePath", "A new image marked with coordinates.", GH_ParamAccess.list);
            //pManager.AddTextParameter("Color Values", "Values", "Color infomation that extracted from the input image.", GH_ParamAccess.list);
            pManager.AddTextParameter("Color Values", "colors", "Color infomation that extracted from the input image.", GH_ParamAccess.list);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }
        

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();

            FilePaths = new List<string>();
            this.Bitmaps = new List<Bitmap>();
            this.Bitmap = null;
            this.Message = null;
            this.currentBitmapIndex = 0;

            this.TempExtrCoordinates2 = new List<Drawing.Point>(this.ExtrCoordinates);
            this.ExtrCoordinates = TempExtrCoordinates;
            this.TempExtrCoordinates = new List<Drawing.Point>();
            
            this.ExtrColors = new List<Color>();
            
        }

        ////happens befor solution
        //private void OnScaleParam_ObjectChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        //{
        //    this.TempExtrCoordinates = new List<Drawing.Point>(this.ExtrCoordinates);
        //}
        
        //happens after solution
        private void OnScale_SolutionExpired(IGH_DocumentObject sender, GH_SolutionExpiredEventArgs e)
        {
            //((GH_Slider)sender).ValueChanged
            if (this.Params.Input[1].SourceCount>0)
            {
                this.ExtrCoordinates = this.TempExtrCoordinates2;
                this.TempExtrCoordinates2 = new List<Drawing.Point>();

                this.ExtrColors = GetColors(this.ExtrCoordinates, this.Bitmap);
                this.UpdateColorParamValues();
            }
            else
            {
                sender.SolutionExpired -= this.OnScale_SolutionExpired;
            }
            
        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (e.ParameterSide == GH_ParameterSide.Output) return;

            if (e.ParameterIndex == 1)
            {
                this.isScaleChanged = true;
                this.TempExtrCoordinates = new List<Drawing.Point>(this.ExtrCoordinates);
                if (this.Params.Input[1].SourceCount>0)
                {
                    this.Params.Input[1].Sources[0].SolutionExpired -= OnScale_SolutionExpired;
                    this.Params.Input[1].Sources[0].SolutionExpired += OnScale_SolutionExpired;
                }

            }
            


        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var filePaths = new List<string>();
            
            double scale = 1;

            if (!DA.GetDataList(0, filePaths)) return;

            if (!DA.GetData(1, ref scale))
            {
                return;
            }
            else
            {
                this.Scale = checkScale(scale);
            }

            this.Bitmaps = ProcessImgs(filePaths, ref this.FilePaths);
            if (Bitmaps.IsNullOrEmpty())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No valid images");
                return;
            }

            //return the processed files
            DA.SetDataList(0, this.FilePaths);


            //current img to be shown
            this.Bitmap = Bitmaps[this.currentBitmapIndex];
            string filePath = this.FilePaths[this.currentBitmapIndex];
            

            //get colors from input coordinates
            var imgCoordinates = new List<Point3d>();
            if (DA.GetDataList(2, imgCoordinates))
            {
                if (!imgCoordinates.IsNullOrEmpty() && !isScaleChanged)
                {
                    this.ExtrCoordinates = imgCoordinates.Select(_ => new Drawing.Point((int)_.X, (int)_.Y)).ToList();
                }
                else
                {
                    this.isScaleChanged = false;
                }
                
            }

            this.ExtrColors = GetColors(this.ExtrCoordinates, this.Bitmap);
            DA.SetDataList(1, this.ExtrColors);
            
            var savedFile = SaveImg(filePath, this.SaveImgWithCoords);
            //var imgs = new List<string>(this.FilePaths);
            //imgs[currentBitmapIndex] = savedFile;
            //DA.SetDataList(0, imgs);


        }

        private List<Bitmap> ProcessImgs(List<string> filePaths, ref List<string> newFilePaths)
        {
            filePaths = filePaths.Select(_ => CheckImg(_)).ToList();
            filePaths.RemoveAll( _=>_ == null);
            var bitmaps = new List<Bitmap>();

            if (filePaths.IsNullOrEmpty())
            {
                return bitmaps;
            }

            
            foreach (var item in filePaths)
            {
                newFilePaths.Add(item);
                bitmaps.Add(new Bitmap(item));
            }
            
            return bitmaps;
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
            newAttri.mouseImgClickEvent += OnMouseImgClickEvent;
            newAttri.mouseNavClickEvent += OnMouseNavClickEvent;
            m_attributes = newAttri;
            
        }

        private void OnMouseNavClickEvent(object sender, bool clickedRightButton)
        {
            if (!clickedRightButton)
            {
                if (currentBitmapIndex > 0)
                {
                    currentBitmapIndex--;
                }
                else
                {
                    currentBitmapIndex = this.Bitmaps.Count -1;
                    
                }
            }
            else
            {
                if (currentBitmapIndex < this.Bitmaps.Count-1)
                {
                    currentBitmapIndex++;
                }
                else
                {
                    currentBitmapIndex = 0;

                }
            }

            this.Message = string.Format("({0}/{1})", currentBitmapIndex + 1, this.Bitmaps.Count);
            this.Bitmap = this.Bitmaps[currentBitmapIndex];

            this.ExtrColors = GetColors(this.ExtrCoordinates, this.Bitmap);

            var savedFile = SaveImg(this.FilePaths[this.currentBitmapIndex], this.SaveImgWithCoords);
            var imgs = new List<string>( this.FilePaths);
            imgs[currentBitmapIndex] = savedFile;
            this.Params.Output[0].ExpireSolution(false);
            this.Params.Output[0].AddVolatileDataList(new GH_Path(0,0), imgs);

            ////update color outputs
            //this.Params.Output[1].ExpireSolution(false);
            //this.Params.Output[1].ClearData();
            //this.Params.Output[1].AddVolatileDataList(new GH_Path(0), this.ExtrColors);
            //GH.Instances.ActiveCanvas.Document.NewSolution(false);
            this.UpdateColorParamValues();
        }

        private void OnMouseImgClickEvent(object sender, Drawing.Point clickedPtOnOriginalBitmap)
        {
            this.ExtrCoordinates.Add(clickedPtOnOriginalBitmap);
            var clickedColor = this.Bitmap.GetPixel(clickedPtOnOriginalBitmap.X, clickedPtOnOriginalBitmap.Y);
            this.ExtrColors.Add(clickedColor);

            string savedFile = SaveImg(this.FilePaths[this.currentBitmapIndex], this.SaveImgWithCoords);

            this.Message = "Clicked at: " + clickedPtOnOriginalBitmap + "\n" + clickedColor.ToString();


            var imgs = new List<string>(this.FilePaths);
            imgs[currentBitmapIndex] = savedFile;
            this.Params.Output[0].ExpireSolution(false);
            this.Params.Output[0].AddVolatileDataList(new GH_Path(0, 0), imgs);


            this.UpdateColorParamValues();
        }
        
        private string CheckImg(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return filePath;
            }

            string tiffFile = string.Empty;

            
            var folder = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileExtension = Path.GetExtension(filePath);

            var isHDR = fileExtension.ToUpper() == ".HDR";

            //convert HDR
            if (File.Exists(filePath) && isHDR)
            {
                tiffFile = folder + "\\"+fileName + "_LB.TIF";
                //tiffFile = filePath.Substring(0, filePath.Length - 4) + "_LB.TIF";
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
                tiffFile = folder + "\\" + fileName + "_LB" + fileExtension;
                //tiffFile = filePath.Insert(filePath.Length - 4, "_LB");
                var isNewImg = true;
                if (File.Exists(tiffFile))
                {
                    var hdrTimeStamp = File.GetLastWriteTime(filePath);
                    var tifTimeStamp = File.GetLastWriteTime(tiffFile);
                    isNewImg = DateTime.Compare(hdrTimeStamp, tifTimeStamp) == 1;
                }

                if (isNewImg)
                {
                    File.Delete(tiffFile);
                    File.Copy(filePath, tiffFile, true);
                }

                
            }
            else
            {
                return string.Empty;
            }

            return tiffFile;
        }

        private double checkScale (double scale)
        {
            
            if (scale > 10)
            {
                scale = 10;
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum valid scale is 10x. The scale is re-set to 10.");

            }
            else if (scale < 0.5)
            {
                scale = 0.5;
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Minimum valid scale is 0.5x. The scale is re-set to 0.5.");

            }

            return scale;
            
        }
        
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            //base.AppendAdditionalComponentMenuItems(menu);
            var newMenu = menu;
            newMenu.Items.RemoveAt(1); // remove Preview
            newMenu.Items.RemoveAt(2); // remove Bake

            Menu_AppendItem(newMenu, "Clear clicked pixel coordinates", ClearValues);
            
            Menu_AppendItem(newMenu, "Extract all pixel coordinates",OnExtractPtToGhPoints);
            Menu_AppendItem(newMenu, "Disable clickable image", OnDisableImgClickable, true, this.DisableClickable);
            Menu_AppendItem(newMenu, "Save this image with pixel coordinates", OnSaveImgWithCoords, true, this.SaveImgWithCoords);
            Menu_AppendSeparator(newMenu);
            Menu_AppendItem(newMenu, "Export GIF",OnExportGif);

            //var menuItemScale = Menu_AppendItem(menu, "Viewport scale (0.5-10)");
            //Menu_AppendTextItem(menuItemScale.DropDown, Scale.ToString(), OnKeydownEventHandler_Scale, OnTextChanged_Scale, true);

        }

        private void OnExportGif(object sender, EventArgs e)
        {
            //var path = @"C:\Colibri\WithCrd\aa.gif";

            var saveToGif = Path.GetDirectoryName(this.FilePaths[currentBitmapIndex]) + "\\Exported.gif";
            this.Bitmaps.SaveAnimatedGifImage(saveToGif);
            
        }

        private void ClearValues(object sender, EventArgs e)
        {

            var takeCount = this.Params.Input[2].VolatileDataCount;
            //MessageBox.Show(takeCount.ToString());
            this.ExtrCoordinates = this.ExtrCoordinates.Take(takeCount).ToList();
            this.ExtrColors = this.ExtrColors.Take(takeCount).ToList();
            
            //this.Params.Output[1].ExpireSolution(false);
            //this.Params.Output[1].ClearData();
            //this.Params.Output[1].AddVolatileDataList(new GH_Path(0, 0), this.ExtrColors);

            this.Message = null;
            //this.ExpireSolution(true);
            this.Attributes.ExpireLayout();
            //GH.Instances.ActiveCanvas.Document.NewSolution(false);
            this.UpdateColorParamValues();
        }

        private void OnSaveImgWithCoords(object sender, EventArgs e)
        {
            this.SaveImgWithCoords = !this.SaveImgWithCoords;
            this.ExpireSolution(true);
        }

        private void OnExtractPtToGhPoints(object sender, EventArgs e)
        {
            var GHPoints = new List<GH_Point>();
            foreach (var item in this.ExtrCoordinates)
            {
                var pt = new Point3d(item.X, item.Y, 0);
                GHPoints.Add(new GH_Point(pt));
            }
            var thisParamAttr = this.Params.Input[2].Attributes;
            var paramPt = new GH.Kernel.Parameters.Param_Point();
            paramPt.CreateAttributes();
            paramPt.PersistentData.AppendRange(GHPoints);
            paramPt.Attributes.Pivot = new PointF(thisParamAttr.Bounds.Left - 80, thisParamAttr.Bounds.Y + 10);
            GH.Instances.ActiveCanvas.Document.AddObject(paramPt, false);
            paramPt.ExpireSolution(false);

            //create a group
            var GhGroup = new GH_Group();
            GhGroup.CreateAttributes();
            GhGroup.Colour = Color.White;
            GhGroup.NickName = "Extracted Coordinates";
            GhGroup.AddObject(paramPt.InstanceGuid);
            GH.Instances.ActiveCanvas.Document.AddObject(GhGroup, false);
            GhGroup.ExpireSolution(false);

            GH.Instances.ActiveCanvas.Document.NewSolution(false);


        }

        private void OnDisableImgClickable(object sender, EventArgs e)
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

            if (reader.ItemExists("SaveImgWithCoords"))
            {
                this.DisableClickable = reader.GetBoolean("SaveImgWithCoords");
            }
            return base.Read(reader);
        }

        //Write
        public override bool Write(GH_IWriter writer)
        {
            writer.SetDouble("scale", this.Scale);
            writer.SetBoolean("DisableClickable", this.DisableClickable);
            writer.SetBoolean("SaveImgWithCoords", this.SaveImgWithCoords);
            return base.Write(writer);  
        }

        

        private List<Color> GetColors(List<Drawing.Point> imgCoordinates, Bitmap inBitmap)
        {
            var colors = new List<Color>();
            var validCoordinates = new List<Drawing.Point>();
           
            foreach (var item in imgCoordinates)
            {
                int x = item.X;
                int y = item.Y;
                bool isValidX = x >= 0 && x <= inBitmap.Width;
                bool isValidY = y >= 0 && y <= inBitmap.Height;

                if (isValidX && isValidY)
                {
                    validCoordinates.Add(new Drawing.Point(x, y));
                    colors.Add(inBitmap.GetPixel(x, y));
                    //((ImageFromPathAttrib)m_attributes).displayCoordinates(new System.Drawing.Point(x,y));
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "{ "+item.ToString() + "} is not a valid coordinate.\nIt should be no bigger than current image's size " + inBitmap.Size);
                }
                
            }

            this.ExtrCoordinates = validCoordinates;
            return colors;

        }
        
        //Save Image to file
        public string SaveImg(string filePath, bool ifDrawCoordinates)
        {
            Bitmap bmp = new Bitmap(this.Bitmap);
            var fileName = Path.GetFileName(filePath);
            //fileName = fileName.Insert(fileName.Length - 4, "crd");
            var saveFolder = Path.GetDirectoryName(filePath) + @"\WithCrd\";

            var saveToFile = saveFolder + fileName;

            if (!this.ExtrCoordinates.IsNullOrEmpty() && ifDrawCoordinates)
            {
                foreach (var item in this.ExtrCoordinates)
                {
                    bmp.DrawCircle(item.X, item.Y, 3, Color.White);

                }
                //saveToFile = filePath.Insert(filePath.Length - 4, "d");
                
            }
            else
            {
                //return the original filepath
                return filePath;
            }


            try
            {
                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);

                //if (File.Exists(saveToFile))
                //{
                //    File.Delete(saveToFile);
                //}
                
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                bmp.Save(saveToFile);
            }
            
            return saveToFile;


        }
        
        private void UpdateColorParamValues()
        {
            //update color outputs
            this.Params.Output[1].ExpireSolution(false);
            this.Params.Output[1].ClearData();
            this.Params.Output[1].AddVolatileDataList(new GH_Path(0,0), this.ExtrColors);
            GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }
    }
}
