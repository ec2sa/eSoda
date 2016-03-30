using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DTO
{
    public class SearchDocumentResultItem
    {
        public string DocumentNumber { get; set; }
        public int SystemNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string DocumentCategory { get; set; }
        public string DocumentType { get; set; }
        public string ClientName { get; set; }
        public string Mark { get; set; }
        public string DocumentStatus { get; set; }
        public string Owner { get; set; }
        public string FoundInDescription { get; set; }
        public string FoundInContent { get; set; }
        public string WhereInContent { get; set; }
        public bool IsInDescription { get; set; }
        public bool IsInContent { get; set; }

        
    }

    public class SearchDocumentConditions
    {
        public Guid UserGuid { get; set; }
        public bool HasExtendedSearchRole { get; set; }

        public int NumberItem { get; set; }
        public int DocumentCategory{ get; set; }
        public int DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public int SystemNumber { get; set; }
        public string ClientName { get; set; }
        public DateTime? DocumentStartDate { get; set; }
        public DateTime? DocumentEndDate { get; set; }
        public string Mark { get; set; }
        public string Status { get; set; }
        public string Text{ get; set; }
        public bool SearchDescription { get; set; }
        public bool SearchContent { get; set; }

        public int StartPage { get; set; }
        public int PageSize { get; set; }
    }

    public class SearchCriteriasState
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string DocumentNumber { get; set; }
        public string SystemNumber { get; set; }
        public string ClientName { get; set; }
        public string DocumentStartDate { get; set; }
        public string DocumentEndDate { get; set; }
        public string Mark { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public bool FoundInDescription { get; set; }
        public bool FoundInContent { get; set; }
        public string WhereInContent { get; set; }
    }
}
