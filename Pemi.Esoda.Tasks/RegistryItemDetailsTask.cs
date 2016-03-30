using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
	public class RegistryItemDetailsTask:IRegistryItemDetailsTask
	{
		private RegistryDAO dao = new RegistryDAO();

		#region IRegistryItemDetailsTask Members

		string IRegistryItemDetailsTask.GetItem(int itemId, int registryId,bool isRF)
		{
			using (XmlReader xr = dao.GetItem(itemId, registryId,isRF))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

        string IRegistryItemDetailsTask.GetItemHistory(int registryId, int itemId, bool isRF)
		{
			using (XmlReader xr = dao.GetItemHistory(registryId, itemId,isRF))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}
	
        public bool IsGetDailyLogItemAccessDenied(int registryId, int itemId, Guid userId)
        {
            return dao.IsDailyLogItemAccessDenied(registryId, itemId, userId);
        }

        #endregion
    }
}
