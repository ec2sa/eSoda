using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IEditRegistryItemTask
	{
    int AcquireItemID(int registryId, Guid userID, string userName, string fullName,bool isRF);

    string GetItem(int itemId, int registryId, bool isRF);

    int UpdateItem(int itemId, int registryId, string itemContent, Guid userID, string userName, string fullName, bool isRF);

		Collection<SimpleLookupDTO> GetOrganizationalUnits();

		Collection<SimpleLookupDTO> GetEmployees(int organizationalUnitId);

		Collection<SimpleLookupDTO> GetCustomers(int customerType);

    Collection<SimpleLookupDTO> GetDocumentTypes(int categoryId);

		Collection<SimpleLookupDTO> GetDocumentCategories();

		Collection<SimpleLookupDTO> GetCorrespondenceTypes();

		int CreateCustomer(CustomerDTO newCustomer);

        bool IsGetDailyLogItemAccessDenied(int registryId, int itemId, Guid userId);

	}
}
