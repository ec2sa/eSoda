using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Web;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Reflection;
using Pemi.eSoda.CustomForms.Inferfaces;
using System.Web.Security;
using System.Configuration;


namespace Pemi.Esoda.DataAccess
{
    public class CustomFormDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
        
        private CustomFormDTO _customForm;

        private bool _isValid = false;

        private bool validate()
        {
            string assemblyName = HttpContext.Current.Server.MapPath("~/temp/" + _customForm.Filename);
            AppDomain tmpappd = AppDomain.CreateDomain("vad");
            
            Assembly cf =  Assembly.LoadFile(assemblyName);
            if (cf == null)
                throw new ArgumentException("Unable to load assembly. ");

            AssemblyName[] assemblies = cf.GetReferencedAssemblies();
            foreach (AssemblyName an in assemblies)
            {
                if(an.FullName.Contains("System.Data.SqlClient")
                    || an.FullName.Contains("System.Data")
                    || an.FullName.Contains("System.Net")
                    )
                    throw new ArgumentException("System.Data.* and System.Net references are not allowed. Nice try:-)");
            }


            string className = null;
            foreach (Type t in cf.GetTypes())
            {
                foreach (Type icf in t.GetInterfaces())
                {
                    if (icf.Equals(typeof(ICustomForm)))
                        className = t.FullName;
                }
            }
            if (string.IsNullOrEmpty(className))
                throw new ArgumentException("Unable to find class implementing ICustomForm interface");

            _customForm.ClassName = className;

            _isValid = true;
            return true;
        }

        public CustomFormDAO() { }

        public CustomFormDAO(CustomFormDTO customForm)
        {
            _customForm=customForm;
        }

        public bool IsValid()
        {
            if (_customForm == null)
                return false;
            if (_isValid)
                return true;

            return validate();
        }

        //public void SaveMSO()
        //{
        //    string documentDirectory=ConfigurationManager.AppSettings["katalogDokumentow"];
        //    string msoDirectory =Path.Combine(documentDirectory,"MSOIntegration");

        //   
        //    if (!Directory.Exists(msoDirectory))
        //    {
        //        Directory.CreateDirectory(msoDirectory);
        //    }

        //    if (string.IsNullOrEmpty(_customForm.WordFilename) || string.IsNullOrEmpty(_customForm.WordSchemaFilename))
        //        return;

        //    //generowanie hasha [tylko dla worda]
        //    byte[] hash = null;
        //    using (FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath("~/temp/" + _customForm.WordFilename)))
        //    {
        //        hash = MD5.Create().ComputeHash(fs);
        //    }

        //    //przenoszenie plikow do katalogu MSOIntegration
        //    if (!File.Exists(Path.Combine(msoDirectory, _customForm.WordFilename)) && !File.Exists(Path.Combine(msoDirectory,_customForm.WordSchemaFilename)))
        //    {
        //        File.Move(Path.Combine(HttpContext.Current.Server.MapPath("~/temp/"), _customForm.WordFilename), Path.Combine(msoDirectory, _customForm.WordFilename));
        //        File.Move(Path.Combine(HttpContext.Current.Server.MapPath("~/temp/"), _customForm.WordSchemaFilename), Path.Combine(msoDirectory, _customForm.WordSchemaFilename));



        //        if (_customForm.DocumentTypeID == 0)
        //            InsertDocumentType(_customForm, hash);
        //        else
        //            UpdateDocumentType(_customForm, hash);

        //    }
          
        //}

