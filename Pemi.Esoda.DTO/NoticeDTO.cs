using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class NoticeDTO
    {
        public int? NoticeID { get; set; }
        public string Notice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public NoticeDTO(int? noticeID, string notice, DateTime? startDate, DateTime? endDate, bool isActive)        
        {
            this.NoticeID = noticeID;
            this.Notice = notice;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.IsActive = isActive;
        }        
    }
}
