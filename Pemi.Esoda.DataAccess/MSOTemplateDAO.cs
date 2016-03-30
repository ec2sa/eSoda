using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;

namespace Pemi.Esoda.DataAccess
{
    public class MSOTemplateDAO
    {
        public MSOTemplateDTO GetMSOTemplate(int? templateID)
        {
            return GetMSOTemplate(templateID, false, null, null);

            #region todel
            //SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            //if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            //MSOTemplateDTO template = null;
            //DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzSzablonMSO", templateID, false);

            //using (IDataReader dr = db.ExecuteReader(cmd))
            //{
            //    if (dr.Read())
            //    {
            //        template = new MSOTemplateDTO(
            //        int.Parse(dr["id"].ToString()),
            //        dr["filename"].ToString(),
            //        dr["originalFileName"].ToString());
            //    }
            //};

            //return template;
            #endregion
        }

        public MSOTemplateDTO GetLAWTemplate(int docID)
        {
            return GetMSOTemplate(null, false, "legalAct", docID);
        }

        public MSOTemplateDTO GetMSOTemplate(int? templateID, bool isSecure, string templateType, int? docID)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            MSOTemplateDTO template = null;
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzSzablonMSO", templateID, isSecure, templateType, docID);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    template = new MSOTemplateDTO(
                    int.Parse(dr["id"].ToString()),
                    dr["filename"].ToString(),
                    dr["originalFileName"].ToString(),
                    bool.Parse(dr["isSecure"].ToString()),
                    bool.Parse(dr["isActive"].ToString())
                    );
                }
            };

            return template;
        }

        public int? SetTemplate(int? templateID, string fileName, string originalFileName)
        {
            return SetTemplate(templateID, fileName, originalFileName, false, false);

            #region todel
            //if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(originalFileName))
            //    throw new ArgumentException("Proszę wybrać plik z szablonem MS Word!");

            //SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            //if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            //DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszSzablonMSO", templateID, fileName, originalFileName, false);
            //int? returnTemplateID = null;
            //using (IDataReader dr = db.ExecuteReader(cmd))
            //{
            //    if (dr.Read())
            //    {
            //        returnTemplateID = int.Parse(dr["id"].ToString());                    
            //    }
            //};

            //return returnTemplateID;
            #endregion
        }

        public int? SetTemplate(int? templateID, string fileName, string originalFileName, bool isSecure)
        {
            return SetTemplate(templateID, fileName, originalFileName, isSecure, false);

            #region todel
            //if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(originalFileName))
            //    throw new ArgumentException("Proszę wybrać plik z szablonem MS Word!");

            //SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            //if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            //DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszSzablonMSO", templateID, fileName, originalFileName, isSecure);
            //int? returnTemplateID = null;
            //using (IDataReader dr = db.ExecuteReader(cmd))
            //{
            //    if (dr.Read())
            //    {
            //        returnTemplateID = int.Parse(dr["id"].ToString());
            //    }
            //};

            //return returnTemplateID;
            #endregion
        }

        public int? SetTemplate(int? templateID, string fileName, string originalFileName, bool isSecure, bool isActive)
        {
            if (String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(originalFileName))
                throw new ArgumentException("Proszę wybrać plik z szablonem MS Word!");

            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszSzablonMSO", templateID, fileName, originalFileName, isSecure, isActive);
            int? returnTemplateID = null;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    returnTemplateID = int.Parse(dr["id"].ToString());
                }
            };

            return returnTemplateID;
        }

        public MSOTemplateDTO GetCurrentMSOTemplate()
        {
            return GetCurrentMSOTemplate(false);

            #region todel
            //SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            //if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            //MSOTemplateDTO template = null;
            //DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzBiezacySzablonMSO]",false);

            //using (IDataReader dr = db.ExecuteReader(cmd))
            //{
            //    if (dr.Read())
            //    {
            //        template = new MSOTemplateDTO(
            //        int.Parse(dr["id"].ToString()),
            //        dr["filename"].ToString(),
            //        dr["originalFileName"].ToString());
            //    }
            //};

            //return template;
            #endregion
        }

        public MSOTemplateDTO GetCurrentMSOTemplate(bool isSecure)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            MSOTemplateDTO template = null;
            DbCommand cmd = db.GetStoredProcCommand("[Dokumenty].[pobierzBiezacySzablonMSO]", isSecure);

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    template = new MSOTemplateDTO(
                    int.Parse(dr["id"].ToString()),
                    dr["filename"].ToString(),
                    dr["originalFileName"].ToString(),
                    bool.Parse(dr["isSecure"].ToString()),
                    bool.Parse(dr["isActive"].ToString())
                    );
                }
            };

            return template;
        }

        public int? AddLegalActSettings(string version, string schema, string xslt)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.dodajUstawieniaAktuPrawnego", version, schema, xslt);
            int? settingsID = null;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    settingsID = int.Parse(dr["id"].ToString());
                }
            };

            return settingsID;
        }

        public int? SaveLegalActSettings(int? id, string schema, string xslt)
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszUstawieniaAktuPrawnego", id, schema, xslt);
            int? settingsID = null;
            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    settingsID = int.Parse(dr["id"].ToString());
                }
            };

            return settingsID;
        }

        public LegalActsSettingsDTO GetLegalActsSettings()
        {
            SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;
            if (db == null) throw new Exception("Do poprawnego działania wymagany jest SQL Server 2005!");

            LegalActsSettingsDTO settings = null;
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.pobierzUstawieniaAktuPrawnego");

            using (IDataReader dr = db.ExecuteReader(cmd))
            {
                if (dr.Read())
                {
                    settings = new LegalActsSettingsDTO(int.Parse(dr["id"].ToString()), dr["numerWersji"].ToString(), (DateTime)dr["dataUtworzenia"], dr["xsd"].ToString(), dr["xslt"].ToString());
                }
            };

            return settings;
        }
    }
}
