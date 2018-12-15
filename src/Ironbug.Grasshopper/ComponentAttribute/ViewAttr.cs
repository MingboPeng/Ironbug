using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

//using Embryo.Generic;

namespace Ironbug.Grasshopper
{
    //Reference Embryo https://github.com/johnharding/Embryo/blob/master/src/embryoA/Visulise/ImageFromPathAttrib.cs

    public class ImageFromPathAttrib : GH_ComponentAttributes
    {
        //String myPath;
        private const int rawSize = 320;

        private const int TopOffset = 60;
        private const int NavButtonSize = 20;
        private int BtmOffset = 0;

        //float XYRatio =1;
        private double scale;

        //public string imgPath = string.Empty;
        private List<Point> coordinates = new List<Point>();

        //List<string> currentValues = new List<string>();
        private Bitmap imgBitmap;

        private Graphics MyGraphics;
        private Ladybug_ImageViewer ViewOwner;

        public ImageFromPathAttrib(Ladybug_ImageViewer owner)
            : base(owner)
        {
            this.imgBitmap = owner.Bitmap;
            this.scale = owner.Scale;
            this.ViewOwner = (Ladybug_ImageViewer)this.Owner;
        }

        protected override void Layout()
        {

            this.scale = this.ViewOwner.Scale;
            this.imgBitmap = this.ViewOwner.Bitmap;
            this.coordinates = this.ViewOwner.ExtractedCoordinates;

            if (this.ViewOwner.Bitmaps.Count > 1)
            {
                this.BtmOffset = 30;
            }
            else
            {
                this.BtmOffset = 0;
            }

            if (this.imgBitmap == null)
            {
                this.Bounds = GetBounds(this.Pivot, new SizeF(rawSize, rawSize - TopOffset), TopOffset, BtmOffset, 1);
            }
            else
            {
                ////dynamic size
                //this.Bounds = getBounds(this.Pivot, this.imgBitmap.Size, offsetTop, scale);

                //Fixed size
                //double XYRatio = this.imgBitmap.Width / this.imgBitmap.Height;
                var size = new SizeF(rawSize, (float)((double)rawSize / (double)this.imgBitmap.Width * (double)this.imgBitmap.Height));
                this.Bounds = GetBounds(this.Pivot, size, TopOffset, BtmOffset, scale);
            }

            //locate the inputs outputs
            RectangleF inputRect = new RectangleF(Pivot, new SizeF(10f, 54f));
            inputRect.X += Owner.Params.InputWidth;

            RectangleF outRect = new RectangleF(Pivot, new SizeF(10f, 54f));
            outRect.X += Bounds.Width - Owner.Params.OutputWidth - 10;
            
            LayoutInputParams(Owner, inputRect);
            LayoutOutputParams(Owner, outRect);
        }

        private RectangleF GetBounds(PointF location, SizeF imgSizeXY, int topOffset, int btmOffset, double scale)
        {
            RectangleF rec = new RectangleF();
            rec.Location = location;
            rec.Width = imgSizeXY.Width * (float)scale;
            rec.Height = imgSizeXY.Height * (float)scale + topOffset + btmOffset;
            rec.Inflate(2f, 2f);

            return (RectangleF)GH_Convert.ToRectangle(rec);
        }

        private RectangleF GetImgBounds(RectangleF bounds, int topOffset, int btmOffset)
        {
            RectangleF rec = bounds;
            rec.Y += topOffset;
            rec.Height = rec.Height - topOffset - btmOffset;

            rec.Inflate(-2f, -2f);
            return rec;
        }

        private RectangleF GetNavBounds(RectangleF imgBound, int btmOffset, int size, bool rightSide)
        {
            RectangleF rec = imgBound;

            if (rightSide)
            {
                rec.X += (rec.Width - size - 10);
            }
            else
            {
                rec.X += 10;
            }

            rec.Y += (rec.Height + (btmOffset - size) / 2);
            rec.Width = size;
            rec.Height = size;

            return rec;
        }

        //protected override void PrepareForRender(GH_Canvas canvas)
        //{
        //    base.PrepareForRender(canvas);

        //    this.ExpireLayout();

        //}
        protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
        {
            base.Render(canvas, graphics, channel);

            MyGraphics = graphics;

            if (channel == GH_CanvasChannel.Objects)
            {
                // Get the size to begin with

                if (this.imgBitmap != null)
                {
                    DisplayImg(this.imgBitmap);
                    DisplayNav();
                }
                else
                {
                    DisplayDefaultComponent();
                }
            }
        }

        private void DisplayNav()
        {
            if (this.BtmOffset == 0) return;

            RectangleF recImg = GetImgBounds(this.Bounds, TopOffset, BtmOffset);
            Pen pen = new Pen(new SolidBrush(Color.White));
            //graphics.FillEllipse(myBrush, relativePt.X, relativePt.Y, dotSize, dotSize);
            RectangleF recNavLeft = GetNavBounds(recImg, BtmOffset, NavButtonSize, false);
            RectangleF recNavRight = GetNavBounds(recImg, BtmOffset, NavButtonSize, true);

            MyGraphics.DrawEllipse(pen, recNavLeft);
            MyGraphics.DrawEllipse(pen, recNavRight);

            MyGraphics.DrawString("◄", new Font("Arial", 10), new SolidBrush(Color.White), recNavLeft.X, recNavLeft.Y + 3);
            MyGraphics.DrawString("►", new Font("Arial", 10), new SolidBrush(Color.White), recNavRight.X + 3, recNavRight.Y + 3);
        }

