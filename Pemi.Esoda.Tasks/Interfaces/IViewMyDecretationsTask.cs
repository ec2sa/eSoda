using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
    public interface IViewMyDecretationsTask
    {
        IList<MyDecretationsSearchResult> GetDecretations(MyDecretationsSearchConditions sc, out int totalRowCount,object userGuid);
        IList<SimpleLookupDTO> GetDepartments();
        IList<SimpleLookupDTO> GetClients();
    }
}
