using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
	public interface IViewItemHistoryTask
	{
		string GetHistoryItems(int registryId, int itemId);
	}
}
