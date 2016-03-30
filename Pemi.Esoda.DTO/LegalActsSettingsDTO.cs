using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Pemi.Esoda.DTO
{
    public class LegalActsSettingsDTO
    {
        public int ID { get; set; }
        public string Version { get; set; }
        public DateTime CreationDate {get; set;}
        public string Schema { get; set; }
        public string Xslt { get; set; }

        public LegalActsSettingsDTO(int id, string version, DateTime creationDate, string schema, string xslt)
        {
            ID = id;
            Version = version;
            CreationDate = creationDate;
            Schema = schema;
            Xslt = xslt;
        }
    }
}
