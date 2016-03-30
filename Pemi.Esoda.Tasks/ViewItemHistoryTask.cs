using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
	public class ViewItemHistoryTask:IViewItemHistoryTask
	{
		private RegistryDAO dao = new RegistryDAO();

		#region IViewItemHistoryTask Members

		string IViewItemHistoryTask.GetHistoryItems(int registryId, int itemId)
		{
			using (XmlReader xr = dao.GetItemHistory(registryId, itemId,false))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

		#endregion
	}
}
