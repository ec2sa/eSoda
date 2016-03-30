using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Pemi.eSoda.CustomForms;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentFormHistoryView
	{        
        int DocumentId { get; }        
        string Message { set; }
        IList<CustomFormHistoryItemDTO> HistoryList { set; }
        string HistoryData { set; }
        CustomFormHistoryItemDTO HistoryItem { set; }
	}
}
