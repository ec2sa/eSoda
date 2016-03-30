using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tasks
{
	public class GetImageTask:IGetImageTask
	{
		#region IGetImageTask Members

		System.IO.Stream IGetImageTask.GetImage(Guid imageID)
		{
            
			IItemStorage storage = ItemStorageFactory.Create();
            if (storage.Exists(imageID))
                return storage.Load(imageID);
			return null;
		}

		#endregion
	}
}
