using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Pemi.Esoda.DataAccess
{
    public class SearchDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public bool isShowDecretactionLink(Guid userGuid)
        {
            bool isShow = false;

            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Wyszukiwarki.isShowDecretationLink(@idTozsamosci)");
            db.AddInParameter(cmd, "@idTozsamosci", System.Data.DbType.Guid);
            db.SetParameterValue(cmd, "@idTozsamosci", userGuid);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                   isShow = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }

            };

            return isShow;
        }
    }
}
