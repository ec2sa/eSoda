using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
	public interface IAssignedItemsTask
	{
		string GetAssignedItems(Guid userId);
	}
}
