using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Xml;
using System.Data.Common;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;
using System.Data;
using System.IO;
using Pemi.Esoda.DataAccess.Utils;
using System.ComponentModel;

namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class DocumentDAO
    {
        public enum ESPDocumentStatus
        { 
            Awaiting=1, 
            Downloading=2, 
            Downloaded=3,
            Adding=4,
            Added=5
        };

        public XmlReader GetDocument(int documentId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzDokument", documentId);
            return CommonMethods.GetXmlReaderAndCloseConnection(cmd,db); // zamykany
        }

        public XmlReader GetDocumentDataXML(int documentId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzDaneDokumentuXML", documentId);
            return CommonMethods.GetXmlReaderAndCloseConnection(cmd,db); // zamykany
        }

        public int IsDocumentInAnyCase(int documentId) // returns caseId, if yes
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Sprawy.czyDokumentWSprawie", documentId);
            object cId = db.ExecuteScalar(cmd); // zamykany
            int caseId=-1;
            if (cId != null)
            {
                if (int.TryParse(cId.ToString(), out caseId))
                    return caseId;
                else
                    return -1;
            }
            return -1;
        }

        public Collection<DocumentItemDTO> GetItems(int documentId)
        {
            List<DocumentItemDTO> lista = new List<DocumentItemDTO>();
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzListeElementow", documentId))
            {
                while (dr.Read())
                {
                    lista.Add(new DocumentItemDTO(dr.GetGuid(0),dr.GetGuid(7), dr.GetString(1), dr.GetString(3), dr.GetString(2), dr.GetBoolean(4), dr.GetBoolean(5), dr.GetGuid(6),DateTime.Today));
                }
            }
            return new Collection<DocumentItemDTO>(lista);
        }

        //public Collection<DocumentItemDTO> GetItemsWithOldVersions_beta(int documentId)
        //{
        //    List<DocumentItemDTO> lista = new List<DocumentItemDTO>();
           
        //    Database db = DatabaseFactory.CreateDatabase();
        //    DocumentItemDTO newItem = null;
        //    using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzElementyDokumentuZHistoria", documentId))
        //    {
        //        while (dr.Read())
        //        {
        //            if (dr.GetGuid(7) == Guid.Empty)
        //            {
        //                newItem = new DocumentItemDTO(dr.GetGuid(0), dr.GetString(2), dr.GetString(4), dr.GetString(3), dr.GetBoolean(6), dr.GetBoolean(5), dr.GetGuid(7), dr.GetDateTime(1));
        //                lista.Add(newItem);
        //            }
        //            else
        //                newItem.PreviousVersions.Add(new DocumentItemDTO(dr.GetGuid(0), dr.GetString(2), dr.GetString(4), dr.GetString(3), dr.GetBoolean(6), dr.GetBoolean(5), dr.GetGuid(7), dr.GetDateTime(1)));
        //        }
        //    }
        //    return new Collection<DocumentItemDTO>(lista);
        //}

        public Collection<DocumentItemDTO> GetItemsWithOldVersions(int documentId)
        {
            List<DocumentItemDTO> lista = new List<DocumentItemDTO>();
            List<DocumentItemDTO> listaDzieci = new List<DocumentItemDTO>();
            Database db = DatabaseFactory.CreateDatabase();
            DocumentItemDTO newItem = null;
            Hashtable hashDocElements = new Hashtable();            

            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzElementyDokumentuZHistoria", documentId))
            {
                while (dr.Read())
                {
                    newItem = new DocumentItemDTO(dr.GetGuid(0),dr.GetGuid(8), dr.GetString(2), dr.GetString(4), dr.GetString(3), dr.GetBoolean(6), dr.GetBoolean(5), dr.GetGuid(7), dr.GetDateTime(1), (dr["rodzaj"].ToString()=="U"?DocumentItemCategory.Uploaded:DocumentItemCategory.Created));
                    if (newItem.OriginalItemID == Guid.Empty)
                    {
                        lista.Add(newItem);
                    }
                    else
                        listaDzieci.Add(newItem);
                }
            }

            IEnumerator listaEnum = lista.GetEnumerator();

            while (listaEnum.MoveNext())
            {
                IEnumerator listaDzieciEnum = listaDzieci.GetEnumerator();
                while (listaDzieciEnum.MoveNext())
                {
                    DocumentItemDTO parent = (DocumentItemDTO)listaEnum.Current;
                    DocumentItemDTO child = (DocumentItemDTO)listaDzieciEnum.Current;

                    if (child.OriginalItemID == parent.ID)
                    {
                        parent.PreviousVersions.Add(child);
                        listaDzieci.Remove(child);
                        listaDzieciEnum = listaDzieci.GetEnumerator();
                    }
                }
            }

            return new Collection<DocumentItemDTO>(lista);
        }

        public XmlReader GetDocumentHistory(int documentId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzHistorie", documentId);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd,db);
            return xr; // zamykany
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DataView GetDocumentsESPList(string sortParam)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzDokumentyESP]");
            DataSet dsDocESP = db.ExecuteDataSet(cmd);
            DataView dvDocESP = null;
            if (dsDocESP.Tables.Count > 0)
            {
                dvDocESP = new DataView(dsDocESP.Tables[0]);
                dvDocESP.Sort = sortParam;
            }
            return dvDocESP;
        }

        public bool ESPDocExist(string docId)
        {
            bool result = false;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[czyJestDokumentESP]", docId);
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                result = dr.Read();
            }
            return result;
        }

        public string GetESPDocId(Guid espGuid)
        {
            string result = string.Empty;
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzIdDokumentuESP]", espGuid);
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        result = dr.GetString(0);
                    }
                }
            }
            return result;
        }

        public void AddAwaitingESPDocument(string docId, string nazwa, string opis)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[dodajDokumentESP]", docId, nazwa, opis);
            db.ExecuteNonQuery(cmd);
        }

        public void SetESPDocumentStatus(Guid docId, ESPDocumentStatus status)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[ustawStatusDokumentuESP]", docId, status);
            db.ExecuteNonQuery(cmd);
        }

        public void SetESPDocumentData(Guid docId, string xmlData, string xslData, string attachmets)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[ustawDaneDokumentuESP]", docId, xmlData, xslData, attachmets);
            db.ExecuteNonQuery(cmd);
        }

        public IDataReader GetESPDocumentData(Guid ?docId)
        {
            if (docId != null)
            {
                SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
                if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

                DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzDokumentESP]", docId);
                return db.ExecuteReader(cmd);
            }
            else
                return null;
        }

        public void DeleteESPDocument(Guid docId, int newDocId)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[usunDokumentESP]", docId, newDocId);
            db.ExecuteNonQuery(cmd);
        }

        public IDataReader GetDocumentXMLXSL(int ?docId)
        {
            if (docId != null)
            {
                Database db = DatabaseFactory.CreateDatabase();
                if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzXMLiXSLDokumentu", (int)docId);
                return db.ExecuteReader(cmd);
            }
            else
                return null;
        }

        public IDataReader GetDocumentXMLXSLFromReg(int? itemId)
        {
            if (itemId != null)
            {
                Database db = DatabaseFactory.CreateDatabase();
                if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzXMLiXSLDokumentuZRejestru", (int)itemId);
                return db.ExecuteReader(cmd);
            }
            else
                return null;
        }

        public IDataReader GetDocumentRegistryItems(int documentId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzPozycjeSkojarzoneZDokumentem", documentId);
            return db.ExecuteReader(cmd);
        }

        public DocumentItemDTO GetItem(Guid itemID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzDaneElementu", itemID))
            {
                if (dr.Read())
                {
                    return new DocumentItemDTO(dr.GetGuid(0),dr.GetGuid(6), dr.GetString(1), dr.GetString(2), dr.GetString(4), dr.GetBoolean(5), dr.GetBoolean(3));
                }
               
            } 
            return null;
        }

        public IDataReader GetDocumentCategories()
        {
            return GetDocumentCategories(false);
        }

        public IDataReader GetDocumentCategories(bool wszystkie)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[listaKategorii]", wszystkie);
            return db.ExecuteReader(cmd);
        }

        public int AddNewDocument(Guid userId, string metadataXml)
        {
            Database db = DatabaseFactory.CreateDatabase();
            return (int)db.ExecuteScalar("[Dokumenty].[dodajNowyPustyDokument]", userId, metadataXml);
        }

        public bool AddDocCategory(string nazwa, string skrot, bool aktywna)
        {
            bool result = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[dodanieKategorii]", nazwa, skrot);
            try
            {
                db.ExecuteNonQuery(cmd);
                result = true;
            }
            catch 
            {
                result = false;
            }
            return result;
        }

        public bool UpdateDocCategory(int id, string nazwa, string skrot, bool aktywna)
        {
            bool result = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[edycjaKategorii]", id, nazwa, skrot, aktywna);
            try
            {
                db.ExecuteNonQuery(cmd);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public IDataReader GetCategory(int catId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzKategorie]", catId);
            return db.ExecuteReader(cmd);
        }

      public IDataReader GetCaseAndBriefcase(int documentId)
      {
        Database db = DatabaseFactory.CreateDatabase();
        if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

        DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzSpraweiTeczkeDokumentu", documentId);
        return db.ExecuteReader(cmd);
      }

      public IDataReader GetDocTypesForCategory(int catId)
      {
          return GetDocTypesForCategory(catId, false);
      }
     
        public IDataReader GetDocTypesForCategory(int catId, bool wszystkie)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[listaRodzajow]", catId, wszystkie);
            return db.ExecuteReader(cmd);
        }

        public IDataReader GetDocTypeForCategory(int catId, int docId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobiezRodzajDokumentu]", catId, docId);
            return db.ExecuteReader(cmd);
        }

        public bool AddDocTypeForCategory(int catId, string nazwa, string skrot, bool aktywna)
        {
            bool result = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            try
            {
                DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[dodanieRodzaju]", catId, nazwa, skrot);
                db.ExecuteReader(cmd);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public void UpdateDocTypeForCategory(int catId, int typeId, string nazwa, string skrot, bool aktywna)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[edycjaRodzaju]", typeId, catId, nazwa, skrot);
            db.ExecuteReader(cmd);
        }

        public bool AddNewDocumentItem(int documentId, string originalName, string description, Stream content, string mimeType, ref Guid itemGuid,DocumentItemCategory uploadMode)
        {
            IItemStorage storage = ItemStorageFactory.Create();
            Guid itemId = storage.Save(content);
          
            Database db = DatabaseFactory.CreateDatabase();
            bool browsable = (mimeType == "image/tiff");
            try
            {
                object res=db.ExecuteScalar("[Dokumenty].[dodajNowyElementDokumentu]", documentId, itemId, originalName, description, mimeType, false, browsable,uploadMode);
                itemGuid = res != null ? (Guid)res : Guid.Empty;
                return true;
            }
            catch
            {
                storage.Delete(itemId);
                return false;
            }
        }

        public bool AddNewVersionOfDocumentItem(int documentId, Guid originalItemId, string description, Stream content, string mimeType, string name, DocumentItemCategory uploadMode)
        {
            IItemStorage storage = ItemStorageFactory.Create();
            Guid itemId = storage.Save(content);
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                db.ExecuteNonQuery("[Dokumenty].dodajNowaWersjeElementuDokumentu", documentId, itemId, originalItemId, description, mimeType, name,uploadMode);
                return true;
            }
            catch
            {
                storage.Delete(itemId);
                return false;
            }
        }

        public Guid CheckIfDocumentItemExists(string documentId, string originalName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object result = db.ExecuteScalar("Dokumenty.czyIstniejeJuzPlik", documentId, originalName);
            if (result == null)
                return Guid.Empty;
            else
                return (Guid)result;
        }

        public void UpdateDocRecvStatus(Guid userId, int docId, int status)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[ustawStatusOdbioru]", userId, docId, status);
            db.ExecuteNonQuery(cmd);
        }

        public bool IsDocVisibleForUser(int docId, Guid userId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[IsDocVisibleForUser]", userId, docId);
            DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd);
            return dr.Read();
        }

        public int GetRegistryPositionForDocument(int docId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Couldn't connect to database");

            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzPozycjeRejestru]", docId);
            object elemId = db.ExecuteScalar(cmd);
            int pozId = -1;
            if (elemId != null && int.TryParse(elemId.ToString(), out pozId))
                return pozId;
            else
                return -1;
        }

        public int CopyDocument(Database db, DbTransaction tran, int idDokumentu, Guid userId, bool kopia, int idWydzialu, int idPracownika, bool skany, bool historia)
        {                                                        
            object copyId = db.ExecuteScalar(tran, "DOkumenty.TworzKopie", idDokumentu, userId, kopia, idWydzialu, idPracownika, skany, historia);
            int docCopyId = -1;
            if (copyId != null && int.TryParse(copyId.ToString(), out docCopyId))
                return docCopyId;
            else
                return -1;
        }

        public bool IsCopy(int docId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Couldn't connect to database");


            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[czyKopia]", docId);
            object o = db.ExecuteScalar(cmd);
            bool isCopy;

            if (o != null && bool.TryParse(o.ToString(), out isCopy))
            {
                return isCopy;
            }
            else
            {
                return false;
            }                
        }

        public Guid GetDocumentItemStorageID(Guid imageID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object res=db.ExecuteScalar("Dokumenty.pobierzFSGUID", imageID);
            return res != null ? new Guid(res.ToString()) : Guid.Empty;

        }
		public DokumentDetails GetDocumentDetails(int docId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Couldn't connect to database");

            DokumentDetails dd = null;

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzDodatkoweDane", docId);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    dd = new DokumentDetails(bool.Parse(dr["isCopy"].ToString()), dr["description"].ToString(), dr["notice"].ToString(), bool.Parse(dr["isInCase"].ToString()));                    
                }
            }

            return dd;
        }

        public bool IsCustomFormVisible(int docId)
        {
            bool isVisible = false;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Dokumenty.czyFormularzJestWidoczny(@docId)");
            db.AddInParameter(cmd, "@docId", DbType.Int32);
            db.SetParameterValue(cmd, "@docId", docId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isVisible = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            }

            return isVisible;
            
        }
       

           public bool CanCreateLegalAct(int typeId)
        {
            bool canCreate = false;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Dokumenty.czyMoznaTworzycAktyPrawne(@idRodzaju)");
            db.AddInParameter(cmd, "@idRodzaju", DbType.Int32);
            db.SetParameterValue(cmd, "@idRodzaju", typeId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    canCreate = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            };

            return canCreate;

        }


        public bool CanCreateMSOForm(int typeId)
        {
            bool canCreate = false;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Dokumenty.czyMoznaTworzycFormularzMSO(@idRodzaju)");
            db.AddInParameter(cmd, "@idRodzaju", DbType.Int32);
            db.SetParameterValue(cmd, "@idRodzaju", typeId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    canCreate = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            };

            return canCreate;

        }

        public bool CanCreateMSOTemplate()
        {
            bool canCreate = false;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Dokumenty.czyMoznaTworzycSzablonMSO()");
           
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    canCreate = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            };

            return canCreate;

        }

        public CustomFormVisibilityDTO GetCustomFormVisibility(int docId)
        {
            CustomFormVisibilityDTO v = null;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzWidocznoscWidokowFormularza", docId);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    v = new CustomFormVisibilityDTO(
                        bool.Parse(dr["isCustomFormVisible"].ToString()),
                        bool.Parse(dr["isWordFormVisible"].ToString()),
                        bool.Parse(dr["isWordFormEditVisible"].ToString()),
                        bool.Parse(dr["isXmlVisible"].ToString()),
                        bool.Parse(dr["isHistory"].ToString()),
                        bool.Parse(dr["isLegalActXmlVisible"].ToString()),
                        bool.Parse(dr["isLegalActHistoryVisible"].ToString()),
                        bool.Parse(dr["isSendToEPUAPVisible"].ToString())
                        );
                }
            };

            return v;
        }

        public bool IsCustomFormFilled(int docId)
        {
            bool isFilled = false;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Dokumenty.czyFormularzWypelniony(@docId)");
            db.AddInParameter(cmd, "@docId", DbType.Int32);
            db.SetParameterValue(cmd, "@docId", docId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isFilled = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            };
            return isFilled;
        }

        public int GetDocumentIDForGuid(Guid documentGuid)
        {
            Database db = DatabaseFactory.CreateDatabase();

            return (int)db.ExecuteScalar("Dokumenty.pobierzID", documentGuid);
        }

        public string GetDocumentEPUAPResponseAddress(int documentID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            return (string)db.ExecuteScalar("Dokumenty.pobierzAdresOdpowiedziEPUAP", documentID);
        }

        public bool IsConfirmationNeeded(int docId)
        {
            bool confirmationNeeded = false;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select [Dokumenty].[czyDoWiadomosci](@docId)");
            db.AddInParameter(cmd, "@docId", DbType.Int32);
            db.SetParameterValue(cmd, "@docId", docId);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    confirmationNeeded = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            };

            return confirmationNeeded;
        }

        public bool ConfirmReading(int docId)
        {
            Database db=DatabaseFactory.CreateDatabase();
            try
            {
                db.ExecuteNonQuery("Dokumenty.potwierdzZapoznanie", docId);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
             
        }

        public XmlReader GetDataMatrix(int docId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

          
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.GetDataMatrix", docId);
            return CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);

        }

        public bool SetDataMatrix(int docId, Guid userId, XmlReader content)
        {
            return SetDataMatrix(docId, userId, content, false);
        }

        public bool SetDataMatrix(int docId, Guid userId,XmlReader content,bool generateCodeOnly)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.SetDataMatrix", docId, userId, content,generateCodeOnly);
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return false;
            }
        }

        public List<DataMatrixHistoryItem> GetDataMatrixHistory(int docID)
        {
            List<DataMatrixHistoryItem> items = new List<DataMatrixHistoryItem>();

            Database db = DatabaseFactory.CreateDatabase();

            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.getDataMatrixHistory", docID);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    while (dr.Read())
                    {
                        items.Add(
                            new DataMatrixHistoryItem()
                            {
                                Date=(DateTime)dr["DATA"]
                                ,Content=dr["dane"].ToString()
                            }
                            );
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return null;
            }
        }
    }

    public class DataMatrixHistoryItem
    {
        public DateTime Date { get; set; }
        public string Content { get; set; }

    }

   
}