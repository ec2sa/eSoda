using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace BarcodeToolkit
{
    public class BarcodeTool
    {
        private ICodeGenerator _codeGenerator;

        private float _pdfWidth = 210;
        /// <summary>
        /// PDF document page width in mm.
        /// </summary>
        public float PDFDocumentWidth
        {
            get
            {
                return _pdfWidth;
            }
            set
            {
                _pdfWidth = value;
            }
        }

        private float _pdfHeight = 297;
        /// <summary>
        /// PDF document page height in mm.
        /// </summary>
        public float PDFDocumentHeight
        {
            get
            {
                return _pdfHeight;
            }
            set
            {
                _pdfHeight = value;
            }
        }

        /// <summary>
        /// Data to generate barcode.
        /// </summary>
        public BarcodeData DataToEncode { get; set; }

        public BarcodeTool(){
            _codeGenerator = new CodeGenerator();
        }

        /// <summary>
        /// Gets barcode of DataToEncode. DataToEncode property cannot be null.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBarcode(ImageFormat imageFormat)
        {
                BarcodeInfo info = GetBarcodeInfo();
                return _codeGenerator.GetCodeContent(info, imageFormat);
        }

        /// <summary>
        /// Gets barcode of DataToEncode. DataToEncode property cannot be null.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBarcode(ImageFormat imageFormat, int sizeFactor)
        {
            BarcodeInfo info = GetBarcodeInfo();
            return _codeGenerator.GetCodeContent(info, sizeFactor, imageFormat);
        }

        private BarcodeInfo GetBarcodeInfo()
        {
            if (DataToEncode != null)
            {
                return _codeGenerator.GetBarcode(DataToEncode);
            }
            else
                throw new Exception("DataToEncode cannot be null");
        }

        /// <summary>
        /// Saves barcode of DataToEncode to specified file. DataToEncode property cannot be null.
        /// </summary>
        /// <param name="filename">Barcode filename</param>
        public void SaveBarcodeImage(string filename)
        {
            _codeGenerator.SaveImage(GetBarcodeInfo(), filename);
        }

        /// <summary>
        /// Gets content of PDF file with barcode in specified position.
        /// </summary>
        /// <param name="barcodePositionTop">Distance from top in mm.</param>
        /// <param name="barcodePositionLeft">Distance from left in mm.</param>
        /// <returns></returns>
        public byte[] GetPDFWithBarcode(int barcodePositionTop, int barcodePositionLeft)
        {
            PDFGenerator pdfGenerator = new PDFGenerator(PDFDocumentWidth, PDFDocumentHeight);
            pdfGenerator.InsertCode(GetBarcode(ImageFormat.Png), barcodePositionTop, barcodePositionLeft);
            return pdfGenerator.GetPDFFile();
        }

        /// <summary>
        /// Gets content of PDF file with barcode in specified position.
        /// </summary>
        /// <param name="barcodePositionTop">Distance from top in mm.</param>
        /// <param name="barcodePositionLeft">Distance from left in mm.</param>
        /// <param name="sizeFactor"> Pixel square side (1 pixel =  sizeFactor x sizeFactor) </param>
        /// <returns></returns>
        public byte[] GetPDFWithBarcode(int barcodePositionTop, int barcodePositionLeft, int sizeFactor)
        {
            PDFGenerator pdfGenerator = new PDFGenerator(PDFDocumentWidth, PDFDocumentHeight);
            pdfGenerator.InsertCode(GetBarcode(ImageFormat.Png, sizeFactor), barcodePositionTop, barcodePositionLeft);
            return pdfGenerator.GetPDFFile();
        }


        /// <summary>
        /// Saves Html file with specified barcode image.
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <param name="barcodeImageSrc"></param>
        /// <param name="barcodePositionTop"></param>
        /// <param name="barcodePositionLeft"></param>
        public void SaveHTMLWithBarcode(string htmlPath, string barcodeImageSrc, int barcodePositionTop, int barcodePositionLeft)
        {
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            htmlGenerator.InsertCode(barcodeImageSrc, barcodePositionLeft, barcodePositionTop);
            htmlGenerator.Save(htmlPath);
        }


        /// <summary>
        /// Saves html with barcode.
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <param name="barcodePositionTop"></param>
        /// <param name="barcodePositionLeft"></param>
        public void SaveHTMLWithBarcode(string htmlPath, int barcodePositionTop, int barcodePositionLeft)
        {
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            htmlGenerator.InsertCode(GetBarcode(ImageFormat.Png), barcodePositionLeft, barcodePositionTop);
            htmlGenerator.Save(htmlPath);
        }

        /// <summary>
        /// Gets html with barcode source.
        /// </summary>
        /// <param name="barcodePositionTop"></param>
        /// <param name="barcodePositionLeft"></param>
        /// <returns></returns>
        public string GetHTMLWithBarcode(int barcodePositionTop, int barcodePositionLeft)
        {
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            htmlGenerator.InsertCode(GetBarcode(ImageFormat.Png), barcodePositionLeft, barcodePositionTop);
            return htmlGenerator.GetHTML();
        }

        public byte[] GetPDFWithBarcodeAndAddress(int barcodePositionTop, int barcodePositionLeft, string[] addressLines)
        {
            PDFGenerator pdfGenerator = new PDFGenerator(PDFDocumentWidth, PDFDocumentHeight);
            pdfGenerator.InsertCode(GetBarcode(ImageFormat.Png), barcodePositionTop, barcodePositionLeft,addressLines);
            return pdfGenerator.GetPDFFile();
        }
    }
}
