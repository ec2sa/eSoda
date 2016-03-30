using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class SearchCaseResultItem
    {
        public int NumberItem { get; set; }
        public string CaseNumber { get; set; }
        public string CaseType { get; set; }
        public string ClientName { get; set; }
        public string PaperSymbol { get; set; }
        public DateTime? PaperDate { get; set; }
        public DateTime? CaseStartDate { get; set; }
        public DateTime? CaseEndDate { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public int CaseID { get; set; }
    }

    public class SearchCaseConditions
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public int DepartmentID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartPage { get; set; }
        public int PageSize { get; set; }
    }
}
