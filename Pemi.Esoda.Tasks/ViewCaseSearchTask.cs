using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public class ViewCaseSearchTask : IViewCaseSearchTask
	{
        private SearchCaseDAO dao = new SearchCaseDAO();

        #region IViewCaseSearchTask Members

        public IList<SearchCaseResultItem> GetCases(SearchCaseConditions sc, out int totalRowCount)
        {
            return dao.GetCases(sc, out totalRowCount);
        }

        public IList<SimpleLookupDTO> GetDepartments()
        {
            return dao.GetDepartments();
        }

        public IList<SimpleLookupDTO> GetClients()
        {
            return dao.GetClients();
        }

        #endregion
    }
}
