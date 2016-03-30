using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
    public interface IViewDocumentSearchTask
    {
        IList<SearchDocumentResultItem> GetDocuments(SearchDocumentConditions sc, out int totalRowCount);
        IList<SimpleLookupDTO> GetCategories();
        IList<SimpleLookupDTO> GetTypes(int categoryID);
        IList<SimpleLookupDTO> GetStatuses();
    }
}
