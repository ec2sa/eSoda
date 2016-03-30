using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters
{
	public interface IViewCaseView
	{
        int CaseId { get;}  
		string CaseInfo { set;}
		      
        CaseDTO CaseData { set; }
        string CaseSignature { set; }
	}
}
