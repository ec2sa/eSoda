using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.Presenters
{
	public interface IEditRegistyItemView
	{
        
		int ItemID { set;}
		string ItemContent { get; set;}
		int CustomerType { get;set;}
		int CustomerId { get;set;}
		int EmployeeId { get;set;}
		int OrganizationalUnitId { get;set;}
		int DocumentType { get; set;}
		int CorrespondenceType { get; set;}
		int DocumentCategory { get; set;}
    string UserName { get;}
    string UserFullName { get;}
		
		Collection<SimpleLookupDTO> Customers { set;}
		Collection<SimpleLookupDTO> OrganizationalUnits { set;}
		Collection<SimpleLookupDTO> Employees { set;}
		Collection<SimpleLookupDTO> DocumentTypes { set;}
		Collection<SimpleLookupDTO> CorrespondenceTypes { set;}
		Collection<SimpleLookupDTO> DocumentCategories { set;}


        bool IsInvoice { get; set; }
		bool IsInPreviewState { set;}
		bool IsInEditState { set;}
		bool IsInInsertState { set;}
		bool IsInCustomerInsertState { set;}
        bool IsDailyLogItemAccessDenied { set; }
		event EventHandler<ExecutingCommandEventArgs> ItemCommand;

	}
}
