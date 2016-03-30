using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pemi.Esoda.DataAccess
{
    public class CaseListDAO
    {
        private ESodaDataContext ctx = new ESodaDataContext();

        private IQueryable<Cases> searchQuery(CaseListFilter filter)
        {
            var query = ctx.Cases.AsQueryable();

            if (filter.Year.HasValue)
                query = query.Where(c => c.Year == filter.Year);

            if (!string.IsNullOrEmpty(filter.Prefix))
                query = query.Where(c => c.Prefix.StartsWith(filter.Prefix));

            if (!string.IsNullOrEmpty(filter.Suffix))
                query = query.Where(c => c.Suffix.StartsWith(filter.Suffix));

            if (filter.IsActive.HasValue)
                query = query.Where(c => c.IsActive == filter.IsActive);

            if (filter.IsArchive.HasValue)
                query = query.Where(c => c.IsArchive == filter.IsArchive);

            return query;
        }

        public int GetCasesCount(CaseListFilter filter, int maximumRows, int startRowIndex)
        {
            return searchQuery(filter).Count();
        }

        public IQueryable<Cases> GetCases(CaseListFilter filter, int maximumRows, int startRowIndex)
        {
            return searchQuery(filter).OrderBy(c=>c.Title).Skip(startRowIndex).Take(maximumRows);
        }

    }

    public class CaseListFilter
    {
        public int? Year { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsArchive { get; set; }
       

        public CaseListFilter()
        {
            Year = DateTime.Today.Year;
            IsActive = true;
            IsArchive = false;
          
        }
    }
}
