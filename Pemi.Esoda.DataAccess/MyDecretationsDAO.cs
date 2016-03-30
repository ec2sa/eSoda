using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Pemi.Esoda.DTO;
using System.Data.Common;
using System.Web;
using System.Web.Security;
using System.Data;

namespace Pemi.Esoda.DataAccess
{
    public class MyDecretationsDAO
    {
        private Database db = DatabaseFactory.CreateDatabase();

        public IList<MyDecretationsSearchResult> GetDecretations(MyDecretationsSearchConditions sc,out int totalRowCount,object userGuid)
        {
            IList<MyDecretationsSearchResult> items = new List<MyDecretationsSearchResult>();

            DbCommand cmd = db.GetStoredProcCommand("Wyszukiwarki.getDecretations",sc.SenderName,sc.RegistryNumber, sc.StartDate,sc.EndDate, sc.StartPage, sc.PageSize, userGuid);
            
            totalRowCount = 0;

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    MyDecretationsSearchResult res = new MyDecretationsSearchResult();
                    res.CreationDate = DateTime.Parse(dr["dataUtworzenia"].ToString());
                    res.DecretationDate = DateTime.Parse(dr["dataWykonania"].ToString());
                    res.CurrentDepartment = dr["wydzial"].ToString();
                    res.CurrentEmployee = dr["pracownik"].ToString();
                    res.DocumentID = int.Parse(dr["idDokumentu"].ToString());
                    res.DocumentSignature = dr["znakPisma"].ToString();
                    res.DocumentType = dr["rodzaj"].ToString();
                    res.OrdinalNumber = int.Parse(dr["lp"].ToString());
                    if (!dr.IsDBNull(1))
                        res.RegistryNumber = (int)dr["nrDziennika"];
                    res.SenderName = dr["interesant"].ToString();
                    res.Status = dr["status"].ToString();
                    res.TotalRows = (int)dr["totalRows"];
                    res.IsInvoice = dr["jestFaktura"].ToString();
                    totalRowCount = res.TotalRows;
                    items.Add(res);
                }
            }
            return items;
           
        }


    }
}
