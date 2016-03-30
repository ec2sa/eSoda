using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using Pemi.Esoda.Web.UI.ESPData;
using Pemi.Esoda.Web.UI.PemiESP;
using System.Xml;
using Pemi.Esoda.DataAccess;
using System.Text;
using System.Data.Common;
using Pemi.Esoda.Presenters;
using Pemi.Esoda.Presenters.Interfaces;
using System.IO;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI
{
    public partial class DokumentyOczekujace : BaseContentPage, IAwaitingESPDocumentView
    {
        AwaitingESPDocumentsPresenter presenter;
        event EventHandler<ExecutingCommandEventArgs> executingCommand;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new AwaitingESPDocumentsPresenter(this, new WebSessionProvider());
            presenter.ExecuteCommand += new EventHandler<ExecutingCommandEventArgs>(ExecuteCommand);
            if (!IsPostBack)
                presenter.Initialize();
            lnkAddDocument.Visible = false;
        }

        protected void OnExecutingCommand(ExecutingCommandEventArgs e)
        {
            if (executingCommand != null)
                executingCommand(this, e);
        }

        protected void gvESPDocumentsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "idStatusu").ToString();
                LinkButton lnkPreview = (LinkButton)e.Row.FindControl("lnkPreview");
                if (lnkPreview != null)
                {
                    lnkPreview.Visible = status.Equals("3") || status.Equals("4");
                }

                LinkButton lnkDownload = (LinkButton)e.Row.FindControl("lnkDownload");
                if (lnkDownload != null)
                {
                    lnkDownload.Visible = status.Equals("1");
                }
            }
        }

        void ExecuteCommand(object sender, ExecutingCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "newItem":
                    Session["itemId"] = e.CommandArgument;
                    (new DocumentDAO()).SetESPDocumentStatus(new Guid(Session["ESPDocId"].ToString()), DocumentDAO.ESPDocumentStatus.Adding);
                    Response.Redirect("EdycjaPozycjiDziennika.aspx");
                    break;

                default:
                    break;
            }
        }

        protected void gvESPDocumentsList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DocumentDAO docDao = new DocumentDAO();
            string sTicket = (ConfigurationManager.AppSettings["espticket"] != null) ? ConfigurationManager.AppSettings["espticket"].ToString() : string.Empty;

            switch (e.CommandName)
            {
                case "preview":
                    Session["ESPDocId"] = e.CommandArgument;
                    using (DbDataReader dr = (DbDataReader)docDao.GetESPDocumentData(new Guid(e.CommandArgument.ToString())))
                    {
                        if (dr.Read())
                        {
                            lnkAddDocument.Visible = dr["idStatusu"].ToString().Equals("3");
                            docPreview.DocGuid = new Guid(e.CommandArgument.ToString());
                        }
                    }                    
                    break;

                case "download":
                    ESPWebService ws = new ESPWebService();
                    if (ws != null)
                    {
                        
                        Guid docGuid = new Guid(e.CommandArgument.ToString());
                        docDao.SetESPDocumentStatus(docGuid, DocumentDAO.ESPDocumentStatus.Downloading);
                        try
                        {

                            string docId = docDao.GetESPDocId(docGuid);
                            if (docId != string.Empty)
                            {
                                string docData = ws.GetDocument(sTicket, docId);
                                if (docData.Contains("<error>"))
                                {
                                    WebMsgBox.Show(this, "Błąd pobierania dokumentu ESP");
                                }
                                else
                                {
                                    byte[] data = Convert.FromBase64String(docData);

                                    ws.Dispose();
                                    string xmlDoc = Encoding.UTF8.GetString(data);
                                   
                                    // pobranie XML i XSLT dokumentu
                                    XmlDocument doc = new XmlDocument();
                                    doc.LoadXml(xmlDoc);

                                    data = Convert.FromBase64String(doc.SelectSingleNode("//Dokument[@typ='Dane']").InnerText);
                                    XmlDocument xmlData = new XmlDocument();
                                    xmlData.LoadXml(Encoding.UTF8.GetString(data).Replace("UTF-8", "UTF-16").Replace("utf-8", "utf-16"));

                                    data = Convert.FromBase64String(doc.SelectSingleNode("//Dokument[@typ='Styl']").InnerText);
                                    XmlDocument xslData = new XmlDocument();
                                    xslData.LoadXml(Encoding.UTF8.GetString(data).Replace("UTF-8", "UTF-16").Replace("utf-8", "utf-16"));

                                    IItemStorage storage = ItemStorageFactory.Create();

                                    StringBuilder sb = new StringBuilder();
                                    XmlWriter xw = XmlWriter.Create(sb);
                                    xw.WriteStartElement("zalaczniki");

                                    foreach (XmlNode node in doc.SelectNodes("//Dokument[@typ='Zalacznik']"))
                                    {
                                        // dodawanie zalacznikow
                                        data = Convert.FromBase64String(node.InnerText);
                                        MemoryStream ms = new MemoryStream(data);
                                        Guid attachmentGuid = storage.Save(ms);

                                        xw.WriteStartElement("zalacznik");
                                        xw.WriteAttributeString("id", attachmentGuid.ToString());
                                        xw.WriteAttributeString("nazwa", node.Attributes["Nazwa"].Value);
                                        xw.WriteAttributeString("mime", node.Attributes["Mime"].Value);
                                        xw.WriteEndElement();
                                    }

                                    xw.WriteEndElement();
                                    xw.Close();

                                    docDao.SetESPDocumentData(docGuid, xmlData.InnerXml, xslData.InnerXml, sb.ToString());
                                    docDao.SetESPDocumentStatus(docGuid, DocumentDAO.ESPDocumentStatus.Downloaded);

                                    ws.ConfirmDocumentReceive(sTicket, docGuid.ToString());
                                    gvESPDocumentsList.DataBind();
                                }
                            }
                            else
                            {
                                WebMsgBox.Show(this, "Nie można pobrać ID dokumentu ESP");
                            }
                        }
                        catch //(Exception ex)
                        {
                            docDao.SetESPDocumentStatus(docGuid, DocumentDAO.ESPDocumentStatus.Awaiting);
                        }
                    }                    
                    break;

                default:
                    docPreview.XmlData = string.Empty;
                    lnkAddDocument.Visible = false;
                    break;
            }
        }

        protected void lnkCheckESPForDocuments_Click(object sender, EventArgs e)
        {
            string sTicket = (ConfigurationManager.AppSettings["espticket"] != null) ? ConfigurationManager.AppSettings["espticket"].ToString() : string.Empty;

            ESPWebService ws = new ESPWebService();
            
            if (ws != null)
            {
                ws.Discover();
                string xmlDocList = ws.GetAwaitingDocuments(sTicket);
                ws.Dispose();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlDocList);

                if (doc.SelectSingleNode("//error") != null)
                {
                    WebMsgBox.Show(this, doc.SelectSingleNode("//error").Value);
                }
                else
                {
                    DocumentDAO docDAO = new DocumentDAO();
                    foreach (XmlNode node in doc.SelectNodes("//dokument"))
                    {
                        if (!docDAO.ESPDocExist(node.Attributes["id"].Value))
                            docDAO.AddAwaitingESPDocument(node.Attributes["id"].Value, node.Attributes["nazwa"].Value, node.Attributes["opis"].Value);
                    }
                    gvESPDocumentsList.DataBind();
                }
            }
            else
            {
                WebMsgBox.Show(this, "Nie udało się połączyć z ESP");
            }
        }

        #region IAwaitingESPDocumentView Members

        public int ItemID
        {
            set { throw new NotImplementedException(); }
        }

        event EventHandler IAwaitingESPDocumentView.AddESPDocument
        {
            add { lnkAddDocument.Click += value; }
            remove { lnkAddDocument.Click -= value; }
        }

        int IAwaitingESPDocumentView.RegistryID
        {
            get { return 0; }
            set { }
        }

        public event EventHandler<ExecutingCommandEventArgs> CommandExecuting;

        #endregion
    }
}