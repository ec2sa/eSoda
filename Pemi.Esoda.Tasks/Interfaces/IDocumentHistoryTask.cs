using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
	public interface IDocumentHistoryTask
	{
		string GetDocumentHistory(int documentId);
	}
}
