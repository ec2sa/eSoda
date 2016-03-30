using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.ComponentModel;
using System.Data;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class ReportsDAO
    {
        public void CreateSubscription(string reportName, string reportParameters, int recipientID, string format)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Reports].[CreateSubscription]", reportName, reportParameters, recipientID, format);
            db.ExecuteNonQuery(cmd);
        }

        public void DeleteSubscription(int subscriptionID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Reports].[DeleteSubscription]", subscriptionID);
            db.ExecuteNonQuery(cmd);
        }

        public IDataReader GetSubscriptions()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Reports].[GetSubscriptions]");
            return db.ExecuteReader(cmd);
        }
    }
}
