using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace OCRLibrary
{
    public interface IImageSplitter
    {
       List<Bitmap> GetPagesFromImage(Image img);
       
        List<Bitmap> GetPagesFromImage(string filename);

       bool TryLoad(string fileName, out Image img);
    }

    public interface IOCRTool<T>
    {
        List<T> GetTextContentFromPages(List<Bitmap> pages);
    }
}
