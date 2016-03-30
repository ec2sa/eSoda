using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tasks
{
	public class ViewDocumentItemsTask:IViewDocumentItemsTask
	{
		private DocumentDAO dao = new DocumentDAO();
		#region IViewDocumentItemsTask Members

		System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.DocumentItemDTO> IViewDocumentItemsTask.GetDocumentItems(int documentID)
		{
			return dao.GetItemsWithOldVersions(documentID);
		}

		#endregion
	}
}
