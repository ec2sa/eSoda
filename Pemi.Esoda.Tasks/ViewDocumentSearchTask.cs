using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
    public class ViewDocumentSearchTask : IViewDocumentSearchTask
    {
        private SearchDocumentDAO dao = new SearchDocumentDAO();

        #region IViewDocumentSearchTask Members

        public IList<SearchDocumentResultItem> GetDocuments(SearchDocumentConditions sc, out int totalRowCount)
        {
            return dao.GetDocuments(sc, out totalRowCount);
        }

        public IList<SimpleLookupDTO> GetCategories()
        {
            return dao.GetCategories();
        }

        public IList<SimpleLookupDTO> GetTypes(int categoryID)
        {
            return dao.GetTypes(categoryID);
        }

        public IList<SimpleLookupDTO> GetStatuses()
        {
            return dao.GetStatuses();
        }

        #endregion
    }
}
