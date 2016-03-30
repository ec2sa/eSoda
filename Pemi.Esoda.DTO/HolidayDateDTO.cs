using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class HolidayDateDTO
    {
        public int? ID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public HolidayDateDTO() { }

        public HolidayDateDTO(int? id, DateTime date, string description)
        {            
            this.ID = id;
            this.Date = date;
            this.Description = description;
        }
    }
}
