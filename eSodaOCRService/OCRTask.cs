using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OCRLibrary;
using System.Drawing;

namespace OCRService
{
    internal class OCRTask
    {
        private TimeSpan startHour;

        private TimeSpan endHour;

        private OCRTool ocr;

        public OCRTask(string startHour, string endHour,string tessdataDir)
        {
            TimeSpan tss,tse;
            if (!TimeSpan.TryParse(startHour, out tss) || !TimeSpan.TryParse(endHour, out tse))
                throw new ArgumentException("Invalid startHour or endHour value");
            this.startHour = tss;
            this.endHour = tse;
            this.ocr = new OCRTool(tessdataDir);
        }

        public bool IsInTimeRange
        {
            get
            {
                if (startHour == endHour)
                    return false;

                DateTime startDate = DateTime.Today + startHour;
                DateTime endDate = DateTime.Today + endHour;

                if (endDate < startDate)
                    endDate.AddDays(1);

                DateTime now = DateTime.Now;
                return now>=startDate && now<=endDate;
            }
        }

        public List<PageInfo> DoOCR(string filename){
            IImageSplitter splitter=new ImageSplitter();
            Image img;
            
            if(!splitter.TryLoad(filename,out img))
                return null;

            return this.ocr.GetTextContentFromPages(splitter.GetPagesFromImage(img));
        }


    }
}
