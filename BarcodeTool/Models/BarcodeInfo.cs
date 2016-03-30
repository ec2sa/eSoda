using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcodeToolkit
{
    public class BarcodeInfo
    {
        public int ResultCode { get; set; }
        public byte[] Data { get; set; }
        public int Width { get; set; }
    }
}
