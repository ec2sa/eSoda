using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Pemi.eSoda.CustomForms;

namespace Pemi.Esoda.Presenters
{
	public interface IViewDocumentFormXmlView
	{        
        int DocumentId { get; }
        string Message { set; }
        CustomFormDTO CustomForm { set; }        
	}
}
