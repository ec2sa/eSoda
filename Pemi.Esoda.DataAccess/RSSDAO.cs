using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Xml;

namespace Pemi.Esoda.DataAccess
{
    public class RSSDAO
    {

        public string GetTicketforUser(Guid userID,int rssID)
        {   
                 SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
                 DbCommand cmd = db.GetStoredProcCommand("Akcje.getTicketForUser", userID, rssID);

                 return db.ExecuteScalar(cmd).ToString();
        }
        public XmlReader GetAwaitingDocuments(Guid ticket){
             SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
             DbCommand cmd = db.GetStoredProcCommand("Akcje.RSSOczekujacedokumenty", ticket,0);
             cmd.CommandTimeout = 600;
             return CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
         
        }

        public string GetRSSUrl()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            DbCommand cmd = db.GetStoredProcCommand("dbo.GetRSSUrl");
            return db.ExecuteScalar(cmd).ToString();
        }

        public void SetRSSUrl(string url)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            DbCommand cmd = db.GetStoredProcCommand("dbo.SetRSSUrl",url);
            db.ExecuteNonQuery(cmd);

        }
    }
}
