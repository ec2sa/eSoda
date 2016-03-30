using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

namespace BarcodeToolkit
{
    class CodeGenerator : ICodeGenerator
    {
        private string _imageFileName { get; set; }

        public BarcodeInfo GetBarcode(BarcodeData encodeData)
        {
            Matrix matrix = new Matrix();

            int width = 144;
            
            BarcodeInfo result = new BarcodeInfo();
            
            

            result.ResultCode = 
                CreateDataMatrix(ref matrix, ref width, encodeData.Name, encodeData.Address, 
                encodeData.Post, encodeData.Amount, encodeData.Receiving, encodeData.Notes, 
                encodeData.Additions, encodeData.Department);
            
            result.Width = width;
            result.Data = matrix.matrix;

            return result;
        }

        public void SaveImage(BarcodeInfo barcodeInfo, string filename)
        {
            SaveImage(barcodeInfo, filename, 2);
        }

        public void SaveImage(BarcodeInfo info, string filename, int sizeFactor)
        {
            _imageFileName = filename;
            BarcodeBitmap bmp = new BarcodeBitmap(info.Width, info.Width, sizeFactor);

            Color black = Color.Black;
            Color white = Color.White;
            for (int i = 0; i < info.Width; i++)
            {
                for (int j = 0; j < info.Width; j++)
                {
                    if (info.Data[j + i * info.Width] == 1)
                        bmp.SetPixel(j, i, black);
                    else
                        bmp.SetPixel(j, i, white);
                }
            }

            bmp.Save(_imageFileName, ImageFormat.Bmp);
            
        }

        public byte[] GetCodeContent(BarcodeInfo info, ImageFormat imageFormat)
        {
            return GetCodeContent(info, 2, imageFormat); 
        }

        public byte[] GetCodeContent(BarcodeInfo info, int sizeFactor, ImageFormat imageFormat)
        {
            BarcodeBitmap bmp = new BarcodeBitmap(info.Width, info.Width, sizeFactor);

            Color black = Color.Black;
            Color white = Color.White;
            for (int i = 0; i < info.Width; i++)
            {
                for (int j = 0; j < info.Width; j++)
                {
                    if (info.Data[j + i * info.Width] == 1)
                        bmp.SetPixel(j, i, black);
                    else
                        bmp.SetPixel(j, i, white);
                }
            }
            using(MemoryStream stream = new MemoryStream())
            {
                bmp.GetBitmap().Save(stream,imageFormat);
                byte[] content = new byte[stream.Length];
                return stream.GetBuffer();
            }
        }

        public void Open()
        {
            Process.Start(_imageFileName);
        }

        [DllImport("BARCODE.DLL")]
        private static extern int CreateDataMatrix(
            ref Matrix matrix,
            ref int width,
            [MarshalAs(UnmanagedType.LPStr)]string nazwa,
            [MarshalAs(UnmanagedType.LPStr)]string adres,
            [MarshalAs(UnmanagedType.LPStr)]string poczta,
            int kwota,
            int pobranie,
            [MarshalAs(UnmanagedType.LPStr)]string uwagi,
            int dodatki,
            [MarshalAs(UnmanagedType.LPStr)]string dzial);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Matrix
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=144*144)]
        public byte[] matrix;
    };
}
