using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewDocumentTask
	{
		DocumentDTO GetDocumentData(int id);
	}
}
