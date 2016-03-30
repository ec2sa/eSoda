using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tasks
{
	public interface IViewDocumentActionsTask
	{
    string GetAvailableActions(int documentId, Guid userId, ActionType actionType);
	}
}
