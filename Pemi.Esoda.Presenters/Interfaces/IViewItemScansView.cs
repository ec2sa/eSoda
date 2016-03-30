using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters{

	public interface IViewItemScansView
	{
		string ItemContent { set;}
		Collection<DocumentItemDTO> ScanListItems { set;}
		string PreviewImage { set;}
		bool IsScanSelected { get; set;}
		string RedirectTo { set;}
		int ItemID { set;}
        bool IsDailyLogItemAccessDenied { set; }
        bool IsInvoice { get; }

	}
}
