using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Tasks
{
    public class ViewCaseDocumentsTask:IViewCaseDocumentsTask
    {
        private CaseDAO dao = new CaseDAO();

        #region IViewCaseDocumentsTask Members

        string IViewCaseDocumentsTask.GetCaseDocuments(int caseId)
        {
            using (XmlReader xr =  dao.GetContainedDocuments(caseId))
            {
                if (!xr.Read())
                    return string.Empty;
                return xr.ReadOuterXml();
            }
        }

        #endregion
    }
}
