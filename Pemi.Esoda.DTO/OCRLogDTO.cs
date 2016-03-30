using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class OCRLogDTO
    {
        public DateTime LogDate { get; set; }
        public int OCRed { get; set; }
        public int UnOCRable { get; set; }
        public int Total { get; set; }
        public int ScansPagesOCRed { get; set; }
        public int ScansRemainedToOCR { get; set; }

    }
}
