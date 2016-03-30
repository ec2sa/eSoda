using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BarcodeToolkit
{
    public class BarcodeData
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Post{ get; set; }
        public int Amount{ get; set; }
        public int Receiving { get; set; }
        public string Notes { get; set; }
        public int Additions { get; set; }
        public string Department { get; set; }
     
        public DateTime? SendDate { get; set; }

        [XmlElement(IsNullable = true)]
        public string RKWNumber { get; set; }

        [XmlElement(IsNullable = true)]
        public string SentBy { get; set; }
    }
}
