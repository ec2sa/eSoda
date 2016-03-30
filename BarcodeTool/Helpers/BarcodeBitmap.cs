using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace BarcodeToolkit
{
    public class BarcodeBitmap
    {
        private Bitmap _bmp;
        private int _sizeFactor;
        
        public BarcodeBitmap(int x, int y, int sizeFactor)
        {
            _bmp = new Bitmap(x * sizeFactor, y * sizeFactor);
            _sizeFactor = sizeFactor;
        }
        public void SetPixel(int x, int y, Color color)
        {
            for (int i = 0; i < _sizeFactor; i++)
            {
                for (int j = 0; j < _sizeFactor; j++)
                {
                    _bmp.SetPixel((x*_sizeFactor) + i, (y*_sizeFactor) + j, color);
                }
            }
        }

        public void Save(string filename, ImageFormat imageFormat)
        {
            _bmp.Save(filename, imageFormat);
        }
        public Bitmap GetBitmap()
        {
            return _bmp;
        }
    }
}
