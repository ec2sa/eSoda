using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters.Interfaces
{
    public interface IViewMyDecretationsView
    {
        string Message { set; }
        MyDecretationsSearchConditions SeachConditions { get; set; }
        IList<MyDecretationsSearchResult> SearchResults { set; }
        IList<SimpleLookupDTO> Clients { set; }
        IList<SimpleLookupDTO> Departments { set; }
        
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        int PageSize { get; set; }
        event EventHandler<PagerEventArgs> PageChanged;
    }
}
