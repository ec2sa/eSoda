using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Pemi.Esoda.DataAccess
{
    public class NoticeDAO
    {
        public NoticeDTO GetNotice(int? noticeID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            NoticeDTO notice = null;
            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.pobierzKomunikat", noticeID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    notice = new NoticeDTO(
                    int.Parse(dr["id"].ToString()),
                    dr["notice"].ToString(),
                    String.IsNullOrEmpty(dr["startDate"].ToString()) ? null : (DateTime?)(dr["startDate"]),
                    String.IsNullOrEmpty(dr["endDate"].ToString()) ? null : (DateTime?)(dr["endDate"]),
                    bool.Parse(dr["isActive"].ToString()));
                }
            };

            return notice;
        }

        public int? SetNotice(int? noticeID, string notice, DateTime? startDate, DateTime? endDate, bool isActive)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");
            
            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.zapiszKomunikat", noticeID, notice, startDate==DateTime.MinValue?null:startDate, endDate==DateTime.MinValue?null:endDate, isActive);
            int? returnNoticeID = null;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {                   
                    returnNoticeID = int.Parse(dr["id"].ToString());                    
                }
            };

            return returnNoticeID;
        }

        public string GetCurrentNotice()
        {            
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            string notice = null;

            DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.pobierzAktualnyKomunikat", DateTime.Now);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    notice = (string)dr["notice"];
                }
            }

            return notice;
        }
    }
}
