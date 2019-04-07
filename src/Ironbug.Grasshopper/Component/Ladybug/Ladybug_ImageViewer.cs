using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Drawing = System.Drawing;

using GH = Grasshopper;
using Rhino.Geometry;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Special;
using GH_IO.Serialization;
using Ironbug.Grasshopper.Properties;

using Ironbug.Core;

namespace Ironbug.Grasshopper
{
    public class Ladybug_ImageViewer : GH_Component
    {
        
        public List<string> FilePaths = new List<string>();
        public List<Bitmap> Bitmaps = new List<Bitmap>();
        public int currentBitmapIndex = 0;

        public Bitmap Bitmap;
        
        public double Scale = 1;
        public List<Drawing.Point> ExtractedCoordinates = new List<Drawing.Point>();
        private List<Drawing.Point> TempExtrCoordinates = new List<Drawing.Point>();
        private List<Drawing.Point> TempExtrCoordinates2 = new List<Drawing.Point>();
        public List<List<Color>> ExtractedColors = new List<List<Color>>();

        private List<List<double>> PValues = new List<List<double>>();
        public List<List<double>> ExtractedPValues = new List<List<double>>();

        public bool DisableClickable = true;
        public bool ifSaveThis = false;
        public bool ifSaveAll = false;
        public bool IsImgList = false;
        public bool IsGetPValue = false;

        private bool isScaleChanged = false;
        private int GifFrameDuration = 100;
        //private string RADPath = @"C:\Radiance\bin";
        //public string newFilePath = string.Empty;
        
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        
        public Ladybug_ImageViewer()
          : base("Ladybug_ImageViewer", "ImageViewer",
              "Preview image files\n\nPlease find the source code from:\nhttps://github.com/MingboPeng/Ironbug/src/Ironbug.Grasshopper",
              "Ladybug", "5 | Extra")
        {
            this.Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }


        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("imagePath_", "imagePath_", "one or a list of image file path.", GH_ParamAccess.list);
            pManager.AddPointParameter("coordinates_", "coordinates_", "A list of points for extracting colors from the source image.", GH_ParamAccess.list);
            pManager.AddNumberParameter("_scale_", "_scale_", "Set this image view port scale.", GH_ParamAccess.item, 1);
            //pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[2].Optional = true;

            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        
        
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("imagePath", "imagePath", "A new image marked with coordinates.", GH_ParamAccess.list);
            pManager.AddTextParameter("values", "values", "Color information or Radiance value that extracted from the input image.\nRadiance value only available on HDR image, and the unit is based on the Radiance study type.\n -Illuminance: lux\n -Luminance: cd/m2", GH_ParamAccess.list);
            pManager.AddTextParameter("GIF", "GIF", "Generates an animated gif image when there is a list of images.", GH_ParamAccess.item);

            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }
        

        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();

            FilePaths = new List<string>();
            this.Bitmaps = new List<Bitmap>();
            this.Bitmap = null;
            this.Message = null;
            this.currentBitmapIndex = 0;

            this.TempExtrCoordinates2 = new List<Drawing.Point>(this.ExtractedCoordinates);
            this.ExtractedCoordinates = TempExtrCoordinates;
            this.TempExtrCoordinates = new List<Drawing.Point>();
            
            this.ExtractedColors = new List<List<Color>>() { new List<Color>() };

            this.PValues = new List<List<double>> { new List<double>() };
            this.ExtractedPValues = new List<List<double>>() { new List<double>() };

            ////Check the Radiance folder
            //var radPath = GetRADPath();
            //if (!string.IsNullOrWhiteSpace(radPath))
            //{
            //    this.RADPath = radPath;
            //}

            //Remove old ImageViewer
            //var defaultFd = GH.Folders.DefaultAssemblyFolder;
            //var oldV = Path.Combine(defaultFd, "Ladybug_ImageViewer.gha");
            //var foundOld = Directory.GetFiles(defaultFd, "Ladybug_ImageViewer.gha");
            //if (File.Exists(oldV))
            //{
            //    File.Delete(oldV);
            //    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "The old ImageViewer has been removed, restart the Grasshopper will fix two ImageViewer issue!");
            //}

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
            if (this.Params.Input[2].SourceCount>0)
            {
                this.ExtractedCoordinates = this.TempExtrCoordinates2;
                this.TempExtrCoordinates2 = new List<Drawing.Point>();

                if (this.ifSaveAll)
                {
                    this.ExtractedColors = GetColors(this.ExtractedCoordinates, this.Bitmaps);
                }
                else
                {
                    this.ExtractedColors[0] = GetColors(this.ExtractedCoordinates, this.Bitmap);
                }
                
                this.UpdateValueOutputData();
                GH.Instances.ActiveCanvas.Document.NewSolution(false);
            }
            else
            {
                sender.SolutionExpired -= this.OnScale_SolutionExpired;
            }
            
        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (e.ParameterSide == GH_ParameterSide.Output) return;

