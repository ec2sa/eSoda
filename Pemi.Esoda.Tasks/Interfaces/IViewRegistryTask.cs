using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.Core.Domain;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewRegistryTask
	{
		int TotalItemsCount { get;}

		int GetCurrentRegistryId(int definitionId, int year);

		Collection<SimpleLookupDTO> GetCorrespondenceCategories();

		Collection<SimpleLookupDTO> GetCorrespondenceTypes();

		string GetItemsPage(Guid userId, int registryId, int pageNumber, int pageSize, DateTime startDate, DateTime endDate,
			string searchIncomeDate, string searchDocumentDate, string searchDocumentNumber, string searchSenderName,
			int searchCorrespondenceCategory,int searchCorrespondenceType, string searchCategoryValue,
            int searchCorrespondenceKind, string searchTypeValue, int searchCorrespondenceStatus, int searchCorrespondenceDept, int searchCorrespondenceWorker,bool isRF);

        string GetItemsPage(Guid userId, int registryId, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, bool showInvoices);

        string GetItemsPage(Guid userId, int registryId, int pageNumber, int pageSize, DateTime startDate, DateTime endDate, int year);

        Collection<SimpleLookupDTO> GetCorrespondenceKinds(int catId);
        Collection<SimpleLookupDTO> GetCorrespondenceStatus();
        Collection<SimpleLookupDTO> GetCorrespondenceDepts();
        Collection<SimpleLookupDTO> GetCorrespondenceWorkers(int deptId);
    }
}
