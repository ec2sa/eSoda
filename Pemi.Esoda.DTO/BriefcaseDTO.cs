using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class BriefcaseGroupItemDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public BriefcaseGroupItemDTO(int id, string name, bool isChecked)
        {
            this.ID = id;
            this.Name = name;
            this.IsChecked = isChecked;
        }
    }

    public class BriefcaseItemDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public bool IsNewYearCopy { get; set; }
    }
}
