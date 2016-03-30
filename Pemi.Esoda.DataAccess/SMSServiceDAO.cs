using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Pemi.Esoda.DTO;
using System.Data;

namespace Pemi.Esoda.DataAccess
{
    public class SMSServiceDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public SMSServiceConfiguration GetSMSConfig()
        {
            using (IDataReader dr = db.ExecuteReader(CommandType.StoredProcedure, "dbo.PobierzKonfiguracjeSMS"))
            {
                if (dr.Read())
                {
                    return new SMSServiceConfiguration()
                    {
                        Username = dr["nazwaUzytkownika"].ToString()
                        ,
                        Password = dr["haslo"].ToString()
                        ,
                        Sender = dr["nadawca"].ToString()
                        ,
                        IsFlash = (bool)dr["czyFlash"]
                        ,
                        IsTest = (bool)dr["czyTest"]
                        ,
                        MessageTemplate=dr["szablon"].ToString()
                    };
                }
            }
            return null;
        }


        public bool SaveSMSConfig(SMSServiceConfiguration config)
        {
            try
            {
                db.ExecuteNonQuery("dbo.ZapiszKonfiguracjeSMS", config.Sender, config.Username, config.Password, config.IsFlash, config.IsTest,config.MessageTemplate);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return false;
        }

    }
}