            if (e.ParameterIndex == 2)
            {
                this.isScaleChanged = true;
                this.TempExtrCoordinates = new List<Drawing.Point>(this.ExtractedCoordinates);
                if (this.Params.Input[2].SourceCount>0)
                {
                    this.Params.Input[2].Sources[0].SolutionExpired -= OnScale_SolutionExpired;
                    this.Params.Input[2].Sources[0].SolutionExpired += OnScale_SolutionExpired;
                }

            }
            
        }

        
        
        
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var filePaths = new List<string>();
            double scale = 1;

            if (!DA.GetDataList(0, filePaths)) return;

            if (!DA.GetData(2, ref scale))
            {
                return;
            }
            else
            {
                this.Scale = CheckScale(scale);
            }

            this.Bitmaps = ProcessImgs(filePaths, ref this.FilePaths);
            if (Bitmaps.IsNullOrEmpty())
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No valid images");
                return;
            }

            //this will disable the animate GIF
            this.IsImgList = this.FilePaths.Count > 1;
            
            //current img to be shown
            this.Bitmap = this.Bitmaps[this.currentBitmapIndex];
            
            //string filePath = this.FilePaths[this.currentBitmapIndex];

            //get colors from input coordinates
            var imgCoordinates = new List<Point3d>();
            if (DA.GetDataList(1, imgCoordinates))
            {
                if (!imgCoordinates.IsNullOrEmpty() && !isScaleChanged)
                {
                    this.ExtractedCoordinates = imgCoordinates.Select(_ => new Drawing.Point((int)_.X, (int)_.Y)).ToList();
                }
                else
                {
                    this.isScaleChanged = false;
                }
                
            }
            

            var imgs = new List<string>();

            if (this.ifSaveAll)
            {
                this.ExtractedColors = GetColors(this.ExtractedCoordinates, this.Bitmaps);
                imgs = SaveImgs(this.FilePaths);

                
            }
            else if (this.ifSaveThis)
            {
                this.ExtractedColors[0] = GetColors(this.ExtractedCoordinates, this.Bitmap);
                imgs.Add(SaveImg(this.FilePaths, this.currentBitmapIndex));
            }
            else // just show
            {
                this.ExtractedColors[0] = GetColors(this.ExtractedCoordinates, this.Bitmap);
                imgs.Add(this.FilePaths[this.currentBitmapIndex]);
            }

            if (this.IsGetPValue)
            {
                var pValues = this.GetPValues(FilePaths[this.currentBitmapIndex],this.ExtractedCoordinates);
                this.ExtractedPValues[0] = pValues.ToList();
            }

            DA.SetDataList(0, imgs);

            DA.SetDataTree(1, FormatColorOrPValueDataTree());
            
        }

        private List<Bitmap> ProcessImgs(List<string> filePaths, ref List<string> newFilePaths)
        {
            var postFilePaths = filePaths.Select(_ => CheckImg(_)).ToList();
            //postFilePaths.RemoveAll( _=>_ == null);
            var bitmaps = new List<Bitmap>();

            if (postFilePaths.IsNullOrEmpty())
            {
                return bitmaps;
            }
            for (int i = 0; i < postFilePaths.Count; i++)
            {
                if (postFilePaths[i]!=null && File.Exists( postFilePaths[i]))
                {
                    newFilePaths.Add(filePaths[i]);
                    using (var bitMap = new Bitmap(postFilePaths[i]))
                    {
                        bitmaps.Add(new Bitmap(bitMap));
                    }
                }
                
            }
            
            
            return bitmaps;
        }
        

        private string CheckImg(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input image path is not valid!");
                return null;
            }

            string tiffFile = string.Empty;


            var folder = Path.GetDirectoryName(filePath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var fileExtension = Path.GetExtension(filePath).ToUpper();
            var tempPath = Path.GetTempPath() + @"\Ladybug\ImageViewer";

            Directory.CreateDirectory(tempPath);

            var isHDR = fileExtension == ".HDR";
            var isPNG = fileExtension == ".PNG";
            var isJPG = fileExtension == ".JPG";
            var isGIF = fileExtension == ".GIF";
            var isTIF = fileExtension == ".TIF" || fileExtension == ".TIFF";

            //convert HDR
            if (isHDR)
            {
                tiffFile = tempPath + "\\" + fileName + "LB.TIF";
                var isNewHDR = true;

                if (File.Exists(tiffFile))
                {
                    //check image size
                    var hdrSize = new FileInfo(filePath).Length;
                    if (hdrSize <=1)
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input image is not a valid image!");
                        return null;
                    }
                    
                    var hdrTimeStamp = File.GetLastWriteTime(filePath);
                    var tifTimeStamp = File.GetLastWriteTime(tiffFile);

                    //if ==1: hdrTimeStamp is later than tifTimeStamp
                    //isNewHDR = true, to convert to a new tiff
                    isNewHDR = DateTime.Compare(hdrTimeStamp, tifTimeStamp) == 1;
                }

                if (isNewHDR)
                {
                    var radTiff = new Honeybee.Radiance.Command.RaTiff_Legacy(filePath, tiffFile);
                    radTiff.Execute();
                    if (!File.Exists(tiffFile))
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Cannot read this HDR file, please check if you have Radiance installed!\n\rYou might also want to use 'Honeybee_Convert HDR to TIF' component to convert HDR image first!");
                        return null;
                    }

                }

            }
            else if (isJPG || isPNG || isGIF || isTIF)
            {
                tiffFile = tempPath + "\\" + fileName + fileExtension;
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
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input image doesn't exist or is not supported format!");
                return null;
            }

            return tiffFile;
        }




        //private void ConvertImgs(List<string> allImgs)
        //{
        //    var cmdStrings = new List<string>();
        //    cmdStrings.Add(@"SET RAYPATH=.;C:\Radiance\lib&PATH=C:\Radiance\bin;$PATH");

        //    foreach (var item in allImgs)
        //    {
        //        var fileExtension = Path.GetExtension(item);
        //        var isHDR = fileExtension.ToUpper() == ".HDR";

        //        if (isHDR)
        //        {
        //            var tempPath = Path.GetTempPath() + @"\Ladybug\ImageViewer";
        //            Directory.CreateDirectory(tempPath);

        //            var fileName = Path.GetFileNameWithoutExtension(item);
        //            var tiffFile = tempPath + "\\" + fileName + ".TIF";
        //            string cmdStr1 = @"ra_tiff " + item + " " + tiffFile;
        //            cmdStrings.Add(cmdStr1);
        //        }

        //    }

        //    if (cmdStrings.Count>1)
        //    {
        //        CMD.HDR2TIF(cmdStrings);
        //    }

        //}

        private double CheckScale(double scale)
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


        
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return Resources.Ladybug_Viewer_24;
                //return null;
            }
        }

        
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        
        public override Guid ComponentGuid
        {
            get { return new Guid("949F869E-E51E-4776-BF60-1C54760C453F"); }
        }



        public override void CreateAttributes()
        {
            var newAttri = new ImageFromPathAttrib(this);
            newAttri.mouseImgClickEvent += OnMouseImgClickEvent;
            newAttri.mouseNavClickEvent += OnMouseNavClickEvent;
            m_attributes = newAttri;
            
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            //base.AppendAdditionalComponentMenuItems(menu);
            var newMenu = menu;
            newMenu.Items.RemoveAt(1); // remove Preview
            newMenu.Items.RemoveAt(2); // remove Bake

            Menu_AppendItem(newMenu, "Clear clicked pixel coordinates", OnClearValues);
            Menu_AppendItem(newMenu, "Get Radiance values (only works with HDR image)", OnGetPValues, true, this.IsGetPValue);
            Menu_AppendSeparator(newMenu);
            Menu_AppendItem(newMenu, "Extract all pixel coordinates",OnExtractPtToGhPoints);
            Menu_AppendItem(newMenu, "Disable click-able image", OnDisableImgClickable, true, this.DisableClickable);
            Menu_AppendSeparator(newMenu);
            Menu_AppendItem(newMenu, "Save this image with marked coordinates", OnSaveThisWithCoords, true, this.ifSaveThis);
            Menu_AppendItem(newMenu, "Save all images with marked coordinates", OnSaveAllImgsWithCoords, this.IsImgList, this.ifSaveAll);
            Menu_AppendSeparator(newMenu);
            Menu_AppendItem(newMenu, "Export animated GIF",OnExportGif,this.IsImgList);

            var menuItemScale = Menu_AppendItem(menu, "Animation frame duration (ms)");
            Menu_AppendTextItem(menuItemScale.DropDown, this.GifFrameDuration.ToString(), OnKeydownEventHandler_GifFrameDuration, OnTextChanged_Scale, true);
            //Menu_AppendDigitScrollerItem(menuItemScale.DropDown, 1, 5000, 100, 0);
        }

        private void OnTextChanged_Scale(GH_MenuTextBox sender, string text)
        {
            //throw new NotImplementedException();
        }

        private void OnKeydownEventHandler_GifFrameDuration(GH_MenuTextBox sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter){
                
                int.TryParse(sender.Text, out this.GifFrameDuration);
                
            }
            
        }

        #region Events

        private void OnExportGif(object sender, EventArgs e)
        {
            //var path = @"C:\Colibri\WithCrd\aa.gif";

            var saveToGif = Path.GetDirectoryName(this.FilePaths[0]) + "\\Exported_"+ this.GifFrameDuration + ".gif";
            this.Bitmaps.SaveAnimatedGifImage(saveToGif,this.GifFrameDuration);

            this.Params.Output[2].ExpireSolution(false);
            this.Params.Output[2].ClearData();
            this.Params.Output[2].AddVolatileData(new GH_Path(0, 0),0, saveToGif);
            GH.Instances.ActiveCanvas.Document.NewSolution(false);

        }

        private void OnClearValues(object sender, EventArgs e)
        {

            var takeCount = this.Params.Input[1].VolatileDataCount;
            this.ExtractedCoordinates = this.ExtractedCoordinates.Take(takeCount).ToList();
            
            this.ExtractedColors = this.ExtractedColors.Select(
                    _ => _.Take(takeCount).ToList()
                ).ToList();
                

            this.UpdateImgs();

            this.Message = null;
            this.Attributes.ExpireLayout();
            this.UpdateValueOutputData();
            GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }

        private void OnGetPValues(object sender, EventArgs e)
        {
            this.IsGetPValue = !this.IsGetPValue;
            var currentFile = this.FilePaths[this.currentBitmapIndex];

            this.GetPValues(currentFile);
            this.OnClearValues(sender, new EventArgs());

        }

        private IEnumerable<double> GetPValues(string HDRImg)
        {
            
            var isHDR = Path.GetExtension(HDRImg).ToUpper() == ".HDR";
            if (!isHDR)
            {
                new Exception("Input image is not Radiance HDR!");
            }
            var pvalue = new Honeybee.Radiance.Command.PValue_Legacy(HDRImg);
            var outputs = pvalue.Execute();

            //TODO: this is not the right way to do...
            this.PValues[this.currentBitmapIndex] = outputs.ToList();
            return outputs;
        }

        private IEnumerable<double> GetPValues(string HDRImg, List<Drawing.Point> ExtractedCoordinates)
        {
            this.GetPValues(HDRImg);
            var pValues = new List<double>();

            foreach (var point in ExtractedCoordinates)
            {
                var validPoint = this.ValidateCoordinate(point, this.Bitmap);
                var pValue = this.GetPValueFromXY(validPoint, this.PValues[this.currentBitmapIndex]);
                pValues.Add(pValue);
                
            }

            return pValues;

        }

        private double GetPValueFromXY(Drawing.Point point, List<double> RadValues)
        {
            var radValue = 0.0;

            var x0 = Bitmap.Width;
            var y0 = Bitmap.Height;
            var x1 = point.X;
            var y1 = point.Y;
            
            var ValueIndex = (y1 - 1) * x0 + x1;
            var rgb_avg = RadValues[ValueIndex];
            radValue = rgb_avg * 179;

            return radValue;
        }


        private void OnSaveThisWithCoords(object sender, EventArgs e)
        {

            this.ifSaveThis = !this.ifSaveThis;
            var imgs = new List<string>();

            if (this.ifSaveThis)
            {
                this.ifSaveAll = false;
                imgs.Add( SaveImg(this.FilePaths, this.currentBitmapIndex) );
                

            }
            else //just show
            {
                imgs.Add(this.FilePaths[this.currentBitmapIndex]);
            }
            

            this.Params.Output[0].ExpireSolution(false);
            this.Params.Output[0].ClearData();
            this.Params.Output[0].AddVolatileDataList(new GH_Path(0, 0), imgs);

            this.ExtractedColors = new List<List<Color>>() { GetColors(this.ExtractedCoordinates, this.Bitmap) };
            this.UpdateValueOutputData();
            GH.Instances.ActiveCanvas.Document.NewSolution(false);


        }

        private void OnSaveAllImgsWithCoords(object sender, EventArgs e)
        {
            this.ifSaveAll = !this.ifSaveAll;
            var imgs = new List<string>();
            if (this.ifSaveAll)
            {
                this.ifSaveThis = false;

                imgs = SaveImgs(this.FilePaths);
                this.ExtractedColors = GetColors(this.ExtractedCoordinates, this.Bitmaps);
                
            }
            else //just show
            {
                imgs.Add(this.FilePaths[this.currentBitmapIndex]);
                this.ExtractedColors = new List<List<Color>>() { GetColors(this.ExtractedCoordinates, this.Bitmap) };
            }

            this.Params.Output[0].ExpireSolution(false);
            this.Params.Output[0].AddVolatileDataList(new GH_Path(0, 0), imgs);

            this.UpdateValueOutputData();
            GH.Instances.ActiveCanvas.Document.NewSolution(false);

        }

        private void OnExtractPtToGhPoints(object sender, EventArgs e)
        {
            var GHPoints = new List<GH_Point>();
            foreach (var item in this.ExtractedCoordinates)
            {
                var pt = new Point3d(item.X, item.Y, 0);
                GHPoints.Add(new GH_Point(pt));
            }
            var thisParamAttr = this.Params.Input[1].Attributes;
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

        #endregion


        #region Interaction Events

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
                    currentBitmapIndex = this.Bitmaps.Count - 1;

                }
            }
            else
            {
                if (currentBitmapIndex < this.Bitmaps.Count - 1)
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

            
            if (!this.ifSaveAll)
            {
                this.ExtractedColors = new List<List<Color>>
                {
                    GetColors(this.ExtractedCoordinates, this.Bitmap)
                };
            }
            
            

            this.UpdateValueOutputData();
            this.UpdateImgs();
            GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }

        private void OnMouseImgClickEvent(object sender, Drawing.Point clickedPtOnOriginalBitmap)
        {
            var clickedColor = new Color();
            var msg = string.Empty;
            this.ExtractedCoordinates.Add(clickedPtOnOriginalBitmap);

            if (this.ifSaveAll)
            {
                this.ExtractedColors = GetColors(this.ExtractedCoordinates, this.Bitmaps);
            }
            else
            {
                if (IsGetPValue)
                {
                    var clickedPValue =  this.GetPValueFromXY( clickedPtOnOriginalBitmap,this.PValues[this.currentBitmapIndex]);
                    this.ExtractedPValues[0].Add(clickedPValue);
                    msg = "Clicked at: " + clickedPtOnOriginalBitmap + "\nRadiance Value: " + clickedPValue.ToString();
                }
                else
                {
                    clickedColor = this.Bitmap.GetPixel(clickedPtOnOriginalBitmap.X, clickedPtOnOriginalBitmap.Y);
                    this.ExtractedColors[0].Add(clickedColor);
                    msg = "Clicked at: " + clickedPtOnOriginalBitmap + "\n" + clickedColor.ToString();
                }
                
            }

            this.Message = msg;

            this.UpdateImgs();

            this.UpdateValueOutputData();
            GH.Instances.ActiveCanvas.Document.NewSolution(false);
        }
        #endregion

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
            writer.SetBoolean("SaveImgWithCoords", this.ifSaveThis);
            return base.Write(writer);  
        }

        

        private List<Color> GetColors(List<Drawing.Point> imgCoordinates, Bitmap inBitmap)
        {
            var colors = new List<Color>();
            var validCoordinates = new List<Drawing.Point>();

            var bitmap = inBitmap;

            foreach (var item in imgCoordinates)
            {
                var validCoordinate = ValidateCoordinate(item, inBitmap);
                validCoordinates.Add(validCoordinate);
                colors.Add(bitmap.GetPixel(validCoordinate.X, validCoordinate.Y));
            }
            

            this.ExtractedCoordinates = validCoordinates;
            return colors;

        }

        private Drawing.Point ValidateCoordinate(Drawing.Point imgCoordinate, Bitmap inBitmap)
        {
            var validCoordinate = new Drawing.Point();
            var bitmap = inBitmap;
            
            int x = imgCoordinate.X;
            int y = imgCoordinate.Y;
            bool isValidX = x >= 0 && x <= bitmap.Width;
            bool isValidY = y >= 0 && y <= bitmap.Height;

            if (isValidX && isValidY)
            {
                validCoordinate = imgCoordinate;
            }
            else
            {
                new Exception("{ " + validCoordinate.ToString() + "} is not a valid coordinate.\nIt should be no bigger than current image's size " + bitmap.Size);
            }

            
            return validCoordinate;
        }

        private List<List<Color>> GetColors(List<Drawing.Point> imgCoordinates, List<Bitmap> inBitmap)
        {
            var allColors = new List<List<Color>>();
            foreach (var item in inBitmap)
            {
                var colors = GetColors(imgCoordinates, item);
                allColors.Add(colors);
            }
            return allColors;
            
        }

        //Save Image to file
        public string SaveImg(List<string> saveToFiles, int currentIndex)
        {
            var bitmap = this.Bitmaps[currentIndex];

            if (bitmap == null)
            {
                return null;
            }

            var saveTofilePath = saveToFiles[currentIndex];

            var fileName = Path.GetFileNameWithoutExtension(saveTofilePath)+".TIF";
            var saveFolder = Path.GetDirectoryName(saveTofilePath) + @"\WithCrd\";
            var saveToFile = saveFolder + fileName;

            using (Bitmap bmp = new Bitmap(bitmap))
            {
                
                if (!this.ExtractedCoordinates.IsNullOrEmpty())
                {
                    foreach (var item in this.ExtractedCoordinates)
                    {
                        bmp.DrawCircle(item.X, item.Y, 3, Color.White);

                    }
                    
                }
                else
                {
                    //return the original filepath
                    return saveTofilePath;
                }


                try
                {
                    if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
                    bmp.Save(saveToFile);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }

            return saveToFile;

        }
        
        public List<string> SaveImgs(List<string>saveToFiles)
        {
            var newFiles = new List<string>();
            for (int i = 0; i < saveToFiles.Count; i++)
            {
                var newFile = SaveImg(saveToFiles,i);
                if (File.Exists(newFile))
                {
                    newFiles.Add(newFile);
                }
            }


            return newFiles;
        }

        public void UpdateImgs()
        {
            var imgs = new List<string>();
            if (this.ifSaveAll)
            {
                imgs = SaveImgs(this.FilePaths);
            }
            else if (this.ifSaveThis)
            {
                var savedFile = SaveImg(this.FilePaths, this.currentBitmapIndex);
                imgs.Add(savedFile);
            }
            else // just show, do nothing
            {
                imgs.Add(this.FilePaths[this.currentBitmapIndex]);
            }

            this.Params.Output[0].ExpireSolution(false);
            this.Params.Output[0].AddVolatileDataList(new GH_Path(0, 0), imgs);
        }

        private void UpdateValueOutputData()
        {
            GH_Structure<GH_String> outputDataTree = this.FormatColorOrPValueDataTree();
            
            //update color outputs
            this.Params.Output[1].ExpireSolution(false);
            this.Params.Output[1].ClearData();
            this.Params.Output[1].AddVolatileDataTree(outputDataTree);
            
            //this.Params.Output[1].AddVolatileDataList(new GH_Path(0, 1), this.ExtrColors);
            
        }

        private GH_Structure<GH_String> FormatColorOrPValueDataTree()
        {
            
            GH_Structure<GH_String> outputDataTree = new GH_Structure<GH_String>();

            if (this.IsGetPValue)
            {
                List<List<double>> allPValues = this.ExtractedPValues;
                for (int i = 0; i < allPValues.Count; i++)
                {
                    var ghColors = allPValues[i].Select(_ => new GH_String(_.ToString()));
                    outputDataTree.AppendRange(ghColors, new GH_Path(i));
                }
            }
            else
            {
                List<List<Color>> allColors = this.ExtractedColors;
                for (int i = 0; i < allColors.Count; i++)
                {
                    var ghColors = allColors[i].Select(_ => new GH_String(_.ToString()));
                    outputDataTree.AppendRange(ghColors, new GH_Path(i));
                }
            }

            

            return outputDataTree;
        }
    }
}
