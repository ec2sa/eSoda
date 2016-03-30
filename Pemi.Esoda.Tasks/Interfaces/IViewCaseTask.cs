using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewCaseTask
	{
		string GetCaseInfo(int caseId);
        CaseDTO GetCaseData(int caseId);
        string GetCaseSignature(int caseId);
	}
}
