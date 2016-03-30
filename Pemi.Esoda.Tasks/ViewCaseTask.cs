using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Core.Domain;

namespace Pemi.Esoda.Tasks
{
	public class ViewCaseTask:IViewCaseTask
	{
		private CaseDAO dao = new CaseDAO();

		#region IViewCaseTask Members

		string IViewCaseTask.GetCaseInfo(int caseId)
		{
			using (XmlReader xr = dao.GetCase(caseId))
			{
				if (!xr.Read())
					return string.Empty;
				return xr.ReadOuterXml();
			}
		}

        CaseDTO IViewCaseTask.GetCaseData(int caseId)
        {
            Case _case = new Case(caseId);
            return _case.GetCaseData();
        }
		
        public string GetCaseSignature(int caseId)
        {
            return dao.GetCaseSignature(caseId);
        }

        #endregion
    }
}
