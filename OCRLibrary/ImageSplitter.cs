using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OCRLibrary
{
    public class ImageSplitter : IImageSplitter
    {
        public readonly ImageFormat PageFormat = ImageFormat.Tiff;

        #region IImageSplitter Members

        public bool TryLoad(string fileName, out Image img)
        {
            try
            {
                img = null;
                img = Image.FromFile(fileName);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("ImageSplitter.TryLoad: " + ex.Message);
                img = null;
                return false;
            }

        }

        public List<Bitmap> GetPagesFromImage(Image img)
        {
            List<Bitmap> pages = new List<Bitmap>();
            int framesCount = img.GetFrameCount(FrameDimension.Page);

            for (int i = 0; i < framesCount; i++)
            {
                img.SelectActiveFrame(FrameDimension.Page, i);
                using (MemoryStream sms = new MemoryStream())
                {
                    img.Save(sms, PageFormat);
                    using (MemoryStream rms = new MemoryStream(sms.ToArray()))
                    {
                        Bitmap b = Image.FromStream(rms) as Bitmap;
                        if (b != null)
                            pages.Add(b);
                    }
                }
            }
            return pages;
        }


        public List<Bitmap> GetPagesFromImage(string filename)
        {
            Image img;

            if (TryLoad(filename, out img))
            {
                return GetPagesFromImage(img);
            }
            return new List<Bitmap>();
        }

        #endregion
    }
}
