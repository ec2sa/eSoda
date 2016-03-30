using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface IRegistryItemDetailsView
	{
		string ItemContent { set;}
		string HistoryItems { set;}
		int ItemID { set;}
        bool IsDailyLogItemAccessDenied { set; }
        bool IsInvoice { get; }
	}
}
