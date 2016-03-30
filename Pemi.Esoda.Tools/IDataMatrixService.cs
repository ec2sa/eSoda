using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarcodeToolkit;
using System.Drawing.Imaging;

namespace Pemi.Esoda.Tools
{
    public interface IDataMatrixService
    {
        byte[] GetDataMatrix(BarcodeData data, ImageFormat format);
        byte[] GetDataMatrixAsPdf(BarcodeData data,int left,int top,int width,int height);
        string GetDataMatrixAsHtml(BarcodeData data, int left, int top);
        byte[] GetDataMatrixAsPdf(BarcodeData data);
        byte[] GetDataMatrixWithAddressAsPdf(BarcodeData data,string[] AddressLines);
        string GetDataMatrixAsHtml(BarcodeData data);
    }
}
