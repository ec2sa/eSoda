using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DataAccess
{
	public class DataAccessLogEntry
	{
		public static void Create(string message,bool isSuccess)
		{
			Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry logEntry = new Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry();
			logEntry.Categories.Add("DataAccess");
			logEntry.Categories.Add(isSuccess ? "Sukces" : "B³¹d");
			logEntry.EventId = 1;
			logEntry.Priority = 1;
			logEntry.Message = message;
			Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(logEntry);
		}
	}
}
