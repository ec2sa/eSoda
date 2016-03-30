using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data;
using System.Data.Common;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;
using System.Xml.XPath;
using System.IO;
using System.ComponentModel;
using System.Web.Security;
using Pemi.Esoda.DataAccess.SkanyDatasetTableAdapters;
using System.Data.SqlClient;


namespace Pemi.Esoda.DataAccess
{
    [DataObject]
    public class RegistryDAO
    {
        public XmlReader GetItemsPage(Guid userId, int registryId, int startingIndex, int pageSize, DateTime startDate, DateTime endDate,
            string searchIncomeDate, string searchDocumentDate, string searchDocumentNumber, string searchSenderName,
            int searchCorrespondenceCategory, int searchCorrespondenceType, string searchCategoryValue,
            int searchCorrespondenceKind, string searchTypeValue, int searchCorrespondenceStatus,
            int searchCorrespondenceDept, int searchCorrespondenceWorker, out int totalItemsCount, bool isRF)
        {
            //System.Diagnostics.Debug.WriteLine("strtingindex: " + startingIndex.ToString());
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzStroneDziennika", userId, registryId, startingIndex, pageSize,
                startDate, endDate, null, searchIncomeDate, searchDocumentDate, searchDocumentNumber, searchSenderName,
                searchCorrespondenceCategory, null, searchCorrespondenceKind, searchCategoryValue, searchCorrespondenceType, searchTypeValue,
                searchCorrespondenceStatus, searchCorrespondenceDept, searchCorrespondenceWorker, isRF);

            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
            totalItemsCount = (int)cmd.Parameters["@liczbaRekordow"].Value;
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);

