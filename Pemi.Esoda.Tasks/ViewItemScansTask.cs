using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public class ViewItemScansTask:IViewItemScansTask
	{
		private RegistryDAO dao = new RegistryDAO();

		#region IViewItemScansTask Members

        string IViewItemScansTask.GetRegistryItem(int itemId, int registryId, bool isRF)
		{
			using (XmlReader xr = dao.GetItemsPageItem(registryId, itemId,isRF))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}


        System.Collections.ObjectModel.Collection<DocumentItemDTO> IViewItemScansTask.GetItemsScans(int itemId, int registryId, bool isRF)
		{
			return dao.GeItemsScans(registryId, itemId,isRF);
		}


		void IViewItemScansTask.SaveChanges(string scanID, string description, bool isMain)
		{
			dao.SaveScanChanges(scanID, description, isMain);

		}


		void IViewItemScansTask.ReleaseScan(string scanID)
		{
			dao.ReleaseScan(scanID);
		}

		
        public bool IsGetDailyLogItemAccessDenied(int registryId, int itemId, Guid userId)
        {
            return dao.IsDailyLogItemAccessDenied(registryId, itemId, userId);
        }

        #endregion
    }
}
