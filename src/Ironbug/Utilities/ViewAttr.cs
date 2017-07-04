using System;
using System.Drawing;
using GH = Grasshopper;
using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Attributes;
using System.Collections.Generic;
using Grasshopper.Kernel.Data;
using FreeImageAPI;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

//using Embryo.Generic;

namespace Ironbug
{
    //from Embryo https://github.com/johnharding/Embryo/blob/master/src/embryoA/Visulise/ImageFromPathAttrib.cs

    public class ImageFromPathAttrib : GH_ComponentAttributes
    {
        //String myPath;
        const int rawSize = 300;
        const int size = rawSize + 4; // added 2 pixel for each side for boder
        const int offsetTop = 60;
        float sizeX,sizeY;
        float img2bitmapFactor;
        float scale = 1;
        string imgPath = string.Empty;
        Bitmap bitmap;
        Graphics Graphics;
        private RectangleF ImgBounds { get; set; }


        public ImageFromPathAttrib(View owner)
            : base(owner)
        {
            sizeX = size;
            sizeY = size;
            //offsetTop = 60;

            img2bitmapFactor = 1;
        }

        protected override void Layout()
        {
            //Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
            PointF BoundsLocation = GH_Convert.ToPoint(Pivot);

            //rec0.Size = new Size(sizeX, sizeY);
            //rec0.Location = BoundsLocation;
            ////Bounds.Width = sizeX;
            //Bounds = rec0;
            //Bounds.Location = BoundsLocation;

            Bounds = new RectangleF(Pivot, new SizeF(sizeX, sizeY));
            RectangleF inputRect = new RectangleF(BoundsLocation, new SizeF(100f, 40f));
            inputRect.X += 65;
            inputRect.Y += 4;

            RectangleF outRect = new RectangleF(BoundsLocation, new SizeF(100f, 40f));
            outRect.X += sizeX - 165;
            outRect.Y += 4;

            LayoutInputParams(Owner, inputRect);
            LayoutOutputParams(Owner, outRect);

            //rec1 for image bound
            RectangleF rec1 = GH_Convert.ToRectangle(Bounds);
            rec1.X += 2;
            rec1.Y += offsetTop;
            rec1.Width -= 4;
            rec1.Height = sizeY-offsetTop-2;
            //rec1.Inflate(-2, -2);
            
            this.ImgBounds = rec1;
        }

        protected override void PrepareForRender(GH_Canvas canvas)
        {
            base.PrepareForRender(canvas);

            GH_Structure<GH_String> myData1 = (GH_Structure<GH_String>)Owner.Params.Input[0].VolatileData;
            GH_Structure<GH_Number> myData2 = (GH_Structure<GH_Number>)Owner.Params.Input[1].VolatileData;

            
            float scaler;
            try
            {
                scaler = (float)myData2.get_DataItem(0).Value;
                //scaler = 1;
                if (scaler > 10)
                {

                    scaler = 10f;
                    Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 10x. I've set your input to this!");
                }
                else if (scaler < 0.5)
                {
                    scaler = 0.5f;
                    Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 0.5x. I've set your input to this!");
                }
            }
            catch
            {
                Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Scale must be a number. Set to 1.0");
                scaler = 1.0f;
            }

            scale = scaler;



            if (myData1.Any())
            {

                imgPath = myData1.get_FirstItem(true).Value;
                if (Path.GetExtension(imgPath).ToUpper() == ".HDR")
                {
                    imgPath = imgPath.Replace(".HDR", ".TIF");
                }
                
            }

        }
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            Graphics = graphics;
            //if (channel == GH_CanvasChannel.Wires)
            //{
            //    base.Render(canvas, graphics, channel);
            //    Layout();
            //}

            //if (channel == GH_CanvasChannel.Last)
            //{
                
            //    Bounds = new RectangleF(Pivot, new SizeF(sizeX, sizeY));
            //    Layout();
            //}

            if (channel == GH_CanvasChannel.Objects)
            {
                // Get the size to begin with
                Layout();

                if (!string.IsNullOrEmpty(imgPath))
                {
                    
                    displayImg(imgPath);
                    
                }
                else
                {
                    displayComponent();
                }

                //this.ExpireLayout();
                
            }
        }

        //private void drawClickPt( Rectangle rect)
        //{
        //    Graphics.FillEllipse(Brushes.Black, rect);
        //    //Graphics.FillRectangle(myBrush, Rectangle.Round(ImgBounds));
        //}

