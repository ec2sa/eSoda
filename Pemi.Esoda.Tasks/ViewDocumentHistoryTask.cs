using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
	public class ViewDocumentHistoryTask:IDocumentHistoryTask
	{
		private DocumentDAO dao = new DocumentDAO();

		#region IDocumentHistoryTask Members

		string IDocumentHistoryTask.GetDocumentHistory(int documentId)
		{
			using (XmlReader xr = dao.GetDocumentHistory(documentId))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

		#endregion
	}
}
