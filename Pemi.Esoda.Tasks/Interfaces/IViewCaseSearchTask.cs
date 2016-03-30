using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Tasks
{
	public interface IViewCaseSearchTask
	{
        IList<SearchCaseResultItem> GetCases(SearchCaseConditions sc, out int totalRowCount);
        IList<SimpleLookupDTO> GetDepartments();
        IList<SimpleLookupDTO> GetClients();
	}
}
