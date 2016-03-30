using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentActionsView
	{
        int DocumentId { get; }
		string Items { set;}
		Guid SelectedItem { get;}
		Guid UserId { get;}
	}
}
