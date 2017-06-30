using System;
using System.Drawing;
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

//using Embryo.Generic;

namespace Ironbug
{
    //from Embryo https://github.com/johnharding/Embryo/blob/master/src/embryoA/Visulise/ImageFromPathAttrib.cs

    public class ImageFromPathAttrib : GH_ComponentAttributes
    {
        //String myPath;
        const int size = 400;
        const int offsetTop = 60;
        int sizeX,sizeY;
        float img2bitmapFactor;
        float scale = 1;
        Bitmap bitmap;
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
            Rectangle rec1 = GH_Convert.ToRectangle(Bounds);
            rec1.Y += offsetTop;
            rec1.Height = sizeY-offsetTop;
            rec1.Inflate(-2, -2);
            
            this.ImgBounds = rec1;
        }


        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);


            //if (channel == GH_CanvasChannel.Wires)
            //{
            //    base.Render(canvas, graphics, channel);
            //    Layout();
            //}

            if (channel == GH_CanvasChannel.Objects)
            {
                GH_Structure<GH_String> myData1 = (GH_Structure<GH_String>)Owner.Params.Input[0].VolatileData;
                GH_Structure<GH_Number> myData2 = (GH_Structure<GH_Number>)Owner.Params.Input[1].VolatileData;

                
                // Get the size to begin with
                Layout();

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
                var bgColor = Color.Gray;

                Pen pen = new Pen(bgColor, 3);
                SolidBrush myBrush = new SolidBrush(bgColor);

                
                Font ubuntuFont = new Font("ubuntu", 8);
                StringFormat myFormat = new StringFormat();

                if (!myData1.IsEmpty)
                {
                    
                    var filePath = myData1.get_FirstItem(true).Value;
                    if (Path.GetExtension(filePath).ToUpper() == ".HDR")
                    {
                        filePath = filePath.Replace(".HDR", ".TIF");
                    }

                    try
                    {
                        bitmap = new Bitmap(filePath);
                        
                    }
                    catch
                    {
                        Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Path must be a valid location");
                        displayComponent(graphics, myBrush, ubuntuFont, pen, myFormat);
                        return;
                    }

                    if (bitmap.Width > 0)
                    {
                        img2bitmapFactor = (float)ImgBounds.Width / (float)bitmap.Width;
                    }
                    else
                    {
                        img2bitmapFactor = 1;
                    }


                    // If we've got an image then draw it
                    //Bounds = new RectangleF(Pivot, new SizeF(myBitmap.Size.Width * scaler, myBitmap.Size.Height * scaler));

                    //viewpotHeight is for ensure the image xy ratio 
                    int viewpotHeight = (int)(bitmap.Height * img2bitmapFactor);
                    Rectangle rec0 = GH_Convert.ToRectangle(Bounds);
                    sizeX = (int)(size * scaler);
                    sizeY = (int)(viewpotHeight) + offsetTop;

                    rec0.Width = sizeX;
                    rec0.Height = sizeY;
                    Bounds = rec0;
                    

                    Rectangle rec1 = rec0;
                    rec1.Y += offsetTop;
                    rec1.Height = viewpotHeight;
                    rec1.Inflate(-2, -2);
                    ImgBounds = rec1;

                    
                    //graphics.DrawImage(myBitmap, Bounds);
                    graphics.DrawImage(bitmap, ImgBounds);

                }
                else
                {
                    displayComponent(graphics, myBrush, ubuntuFont, pen, myFormat);
                }

                myFormat.Dispose();
            }
        }

        void displayComponent(Graphics graphics, Brush myBrush, Font ubuntuFont, Pen pen, StringFormat myFormat)
        {
            //Pivot = GH_Convert.ToPoint(Pivot);
            
            //PointF imgViewBasePt = new PointF(Pivot.X, Pivot.Y + 100);
            //var imgViewBounds = new RectangleF(imgViewBasePt, new SizeF(500, 400));
            //var ImgRec = ImgBounds;
            graphics.FillRectangle(myBrush, Rectangle.Round(ImgBounds));
            //graphics.DrawRectangle(pen, Rectangle.Round(imgViewBounds));
            //graphics.DrawString("johnharding@fastmail.fm", ubuntuFont, Brushes.White, new Point((int)this.Bounds.Location.X + 12, (int)this.Bounds.Location.Y + 480 - 6 - 10), myFormat);
            //graphics.DrawImage(Owner.Icon_24x24, Bounds.Location.X + 12, Bounds.Location.Y + 450 - 10);
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
                    PointF clickedPt = PointF.Subtract(e.CanvasLocation, new SizeF(Pivot.X+2,Pivot.Y+2+offsetTop));
                    
                    PointF convertedPt = new PointF(clickedPt.X / img2bitmapFactor, clickedPt.Y / img2bitmapFactor);
                    //TODO: check 
                    var clickedColor = bitmap.GetPixel((int)convertedPt.X, (int)convertedPt.Y);
                    //MessageBox.Show("clicked at: "+ clickedPt + "; \ne.CanvasLocation: "+ e.CanvasLocation + "; \nImgBounds: " + ImgBounds.Size + "; \nPivot: " + Pivot + "; \nBounds.Location: " + Bounds.Location);
                    MessageBox.Show(clickedPt + "_" +convertedPt + "clicked at: " + clickedColor);
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

