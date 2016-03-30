using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters
{
	public interface IAssignDocumentToCaseView
	{
        int CaseId { set; }
        int DocumentId { get; }
        int SelectedYear { get; }

		Collection<SimpleLookupDTO> BriefcaseList { set;}
		Collection<SimpleLookupDTO> CaseNumbers { set;}
		Collection<SimpleLookupDTO> CaseKind { set;}
        Collection<SimpleLookupDTO> AvailableBriefcasesYears { set; }

		int SelectedBriefcase { get; set;}
		int SelectedCaseNumber {get; set;}
		int SelectedCaseKind { get; set;}
        
		string SelectedCaseDescription { get; set;}
		DateTime? DocumentDate { get; set;}
		string DocumentReferenceNumber { get;set;}
		string DocumentSender { get; set;}
        string DocumentSenderID { get; set;}
        
        bool IsNewCase{set;}

		event EventHandler AssigningToNewCase;
		event EventHandler AssigningToSelectedCase;
		event EventHandler CaseTypeSelected;
        event EventHandler YearChanged;

	}
}
