using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
    public interface IViewCaseHistoryView
    {
        int CaseId { get; }
        string HistoryItems { set;}
        
    }
}
