using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
	public class AssignedItemsTask:IAssignedItemsTask
	{
		private UserDAO dao = new UserDAO();

		#region IAssignedItemsTask Members

		string IAssignedItemsTask.GetAssignedItems(Guid userId)
		{
            //using(XmlReader xr=dao.GetAssignedItems(userId))
            //{
            //    if (!xr.Read())
            //        return string.Empty;
            //    return xr.ReadOuterXml();
            //}
            return dao.GetAssignedItems(userId);
		}

		#endregion
	}
}