        private void DisplayImg(Bitmap inBitmap)
        {
            RectangleF rec = GetImgBounds(this.Bounds, TopOffset, BtmOffset);

            MyGraphics.DrawImage(imgBitmap, rec);

            //draw pixel coordinates
            if (this.coordinates.Count > 0)
            {
                DisplayCoordinates(this.coordinates, this.MyGraphics);
            }
        }

        private void DisplayCoordinates(List<Point> coordinates, Graphics graphics)
        {
            int dotSize = 4;
            RectangleF rec = GetImgBounds(this.Bounds, TopOffset, BtmOffset);

            float img2ViewportRatio = (float)rec.Width / (float)this.imgBitmap.Width;

            foreach (var item in coordinates)
            {
                var relativePt = new PointF(item.X * img2ViewportRatio + rec.X - dotSize / 2, item.Y * img2ViewportRatio + rec.Y - dotSize / 2);
                Pen pen = new Pen(new SolidBrush(Color.White));
                graphics.DrawEllipse(pen, relativePt.X, relativePt.Y, dotSize, dotSize);
            }
        }

        private void DisplayDefaultComponent()
        {
            //reset the comonent
            imgBitmap = null;
            this.scale = 1;
            this.Owner.Message = null;

            RectangleF rec = GetImgBounds(Bounds, TopOffset, BtmOffset);

            Pen pen = new Pen(Color.Gray, 3);
            SolidBrush myBrush = new SolidBrush(Color.Gray);

            Font standardFont = GH_FontServer.Standard; //29
            //Font standardFont4kScreen = new Font(standardFont.Name, 4); //15
            Font standardFontAdjust = GH_FontServer.NewFont(standardFont, (float) Math.Round(120M/ standardFont.Height));
            
            StringFormat myFormat = new StringFormat();

            MyGraphics.FillRectangle(myBrush, Rectangle.Round(rec));
            //graphics.DrawRectangle(pen, Rectangle.Round(imgViewBounds));
            MyGraphics.DrawString("Please use a valid image file path.\nHDR, TIF, PNG, GIF, or JPG image", standardFontAdjust, Brushes.White, new Point((int)rec.X + 12, (int)rec.Y + ((int)rec.Width * 2 / 3) + 10), myFormat);
            MyGraphics.DrawImage(Properties.Resources.Ladybug_Viewer_370, new RectangleF(rec.X, rec.Y, rec.Width, rec.Width * 2 / 3));

            myBrush.Dispose();
            myFormat.Dispose();
        }

        public delegate void ImgClick_Handler(object sender, Point clickedPtOnOriginalBitmap);

        private ImgClick_Handler MouseImgClickEvent;

        public event ImgClick_Handler mouseImgClickEvent
        {
            add
            {
                ImgClick_Handler buttonHandler = MouseImgClickEvent;
                ImgClick_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseImgClickEvent, (ImgClick_Handler)Delegate.Combine(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
            remove
            {
                ImgClick_Handler buttonHandler = MouseImgClickEvent;
                ImgClick_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseImgClickEvent, (ImgClick_Handler)Delegate.Remove(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
        }

        public delegate void NavClick_Handler(object sender, bool clickedRightButton);

        private NavClick_Handler MouseNavClickEvent;

        public event NavClick_Handler mouseNavClickEvent
        {
            add
            {
                NavClick_Handler buttonHandler = MouseNavClickEvent;
                NavClick_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseNavClickEvent, (NavClick_Handler)Delegate.Combine(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
            remove
            {
                NavClick_Handler buttonHandler = MouseNavClickEvent;
                NavClick_Handler comparand;
                do
                {
                    comparand = buttonHandler;
                    buttonHandler = Interlocked.CompareExchange(ref this.MouseNavClickEvent, (NavClick_Handler)Delegate.Remove(comparand, value), comparand);
                }
                while (buttonHandler != comparand);
            }
        }

        public override GH_ObjectResponse RespondToMouseDown(GH_Canvas sender, GH_CanvasMouseEvent e)
        {
            if (e.Button == MouseButtons.Left && imgBitmap != null)
            {
                RectangleF recImg = GetImgBounds(this.Bounds, TopOffset, BtmOffset);
                RectangleF recNavLeft = GetNavBounds(recImg, BtmOffset, NavButtonSize, false);
                RectangleF recNavRight = GetNavBounds(recImg, BtmOffset, NavButtonSize, true);

                //Click inside the img
                if (recImg.Contains(e.CanvasLocation) && !ViewOwner.DisableClickable)
                {
                    float img2ViewportRatio = (float)recImg.Width / (float)this.imgBitmap.Width;
                    PointF clickedPt = PointF.Subtract(e.CanvasLocation, new SizeF(recImg.X, recImg.Y));

                    //convert current pt location on grasshopper view back to original image size system
                    Point PixelPtOnOriginalBitmap = Point.Round(new PointF(clickedPt.X / img2ViewportRatio, clickedPt.Y / img2ViewportRatio));

                    this.MouseImgClickEvent(this, PixelPtOnOriginalBitmap);
                    return GH_ObjectResponse.Handled;
                }
                else if (recNavLeft.Contains(e.CanvasLocation))
                {
                    //Left Nav click
                    this.MouseNavClickEvent(this, false);
                }
                else if (recNavRight.Contains(e.CanvasLocation))
                {
                    //Right Nav click
                    this.MouseNavClickEvent(this, true);
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