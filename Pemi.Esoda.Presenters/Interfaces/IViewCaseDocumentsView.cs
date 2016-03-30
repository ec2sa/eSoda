using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
    public interface IViewCaseDocumentsView
    {
        int CaseId { get; }
        string Items { set;}
        event EventHandler<ExecutingCommandEventArgs> ViewDetails;
        string NextView { set; }        
    }
}
