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
    public class OCRLogsDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public IList<OCRLogDTO> GetLogs()
        {
            return GetLogs(null);
        }
        
        public IList<OCRLogDTO> GetLogs(int? daysCount)
        {
            List<OCRLogDTO> items = new List<OCRLogDTO>();
            IDataReader idr;
            DbCommand cmd = db.GetStoredProcCommand("dbo.pOCRGetLogs");

            if (daysCount.HasValue)
                db.AddInParameter(cmd, "@days", DbType.Int32, daysCount.Value);
            else
                db.AddInParameter(cmd, "@days", DbType.Int32, 7);

            using(idr = db.ExecuteReader(cmd))
            {
                while (idr.Read())
                {
                    items.Add(new OCRLogDTO()
                    {
                        LogDate = idr.GetDateTime(0),
                        OCRed = idr.GetInt32(1),
                        UnOCRable = idr.GetInt32(2),
                        Total = idr.GetInt32(3),
                        ScansPagesOCRed = idr.GetInt32(4),
                        ScansRemainedToOCR = idr.GetInt32(5)
                    });
                }
            }
            return new Collection<OCRLogDTO>(items);
        }
    }
}
