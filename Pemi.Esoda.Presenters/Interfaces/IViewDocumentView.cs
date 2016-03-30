using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentView
	{
		DocumentDTO DocumentData { set; }
        int DocumentId { get; }
	}
}