        public void displayImg(string filePath)
        {
            

            try
            {
                bitmap = new Bitmap(filePath);

            }
            catch
            {
                Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Path must be a valid location");
                displayComponent();
                return;
            }

            //calculate the scale factor
            if (bitmap.Width > 0)
            {
                img2bitmapFactor = (float)ImgBounds.Width / (float)bitmap.Width;
            }
            else
            {
                img2bitmapFactor = 1;
            }


            // If we've got an image then draw it

            //viewpotHeight is for ensure the image xy ratio 
            float viewpotHeight = bitmap.Height * img2bitmapFactor;
            RectangleF rec0 = GH_Convert.ToRectangle(Bounds);
            sizeX = (rawSize * scale) + 4;
            sizeY = viewpotHeight + offsetTop + 2;

            rec0.Width = sizeX;
            rec0.Height = sizeY;
            Bounds = rec0;


            RectangleF rec1 = rec0;
            rec1.X += 2;
            rec1.Y += offsetTop;
            rec1.Width -= 4;
            rec1.Height = viewpotHeight;
            //rec1.Inflate(-2, -2);
            ImgBounds = rec1;


            //graphics.DrawImage(myBitmap, Bounds);
            Graphics.DrawImage(bitmap, ImgBounds);
            
        }

        void displayComponent()
        {

            var bgColor = Color.Gray;

            Pen pen = new Pen(bgColor, 3);
            SolidBrush myBrush = new SolidBrush(bgColor);


            Font ubuntuFont = new Font("ubuntu", 8);
            StringFormat myFormat = new StringFormat();

            //Pivot = GH_Convert.ToPoint(Pivot);

            //PointF imgViewBasePt = new PointF(Pivot.X, Pivot.Y + 100);
            //var imgViewBounds = new RectangleF(imgViewBasePt, new SizeF(500, 400));
            //var ImgRec = ImgBounds;
            Graphics.FillRectangle(myBrush, Rectangle.Round(ImgBounds));
            //graphics.DrawRectangle(pen, Rectangle.Round(imgViewBounds));
            Graphics.DrawString("Please use a valid image file path;\nHDR, TIF, PNG, GIF, or JPG formats", ubuntuFont, Brushes.White, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 380 - 6 - 10), myFormat);
            Graphics.DrawImage(Owner.Icon_24x24, Bounds.Location.X + 12, Bounds.Location.Y + 350 - 10);

            myBrush.Dispose();
            myFormat.Dispose();

        }

        public delegate void Button_Handler(object sender);

        private Button_Handler MouseDownEvent;
        public event Button_Handler mouseDownEvent
        {
            add
            {
                Button_Handler buttonHandler = MouseDownEvent;
                Button_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseDownEvent, (Button_Handler)Delegate.Combine(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
            remove
            {
                Button_Handler buttonHandler = MouseDownEvent;
                Button_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseDownEvent, (Button_Handler)Delegate.Remove(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
        }
        

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
               
                if (ImgBounds.Contains(e.CanvasLocation) && bitmap !=null)
                {
                    
                    //this.MouseDownEvnt(this);                    //SizeF 
                    //PointF clickedPt = PointF.Subtract(e.CanvasLocation, new SizeF(Pivot.X,Pivot.Y+offsetTop));
                    PointF clickedPt = PointF.Subtract(e.CanvasLocation,new SizeF(ImgBounds.X,ImgBounds.Y));
                    
                    //convert current pt location on grasshopper view back to original image size system
                    Point PixelPtOnOriginalBitmap = Point.Round(new PointF(clickedPt.X / img2bitmapFactor, clickedPt.Y / img2bitmapFactor));
                    //TODO: check 
                    var clickedColor = bitmap.GetPixel(PixelPtOnOriginalBitmap.X, PixelPtOnOriginalBitmap.Y);
                    //MessageBox.Show("clicked at: " + clickedPt + "; \ne.CanvasLocation: " + e.CanvasLocation + "; \nImgBounds: " + ImgBounds + "; \nPivot: " + Pivot + "; \nBounds: " + Bounds);
                    //MessageBox.Show("Bitmap:"+bitmap.Size);
                    //MessageBox.Show("img2bitmapFactor:"+ img2bitmapFactor + "; \nImgBounds: " + ImgBounds);
                    this.Owner.Message = clickedColor.ToString();
                    //var colors = new List<byte>() { clickedColor.A, clickedColor.R };


                    //var ptRect = new Rectangle(Point.Round(clickedPt), new Size(2, 2));
                    //Graphics.FillEllipse(Brushes.Black, ptRect);
                    //drawClickPt(ptRect);

                    var currentDataCount = this.Owner.Params.Output[1].VolatileDataCount;

                    this.Owner.Params.Output[1].ExpireSolution(false);
                    this.Owner.Params.Output[1].AddVolatileData(new GH_Path(0), currentDataCount, clickedColor);
                    GH.Instances.ActiveCanvas.Document.NewSolution(false);

                    
                    //MessageBox.Show(clickedPt + "_" +convertedPt + "clicked at: " + clickedColor);
                    return GH_ObjectResponse.Handled;
                }
                else
                {

                    //MessageBox.Show("ImgBounds:" + ImgBounds + "\nclicked at: " + e.CanvasLocation + "\n" + Pivot + "\n" + ImgBounds.Contains(e.CanvasLocation));
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

    }
}

