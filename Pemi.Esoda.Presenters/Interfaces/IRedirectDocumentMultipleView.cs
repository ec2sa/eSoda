using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.Presenters
{
	public interface IRedirectDocumentMultipleView
	{
        int DocumentId { get; }
		Collection<SimpleLookupDTO> OrganizationalUnits { set; }
		Collection<SimpleLookupDTO> Employees { set; }

		event EventHandler ActionExecuted;
		event EventHandler OrganizationalUnitChanged;
        event EventHandler AddToRedirectList;

        int SelectedItemID { get; set; }
        IList<RedirectItem> RedirectList { set; }
		int OrganizationalUnitId { get;set;}
        
        int EmployeeId { get; set; }
        bool WorkOnPaper { get; set; }
        string Note { get; set;  }
        bool CommandID { get; set; }
        bool AllHistory { get; set; }
        bool AllScans { get; set; }
		Guid UserId { get;}
		string ReturnTo { set;}
        string UserName { get;}
        string UserFullName { get;}
        string OUName { get;}
        string EmpName { get;}

        string AddToRedirectButtonName { set; }
        bool ShowCancelChangesButton { set; }

        bool AllHistoryEnable { set; }
        bool AllScanEnable { set; }
        bool WorkOnPaperEnable { set; }

        string Message { set; }
        string Description { set; }
        string Notice { set; }
	}
}
