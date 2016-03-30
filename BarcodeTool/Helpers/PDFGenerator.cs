using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace BarcodeToolkit
{
    public class PDFGenerator
    {
        public float DocumentWidth 
        { 
            get
            { 
                return GetInMilimetres(_document.PageSize.Width); 
            }
            
            set
            {
                float val = GetInPoints(value);
                _document.SetPageSize(new Rectangle(val, DocumentHeight)); 
            }
        }

        public float DocumentHeight
        {
            get
            { 
                return GetInMilimetres(_document.PageSize.Height); 
            }

            set
            {
                float val = GetInPoints(value);
                _document.SetPageSize(new Rectangle(DocumentWidth, val)); 
            }
        }

        private Document _document { get; set; }
        private MemoryStream _ms { get; set; }

        public PDFGenerator(float documentWidth, float documentHeight)
        {
            _document = new Document();
            float rectWidth = GetInPoints(documentWidth);
            float rectHeight = GetInPoints(documentHeight);

            Rectangle pageRect = new Rectangle(rectWidth, rectHeight);
            _document.SetPageSize(pageRect);

            _ms = new MemoryStream();
            PdfWriter.GetInstance(_document, _ms);
        }

        public void InsertCode(string codeFileName, float top, float left)
        {
            _document.Open();

            float pTop = GetInPoints(top);
            float pLeft = GetInPoints(left);

            Image image = Image.GetInstance(codeFileName);

            float posTop = _document.PageSize.Height - pTop - image.Height;
            float posLeft = pLeft;
            
            image.SetAbsolutePosition(posLeft, posTop);
            _document.Add(image);

            _document.Close();
        }

        public void InsertCode(byte[] image, float top, float left)
        {
            _document.Open();

            float pTop = GetInPoints(top);
            float pLeft = GetInPoints(left);

            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(image);

            float posTop = _document.PageSize.Height - pTop - img.Height;
            float posLeft = pLeft;

            img.SetAbsolutePosition(posLeft, posTop);
            _document.Add(img);

            _document.Close();
        }

        public void InsertCode(byte[] image, float top, float left, string[] addressLines)
        {
            _document.Open();
            float pTop = GetInPoints(top);
            float pLeft = GetInPoints(left);
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(image);
            float posTop = _document.PageSize.Height - pTop - img.Height;
            float posLeft = pLeft;
        
            
            
          //  FontFactory.RegisterDirectory(Path.Combine(Environment.SystemDirectory,"Fonts"));
            
            Paragraph address = new Paragraph();

            BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            iTextSharp.text.Font f = new iTextSharp.text.Font(bf,9); 

         
            foreach (var line in addressLines)
            {
             IElement ph = new Phrase(line,f);
                address.Add(ph);
                address.Add(new Chunk(Environment.NewLine));
            }


            PdfPTable t = new PdfPTable(2);            
            PdfPCell c1 = new PdfPCell(img);
            c1.PaddingLeft = pLeft;
            c1.PaddingTop = pTop;
            PdfPCell c2 = new PdfPCell(address);
            
            c2.PaddingTop = pTop;
            c2.PaddingLeft = 30;
            c1.BorderWidth = c2.BorderWidth = 0;
           
                
            t.AddCell(c1);
            t.AddCell(c2);
   

            _document.Add(t);

            _document.Close();
        }

        public byte[] GetPDFFile()
        {
            return _ms.GetBuffer();
        }

        private float GetInPoints(float valueInMilimetres)
        {
            return (float)Math.Round(72 * valueInMilimetres / 10 / (2.54));
        }

        private float GetInMilimetres(float valueInPoints)
        {
            return (float)Math.Round(valueInPoints / 72 * 10 * (2.54));
        }
    }
}
