using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class BriefcaseDAO
    {
        public void InsertBriefcase(int? idJRWA, int? idRodzajuSprawy, string rodzajeSpraw, string prefix, string suffix, int rok, string tytul, int nastepnyNumer, string adresat, bool aktywna, bool archiwalna)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[dodajTeczke]", idJRWA, idRodzajuSprawy, rodzajeSpraw, prefix, suffix, rok, tytul, nastepnyNumer, adresat, aktywna, archiwalna);
            db.ExecuteNonQuery(cmd);
        }

        public void UpdateBriefcase(int id, int? idJRWA, int? idRodzajuSprawy, string rodzajeSpraw, string prefix, string suffix, int rok, string tytul, int nastepnyNumer, string adresat, bool aktywna, bool archiwalna)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[aktualizujTeczke]", id, idJRWA, idRodzajuSprawy, rodzajeSpraw, prefix, suffix, rok, tytul, nastepnyNumer, adresat, aktywna, archiwalna);
            db.ExecuteNonQuery(cmd);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataView GetBriefcaseList(string sortParam)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[listaTeczek2]");
            cmd.CommandTimeout = 600;
            DataView dvBrifcaseList = new DataView(db.ExecuteDataSet(cmd).Tables[0]);
            dvBrifcaseList.Sort = sortParam;
            return dvBrifcaseList;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataSet GetBriefcase(int id)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[pobierzTeczke]", id);
            cmd.CommandTimeout = 600;
            return db.ExecuteDataSet(cmd);
        }

        public XmlReader GetBriefcaseInfo(int briefcaseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.daneTeczki", briefcaseId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr;
        }

        public IDataReader GetBriefcaseYears()
        {
            return DatabaseFactory.CreateDatabase().ExecuteReader("Sprawy.listaLat");
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataSet GetBriefcasesFromYear(int year, Guid userId, string beginsWith, bool doPracownika)
        {
            return DatabaseFactory.CreateDatabase().ExecuteDataSet("Sprawy.listaTeczekZRoku", userId, year, doPracownika, beginsWith);
        }

        public XmlReader GetCasesFromBriefcase(int briefcaseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzListeSpraw ", briefcaseId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr;
        }

        public Collection<SimpleLookupDTO> GetCaseKindsFromBriefcase(int briefcaseId)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Sprawy.pobierzTeczke", briefcaseId))
            {
                if(idr.Read())
                {
                    string rodzajeSpraw = idr["rodzajeSpraw"].ToString();
                   
                    XmlDocument doc = new XmlDocument();
                    if (rodzajeSpraw.Length > 0)
                    {
                        doc.LoadXml(rodzajeSpraw);

                        foreach (XmlElement elem in doc.SelectNodes("//rodzajSprawy"))
                        {
                            lista.Add(new SimpleLookupDTO(int.Parse(elem.Attributes["id"].Value), elem.InnerText));
                        }
                    }
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCaseKindsFromBriefcaseXML(int briefcaseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzListeRodzajowSprawZTeczki", briefcaseId);

            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (XmlReader xdr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db))
            {
                while(xdr.Read())
                {
                    lista.Add(new SimpleLookupDTO(int.Parse(xdr["id"]), xdr.ReadElementContentAsString()));
                    

                    //XmlDocument doc = new XmlDocument();
                    //if (rodzajeSpraw.Length > 0)
                    //{
                    //    doc.LoadXml(rodzajeSpraw);

                    //    foreach (XmlElement elem in doc.SelectNodes("//rodzajSprawy"))
                    //    {
                    //        lista.Add(new SimpleLookupDTO(int.Parse(elem.Attributes["id"].Value), elem.InnerText));
                    //    }
                    //}
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public string GetBriefcaseGroups(Guid userId, string year, string from, string doPracownika,bool archive)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzGrupyTeczek", userId, year, doPracownika ,from,archive);
            try
            {
                
                XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
                if (xr.Read())
                {
                    XmlDocument teczki = new XmlDocument();
                    teczki.LoadXml(xr.ReadOuterXml());

                    int i = -1;
                    foreach (XmlNode node in teczki.SelectNodes("//teczka[@id=-1]"))
                    {
                        node.Attributes["id"].Value = (i--).ToString();
                    }

                    return teczki.OuterXml;
                }
                else
                    return "<teczki/>";
            }
            catch
            {
                return "<teczki/>";
            }            
        }

        public List<BriefcaseGroupItemDTO> GetBriefcaseGroup(int briefcaseId)
        {
            List<BriefcaseGroupItemDTO> items = new List<BriefcaseGroupItemDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzGrupeTeczki", briefcaseId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new BriefcaseGroupItemDTO(int.Parse(dr["id"].ToString()), dr["briefcaseName"].ToString(), bool.Parse(dr["isChecked"].ToString())));
                }
            }

            return items;
        }

        public void AssingBriefcaseToParent(int briefcaseId, int? parentId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.przypiszTeczkeDoRodzica", briefcaseId, parentId);

            db.ExecuteNonQuery(cmd);            
        }

        public DataView GetBriefcaseDetails(int briefcaseID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[pobierzSzczegolyTeczki]", briefcaseID);
            DataView dvBrifcaseList = new DataView(db.ExecuteDataSet(cmd).Tables[0]);            
            return dvBrifcaseList;
        }

        public bool IsBriefcaseGroupPossibility(int briefcaseID)
        {
            bool isPossibility = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Sprawy.czyGrupaTeczkiJestMozliwa(@teczkaID)");
            db.AddInParameter(cmd, "@teczkaID", DbType.Int32);
            db.SetParameterValue(cmd, "@teczkaID", briefcaseID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isPossibility = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            }

            return isPossibility;
        }

        public Guid? GetBriefcaseOwner (int briefcaseID)
        {
            Guid? owner = null;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Sprawy.pobierzWlascicielaTeczki(@idTeczki)");
            db.AddInParameter(cmd, "@idTeczki", DbType.Int32);
            db.SetParameterValue(cmd, "@idTeczki", briefcaseID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    owner = dr.IsDBNull(0) ? null : (Guid?)dr.GetGuid(0);
                }
            }

            return owner;
        }

        public List<BriefcaseItemDTO> GetBriefcasesForDepartment(int departmentID, int year)
        {
            List<BriefcaseItemDTO> items = new List<BriefcaseItemDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzListeTeczekDlaWydzialu", departmentID, year);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new BriefcaseItemDTO()
                    {
                        ID = int.Parse(dr["id"].ToString()),
                        Title = dr["tytul"].ToString(),
                        Year = int.Parse(dr["rok"].ToString()),
                        IsNewYearCopy = bool.Parse(dr["jestPowielonaNaNowyRok"].ToString())
                    });
                }
            }

            return items;
        }

        public int CreateNewYearBriefcase(int briefcaseID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.tworzTeczkeNaNowyRok", briefcaseID);
            int id = -1;

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    id = int.Parse(dr["id"].ToString());
                }
            }

            return id;
        }

        public void CreateNewYearChildBriefcase(int briefcaseID, int parentID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.tworzPodteczkeNaNowyRok", briefcaseID, parentID);
            int id = -1;

            db.ExecuteNonQuery(cmd);            
        }

        public void CreateNewYearBriefcaseWithChildren(int briefcaseID, int newYear)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.tworzTeczkeNaNowyRokWrazZPodteczkami", briefcaseID, newYear);
            

            db.ExecuteNonQuery(cmd);            
        }
       
        public string GetBriefcasesForDepartmentXML(int departmentID, int year)
        {
            try
            {
                SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
                if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

                DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzListeTeczekDlaWydzialuXml", departmentID, year);

                XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
                if (xr.Read())
                {
                    XmlDocument teczki = new XmlDocument();
                    teczki.LoadXml(xr.ReadOuterXml());
                    return teczki.OuterXml;
                }
                else
                {
                    return "<briefcases/>";
                }
            }
            catch
            {
                return "<briefcases/>";
            }
        }

        public List<SimpleLookupDTO> GetBriefcasesAvailableYears()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzDostepneLataTeczek");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new SimpleLookupDTO(int.Parse(dr["Value"].ToString()), dr["Text"].ToString()));
                }
            };

            return items;
        }
    }
}