        public void Save(bool overwrite,out bool allowOverwrite)
        {
            allowOverwrite = false;
            if (!overwrite && !String.IsNullOrEmpty(_customForm.OriginalFilename))
            {
                if (Directory.Exists(Path.Combine(HttpRuntime.BinDirectory, Path.GetFileNameWithoutExtension(_customForm.OriginalFilename))))
                {
                    allowOverwrite = isFileAssignedToCurrentCategory();
                    if (allowOverwrite)
                    {
                        throw new ArgumentException("Plik o podanej nazwie już istnieje. Chcesz nadpisać istniejącą wersję?");
                    }
                    else
                    {
                        throw new ArgumentException("Plik o podanej nazwie już istnieje.");
                    }
                }
            }

            if (overwrite)
            {
                if (!isFileAssignedToCurrentCategory())
                {
                    allowOverwrite = false;
                    throw new ArgumentException("Filename has changed.");
                }
            }
            if (_customForm == null)
                throw new ArgumentException("Unable to save Custom Form");
         

            if (!string.IsNullOrEmpty(_customForm.OriginalFilename))
                validate();

            byte[] hash = null;
            byte[] hashMSO = null;
            try
            {
                if (_customForm.OriginalFilename != null)
                {
                    hash = null;
                    using (FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath("~/temp/" + _customForm.Filename)))
                    {
                        hash = MD5.Create().ComputeHash(fs);
                    }

                    string targetDirectory = Path.Combine(HttpRuntime.BinDirectory,Path.GetFileNameWithoutExtension(_customForm.OriginalFilename));
                    if (!Directory.Exists(targetDirectory))
                        Directory.CreateDirectory(targetDirectory);
                    if (File.Exists(Path.Combine(targetDirectory, _customForm.OriginalFilename)))
                        File.Delete(Path.Combine(targetDirectory, _customForm.OriginalFilename));
                    File.Move(HttpContext.Current.Server.MapPath("~/temp/" + _customForm.Filename), Path.Combine(targetDirectory, _customForm.OriginalFilename));
                }

                if (_customForm.WordOriginalFilename != null)
                {
                    string documentDirectory = ConfigurationManager.AppSettings["katalogDokumentow"];
                    string msoDirectory = Path.Combine(documentDirectory, "MSOIntegration");

                    
                    if (!Directory.Exists(msoDirectory))
                    {
                        Directory.CreateDirectory(msoDirectory);
                    }

                    if (string.IsNullOrEmpty(_customForm.WordFilename) || string.IsNullOrEmpty(_customForm.WordSchemaFilename))
                        return;

                    //generowanie hasha [tylko dla worda]

                    using (FileStream fs = File.OpenRead(HttpContext.Current.Server.MapPath("~/temp/" + _customForm.WordFilename)))
                    {
                        hashMSO = MD5.Create().ComputeHash(fs);
                    }

                    //przenoszenie plikow do katalogu MSOIntegration
                    if (!File.Exists(Path.Combine(msoDirectory, _customForm.WordFilename)) && !File.Exists(Path.Combine(msoDirectory, _customForm.WordSchemaFilename)))
                    {
                        File.Move(Path.Combine(HttpContext.Current.Server.MapPath("~/temp/"), _customForm.WordFilename), Path.Combine(msoDirectory, _customForm.WordFilename));
                        File.Move(Path.Combine(HttpContext.Current.Server.MapPath("~/temp/"), _customForm.WordSchemaFilename), Path.Combine(msoDirectory, _customForm.WordSchemaFilename));

                    }
                }

                if (_customForm.DocumentTypeID == 0)
                    InsertDocumentType(_customForm, hash,hashMSO);
                else
                    UpdateDocumentType(_customForm, hash,hashMSO);

            }
            catch (IOException)
            {
                throw new Exception(string.Format("Plik {0} już istnieje", _customForm.OriginalFilename));
            }
 
