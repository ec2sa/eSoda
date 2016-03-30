using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class RegistryDTO
    {
        public string XslFo { set; get; }      
        public bool IsXslFoExist {
            get
            {
                return !String.IsNullOrEmpty(XslFo);
            }
        }
    }

    public class RegistryItemDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public bool IsNewYearCopy { get; set; }
    }
}
