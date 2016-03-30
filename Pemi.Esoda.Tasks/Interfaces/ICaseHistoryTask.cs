using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
    public interface ICaseHistoryTask
    {
        string GetCaseHistory(int documentId);
    }
}