            catch(Exception){
                throw;
            }
        }

        private bool isFileAssignedToCurrentCategory()
        {
            string filenameDB = (string)db.ExecuteScalar("Dokumenty.pobierzNazwePlikuDlaTypuDokumentu", _customForm.DocumentTypeID);
            if (filenameDB == null)
                return false;
            return filenameDB.ToLower() == _customForm.OriginalFilename.ToLower();
        }


        public CustomFormDTO GetCustomFormData(int documentID,bool isLegalAct)
        {
            Database db = DatabaseFactory.CreateDatabase();
            if (db == null) throw new Exception("Couldn't connect to database");

            CustomFormDTO customForm = null;
            DbCommand cmd ;
            if (!isLegalAct)
                cmd = db.GetStoredProcCommand("Dokumenty.pobierzDaneFormularza", documentID);
            else
                cmd = db.GetStoredProcCommand("Dokumenty.pobierzDaneAktuPrawnego", documentID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                
                if (dr.Read())
                {
                    customForm = new CustomFormDTO(int.Parse(dr["kindID"].ToString()),int.Parse(dr["categoryID"].ToString()), dr["filename"].ToString(), dr["originalFileName"].ToString(), dr["className"].ToString(), bool.Parse(dr["isActive"].ToString()),bool.Parse(dr["aktPrawny"].ToString()), dr["data"].ToString(),dr["FormHash"].ToString(),dr["DataHash"].ToString());
                }
            };

            return customForm;
        }


        public IList<CustomFormHistoryItemDTO> GetCustomFormHistoryList(int documentID,bool isLegalAct)
        {
            List<CustomFormHistoryItemDTO> list = new List<CustomFormHistoryItemDTO>();
            DbCommand cmd;
            if(!isLegalAct)
             cmd = db.GetStoredProcCommand("Dokumenty.pobierzHistorieFormularza", documentID);
            else
                cmd = db.GetStoredProcCommand("Dokumenty.pobierzHistorieAktuPrawnego", documentID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                while (dr.Read())
                {
                    list.Add(new CustomFormHistoryItemDTO(int.Parse(dr["id"].ToString()), DateTime.Parse(dr["data"].ToString()), dr["pelnaNazwa"].ToString()));
                }
            };

            return list;
        }

        public string GetCustomFormHistoryData(int itemID)
        {
            string xmlData = string.Empty;

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzDaneFormularzaZHistorii", itemID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    xmlData = dr["dane"].ToString();
                }
            };

            return xmlData;
        }

        public CustomFormHistoryItemDTO GetCustomFormHistoryItem(int itemID,bool isLegalAct)
        {
            
            CustomFormHistoryItemDTO item = new CustomFormHistoryItemDTO();
            DbCommand cmd;
            if(!isLegalAct)
                cmd = db.GetStoredProcCommand("Dokumenty.pobierzDaneFormularzaZHistorii", itemID);
            else
                cmd = db.GetStoredProcCommand("Dokumenty.pobierzDaneAktuPrawnegoZHistorii", itemID);
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    item.XmlData = dr["dane"].ToString();
                    item.Date = DateTime.Parse(dr["data"].ToString());
                }
            };

            return item;
        }

        public void SetCustomFormData(int documentID,int documentTypeID, string xmlData)
        {
            db.ExecuteNonQuery("Dokumenty.zapiszDaneFormularza", documentID, documentTypeID, xmlData, (Guid)Membership.GetUser().ProviderUserKey);
        }

        private string getHashString(byte[] hash)
        {
            if (hash == null)
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        private void InsertDocumentType(CustomFormDTO doc,byte[] hashCF,byte[] hashMSO)
        {
            
            
            db.ExecuteNonQuery("[Dokumenty].[dodanieRodzaju]" 
                ,doc.DocumentCategoryID
                ,doc.DocumentTypeName
                ,doc.DocumentTypeAbbr
                ,doc.OriginalFilename
                ,doc.Filename
                ,doc.Description
                ,doc.ClassName
                ,doc.IsCFActive
                ,getHashString(hashCF)
                    ,getHashString(hashMSO)
                    ,doc.IsMSOActive
                    ,doc.WordOriginalFilename
                    ,doc.WordFilename
                    ,doc.WordSchemaOriginalFilename
                    ,doc.WordSchemaFilename
                    ,doc.IsLegalAct
                    );
        }

        private void UpdateDocumentType(CustomFormDTO doc, byte[] hash, byte[] hashMSO)
        {
            db.ExecuteNonQuery("[Dokumenty].[edycjaRodzaju]"
                , doc.DocumentTypeID
                , doc.DocumentCategoryID
                , doc.DocumentTypeName
                , doc.DocumentTypeAbbr
                , doc.OriginalFilename
                , doc.Filename
                , doc.Description
                , doc.ClassName
                , doc.IsCFActive
                , getHashString(hash)
                , getHashString(hashMSO)
                    , doc.IsMSOActive
                    , doc.WordOriginalFilename
                    , doc.WordFilename
                    , doc.WordSchemaOriginalFilename
                    , doc.WordSchemaFilename
                    ,doc.IsLegalAct
                );
        }


    }
}