            return xr; // zamykany
        }

        public XmlReader GetItemsPage(Guid userId, int registryId, int startingIndex, int pageSize, DateTime startDate, DateTime endDate, bool showInvoices, out int totalItemsCount)
        {
            //System.Diagnostics.Debug.WriteLine("strtingindex: " + startingIndex.ToString());
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzStroneDziennikaSimple", userId, registryId, startingIndex, pageSize, startDate, endDate, null, showInvoices);
            
            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
            totalItemsCount = (int)cmd.Parameters["@liczbaRekordow"].Value;
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public XmlReader GetItemsPage(Guid userId, int registryId, int startingIndex, int pageSize, DateTime startDate, DateTime endDate, int year, out int totalItemsCount)
        {
            //System.Diagnostics.Debug.WriteLine("strtingindex: " + startingIndex.ToString());
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzStroneDziennikaSimple_withYear", userId, registryId, startingIndex, pageSize, startDate, endDate, year, null);

            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
            totalItemsCount = (int)cmd.Parameters["@liczbaRekordow"].Value;
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public XmlReader GetItemsPageItem(int registryId, int itemId, bool isRF)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzPozycjeStronyDziennika]", registryId, itemId, isRF);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public XmlReader GetItemHistory(int registryId, int itemId, bool isRF)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzHistoriePozycji", registryId, itemId, isRF);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public XmlReader GetUnassignedScans()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzNieprzypisaneSkany");
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public SkanyDataset GetUnassignedScansDataSet(string documentsDirectory)
        {
            SkanyTableAdapter taSkany = new SkanyTableAdapter();
            taSkany.Connection = (SqlConnection)DatabaseFactory.CreateDatabase().CreateConnection();
            SkanyDataset dsSkany = new SkanyDataset();
            taSkany.Fill(dsSkany.Skany, documentsDirectory);

            return dsSkany;
            //SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            //if (db == null) throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            //DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzNieprzypisaneSkanyDataset");
            //ScansDataset dsScans = (ScansDataset)db.ExecuteDataSet(cmd);
            //return dsScans;
        }

        public Collection<DocumentItemDTO> GeItemsScans(int registryId, int itemId, bool isRF)
        {
            List<DocumentItemDTO> lista = new List<DocumentItemDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("[Rejestry].[pobierzSkanySkojarzoneZPozycjaDziennika]", registryId, itemId, isRF))
            {
                while (idr.Read())
                {
                    lista.Add(new DocumentItemDTO(idr.GetGuid(0), idr.GetGuid(11), idr.GetString(3), idr.GetString(5), idr.GetString(4), idr.GetBoolean(7), idr.GetBoolean(6)));
                }
            }
            return new Collection<DocumentItemDTO>(lista);
        }

        public XmlReader GetItem(int itemId, int registryId)
        {
            return GetItem(itemId, registryId, false);
        }
        public XmlReader GetItem(int itemId, int registryId, bool isRF)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzPozycjeDziennika", itemId, registryId, isRF);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public int[] AcquireItemID(int registryId, Guid userId, bool isRF)
        {
            return AcquireItemID(registryId, userId, null, isRF);
        }

        public int[] AcquireItemID(int registryId, Guid userId)
        {
            return AcquireItemID(registryId, userId, null, false);
        }

        public int[] AcquireItemID(int registryId, Guid userId, string ePUAPResponseAddress, bool isRF)
        {
            Database db = DatabaseFactory.CreateDatabase();
            int[] rv = null;
            if (isRF)
            {
                using (IDataReader dr = db.ExecuteReader("Rejestry.rezerwacjaNumeruDziennikaRF", registryId, userId))
                {
                    rv = new int[2];
                    if (dr.Read())
                    {
                        rv[0] = dr.GetInt32(0);
                        rv[1] = dr.GetInt32(1);
                    }

                }
            }
            else
            {
                using (IDataReader dr = db.ExecuteReader("Rejestry.rezerwacjaNumeruDziennika", registryId, userId, ePUAPResponseAddress))
                {
                    rv = new int[2];
                    if (dr.Read())
                    {
                        rv[0] = dr.GetInt32(0);
                        rv[1] = dr.GetInt32(1);
                    }

                }
            }
            return rv;
        }

        public Collection<SimpleLookupDTO> GetDocumentTypes(int categoryId)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Dokumenty.listaRodzajow", categoryId, false))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(2)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetEmployees(int organizationalUnitId)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Uzytkownicy.listaPracownikowKomorkiOrganizacyjnej", organizationalUnitId))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCustomers(int customerType)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Uzytkownicy.listaInteresantow", customerType))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetDocumentCategories()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Dokumenty.listaKategorii", false))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCorrespondenceTypes()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Rejestry.pobierzListeRodzajowKorespondencji"))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCorrespondenceStatus()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Dokumenty.listaStatusowOdbioru"))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCorrespondenceDepts()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("[Uzytkownicy].[listaKomorekOrganizacyjnych]"))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetCorrespondenceWorkers(int deptId)
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();

            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("[Uzytkownicy].[listaPracownikowKomorkiOrganizacyjnej]", deptId))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }



            return new Collection<SimpleLookupDTO>(lista);
        }

        public Collection<SimpleLookupDTO> GetOrganizationalUnits()
        {
            List<SimpleLookupDTO> lista = new List<SimpleLookupDTO>();
            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Uzytkownicy.listaKomorekOrganizacyjnych"))
            {
                while (idr.Read())
                {
                    lista.Add(new SimpleLookupDTO(idr.GetInt32(0), idr.GetString(1)));
                }
            }
            return new Collection<SimpleLookupDTO>(lista);
        }

        public int CreateCustomer(CustomerDTO customer)
        {
            Database db = DatabaseFactory.CreateDatabase();
            int id;
            if (customer.Name == null)
                id = (int)db.ExecuteScalar("Uzytkownicy.dodanieNowegoInteresanta", customer.CustomerTypeId, customer.CustomerCategory, customer.Address.PostalCode, customer.Address.City, customer.Address.Post, customer.Address.Street, customer.Address.Building, customer.Address.Flat, customer.LastName, customer.FirstName, customer.Nip, customer.NumberSMS);
            else
                id = (int)db.ExecuteScalar("Uzytkownicy.dodanieNowegoInteresanta", customer.CustomerTypeId, customer.CustomerCategory, customer.Address.PostalCode, customer.Address.City, customer.Address.Post, customer.Address.Street, customer.Address.Building, customer.Address.Flat, customer.Name, null, customer.Nip, customer.NumberSMS);
            return id;
        }

        public int GetCurrentRegistryId(int definitionId, int year)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            //return (int)db.ExecuteScalar("Rejestry.pobierzIdAktualnegoRejestru", definitionId, year);

            int ret = 0;

            using (IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Rejestry.pobierzIdAktualnegoRejestru", definitionId, year))
            {
                if (idr.Read())
                {
                    ret = idr.GetInt32(0);
                }
            }

            return ret;
        }

        public int SaveItem(int registryId, int itemNumber, string itemContent, bool isRF)
        {
            object obj = null;

            try
            {
                Database db = DatabaseFactory.CreateDatabase();
                if (db == null)
                    throw new Exception("B³¹d po³¹czenia z baz¹ danych");

                DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[zapiszPozycjeDziennika]", registryId, itemNumber, itemContent, isRF);
                cmd.CommandTimeout = 120;
                obj = db.ExecuteScalar(cmd);
                if (obj != null)
                    return (int)obj;
                else
                    return -3;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} - {1} - [{2}]", registryId, itemNumber, itemContent), ex);
            }
        }

        public void AddNewScan(int ownerID, int registryID, int itemNumber, string documentName, string documentDescription, Guid elementGuid, string originalElementName, string elementDescription, string mimeType, bool isMain, bool isBrowsable, string metadane, bool isRF)
        {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("Rejestry.dodanieNowegoSkanu", ownerID, registryID, itemNumber, documentName, documentDescription,
                elementGuid, originalElementName, elementDescription, mimeType, isMain, isBrowsable, metadane, isRF);
        }

        public void ReleaseScan(string scanID)
        {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("Dokumenty.zwolnijElement", new Guid(scanID));
        }

        public void AddExistingScan(int registryID, int itemNumber, Guid elementGuid,
           string elementDescription, bool isMain, bool isRF)
        {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("[Rejestry].[dodanieIstniejacegoSkanu]", registryID, itemNumber,
              elementGuid, elementDescription, isMain, isRF);
        }
        public void SaveScanChanges(string scanID, string description, bool isMain)
        {
            DatabaseFactory.CreateDatabase().ExecuteNonQuery("Dokumenty.zmienOpisElementu", new Guid(scanID), description, isMain);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public XmlReader GetRegistryItems(Guid userId, int registryId, int startRow, int pageSize, out int totalRows)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzStroneRejestru]", userId, registryId, startRow, pageSize);
            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
            totalRows = (int)db.GetParameterValue(cmd, "@RETURN_VALUE");
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr;
        }

        public XmlReader GetRegistrySearchItems(Guid userId, int registryId, int startRow, int pageSize, string criteria, out int totalRows)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");
            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[wyszukajWRejestrze]", userId, registryId, startRow, pageSize, criteria);
            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
            totalRows = (int)db.GetParameterValue(cmd, "@RETURN_VALUE");
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);
            return xr; // zamykany
        }

        public XmlReader GetRegistryItemsForCriteria(int registryId, int startRow, int pageSize, params string[] searchCriteria)
        {
            StringBuilder sbXpath = new StringBuilder();

            IDataReader idr = DatabaseFactory.CreateDatabase().ExecuteReader("Rejestry.pobierzDaneRejestru", registryId);
            if (!idr.Read())
                return null;
            string definicja = idr["definicja"].ToString();
            idr.Close();

            XPathNavigator xpnDef = new XPathDocument(new StringReader(definicja)).CreateNavigator();
            XPathNodeIterator xpiDef = xpnDef.Select("/definicjaRejestru/wyszukiwanie/kryterium");
            sbXpath.Append("/pozycje/pozycja");

            while (xpiDef.MoveNext())
            {
                if (searchCriteria[xpiDef.CurrentPosition - 1].Length > 0)
                {
                    if (sbXpath.Length > 16)
                        sbXpath.Append(" and ");
                    else
                        sbXpath.Append("[");

                    sbXpath.AppendFormat(xpiDef.Current.SelectSingleNode("@xpath").Value, String.Format("\"{0}\"", searchCriteria[xpiDef.CurrentPosition - 1]));
                }
            }

            if (sbXpath.Length > 16)
                sbXpath.Append("]");
            string xpathExpression = sbXpath.ToString();
            int totalRows;
            XPathNavigator xpn = new XPathDocument(GetRegistryItems(new Guid(Membership.GetUser().ProviderUserKey.ToString()), registryId, startRow, pageSize, out totalRows)).CreateNavigator();
            XPathNodeIterator xpi = xpn.Select(xpathExpression);
            StringBuilder sbOut = new StringBuilder();
            sbOut.Append("<pozycje>");
            sbOut.Append(xpnDef.SelectSingleNode("/definicjaRejestru/pola").OuterXml);//dodane aby byla spojna struktura xml-a
            while (xpi.MoveNext())
            {
                sbOut.Append(xpi.Current.OuterXml);
            }
            sbOut.Append("</pozycje>");
            return XmlReader.Create(new StringReader(sbOut.ToString())); // zamykany

        }

        public IDataReader GetRegistryYears()
        {
            return DatabaseFactory.CreateDatabase().ExecuteReader("Rejestry.listaLat");
        }

        public void UpdateCustomer(CustomerDTO customer)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd;
            if (customer.Name == null)
                cmd = db.GetStoredProcCommand("Uzytkownicy.aktualizujInteresanta", customer.ID, customer.CustomerTypeId, customer.CustomerCategory, customer.Address.PostalCode, customer.Address.City, customer.Address.Post, customer.Address.Street, customer.Address.Building, customer.Address.Flat, customer.LastName, customer.FirstName, customer.Nip, customer.NumberSMS);
            else
                cmd = db.GetStoredProcCommand("Uzytkownicy.aktualizujInteresanta", customer.ID, customer.CustomerTypeId, customer.CustomerCategory, customer.Address.PostalCode, customer.Address.City, customer.Address.Post, customer.Address.Street, customer.Address.Building, customer.Address.Flat, customer.Name, null, customer.Nip, customer.NumberSMS);

            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
        }

        public IDataReader GetCustomer(string p)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            if (p.Length > 0)
            {
                DbCommand cmd = db.GetStoredProcCommand("Uzytkownicy.pobierzInteresanta", int.Parse(p));
                return db.ExecuteReader(cmd);
            }
            else
                return null;
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataView GetRegistersList(string sortParam, int rok, int status)
        {
            /* STATUS:
             * 0 - aktywny, niearchwalny
             * 1 - aktywny, archiwalny
             */

            bool? archiwalny = (status == 0) ? false : true;
            bool? aktywny = true;

            if (status == -1)
            {
                aktywny = archiwalny = null;
            }

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");


            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzListeRejestrow]", rok, aktywny, archiwalny);
            DataSet dsRegistersList = db.ExecuteDataSet(cmd);
            DataView dvRegistersList = null;
            if (dsRegistersList.Tables.Count > 0)
            {
                dvRegistersList = new DataView(dsRegistersList.Tables[0]);
                dvRegistersList.Sort = sortParam;
            }
            return dvRegistersList;
        }

        public IDataReader GetRegistry(int regId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzDaneZRejestru]", regId);
            return db.ExecuteReader(cmd);
        }

        public int AddRegistry(int wydzialGlowny, int idDefinicji, string nazwa, int? jrwa, string wydzialy, string wpisy, string xslt, int rok, bool showEntryDate, bool showCreatingUser, bool aktywny, bool archiwalny, string xslfo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[dodanieRejestru]", wydzialGlowny, idDefinicji, nazwa, jrwa, wydzialy, wpisy, xslt, rok, showEntryDate, showCreatingUser, 1, aktywny, archiwalny, xslfo);
            object regid = db.ExecuteScalar(cmd);
            if (regid != null)
                return int.Parse(regid.ToString());
            else
                return -1;
        }

        public void UpdateRegistry(int regId, int wydzialGlowny, int idDefinicji, string nazwa, int? jrwa, string wydzialy, string wpisy, string xslt, int rok, bool showEntryDate, bool showCreatingUser, bool aktywny, bool archiwalny, string xslfo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[aktualizujRejestr]", regId, wydzialGlowny, nazwa, jrwa, wpisy, wydzialy, xslt, showEntryDate, showCreatingUser, 1, aktywny, archiwalny, xslfo, rok);
            db.ExecuteNonQuery(cmd);
        }

        public void UpdateDailyLog(string xslFo)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");
            DbCommand cmd = db.GetStoredProcCommand("Rejestry.aktualizujDziennik", xslFo);
            db.ExecuteNonQuery(cmd);
        }

        public bool IsXslFoExist(int regID)
        {
            bool isExist = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");
            DbCommand cmd = db.GetSqlStringCommand("select Rejestry.czyIstniejeXslFo(@regId)");
            db.AddInParameter(cmd, "@regId", DbType.Int32);
            db.SetParameterValue(cmd, "@regId", regID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isExist = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            }

            return isExist;
        }

        public RegistryDTO GetDailyLog()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");
            RegistryDTO reg = new RegistryDTO();
            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzDefinicjeDziennika");
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    reg.XslFo = dr["xslFo"].ToString();
                }
            };
            return reg;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataView GetRegistryDefinitionList()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzListeDefinicji]");
            DataSet dsListaDefinicji = db.ExecuteDataSet(cmd);
            DataView dvListaDefinicji = null;
            if (dsListaDefinicji.Tables.Count > 0)
            {
                dvListaDefinicji = new DataView(dsListaDefinicji.Tables[0]);
            }
            return dvListaDefinicji;
        }

        public int AddRegistryDefinition(string definicja)
        {
            int defId = -1;

            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[dodanieDefinicji]", new Guid(Membership.GetUser().ProviderUserKey.ToString()), "definicja", definicja, "<transformacja/>", "<transformacja/>");
            object id = db.ExecuteScalar(cmd);
            if (id != null && int.TryParse(id.ToString(), out defId))
                return defId;
            else
                return -1;
        }

        public IDataReader GetRegistryDefinition(int regDefId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzDefinicje]", regDefId);
            return db.ExecuteReader(cmd);
        }

        public IDataReader GetRegistryDefinitionByRegistryId(int registryId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzDefinicjeDlaRejestru]", registryId);
            return db.ExecuteReader(cmd);
        }

        public void UpdateRegistryDefinition(int defId, string definicja)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[aktualizujDefinicje]", defId, definicja);
            cmd.CommandTimeout=600;db.ExecuteNonQuery(cmd);
        }

        public IDataReader GetAvailableRegistries(Guid userId, int objectID, bool isDocument, int year)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Nie uda³o sie po³¹czyæ z baz¹ danych");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzDostepneRejestry", userId, objectID, isDocument, year);
            IDataReader idr = db.ExecuteReader(cmd);
            return idr;
        }

        public bool RegHasData(int regId)
        {
            bool result = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Nie uda³o sie po³¹czyæ z baz¹ danych");

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[czySaDane]", regId);
            using (DbDataReader dr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (dr.HasRows)
                    result = dr.Read();
            }
            return result;
        }

        public int SaveRegistryItem(Guid userId, int registryid, int objectId, string content, bool isInsert)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Couldn't connect to database");

            content = content.Replace("&", "&amp;");
            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[dodaniePozycjiRejestru]", userId, registryid, objectId, content, isInsert);
            object elemId = db.ExecuteScalar(cmd);
            int pozId = -1;
            if (elemId != null && int.TryParse(elemId.ToString(), out pozId))
                return pozId;
            else
                return -1;
        }

        public string GetXsltByRegistryItemID(int regItemID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Couldn't connect to database");

            string xslt = string.Empty;

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzXsltRejestruNpIdPozRejestru", regItemID);
            using (XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db))
            {
                if (xr.Read())
                {
                    xslt = xr.ReadOuterXml();
                }
            }

            return xslt;

        }

        public string GetRegistryXslt(int regID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Couldn't connect to database");

            string xslt = string.Empty;

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzXsltRejestru]", regID);
            using (DbDataReader xr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (xr.Read())
                {
                    xslt = xr["xsltRejestru"].ToString();
                }
            }

            return xslt;

        }

        public string GetRegistryXslFo(int regID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Couldn't connect to database");

            string xslt = string.Empty;

            DbCommand cmd = db.GetStoredProcCommand("[Rejestry].[pobierzXslFoRejestru]", regID);
            using (DbDataReader xr = (DbDataReader)db.ExecuteReader(cmd))
            {
                if (xr.Read())
                {
                    xslt = xr["xslFo"].ToString();
                }
            }

            return xslt;

        }

        public XmlReader GetRegistryItemHistory(int regItemID, Guid userID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Couldn't connect to database");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzHistoriePozycjiRejestru", regItemID);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);

            return xr;

        }

        public XmlReader GetRegistryContent(int regItemID, Guid userID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Couldn't connect to database");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzZawartoscRejestru", regItemID);
            XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db);

            return xr;

        }

        public int GetDailyLogID()
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Couldn't connect to database");
            int id = 0;

            object rawID = db.ExecuteScalar(CommandType.Text, "select Rejestry.IdAktualnegoDziennika()");
            if (rawID == null)
                return 0;
            int.TryParse(rawID.ToString(), out id);
            return id;

        }

        public int GetDocumentIDByDailyLogItem(int regID, int itemID, bool isRF)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Couldn't connect to database");
            int id = -1;

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzIdDokZDziennika", regID, itemID, isRF);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    id = int.Parse(dr["idDokumentu"].ToString());
                }
            };

            return id;
        }

        public string GetRegistryItem(int registryItemID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Couldn't connect to database");

            object rawItem = db.ExecuteScalar("Rejestry.pobierzPozycjeRejestru", registryItemID);
            if (rawItem == null)
                return string.Empty;
            return rawItem.ToString();
        }

        public void GetObjectIDRegistry(int registryItemID, out string objectType, out int objectID)
        {

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzIdObiektuRejestru", registryItemID);

            int docID = 0;
            int caseID = 0;

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    docID = dr.IsDBNull(0) ? 0 : dr.GetInt32(0);
                    caseID = dr.IsDBNull(1) ? 0 : dr.GetInt32(1);
                }
            }

            if (docID != 0)
            {
                objectType = "DOC";
                objectID = docID;
            }
            else if (caseID != 0)
            {
                objectType = "CASE";
                objectID = caseID;
            }
            else
            {
                objectType = "";
                objectID = -1;
            }
        }

        public string GetRegistryType(int registryID)
        {
            string regType = string.Empty;

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.getRegisterType", registryID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    regType = dr["regType"].ToString();
                }
            }

            return regType;
        }

        public int GetRegistryIDByItem(int itemID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Couldn't connect to database");
            int id = 0;

            object rawID = db.ExecuteScalar("Rejestry.pobierzIdRejestruDlaPozycji", itemID);
            if (rawID == null)
                return 0;
            int.TryParse(rawID.ToString(), out id);
            return id;

        }

        public bool IsInMainDepartment(int regId, Guid userID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Couldn't connect to database");

            object rawItem = db.ExecuteScalar("Rejestry.czyJestWWydzialeGlownym", userID, regId);
            if (rawItem == null)
                return false;
            return true;
        }

        public List<RegistryItemDTO> GetRegistriesForDepartment(int departmentID, int year)
        {
            List<RegistryItemDTO> items = new List<RegistryItemDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzListeRejestrowDlaWydzialu", departmentID, year);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new RegistryItemDTO()
                    {
                        ID = int.Parse(dr["id"].ToString()),
                        Title = dr["nazwa"].ToString(),
                        Year = int.Parse(dr["rok"].ToString()),
                        IsNewYearCopy = bool.Parse(dr["jestPowielonyNaNowyRok"].ToString())
                    });
                }
            }

            return items;
        }

        public void CreateNewYearRegistry(int registryID, int newYear)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.tworzRejestNaNowyRok", registryID, newYear);

            db.ExecuteNonQuery(cmd);
        }

        public List<SimpleLookupDTO> GetRegistriesAvailableYears()
        {
            List<SimpleLookupDTO> items = new List<SimpleLookupDTO>();

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.pobierzDostepneLataRejestrow");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new SimpleLookupDTO(int.Parse(dr["Value"].ToString()), dr["Text"].ToString()));
                }
            };

            return items;
        }

        public bool IsDailyLogItemAccessDenied(int registryId, int itemId, Guid userId)
        {
            bool isAccessDenied = false;
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Do poprawnego dzia³ania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetSqlStringCommand("select Rejestry.isDailyLogItemAccessDenied(@registryId,@itemId,@userId)");
            db.AddInParameter(cmd, "@registryId", DbType.Int32);
            db.SetParameterValue(cmd, "@registryId", registryId);
            db.AddInParameter(cmd, "@itemId", DbType.Int32);
            db.SetParameterValue(cmd, "@itemId", itemId);
            db.AddInParameter(cmd, "@userId", DbType.Guid);
            db.SetParameterValue(cmd, "@userId", userId);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    isAccessDenied = dr.IsDBNull(0) ? false : dr.GetBoolean(0);
                }
            }

            return isAccessDenied;
        }

        public int GetBuiltinDocumentCategoryId(string categoryName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            return (int)db.ExecuteScalar(CommandType.Text, "select Dokumenty.pobierzIdKategoriiWbudowanej('" + categoryName + "')");
        }

        public IDataReader GetRKWDataForRegistration(int docId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Nie uda³o sie po³¹czyæ z baz¹ danych");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.PobierzDaneDoRejestracjiRKW", docId);
            IDataReader idr = db.ExecuteReader(cmd);
            return idr;
        }

        public bool RegisterDocumentInRKW(int docID, string correspondenceType, int correspondenceTypeValue, string remarks)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Nie uda³o sie po³¹czyæ z baz¹ danych");

            DbCommand cmd = db.GetStoredProcCommand("Rejestry.DodajPozycjeRKW", docID, correspondenceType, correspondenceTypeValue, remarks);
            try
            {
                db.ExecuteNonQuery(cmd);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public List<RKWItem> GetRKWPage(RKWFilter filter,out int totalPages)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null)
                throw new Exception("Nie uda³o sie po³¹czyæ z baz¹ danych");
            if (filter == null)
                filter = new RKWFilter();
          
            List<RKWItem> items = new List<RKWItem>();
            DbCommand cmd = db.GetStoredProcCommand("Rejestry.PobierzStroneRKW", filter.PageNumber, filter.PageSize, filter.NewerFirst, filter.DateFrom, filter.DateTo, filter.OrganizationalUnit, filter.EntryNumber, filter.Remarks,null);
            DbParameter op = new SqlParameter("@totalPages", SqlDbType.Int);
            
                
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    items.Add(new RKWItem()
                    {
                        Id = (int)dr["Id"],
                        NrPozycji = (int)dr["NrPozycji"],
                        DataRejestracji = DateTime.Parse(dr["DataRejestracji"].ToString()),
                        ZnakPisma = (string)dr["ZnakPisma"],
                        Wydzial = (string)dr["Wydzial"],
                        Pracownik = (string)dr["Pracownik"],
                        TypDokumentu = (string)dr["TypDokumentu"],
                        TypKorespondencji = (string)dr["TypKorespondencji"],
                        NazwaAdresata = (string)dr["NazwaAdresata"],
                        UlicaAdresata = (string)dr["UlicaAdresata"],
                        KodIMiastoAdresata = (string)dr["KodIMiastoAdresata"],
                        Uwagi = (string)dr["Uwagi"]
                    });
                }
            }
            totalPages = (int)cmd.Parameters["@totalPages"].Value;
            return items;
        }
    }

    public class RKWFilter
    {
        public bool NewerFirst { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public string OrganizationalUnit { get; set; }

        public int? EntryNumber { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public string Remarks { get; set; }

        public int TotalPages { get; set; }

        public RKWFilter()
        {
            NewerFirst = true;
            PageNumber = 1;
            PageSize = 15;
            TotalPages = 0;
        }
    }

    public class RKWItem
    {
        public int Id { get; set; }
        public int NrPozycji { get; set; }
        public DateTime DataRejestracji { get; set; }
        public string ZnakPisma { get; set; }
        public string Wydzial { get; set; }
        public string Pracownik { get; set; }
        public string TypDokumentu { get; set; }
        public string TypKorespondencji { get; set; }
        public string NazwaAdresata { get; set; }
        public string UlicaAdresata { get; set; }
        public string KodIMiastoAdresata { get; set; }
        public string Uwagi { get; set; }
    }
}