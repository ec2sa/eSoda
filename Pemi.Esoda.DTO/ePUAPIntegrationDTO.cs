using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class ePUAPIntegrationDTO
    {
        public string DocumentName { get; set; }

        public string DocumentType { get; set; }
        
        public byte[] DocumentContent { get; set; }
        
        public string DocumentSenderName { get; set; }

        public DateTime DocumentSendDate { get; set; }

        public string ResponseAddress { get; set; }
    }
}
