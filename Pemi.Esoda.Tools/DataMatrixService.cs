using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BarcodeToolkit;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tools
{
    public class DataMatrixService : IDataMatrixService
    {
        private BarcodeTool _tool;

        public DataMatrixService()
        {
            _tool = new BarcodeTool();
        }

        public byte[] GetDataMatrix(BarcodeToolkit.BarcodeData data, System.Drawing.Imaging.ImageFormat format)
        {
            _tool.DataToEncode = data;
            return _tool.GetBarcode(format);
        }

        public byte[] GetDataMatrixAsPdf(BarcodeToolkit.BarcodeData data, int left, int top, int width, int height)
        {
            _tool.DataToEncode = data;
            _tool.PDFDocumentWidth = width;
            _tool.PDFDocumentHeight = height;

            return _tool.GetPDFWithBarcode(top, left);
        }

        public string GetDataMatrixAsHtml(BarcodeToolkit.BarcodeData data, int left, int top)
        {
            _tool.DataToEncode = data;
            return _tool.GetHTMLWithBarcode(top, left);
        }

        public byte[] GetDataMatrixAsPdf(BarcodeData data)
        {
            Dictionary<string, string> configData = new EsodaConfigParametersDAO().GetConfig();

            int height = 297; 
            int.TryParse(configData["printPageHeight"], out height);

            int width = 210; 
            int.TryParse(configData["printPageWidth"], out width);

            int left=0; 
            int.TryParse(configData["codePosLeft"], out left);
            
            int top=0; 
            int.TryParse(configData["codePosTop"], out top);

            _tool.DataToEncode = data;
            _tool.PDFDocumentWidth = width;
            _tool.PDFDocumentHeight = height;

            return _tool.GetPDFWithBarcode(top, left);
        }

        public byte[] GetDataMatrixWithAddressAsPdf(BarcodeData data, string[] addressLines)
        {
            Dictionary<string, string> configData = new EsodaConfigParametersDAO().GetConfig();

            int height = 297;
            int.TryParse(configData["printPageHeight"], out height);

            int width = 210;
            int.TryParse(configData["printPageWidth"], out width);

            int left = 0;
            int.TryParse(configData["codePosLeft"], out left);

            int top = 0;
            int.TryParse(configData["codePosTop"], out top);

            _tool.DataToEncode = data;
            _tool.PDFDocumentWidth = width;
            _tool.PDFDocumentHeight = height;

            return _tool.GetPDFWithBarcodeAndAddress(top, left,addressLines);
        }

        public string GetDataMatrixAsHtml(BarcodeData data)
        {
            Dictionary<string, string> configData = new EsodaConfigParametersDAO().GetConfig();

            int left = 0;
            int.TryParse(configData["codePosLeft"], out left);

            int top = 0;
            int.TryParse(configData["codePosTop"], out top);

            _tool.DataToEncode = data;
            return _tool.GetHTMLWithBarcode(top, left);
        }

    }
}
