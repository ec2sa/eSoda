using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class CoverDTO
    {
        public int CoverID { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserDepartment { get; set; }
        public int DublerID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AvailableCoverDTO
    {
        public string UserLogin { get; set; }
        public string UserFullName { get; set; }
    }
}
