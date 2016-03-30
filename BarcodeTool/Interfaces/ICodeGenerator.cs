using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace BarcodeToolkit
{
    public interface ICodeGenerator
    {
        BarcodeInfo GetBarcode(BarcodeData data);
        void SaveImage(BarcodeInfo barcodeInfo, string filename);
        void SaveImage(BarcodeInfo barcodeInfo, string filename, int sizeFactor);
        byte[] GetCodeContent(BarcodeInfo barcodeInfo, ImageFormat imageFormat);
        byte[] GetCodeContent(BarcodeInfo barcodeInfo, int sizeFactor, ImageFormat imageFormat);

        void Open();
    }
}
