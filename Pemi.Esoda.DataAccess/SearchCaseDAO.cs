using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;

namespace Pemi.Esoda.DataAccess
{
    public class SearchCaseDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public IList<SearchCaseResultItem> GetCases(SearchCaseConditions sc, out int totalRowCount)
        {
            List<SearchCaseResultItem> results = new List<SearchCaseResultItem>();
            
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            int? clientID = null;
            if (sc.ClientID != -1) clientID = sc.ClientID;
            int? departmentID = null;
            if (sc.DepartmentID != -1) departmentID = sc.DepartmentID;

            DbCommand cmd = db.GetStoredProcCommand("Wyszukiwarki.getCases", sc.StartPage, sc.PageSize, clientID, sc.ClientName, departmentID, sc.StartDate, sc.EndDate);

            totalRowCount = 0;

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    results.Add(new SearchCaseResultItem()
                    {
                        NumberItem = int.Parse(dr["number"].ToString()),
                        CaseNumber = dr["numerSprawy"].ToString(),
                        CaseType = dr["rodzajSprawy"].ToString(),
                        ClientName = dr["interesant"].ToString(),
                        PaperSymbol = dr["znakPisma"].ToString(),
                        PaperDate =  String.IsNullOrEmpty(dr["dataPisma"].ToString()) ? null : (DateTime?)Convert.ToDateTime(dr["dataPisma"]),
                        CaseStartDate = String.IsNullOrEmpty(dr["dataWszczeciaSprawy"].ToString()) ? null : (DateTime?)Convert.ToDateTime(dr["dataWszczeciaSprawy"]),
                        CaseEndDate = String.IsNullOrEmpty(dr["dataZakonczeniaSprawy"].ToString()) ? null : (DateTime?)Convert.ToDateTime(dr["dataZakonczeniaSprawy"]),
                        Remarks = dr["uwagi"].ToString(),
                        Department = dr["wydzial"].ToString(),
                        CaseID = int.Parse(dr["idSprawy"].ToString())
                    }
                    );
                }

                if (dr.NextResult())
                {
                    if (dr.Read())
                    {
                        totalRowCount = int.Parse(dr["totalRowCount"].ToString());
                    }
                }
            };

            return results;
        }

        public List<SimpleLookupDTO> GetClients()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();


            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Wyszukiwarki.getClients");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {         
                items.Add(new SimpleLookupDTO(-1, "-- wybierz --"));

                while (dr.Read())
                {
                    items.Add(new SimpleLookupDTO(int.Parse(dr["id"].ToString()), dr["name"].ToString()));
                }
            };

            return items;
        }

        public List<SimpleLookupDTO> GetDepartments()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();
            
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Wyszukiwarki.getDepartments");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                items.Add(new SimpleLookupDTO(-1, "-- wybierz --"));

                while (dr.Read())
                {
                    items.Add(new SimpleLookupDTO(int.Parse(dr["id"].ToString()), dr["name"].ToString()));
                }
            };

            return items;
        }
    }
}
