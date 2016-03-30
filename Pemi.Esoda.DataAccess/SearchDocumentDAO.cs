using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Pemi.Esoda.DTO;
using System.Data;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Pemi.Esoda.DataAccess
{
    public class SearchDocumentDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public IList<SearchDocumentResultItem> GetDocuments(SearchDocumentConditions sc, out int totalRowCount)
        {
            List<SearchDocumentResultItem> results = new List<SearchDocumentResultItem>();
            totalRowCount = 0;

            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");
            
            DbCommand cmd = db.GetStoredProcCommand("Wyszukiwarki.getDocuments", sc.UserGuid, sc.HasExtendedSearchRole, 
                sc.StartPage, sc.PageSize, sc.DocumentCategory, sc.DocumentType, sc.DocumentNumber, sc.SystemNumber, sc.ClientName,
                sc.DocumentStartDate, sc.DocumentEndDate, sc.Mark, sc.Status, sc.Text, sc.SearchDescription, sc.SearchContent);
            cmd.CommandTimeout = 120;
            totalRowCount = 0;

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    results.Add(new SearchDocumentResultItem()
                    {
                        DocumentNumber = dr["registryNumber"].ToString(),
                        CreationDate = (DateTime)dr["creationDate"],
                        DocumentCategory = dr["categoryName"].ToString(),
                        DocumentType= dr["typeName"].ToString(),
                        SystemNumber = (int)dr["documentID"],
                        ClientName = dr["sender"].ToString(),
                        DocumentStatus = dr["status"].ToString(),
                        Mark = dr["referenceNumber"].ToString(),
                        Owner = dr["owner"].ToString(),
                        FoundInContent = dr["foundInContent"].ToString(),
                        FoundInDescription = dr["foundInDescription"].ToString(),
                        IsInContent = (bool)dr["isInContent"],
                        IsInDescription = (bool)dr["isInDescription"]
                    });
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

        public IList<SimpleLookupDTO> GetCategories()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Dokumenty.listaKategorii", false))
            {
                items.Add(new SimpleLookupDTO(-1, "-- dowolny --"));

                while (idr.Read())
                {
                    items.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(items);
        }

        public IList<SimpleLookupDTO> GetTypes(int categoryId)
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Dokumenty.listaRodzajow", categoryId, false))
            {
                items.Add(new SimpleLookupDTO(-1, "-- dowolny --"));

                while (idr.Read())
                {
                    items.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(2)));
                }
            }
            return new Collection<SimpleLookupDTO>(items);
        }

        public Collection<SimpleLookupDTO> GetStatuses()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Dokumenty.listaStatusow"))
            {
                //items.Add(new SimpleLookupDTO(-1, "-- wybierz --"));

                while (idr.Read())
                {
                    items.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(items);
        }

    }
}
