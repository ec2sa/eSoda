using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentHistoryView
	{
        int DocumentId { get; }
		string HistoryItems { set;}

	}
}
