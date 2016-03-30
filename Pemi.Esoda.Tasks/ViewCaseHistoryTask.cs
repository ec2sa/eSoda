using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
    public class ViewCaseHistoryTask:ICaseHistoryTask
    {
        private CaseDAO dao = new CaseDAO();

        #region ICaseHistoryTask Members

        string ICaseHistoryTask.GetCaseHistory(int caseId)
        {
            using (XmlReader xr = dao.GetCaseHistory(caseId))
            {
                if (!xr.Read())
                    return string.Empty;
                return xr.ReadOuterXml();
            }
        }

        #endregion
    }
}
