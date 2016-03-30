using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class CustomerDAO
    {
        public IDataReader GetCustomerTypes()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaTypowInteresanta]");
            return db.ExecuteReader(cmd);
        }

        public IDataReader GetCustomersCategoriesByType(int typInteresanta)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[listaKategoriiInteresantowWgTypu]", typInteresanta);
            return db.ExecuteReader(cmd);
        }

        public void AddCustomerCategory(string name, int typeCat)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[dodajKategorieInteresanta]", typeCat, name);
            db.ExecuteNonQuery(cmd);
        }

        public void UpdateCustomerCategory(int id, int typeCat, string nazwa)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[edytujKategorieInteresanta]", id, typeCat, nazwa);
            db.ExecuteNonQuery(cmd);
        }

        public IDataReader GetCustomerCategory(int id)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzKategorieInteresanta]", id);
            return db.ExecuteReader(cmd);
        }

        public void RemoveCustomerCategory(int id)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[usunKategorieInteresanta]", id);
            db.ExecuteNonQuery(cmd);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataView FindCustomer(int idTypu, int idKategorii, string imie, string nazwisko, string nazwa,
            string miejscowosc, string kod, string ulica, string budynek, string lokal, string sortParam,string nip,string poczta,string numerSMS)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            if (imie == null) imie = string.Empty;
            if (nazwisko == null) nazwisko = string.Empty;
            if (nazwa == null) nazwa = string.Empty;
            if (miejscowosc == null) miejscowosc = string.Empty;
            if (kod == null) kod = string.Empty;
            if (ulica == null) ulica = string.Empty;
            if (budynek == null) budynek = string.Empty;
            if (lokal == null) lokal = string.Empty;
            if (sortParam == null) sortParam = string.Empty;
            if (nip == null)
                nip = string.Empty;
            if (poczta == null)
                poczta = string.Empty;
            if (numerSMS == null)
                numerSMS = string.Empty;

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[znajdzInteresanta]", idTypu, idKategorii, imie,
                nazwisko, nazwa, miejscowosc, kod, ulica, budynek, lokal,nip,poczta,numerSMS);
            cmd.CommandTimeout = 600;
            DataSet dsCustomers = db.ExecuteDataSet(cmd);
            if (dsCustomers.Tables.Count > 0)
            {
                DataView dvCustomers = new DataView(dsCustomers.Tables[0]);

                dvCustomers.Sort = sortParam;
                return dvCustomers;
            }
            else
                return null;
        }

        public IDataReader GetCustomer(int id)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Uzytkownicy].[pobierzInteresanta]", id);
            return db.ExecuteReader(cmd);
        }
    }
}