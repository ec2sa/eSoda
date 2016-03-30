using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class LoginHistoryDAO
    {
        public void NotifyLogin(string username, string remoteIP, string status, string zone)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Rejestr logowania");

            if (username == null || username.Equals(""))
                username = "(nieznany u¿ytkownik)";
            if (remoteIP == null || remoteIP.Equals(""))
                remoteIP = "(nieznane IP)";
            if (status == null || status.Equals(""))
                status = "(nieznany status)";
            if (zone == null || zone.Equals(""))
                zone = "(nieznana strefa)";

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[rejestrujLogowanie]", username, remoteIP, status, zone);
            db.ExecuteNonQuery(cmd);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataView GetLoginHistory(string sortParam)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Rejestr logowania");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzHistorieLogowania]");
            cmd.CommandTimeout = 600;
            DataSet dsLoginHistory = db.ExecuteDataSet(cmd);
            DataView dvLoginHistory=null;
            if(dsLoginHistory.Tables.Count>0)
            {
                dvLoginHistory = new DataView(dsLoginHistory.Tables[0]);
                dvLoginHistory.Sort = sortParam;
            }
            return dvLoginHistory;
        }
    }
}
