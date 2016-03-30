using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewDocumentItemsTask
	{
		Collection<DocumentItemDTO> GetDocumentItems(int DocumentID);
	}
}
