using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class SMSServiceConfiguration
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public bool IsFlash { get; set; }
        public bool IsTest { get; set; }
        public string MessageTemplate { get; set; }
    }
}
