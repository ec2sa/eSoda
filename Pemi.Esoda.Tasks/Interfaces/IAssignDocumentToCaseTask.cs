using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.Tasks
{
	public interface IAssignDocumentToCaseTask
	{
		Collection<SimpleLookupDTO> GetBriefcaseList(Guid userId, int year);

		Collection<SimpleLookupDTO> GetCaseNumbersList(int caseTypeId);

		Collection<SimpleLookupDTO> GetCaseKindList();

        Collection<SimpleLookupDTO> GetCaseKindsFromBriefcase(int briefcaseId);

        Collection<SimpleLookupDTO> GetAvailableYears();

		string GetDocumentData(int documentId);

        int AssignDocumentToCase(Guid userId, int documentId, int caseNumber);

		int AssignDocumentToNewCase(Guid userId, int documentId, int caseTypeId, int caseKindId, string description, DateTime? documentDate, string documentSignature,string sender);
	}
}
