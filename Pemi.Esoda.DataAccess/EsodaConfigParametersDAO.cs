using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;

namespace Pemi.Esoda.DataAccess
{
    public class EsodaConfigParametersDAO
    {

        public Dictionary<string, string> GetConfig()
        {
            Database db = DatabaseFactory.CreateDatabase();
            Dictionary<string,string> items=new Dictionary<string,string>();
            using (IDataReader dr = db.ExecuteReader("dbo.getConfigParams"))
            {
                while (dr.Read())
                {
                    items.Add(dr["nazwa"].ToString(), dr["wartosc"].ToString());
                }
            }
            return items;
        }

        public void SetConfigParam(string name, string value)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd = db.GetStoredProcCommand("dbo.setConfigParam", name, value);
            db.ExecuteNonQuery(cmd);
        }
    }

}
