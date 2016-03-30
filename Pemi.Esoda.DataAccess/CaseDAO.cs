using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;
using System.Data;

namespace Pemi.Esoda.DataAccess
{
    public class CaseDAO
    {
        public XmlReader GetCase(int caseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzDaneSprawy", caseId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr;
        }

        public IDataReader GetDataFromCase(int caseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzDaneZSprawy", caseId);
            return db.ExecuteReader(cmd);
        }

        public IDataReader GetCaseData(int caseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzSprawe", caseId);
            return db.ExecuteReader(cmd);
        }

        public XmlReader GetCaseDataXML(int caseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzDaneSprawyXML", caseId);
            return CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
        }

        public int GetCaseNumberFromDocument(int docId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[pobierzNumerSprawyZDokumentu]", docId);
            object elemId = db.ExecuteScalar(cmd);
            int pozId = 0;
            if (elemId != null && int.TryParse(elemId.ToString(), out pozId))
                return pozId;
            else
                return 0;
        }

        public XmlReader GetCaseHistory(int caseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzHistorie ", caseId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr;
        }

        public Collection<SimpleLookupDTO> GetBriefcaseList(Guid userId, int? year)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Sprawy.pobierzListeTeczek", userId, year))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0),string.Format("{0}",idr["tytul"].ToString())));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetAvailableYears()
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

           return new Collection<SimpleLookupDTO>(items);
       }

        public IDataReader GetBriefcaseForCase(int caseId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzTeczkeSprawy", caseId);
            return db.ExecuteReader(cmd);
        }

        public Collection<SimpleLookupDTO> GetCaseNumbers(int briefcaseId)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Sprawy.pobierzListeNumerowSprawZTeczki", briefcaseId))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), string.Format("{0}", idr.GetInt32(1))));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCaseKinds()
        {
            return GetCaseKinds(false);
        }

        public Collection<SimpleLookupDTO> GetCaseKinds(bool showInactive)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Sprawy.listaRodzajowSpraw", showInactive))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetStatusList()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("[Sprawy].[listaStatusow]"))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetOwnersList()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("[Uzytkownicy].[listaPracownikow]"))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr["nazwisko"].ToString() + " " + idr["imie"].ToString()));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public XmlReader GetDocumentDataForCase(int documentId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzDaneDokumentuDlaSprawy ", documentId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr;
        }

        public int AssignDocumentToNewCase(Guid userId, int documentId, int caseTypeId, int caseKindId, string description, DateTime? documentDate, string documentSignature, string sender)
        {
            Database db = DatabaseFactory.CreateDatabase();
            
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.przypiszDokumentDoNowejSprawy", userId, documentId, caseTypeId, caseKindId, description, documentDate, documentSignature, sender, null);
            cmd.CommandTimeout = 120;
            db.ExecuteNonQuery(cmd);
            object newCaseId = cmd.Parameters["@newCaseId"].Value;
            return (int)newCaseId;
        }

        public void AssignDocumentToExistingCase(Guid userId, int documentId, int caseId)
        {
            Database db = DatabaseFactory.CreateDatabase();
              DbCommand cmd = db.GetStoredProcCommand("Sprawy.przypiszDokumentDoIstniejacejSprawy", userId, documentId, caseId);
              cmd.CommandTimeout = 120;
              db.ExecuteNonQuery(cmd);

        }
         

        public XmlReader GetContainedDocuments(int caseId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzListeDokumentow", caseId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr;
        }

        public IDataReader GetCaseTypesList()
        {
            return DatabaseFactory.CreateDatabase().ExecuteReader("Sprawy.listaRodzajowSpraw");
        }

        public void UpdateCase(int id, string opis, string znakPisma, DateTime? dataRozpoczecia, DateTime? dataPisma, DateTime? dataZakonczenia, string uwagi, int status, int nadawca, int interesant, Guid idToz)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.aktualizujDaneSprawy", id, status, nadawca, opis, znakPisma,
              dataPisma == DateTime.MinValue ? null : dataPisma,
              dataRozpoczecia,
              dataZakonczenia == DateTime.MinValue ? null : dataZakonczenia, uwagi, idToz);
            db.ExecuteNonQuery(cmd);
        }

        public void RedirectCase(int caseId, Guid userId, string userName, string userFullname, string note,
            int organizationalUnitId, int employeeId, string ouName, string empName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            List<string> parameters = new List<string>();
            parameters.Add(caseId.ToString());
            parameters.Add(ouName);
            parameters.Add(empName);
            if (db.ExecuteNonQuery("[Akcje].[przekazanieSprawy]", caseId, organizationalUnitId, employeeId) > 0)
            {
                ActionLogger al = new ActionLogger(new ActionContext(new Guid("386D76E7-FAA0-4264-A722-BFB60ACCBD46"), userId, userName, userFullname, parameters));
                al.AppliesToCases.Add(caseId);
                al.ActionData.Add("notatka", note);
                al.ActionData.Add("wydzial", ouName);
                al.ActionData.Add("pracownik", empName);
                al.Execute();
            }
        }

        public bool IsCaseVisibleForUser(int caseId, Guid userId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Sprawy].[IsCaseVisibleForUser]", userId, caseId);
            DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd);
            return dr.Read();            
        }

        public IDataReader GetCaseRegistryItems(int caseId)
        {             
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzPozycjeSkojarzoneZeSprawa", caseId);
            return db.ExecuteReader(cmd);
        }

        //public IDataReader GetCaseMetricsHeader(int caseId)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

        //    DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzNaglowekMetrykiSprawy ", caseId);
        //    return db.ExecuteReader(cmd);
        //}
        //public IDataReader GetCaseMetrics(int caseId)
        //{
        //    Database db = DatabaseFactory.CreateDatabase();
        //    if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

        //    DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzMetrykeSprawy", caseId);
        //    return db.ExecuteReader(cmd);
        //}

        public string GetCaseSignature(int caseId)
        {            
            string caseSignature = string.Empty;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Sprawy.pobierzOznaczenieSprawy(@idSprawy)");
            db.AddInParameter(cmd, "@idSprawy", DbType.Int32);
            db.SetParameterValue(cmd, "@idSprawy", caseId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    caseSignature = dr.IsDBNull(0) ? string.Empty : dr.GetString(0);
                }
            };

            return caseSignature;
        }

        public SMSData GetSMSDataForCase(int caseID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (IDataReader dr = db.ExecuteReader("Akcje.pobierzDaneSprawyDoSMS", caseID))
                {
                    if (dr.Read())
                    {
                        return new SMSData()
                        {
                            PhoneNumber = dr["numerSMS"].ToString()
                            ,
                            Message = dr["wiadomosc"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return new SMSData();
        }

        public XmlReader GetCaseOrDocumentStatus(int registryNumber, int year)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.pobierzStatusSprawy", registryNumber,year);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr;
        }

        public int RemoveDocumentFromCase(int docID, Guid userGuid)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.usunDokumentZeSprawy", docID, userGuid);
            var rv = db.ExecuteScalar(cmd);

            return(int)rv;

        }
    }
    }