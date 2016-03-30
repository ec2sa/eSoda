using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentItemsView
	{
        Collection<DocumentItemDTO> Items { set;}
		string previewImageUrl { set;}
		bool IsInListMode { set;}
		int CurrentPage { get;set;}
		int CurrentScale { get;set;}
		int PageCount { set;}
        int DocumentId { set; get; }
        bool IsMSOTemplateVisible { set; }
        string Message { set; }
		event EventHandler ReturnToFileList;
		event EventHandler<ExecutingCommandEventArgs> ExecutingCommand;
	}
}
