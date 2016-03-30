using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class MyDecretationsSearchConditions
    {
        public string SenderName { get; set; }
        public DateTime? DecretationDate { get; set; }
        public int? RegistryNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int StartPage { get; set; }
        public int PageSize { get; set; }
    }

    public class MyDecretationsSearchResult
    {
        
        public DateTime DecretationDate { get; set; }
        public int? RegistryNumber { get; set; }
        public string SenderName { get; set; }
        public string Status { get; set; }
        public string DocumentSignature { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreationDate { get; set; }
        public int DocumentID { get; set; }
        public int OrdinalNumber { get; set; }
        public string CurrentDepartment { get; set; }
        public string CurrentEmployee { get; set; }
        public int TotalRows { get; set; }
        public string IsInvoice { get; set; }
    }
}
