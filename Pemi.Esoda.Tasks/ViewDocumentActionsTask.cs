using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
	public class ViewDocumentActionsTask:IViewDocumentActionsTask
	{
		private ActionDAO dao = new ActionDAO();

		#region IViewDocumentActionsTask Members

		string IViewDocumentActionsTask.GetAvailableActions(int documentId, Guid userId,ActionType actionType)
		{
			using (XmlReader xr = dao.GetAvailableActions(documentId,userId,ActionMask.Document,actionType))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

		#endregion
	}
}
