using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Web.Security;
using Pemi.Esoda.Tools;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace Pemi.Esoda.Web.Services
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://esoda.pl/MSOIntegration")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class MSOIntegrationService : System.Web.Services.WebService
    {
        
        [WebMethod]
        public OperationResult ProlongeTicket(MSOWebServiceCallContextDTO context, string login, string password)
        {
            try
            {
                if (!Membership.ValidateUser(login, password))
                    return OperationResult.InvalidUsernameOrPassword;

                int rv = new MSOIntegrationDAO().ProlongeTicket(context, (Guid)Membership.GetUser(login).ProviderUserKey);

                switch (rv)
                {
                    case 0:
                        return OperationResult.OK;
                    default:
                        return OperationResult.InvalidTicket;
                }
            }
            catch
            {
                return OperationResult.DBConnectionError;
            }
        }

        [WebMethod]
        public OperationResult SaveForm(XmlDocument xdoc, MSOWebServiceCallContextDTO context, byte[] formContent)
        {
            OperationResult result = OperationResult.OK;

            if (!validateContext(context,true))
            {
                result = OperationResult.InvalidContext;
                return result;
            }

            switch (validateXmlData(xdoc, context))
            {
                case 0:
                    result = OperationResult.InvalidXmlData;
                    break;

                case -1:
                    result = OperationResult.SchemaNotFound;
                    break;

                case -2:
                    result = OperationResult.XmlValidatorError;
                    break;
            }

            MSOIntegrationDAO dao = new MSOIntegrationDAO();
            OperationResult tmpRes = result;
            string extension=null;
            try
            {
                 extension= Path.GetExtension(dao.GetFormExtension(context.DocumentTypeID));
            }
            catch
            {
                return OperationResult.DBConnectionError;
            }
            Guid tempFilename = Guid.NewGuid();
            try
            {
                int rv = dao.TrySaveExistingForm(xdoc, context);
             
                if (rv == 0 && tmpRes != OperationResult.OK)
                    return tmpRes;
              
                    rv = dao.SaveExistingForm(xdoc, context, tempFilename.ToString() + extension);
               
                switch (rv)
                {
                    case -1:
                        result = OperationResult.InvalidDocument;
                        break;
                    case -2:
                        result = OperationResult.InvalidTicket;
                        break;
                    case -3:
                        result = OperationResult.TicketExpired;
                        break;
                    case -4:
                        result = OperationResult.DocumentTypeChanged;
                        break;
                    case -5:
                        result = OperationResult.DocumentContentChanged;
                        break;
                    case -6:
                        result = OperationResult.InvalidFormHash;
                        break;
                }
            }
            catch (Exception)
            {
                result = OperationResult.UnableToSaveDocument;
                return result;
            }

            if (result != OperationResult.OK)
                return result;

            string filePath = Path.Combine(Pemi.Esoda.Tools.MSOIntegrationHelper.MSOProcessingHelper.MSODirectoryPath , tempFilename.ToString() + extension);

            try
            {
                File.WriteAllBytes(filePath, formContent);
            }
            catch
            {
                return OperationResult.UnableToSaveFormContent;
            }
            dao.DeleteTicket(context);
            return result;
        }

        [WebMethod]
        public OperationResult SaveLegalAct(XmlDocument xdoc, MSOWebServiceCallContextDTO context, byte[] formContent,string fileName)
        {
            OperationResult result = OperationResult.OK;

            if (!validateContext(context, true))
            {
                result = OperationResult.InvalidContext;
                return result;
            }

            //switch (validateXmlData(xdoc, context))
            //{
            //    case 0:
            //        result = OperationResult.InvalidXmlData;
            //        break;

            //    case -1:
            //        result = OperationResult.SchemaNotFound;
            //        break;

            //    case -2:
            //        result = OperationResult.XmlValidatorError;
            //        break;
            //}

            MSOIntegrationDAO dao = new MSOIntegrationDAO();
            OperationResult tmpRes = result;
            string extension = Path.GetExtension(fileName);
            //try
            //{
            //    extension = Path.GetExtension(dao.GetFormExtension(context.DocumentTypeID));
            //}
            //catch
            //{
            //    return OperationResult.DBConnectionError;
            //}
            Guid tempFilename = Guid.NewGuid();
            try
            {
                int rv = dao.TrySaveExistingLegalAct(xdoc, context);

                if (rv == 0 && tmpRes != OperationResult.OK)
                    return tmpRes;

                rv = dao.SaveExistingLegalAct(xdoc, context, tempFilename.ToString() + extension,fileName!=null?Path.GetFileName(fileName):null);

                switch (rv)
                {
                    case -1:
                        result = OperationResult.InvalidDocument;
                        break;
                    case -2:
                        result = OperationResult.InvalidTicket;
                        break;
                    case -3:
                        result = OperationResult.TicketExpired;
                        break;
                    case -4:
                        result = OperationResult.DocumentTypeChanged;
                        break;
                    case -5:
                        result = OperationResult.DocumentContentChanged;
                        break;
                    case -6:
                        result = OperationResult.InvalidFormHash;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                result = OperationResult.UnableToSaveDocument;
                return result;
            }

            if (result != OperationResult.OK)
                return result;

            string filePath = Path.Combine(Pemi.Esoda.Tools.MSOIntegrationHelper.MSOProcessingHelper.MSODirectoryPath, tempFilename.ToString() + extension);

            try
            {
                File.WriteAllBytes(filePath, formContent);
            }
            catch
            {
                return OperationResult.UnableToSaveFormContent;
            }
            dao.DeleteTicket(context);
            return result;
        }

        [WebMethod]
        public UploadResult SaveGenericDocument(byte[] documentContent, string originalName, string mimeType, MSOWebServiceCallContextDTO context)
        {
            if (documentContent == null || documentContent.Length == 0)
                return UploadResult.FileContentEmpty;

            if (string.IsNullOrEmpty(originalName) || string.IsNullOrEmpty(mimeType))
                return UploadResult.InvalidFilenameOrMimeType;

            if (!validateContext(context,false))
                return UploadResult.InvalidContext;

            MSOIntegrationDAO dao = new MSOIntegrationDAO();
            try
            {
                switch (dao.CheckTicket(context))
                {
                    case 0:
                        return UploadResult.TicketExpired;
                    case -1:
                        return UploadResult.InvalidTicket;
                }
            }
            catch
            {
                return UploadResult.DBConnectionError;
            }
            Guid tempFilename = Guid.NewGuid();
            string filePath = Path.Combine(Server.MapPath("~/temp"), tempFilename.ToString());

            try
            {
                File.WriteAllBytes(filePath, documentContent);
            }
            catch
            {
                return UploadResult.UnableToSaveFile;
            }
            try
            {
                switch (saveGenericDocument(filePath, originalName, mimeType, context.DocumentGUID, context.LastVersionGuid, context.Description, originalName, context.Ticket))
                {
                    case -1:
                        return UploadResult.DocumentNotFound;
                    case -2:
                        return UploadResult.UnableToAddDocumentItem;

                }
                dao.DeleteTicket(context);
                return UploadResult.OK;
            }
            catch
            {
                return UploadResult.DBConnectionError;
            }
        }

        private int validateXmlData(XmlDocument xdoc, MSOWebServiceCallContextDTO context)
        {
            MSOIntegrationDAO dao = new MSOIntegrationDAO();
            string schemaFile = Path.Combine(Pemi.Esoda.Tools.MSOIntegrationHelper.MSOProcessingHelper.MSODirectoryPath, dao.GetMSOSchemaFileName(context.DocumentGUID));

            if (string.IsNullOrEmpty(schemaFile))
                return -1; //no schemaFile


            XmlValidator xv = new XmlValidator();
            try
            {
                return xv.ValidateWithSchemaFile(xdoc.OuterXml, schemaFile) ? 1 : 0; //1 - xml valid, 0 - xml not valid
            }
            catch
            {
                return -2; //validator error 
            }
        }

        private bool validateContext(MSOWebServiceCallContextDTO context,bool withDocumentTypeID)
        {

            return (
                context != null
                && context.DocumentGUID != Guid.Empty
                && (context.DocumentTypeID > 0 || !withDocumentTypeID)
                // && (context.LastHistoryID == null || context.LastHistoryID > 0)
                && !string.IsNullOrEmpty(context.Ticket)
                );
        }

        private int saveGenericDocument(string filePath, string originalFilename, string mimeType, Guid documentGuid, Guid? elementVersionGuid, string description, string desiredName, string ticket)
        {
            DocumentDAO dao = new DocumentDAO();
            Guid userID = new MSOIntegrationDAO().GetUseGuidFromTicket(ticket);
            Guid itemId = Guid.Empty;

            int documentID = dao.GetDocumentIDForGuid(documentGuid);
            if (documentID == 0)
                return -1;
            FileStream fs = File.OpenRead(filePath);

            try
            {

                if (!elementVersionGuid.HasValue)
                {
                    dao.AddNewDocumentItem(documentID, originalFilename, description, fs, mimeType, ref itemId, DocumentItemCategory.Created);

                    DocumentItemDTO docItem = dao.GetItem(itemId);
                    string[] fullname = Membership.GetUser(userID).Comment.Split(' ');

                    string imie = fullname.Length >= 2 ? fullname[1] : string.Empty;
                    string nazwisko = fullname.Length >= 1 ? fullname[0] : string.Empty;


                    List<string> paramList = new List<string>();
                    paramList.Add(originalFilename);


                    ActionLogger al = new ActionLogger(new ActionContext(new Guid("9cd585bb-2a06-4c24-b415-aa3f8b00ea5f"), userID, Membership.GetUser(userID).UserName, Membership.GetUser(userID).Comment, paramList));

                    al.AppliesToDocuments.Add(documentID);
                    al.ActionData.Add("idDokumentu", documentID.ToString());
                    al.ActionData.Add("idPracownika", Membership.GetUser(userID).UserName);
                    al.ActionData.Add("imie", imie);
                    al.ActionData.Add("nazwisko", nazwisko);
                    al.ActionData.Add("dataDodania", docItem.CreationDate.ToString());
                    al.ActionData.Add("idPliku", docItem.ID.ToString());
                    al.ActionData.Add("nazwaPliku", originalFilename);
                    al.Execute();
                }
                else
                {
                    dao.AddNewVersionOfDocumentItem(documentID, elementVersionGuid.Value, description, fs, mimeType, desiredName, DocumentItemCategory.Created);
                }
            }
            catch
            {
                return -2;
            }
            return 0;

        }
    }

    /// <summary>
    /// Status of Operatiop performed on MSO Integration Documents
    /// </summary>
    public enum OperationResult
    {
        /// <summary>
        /// Operation was successful
        /// </summary>
        OK=0,
        /// <summary>
        /// Context has invalid content
        /// </summary>
        InvalidContext,
        /// <summary>
        /// Unable to find eSoda document (based on context data)
        /// </summary>
        InvalidDocument,
        /// <summary>
        /// Issued ticket has expired. It can be prolonged by calling <see cref="ProlongeTicket"/> method
        /// </summary>
        TicketExpired,
        /// <summary>
        /// Provided ticket is invalid (doesn't exists int eSoda)
        /// </summary>
        InvalidTicket,
        /// <summary>
        /// Xml document is not valid (according to schema)
        /// </summary>
        InvalidXmlData,
        /// <summary>
        /// Form no longer exists in eSoda
        /// </summary>
        InvalidFormHash,
        /// <summary>
        /// Unable to authenticate user while prolonging ticket validity
        /// </summary>
        InvalidUsernameOrPassword,
        /// <summary>
        /// Xml Schema is missing 
        /// </summary>
        SchemaNotFound,
        /// <summary>
        /// DOcument in eSoha has different type
        /// </summary>
        DocumentTypeChanged,
        /// <summary>
        /// Document in eSoda has been modified by someone else
        /// </summary>
        DocumentContentChanged,
        /// <summary>
        /// Document could not be saved in database
        /// </summary>
        UnableToSaveDocument,
        /// <summary>
        /// Error occured during validation (validator could not validate xml document)
        /// </summary>
        XmlValidatorError,
        /// <summary>
        /// Occurs when form content cannot be saved
        /// </summary>
        UnableToSaveFormContent,
        /// <summary>
        /// Occurs when connection with DB is down
        /// </summary>
        DBConnectionError
    }

    /// <summary>
    /// Result of upload file operation
    /// </summary>
    public enum UploadResult{
        /// <summary>
        /// Upload was successful
        /// </summary>
        OK,
        /// <summary>
        /// No file content was received
        /// </summary>
        FileContentEmpty,
        /// <summary>
        /// FIlename or mime type is empty or invalid
        /// </summary>
        InvalidFilenameOrMimeType,
        /// <summary>
        /// Context is empty or has invalid content
        /// </summary>
        InvalidContext,
        /// <summary>
        /// Unable to save received content in temporary directory
        /// </summary>
        UnableToSaveFile,
        /// <summary>
        /// Document provided in context doesn't exists in eSoda
        /// </summary>
        DocumentNotFound,
        /// <summary>
        /// Unable to assign document item to logical document in eSoda
        /// </summary>
        UnableToAddDocumentItem,
        /// <summary>
        /// Issued ticket has expired. It can be prolonged by calling <see cref="ProlongeTicket"/> method
        /// </summary>
        TicketExpired,
        /// <summary>
        /// Provided ticket is invalid (doesn't exists int eSoda)
        /// </summary>
        InvalidTicket,
        /// <summary>
        /// Occurs when connection with DB is down
        /// </summary>
        DBConnectionError
}
}
