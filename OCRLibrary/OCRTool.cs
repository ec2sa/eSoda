using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tessnet2;
using System.Drawing;

namespace OCRLibrary
{
    public class OCRTool:IOCRTool<PageInfo>
    {
        private string tessdataDir;

        public OCRTool(string tessdataDir)
        {
            this.tessdataDir = tessdataDir;
        }

        #region IOCRTool<PageInfo> Members

        public List<PageInfo> GetTextContentFromPages(List<System.Drawing.Bitmap> pages)
        {
            List<PageInfo> results = new List<PageInfo>();

            
            Tesseract ocr = new Tesseract();

            ocr.Init(tessdataDir, "pol", false);

            foreach(Bitmap page in pages){

               List<Word> words = ocr.DoOCR(page, Rectangle.Empty);
               StringBuilder sb = new StringBuilder();
               foreach (Word word in words)
               {
                   sb.AppendFormat("{0} ",word.Text);
               }
               results.Add(new PageInfo() {PageNumber=pages.IndexOf(page)+1,TextContent=sb.ToString() });
            }

            return results;

        }

        #endregion
    }
}
