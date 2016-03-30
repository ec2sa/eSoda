using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Web.UI.ePUAP.Skrytka.PullService;
using Pemi.Esoda.Web.UI.Classes;
using Pemi.Esoda.DTO;
using System.Configuration;
using System.Security.Cryptography;
using System.Xml;
using System.Text;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using System.Web.Security;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Pemi.Esoda.Web.UI.Aplikacje.DziennikKancelaryjny
{
    public partial class ePUAPIntegration : System.Web.UI.Page
    {
        protected int DocumentsCount
        {
            get
            {
                if (ViewState["dcount"] == null)
                    ViewState["dcount"] = 0;
                return (int)ViewState["dcount"];
            }
            set
            {
                ViewState["dcount"] = value;
            }
        }

        protected ePUAPIntegrationDTO Document
        {
            get
            {
                return (ePUAPIntegrationDTO)Session["dcontent"];
            }
            set
            {
                Session["dcontent"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["parentPage"] = Request.QueryString["pp"];

            DocumentsCount = getDocumentsCount();
            if (DocumentsCount > 0)
            {
                lblBoxStatus.Text = "Liczba dokumentów w skrytce: " + DocumentsCount;
            }
            else
            {
                lblBoxStatus.Text = "Brak dokumentów w skrytce";
            }
            if (!IsPostBack)
            {
                if (DocumentsCount > 0)
                    lbtnGetFirstDocument.Visible = true;
                else
                    lbtnGetFirstDocument.Visible = false;
            }
        }

        protected void lbtnGetFirstDocument_Click(object sender, EventArgs e)
        {
            List<ePUAPIntegrationDTO> documents = new List<ePUAPIntegrationDTO>();
            ePUAPIntegrationDTO document = getDocument();
            if (document != null)
            {
                Document = document;
                documents.Add(document);
                if (document.DocumentName.ToLower() == "upp.xml")
                {
                    XmlDocument xmlContent = getXmlContent(document.DocumentContent);
                    if (xmlContent.DocumentElement.NamespaceURI == "http://crd.gov.pl/xml/schematy/UPO/2008/05/09/")
                        documentsGrid.Columns[6].Visible = true;
                }

                documentsGrid.DataSource = documents;
                documentsGrid.DataBind();
            }
        }

        protected void documentsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Confirm")
            {
                try
                {
                    if (confirmDocument())
                    {
                        int registryID;
                        if (Session["registryId"] != null && int.TryParse(Session["registryId"].ToString(), out registryID))
                        {
                            int[] result = new RegistryDAO().AcquireItemID(registryID, new Guid(Membership.GetUser().ProviderUserKey.ToString()), Document.ResponseAddress,false);

                            int posNumber = result[0];
                            int documentID = result[1];

                            Guid itemGuid = Guid.Empty;

                            if (new DocumentDAO().AddNewDocumentItem(documentID, Document.DocumentName, "", new MemoryStream(Document.DocumentContent), Document.DocumentType, ref itemGuid, DocumentItemCategory.Uploaded))
                            {
                                Session["itemId"] = posNumber.ToString();
                                Response.Redirect("~/Aplikacje/DziennikKancelaryjny/EdycjaPozycjiDziennika.aspx?pp=s");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMsg.Text = "Próba potwierdzenia dokumentu nie powiodła się: " + ex.Message;
                }
            }
            else if (e.CommandName == "Delete")
            {
                try
                {
                    confirmDocument();
                    string parentPage = Session["parentPage"].ToString();
                    Response.Redirect("~/Aplikacje/DziennikKancelaryjny/ePUAPIntegration.aspx?pp=" + parentPage);
                }
                catch (Exception ex)
                {
                    errorMsg.Text = "Próba usunięcia dokumentu nie powiodła się: " + ex.Message;
                }
            }
        }

        protected void lbtnRefresh_Click(object sender, EventArgs e)
        {
            string parentPage = Session["parentPage"].ToString();
            Response.Redirect("~/Aplikacje/DziennikKancelaryjny/ePUAPIntegration.aspx?pp=" + parentPage);
        }

        protected void GoBack(object sender, EventArgs e)
        {
            string parentPage = (string)Session["parentPage"];
            if (parentPage == "a")
            {
                Response.Redirect("PrzegladDziennika.aspx");
            }
            else //if (parentPage == "s")
            {
                Response.Redirect("PrzegladDziennikaSimple.aspx");
            }
        }

        private int getDocumentsCount()
        {
            ePUAPHelper helper = new ePUAPHelper();
            try
            {
                ePUAPQueue queue = helper.GetQueueCount();
                if (queue.Status.Code == StatusCode.Success)
                    return queue.QueueCount;
                else
                    errorMsg.Text = queue.Status.Message;
            }
            catch (Exception ex)
            {
                errorMsg.Text = string.Format("Nie udało się pobrać informacji ze skrytki.[EX:{0}]  [SOAP Fault:{1}]", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "---");
            }
            return -1;
        }

        private ePUAPIntegrationDTO getDocument()
        {
            ePUAPIntegrationDTO integrationDTO = new ePUAPIntegrationDTO();
            ePUAPHelper helper = new ePUAPHelper();

            try
            {
                ePUAPDocument document = helper.GetDocument();
                if (document != null)
                {
                    if (document.RequestStatus.Code == StatusCode.Success)
                    {
                        integrationDTO.DocumentType = document.Attachment.FileType;
                        integrationDTO.DocumentName = document.Attachment.FileName;
                        integrationDTO.DocumentContent = document.Attachment.Content;
                        integrationDTO.DocumentSendDate = document.SendDate;
                        integrationDTO.DocumentSenderName = document.Sender.UserName;
                        integrationDTO.ResponseAddress = document.ResponseAddress;

                        return integrationDTO;
                    }
                    else
                        errorMsg.Text = "Błąd pobierania dokumentu ze skrytki: " + document.RequestStatus.Message;
                }
            }
            catch (Exception ex)
            {
                errorMsg.Text = string.Format("Nie udało się pobrać dokumentu ze skrytki.[EX:{0}]  [SOAP Fault:{1}]", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "---");
            }

            return null;
        }

        private bool confirmDocument()
        {
            ePUAPHelper helper = new ePUAPHelper();

            try
            {
                string errorMessage = null;
                if (helper.ConfirmReceipt(Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Document.DocumentContent)), out errorMessage))
                    return true;
                else
                    errorMsg.Text = "Błąd potwierdzenia odbioru dokumentu: " + errorMessage;
            }
            catch (Exception ex)
            {
                errorMsg.Text = string.Format("Nie udało się pobrać dokumentu ze skrytki.[EX:{0}]  [SOAP Fault:{1}]", ex.Message, ex.InnerException != null ? ex.InnerException.Message : "---");
            }
            return false;
        }

        private XmlDocument getXmlContent(byte[] content)
        {
            XmlDocument xmlContent = new XmlDocument();
            xmlContent.LoadXml(Encoding.UTF8.GetString(content));
            return xmlContent;
        }
    }
}
