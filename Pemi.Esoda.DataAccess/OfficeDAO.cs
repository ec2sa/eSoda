using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class OfficeDAO
    {
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void UpdateOfficeData(string PelnaNazwa, string TypUrzedu, string OrganKierujacy, string NIP,
            string REGON, string Miasto, string Ulica, string Budynek, string Lokal, string Telefon,
            string Fax, string WWW, string BIP, string Email)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("OfficeData");

            DbCommand cmd = db.GetStoredProcCommand("[dbo].[aktualizujDaneUrzedu]", PelnaNazwa
        , TypUrzedu  , OrganKierujacy  , NIP  , REGON   , Miasto  , Ulica , Budynek  , Lokal
        , Telefon  , Fax   , WWW   , BIP    , Email);
            db.ExecuteNonQuery(cmd);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataSet GetOfficeData()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("OfficeData");

            DbCommand cmd = db.GetStoredProcCommand("[dbo].[pobierzDaneUrzedu]");
            return db.ExecuteDataSet(cmd);
        }

        public string CheckDB()
        {
            string hasz = string.Empty;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("CheckDB");
            DbCommand cmd = db.GetStoredProcCommand("[dbo].[checkDB]");
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        hasz = dr.GetString(0);
                    }
                }
            }
            return hasz;
        }
    }
}
