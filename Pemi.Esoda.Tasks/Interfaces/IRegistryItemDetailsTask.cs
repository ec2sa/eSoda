using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
	public interface IRegistryItemDetailsTask
	{
		string GetItem(int itemId, int registryId,bool isRF);
		string GetItemHistory(int registryId, int itemId,bool isRF);
        bool IsGetDailyLogItemAccessDenied(int registryId, int itemId, Guid userId);
	}
}
