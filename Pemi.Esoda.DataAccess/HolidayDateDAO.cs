using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Pemi.Esoda.DTO;
using System.Data.Common;
using System.Data;
using System.Collections.Specialized;

namespace Pemi.Esoda.DataAccess
{
    public class HolidayDateDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public void SetHolidayDate(HolidayDateDTO hd)
        {
            DbCommand cmd = db.GetStoredProcCommand("dbo.HolidayDateSet", hd.ID, hd.Date, hd.Description);
            db.ExecuteNonQuery(cmd);
        }

        public HolidayDateDTO GetHolidayDate(int hdID)
        {
            DbCommand cmd = db.GetStoredProcCommand("dbo.HolidayDateGet", hdID);

            HolidayDateDTO hd = new HolidayDateDTO();

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    hd.ID = int.Parse(dr["id"].ToString());
                    hd.Date = DateTime.Parse(dr["holidayDate"].ToString());
                    hd.Description = dr["description"].ToString();
                }
            }

            return hd;
        }

        public void DeleteHolidayDate(int? hdID)
        {
            DbCommand cmd = db.GetStoredProcCommand("dbo.HolidayDateDel", hdID);
            db.ExecuteNonQuery(cmd);
        }

        public IList<HolidayDateDTO> GetHolidayDateList(int year)
        {
            List<HolidayDateDTO> list = new List<HolidayDateDTO>();

            DbCommand cmd = db.GetStoredProcCommand("dbo.HolidayDateListGet", year);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    list.Add(
                        new HolidayDateDTO(
                            int.Parse(dr["id"].ToString()),
                            DateTime.Parse(dr["holidayDate"].ToString()),
                            dr["description"].ToString()));                    
                }
            }

            return list;
        }

        public IList<SimpleLookupDTO> GetAvailableYears()
        {
            List<SimpleLookupDTO> list = new List<SimpleLookupDTO>();

            DbCommand cmd = db.GetStoredProcCommand("dbo.HolidaDateAvailableYersGet");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                //list.Add(new SimpleLookupDTO(-2, "-- wybierz --"));
                while (dr.Read())
                {
                    list.Add(new SimpleLookupDTO(int.Parse(dr["year"].ToString()), dr["year"].ToString()));
                }
            }
            return list;
        }
    }
}
