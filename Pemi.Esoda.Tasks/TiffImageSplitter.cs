using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Pemi.Esoda.Tasks
{
    public class TiffImageSplitter
    {
        public int getPageCount(Image img)
        {
            int pageCount = -1;
            try
            {
                pageCount = img.GetFrameCount(FrameDimension.Page);
            }
            catch (Exception ex)
            {
                pageCount = 0;
            }
            return pageCount;
        }

        public Image getTiffImage(Image sourceImage, int pageNumber)
        {
            MemoryStream ms = null;
            Image returnImage = null;

            try
            {
                ms = new MemoryStream();
                Guid objGuid = sourceImage.FrameDimensionsList[0];
                FrameDimension objDimension = new FrameDimension(objGuid);
                sourceImage.SelectActiveFrame(objDimension, pageNumber);
                sourceImage.Save(ms, ImageFormat.Tiff);
                returnImage = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                returnImage = null;
            }
            return returnImage;
        }

    }
}
