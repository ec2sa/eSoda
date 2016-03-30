using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.IO;

namespace Pemi.Esoda.Tasks
{
	public interface IGetImageTask
	{
		Stream GetImage(Guid imageID);

	}
}
