using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Core.Domain;

namespace Pemi.Esoda.Tasks
{
	public class ViewDocumentTask:IViewDocumentTask
	{
		#region IViewDocumentTask Members

		Pemi.Esoda.DTO.DocumentDTO IViewDocumentTask.GetDocumentData(int id)
		{
			Document doc = new Document(id);
			return doc.GetDocumentData();
		}

		#endregion
	}
}
