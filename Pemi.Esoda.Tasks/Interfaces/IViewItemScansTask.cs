using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewItemScansTask
	{
		string GetRegistryItem(int itemId, int registryId,bool isRF);

        Collection<DocumentItemDTO> GetItemsScans(int itemId, int registryId, bool isRF);

		void SaveChanges(string scanID, string description, bool isMain);

		void ReleaseScan(string scanID);

        bool IsGetDailyLogItemAccessDenied(int registryId, int itemId, Guid userId);
	}
}
