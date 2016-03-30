using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;
using Pemi.Esoda.DTO;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Web;
using System.Data.Common;
using System.IO;
using System.Configuration;
using System.Xml.Schema;
using System.Collections;

namespace Pemi.Esoda.DataAccess
{
    public class MSOIntegrationDAO
    {
        private SqlDatabase db = DatabaseFactory.CreateDatabase() as SqlDatabase;

        public int SaveExistingForm(XmlDocument xdoc, MSOWebServiceCallContextDTO context, string createdWith)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszDaneFormularzaMSO", context.Ticket, context.DocumentGUID, context.DocumentTypeID, context.LastHistoryID, xdoc.OuterXml, false, createdWith);
                db.ExecuteNonQuery(cmd);
                return (int)cmd.Parameters[0].Value;


            }
            catch (SqlException ex)
            {
                throw new ArgumentException("Nie udało się zapisać formularza (" + ex.Message + ")");
            }

        }

        public int TrySaveExistingForm(XmlDocument xdoc, MSOWebServiceCallContextDTO context)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszDaneFormularzaMSO", context.Ticket, context.DocumentGUID, context.DocumentTypeID, context.LastHistoryID, xdoc.OuterXml, true, null);
                db.ExecuteNonQuery(cmd);
                return (int)cmd.Parameters[0].Value;

            }
            catch (SqlException ex)
            {
                throw new ArgumentException("Nie udało się zapisać formularza (" + ex.Message + ")");
            }

        }

        public string GetMSOTemplateFileName(int documentID)
        {
            XmlDocument doc = new XmlDocument();
            string templateFileName = null;
            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzDaneMSO", documentID, null))
            {
                if (!dr.Read())
                    return null;
                templateFileName = dr["nazwaMSO"].ToString();
            }
            return templateFileName;
        }

        public string GetMSOFileName(int documentID)
        {
            XmlDocument doc = new XmlDocument();
            string fileName = null;
            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzDaneMSO", documentID, null))
            {
                if (!dr.Read())
                    return null;
                fileName = dr["nazwaPlikuMSO"].ToString();
            }
            return fileName;
        }

        public string GetMSOSchemaFileName(Guid documentGuid)
        {
            XmlDocument doc = new XmlDocument();
            string templateFileName = null;
            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzDaneMSO", null, documentGuid))
            {
                if (!dr.Read())
                    return null;
                templateFileName = dr["nazwaSchemyMSO"].ToString();
            }
            return templateFileName;
        }

        public XmlDocument GetMSOServiceContext(int documentID, int duration, bool withSchema)
        {
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("es", "http://esoda.pl/schemas/MSOcontext");

            using (IDataReader dr = db.ExecuteReader("Dokumenty.pobierzDaneMSO", documentID, null))
            {
                if (!dr.Read())
                    return null;
                string schema = dr["nazwaSchemyMSO"].ToString();

                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.generujKontekst", documentID, Membership.GetUser().ProviderUserKey, duration);

                using (XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db))
                {
                    if (!xr.Read())
                        return null;
                    doc.Load(xr);
                }

                if (withSchema)
                {
                    string schemaContent = null;

                    if (string.IsNullOrEmpty(schema))
                    {
                        XmlSchemaInference schemaInference = new XmlSchemaInference();
                        CustomFormDTO customFormXmlData = new CustomFormDAO().GetCustomFormData(documentID, false);
                        if (customFormXmlData != null && !string.IsNullOrEmpty(customFormXmlData.XmlData))
                        {
                            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(customFormXmlData.XmlData));
                            XmlTextReader reader = new XmlTextReader(stream);
                            ArrayList schemaSet = new ArrayList(schemaInference.InferSchema(reader).Schemas());
                            XmlSchema xmlSchema = (XmlSchema)schemaSet[0];
                            StringWriter writer = new StringWriter();
                            xmlSchema.Write(writer);
                            schemaContent = writer.ToString();
                        }
                    }
                    else
                    {
                        schemaContent = File.ReadAllText(Path.Combine(ConfigurationManager.AppSettings["katalogDokumentow"] + "\\MSOIntegration", schema));
                    }

                    XmlDocumentFragment frag = null;

                    if (!string.IsNullOrEmpty(schemaContent))
                    {
                        frag = doc.CreateDocumentFragment();
                        schemaContent = schemaContent.Substring(schemaContent.IndexOf("?>") + 2);
                        frag.InnerXml = schemaContent;
                    }

                    if (frag != null)
                    {
                        doc.SelectSingleNode("/es:esodaData/es:schema[1]", mgr).AppendChild(frag);
                    }
                }

                XmlText url = doc.CreateTextNode(HttpContext.Current.Request.Url.AbsoluteUri.Remove(HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.ApplicationPath) + HttpContext.Current.Request.ApplicationPath.Length) + "/services/MSOIntegrationService.asmx");
                doc.SelectSingleNode("/es:esodaData/es:ServiceEndpoint[1]", mgr).AppendChild(url);
                return doc;
            }
        }

        public XmlDocument GetMSOServiceContextBin(int documentID, int duration, Guid? elementGuid, string description, string desiredName)
        {
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("es", "http://esoda.pl/schemas/MSOcontext");


            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.generujKontekstBin", documentID, Membership.GetUser().ProviderUserKey, elementGuid, duration);

            using (XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db))
            {
                if (!xr.Read())
                    return null;
                doc.Load(xr);
            }


            XmlText url = doc.CreateTextNode(HttpContext.Current.Request.Url.AbsoluteUri.Remove(HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.ApplicationPath) + HttpContext.Current.Request.ApplicationPath.Length) + "/services/MSOIntegrationService.asmx");
            doc.SelectSingleNode("/es:esodaData/es:ServiceEndpoint[1]", mgr).AppendChild(url);

            XmlNode node = doc.SelectSingleNode("/es:esodaData/es:context/es:Description[1]", mgr);
            if (node != null && !string.IsNullOrEmpty(description))
            {
                node.RemoveAll();
                node.AppendChild(doc.CreateTextNode(description));
            }
            node = doc.SelectSingleNode("/es:esodaData/es:context/es:DesiredName[1]", mgr);
            if (node != null)
            {
                node.RemoveAll();
                node.AppendChild(doc.CreateTextNode(desiredName));
            }
            return doc;

        }

        public XmlDocument GetMSOServiceContextLAW(int documentID, int duration)
        {
            XmlDocument doc = new XmlDocument();
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("es", "http://esoda.pl/schemas/MSOcontext");

            LegalActsSettingsDTO legalActsSettings = new MSOTemplateDAO().GetLegalActsSettings();

            if (legalActsSettings != null)
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.generujKontekstLAW", documentID, Membership.GetUser().ProviderUserKey, duration);

                using (XmlReader xr = CommonMethods.GetXmlReaderAndCloseConnection(cmd, db))
                {
                    if (!xr.Read())
                        return null;
                    doc.Load(xr);
                }

                XmlDocumentFragment frag = null;

                string schema = legalActsSettings.Schema;

                if (!string.IsNullOrEmpty(schema))
                {
                    frag = doc.CreateDocumentFragment();
                    schema = schema.Substring(schema.IndexOf("?>") + 1);
                    frag.InnerXml = schema;
                }

                if (frag != null)
                {
                    doc.SelectSingleNode("/es:esodaData/es:schema[1]", mgr).AppendChild(frag);
                }

                string xslt = legalActsSettings.Xslt;

                frag = null;

                if (!string.IsNullOrEmpty(xslt))
                {
                    frag = doc.CreateDocumentFragment();
                    xslt = xslt.Substring(xslt.IndexOf("?>") + 1);
                    frag.InnerXml = xslt;
                }

                if (frag != null)
                {
                    doc.SelectSingleNode("/es:esodaData/es:xsl[1]", mgr).AppendChild(frag);
                }

                XmlText url = doc.CreateTextNode(HttpContext.Current.Request.Url.AbsoluteUri.Remove(HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.ApplicationPath) + HttpContext.Current.Request.ApplicationPath.Length) + "/services/MSOIntegrationService.asmx");
                doc.SelectSingleNode("/es:esodaData/es:ServiceEndpoint[1]", mgr).AppendChild(url);

                XmlNode node = doc.SelectSingleNode("/es:esodaData/es:context/es:Description[1]", mgr);
            }
            else
                throw new Exception("Brak ustawień dla aktów prawnych.");
            return doc;
        }

        public int ProlongeTicket(MSOWebServiceCallContextDTO context, Guid userGuid)
        {
            DbCommand cmd = db.GetStoredProcCommand("Dokumenty.prolongeTicket", context.Ticket, context.DocumentGUID, userGuid, null);
            db.ExecuteNonQuery(cmd);
            return (int)cmd.Parameters[0].Value;
        }

        public int CheckTicket(MSOWebServiceCallContextDTO context)
        {
            return (int)db.ExecuteScalar("Dokumenty.checkTicket", context.Ticket, context.DocumentGUID);
        }

        public string GetFormExtension(int documentTypeID)
        {
            return db.ExecuteScalar("Dokumenty.getFormExtension", documentTypeID).ToString();
        }

        public void DeleteTicket(MSOWebServiceCallContextDTO context)
        {
            db.ExecuteNonQuery("Dokumenty.deleteTicket", context.Ticket);
        }

        public Guid GetUseGuidFromTicket(string ticket)
        {
            object result = db.ExecuteScalar("Dokumenty.getUserIDFromTicket", ticket);

            if (result != null)
                return (Guid)result;
            else
                return Guid.Empty;
        }

        public int TrySaveExistingLegalAct(XmlDocument xdoc, MSOWebServiceCallContextDTO context)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszDaneAktuPrawnego", context.Ticket, context.DocumentGUID, context.DocumentTypeID, context.LastHistoryID, xdoc.OuterXml, true, null, null);
                db.ExecuteNonQuery(cmd);
                return (int)cmd.Parameters[0].Value;

            }
            catch (SqlException ex)
            {
                throw new ArgumentException("Nie udało się zapisać aktu prawnego (" + ex.Message + ")");
            }

        }

        public int SaveExistingLegalAct(XmlDocument xdoc, MSOWebServiceCallContextDTO context, string fileName, string originalFileName)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("Dokumenty.zapiszDaneAktuPrawnego", context.Ticket, context.DocumentGUID, context.DocumentTypeID, context.LastHistoryID, xdoc.OuterXml, false, fileName, originalFileName);
                db.ExecuteNonQuery(cmd);
                return (int)cmd.Parameters[0].Value;


            }
            catch (SqlException ex)
            {
                throw new ArgumentException("Nie udało się zapisać aktu prawnego (" + ex.Message + ")");
            }
        }
    }
}
