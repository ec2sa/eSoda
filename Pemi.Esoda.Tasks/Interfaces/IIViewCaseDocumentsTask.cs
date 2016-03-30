using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
    public interface IViewCaseDocumentsTask
    {
        string GetCaseDocuments(int caseId);
    }
}
