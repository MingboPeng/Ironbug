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
        //int sizeX, sizeY;

        public ImageFromPathAttrib(View owner)
            : base(owner)
        {
            //sizeX = 500;
            //sizeY = 500;
        }

        protected override void Layout()
        {
            Pivot = GH_Convert.ToPoint(Pivot);
            Bounds = new RectangleF(Pivot, new SizeF(500, 500));
            RectangleF myRect = new RectangleF(Bounds.Location, new SizeF(100f, 60f));
            myRect.X += 16;
            myRect.Y += 4;
            LayoutInputParams(Owner, myRect);
            ImgBounds = Bounds;
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
                //GH_Structure<GH_Number> myData2 = (GH_Structure<GH_Number>)Owner.Params.Input[1].VolatileData;
                //GH_Structure<GH_Number> myData2 = new GH_Structure<GH_Number>();

                // Get the size to begin with
                Layout();

                float scaler;
                try
                {
                    //scaler = (float)myData2.get_DataItem(0).Value;
                    scaler = 1;
                    if (scaler > 10)
                    {
                        scaler = 10f;
                        Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Maximum scale is 10x. I've set your input to this");
                    }
                    if (scaler < 0)
                    {
                        scaler = 1.0f;
                        Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Scale cannot be negative. Stop messing with me!");
                    }
                }
                catch
                {
                    Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Scale must be a number. Set to 1.0");
                    scaler = 1.0f;
                }
                var bgColor = Color.Gray;

                Pen pen = new Pen(bgColor, 3);
                SolidBrush myBrush = new SolidBrush(bgColor);


                //if (Owner.RuntimeMessageLevel == GH_RuntimeMessageLevel.Blank)
                //{
                //    pen = new Pen(bgColor, 3);
                //    myBrush = new SolidBrush(bgColor);
                //}
                //else
                //{
                //    pen = new Pen(bgColor, 3);
                //    myBrush = new SolidBrush(bgColor);
                //}

                //graphics.FillEllipse(myBrush, Bounds.Location.X - 4 - 1, Bounds.Location.Y + 19 - 4, 8, 8);
                //graphics.FillEllipse(myBrush, Bounds.Location.X - 4 - 1, Bounds.Location.Y + 49 - 4, 8, 8);

                Font ubuntuFont = new Font("ubuntu", 8);
                StringFormat myFormat = new StringFormat();

                if (!myData1.IsEmpty)
                {
                    Bitmap myBitmap;
                    var filePath = myData1.get_FirstItem(true).Value;
                    if (Path.GetExtension(filePath).ToUpper() == ".HDR")
                    {
                        filePath = filePath.Replace(".HDR", ".TIF");
                    }

                    try
                    {
                        myBitmap = new Bitmap(filePath);
                    }
                    catch
                    {
                        Owner.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Path must be a valid location");
                        displayComponent(graphics, myBrush, ubuntuFont, pen, myFormat);
                        return;
                    }

                    // If we've got an image then draw it
                    Bounds = new RectangleF(Pivot, new SizeF(myBitmap.Size.Width * scaler, myBitmap.Size.Height * scaler));
                    graphics.DrawImage(myBitmap, Bounds);
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
            var imgViewBounds = new RectangleF(Pivot, new SizeF(500, 400));
            graphics.FillRectangle(myBrush, Rectangle.Round(imgViewBounds));
            graphics.DrawRectangle(pen, Rectangle.Round(imgViewBounds));
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

        private System.Drawing.RectangleF ImgBounds { get; set; }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                
                RectangleF rec = new RectangleF(Pivot, new SizeF(500, 400));
                if (rec.Contains(e.CanvasLocation))
                {
                    //this.MouseDownEvent(this);
                    MessageBox.Show("clicked at: "+ e.CanvasLocation);
                    return GH_ObjectResponse.Handled;
                }
            }
            return base.RespondToMouseDown(sender, e);
        }

    }
}

