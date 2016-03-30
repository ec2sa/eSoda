using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.Presenters
{
	public interface IRedirectDocumentView
	{
        int DocumentId { get; }
		Collection<SimpleLookupDTO> OrganizationalUnits { set;}
		Collection<SimpleLookupDTO> Employees { set;}
		event EventHandler ActionExecuted;
		event EventHandler OrganizationalUnitChanged;
		int OrganizationalUnitId { get;set;}
		int EmployeeId { get;}
		Guid UserId { get;}
		string ReturnTo { set;}
    string UserName { get;}
    string UserFullName { get;}
    string OUName { get;}
    string EmpName { get;}
    bool WorkOnPaper { get;}
    string Note { get;}
	}
}
