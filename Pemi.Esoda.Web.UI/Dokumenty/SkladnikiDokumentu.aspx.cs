using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Presenters;
using System.IO;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;
using System.Data.SqlClient;

namespace Pemi.Esoda.Web.UI
{
    public partial class SkladnikiDokumentu : BaseContentPage, IViewDocumentItemsView
    {
        public string APath
        {
            get
            {
                return Page.Request.ApplicationPath == "/" ? "" : Page.Request.ApplicationPath;
            }
        }

        public string temporaryFileName
        {
            get
            {                
                if (Session["{78582398-0E25-4825-89D7-D569C4826015}"] == null) return string.Empty;
                return Session["{78582398-0E25-4825-89D7-D569C4826015}"].ToString();
            }
            set
            {
                Session["{78582398-0E25-4825-89D7-D569C4826015}"] = value;
            }
        }

        private int documentId;

        private ViewDocumentItemsPresenter presenter;

        private EventHandler<ExecutingCommandEventArgs> obslugaPolecen;

        private void OnObslugaPolecen(ExecutingCommandEventArgs e)
        {
            if (obslugaPolecen != null)
                obslugaPolecen(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
                  
            listaPlikow.ItemCommand += new RepeaterCommandEventHandler(listaPlikow_ItemCommand);
            presenter = new ViewDocumentItemsPresenter(this, new WebSessionProvider());
            Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "LinkButton2";
            if (!IsPostBack)
            {
                int docId = CoreObject.GetId(Request);
                if (docId > 0)
                {
                    if (!Page.User.IsInRole("Administratorzy") && !(new DocumentDAO()).IsDocVisibleForUser(docId, new Guid(Membership.GetUser().ProviderUserKey.ToString())))
                    {
                        BaseContentPage.SetError("Nie masz uprawnieñ do tego dokumentu", "~/OczekujaceZadania.aspx");
                    }
                }

                documentItemUploaderContent.Visible = false;
                documentItemWordUploaderContetnt.Visible = false;

                try
                {
                    presenter.Initialize();
                }
                catch (Exception ex)
                {
                    //throw;
                    BaseContentPage.SetError("Nie wybrano dokumentu", "~/OczekujaceZadania.aspx");
                }

                RedirectToMSOTemplate();
                RedirectToLegalAct();
            }
            presenter.OnLoad();

            DocumentItemUploader1.CancelUpload += new EventHandler(DocumentItemUploader1_CancelUpload);
            DocumentItemWordUploader.CancelUpload += new EventHandler(DocumentItemWordUploader_CancelUpload);

            
        }

        private void RedirectToMSOTemplate()
        {
            if (!String.IsNullOrEmpty(Page.Request["itemGUID"]) && !String.IsNullOrEmpty(Page.Request["mode"]) && !String.IsNullOrEmpty(Page.Request["id"]))
            {
                string url = String.Format("SkladnikDoc.aspx?id={0}&itemGUID={1}&mode={2}", Page.Request["id"].ToString(), Page.Request["itemGUID"].ToString(), Page.Request["mode"].ToString());

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "1;url=" + url;
                Page.Header.Controls.Add(meta);
            }
            else if (!String.IsNullOrEmpty(Page.Request["mode"]) && !String.IsNullOrEmpty(Page.Request["id"]))
            {
                string url = String.Format("SkladnikDoc.aspx?id={0}&mode={1}", Page.Request["id"].ToString(), Page.Request["mode"].ToString());

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "1;url=" + url;
                Page.Header.Controls.Add(meta);
            }  
        }

        void DocumentItemWordUploader_CancelUpload(object sender, EventArgs e)
        {
            documentItemUploaderContent.Visible = false;
            documentItemWordUploaderContetnt.Visible = false;
            wyczyscZaznaczenie(null);
        }

        protected void listaPlikow_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "openWord")
            {
                Response.Redirect(String.Format("~/Dokumenty/SkladnikiDokumentu.aspx?id={0}&itemGUID={1}&mode={2}", CoreObject.GetId(Request), e.CommandArgument.ToString(), "e"));//e - przy edycji                
            }
            else if (e.CommandName == "newVersion")
            {
                documentItemUploaderContent.Visible = true;
                DocumentItemUploader1.IdOryginalu = new Guid(e.CommandArgument.ToString());
                (e.Item.FindControl("blok") as Panel).CssClass = "currentItem opcjePodgladuPliku";
                wyczyscZaznaczenie(e.Item);
            }
            else
                if (e.CommandName == "toggleVersions")
                {
                    e.Item.FindControl("wersje").Visible = !(e.Item.FindControl("wersje").Visible);
                }
                else
                    OnObslugaPolecen(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        private void wyczyscZaznaczenie(RepeaterItem repeaterItem)
        {
            foreach (RepeaterItem ri in listaPlikow.Items)
            {
                if (ri != repeaterItem)
                    (ri.FindControl("blok") as Panel).CssClass = "opcjePodgladuPliku";
            }
        }

        protected void listaNarzedzi_Command(object sender, CommandEventArgs e)
        {
            OnObslugaPolecen(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        protected void zmianaParametrow(object sender, EventArgs e)
        {
            OnObslugaPolecen(new ExecutingCommandEventArgs("changePageOrScale", string.Empty));
        }

        protected void dodawanieNowegoPliku(object sender, EventArgs e)
        {
            documentItemUploaderContent.Visible = true;
            documentItemWordUploaderContetnt.Visible = false;
            wyczyscZaznaczenie(null);
        }

        void DocumentItemUploader1_CancelUpload(object sender, EventArgs e)
        {
            documentItemUploaderContent.Visible = false;
            documentItemWordUploaderContetnt.Visible = false;
            wyczyscZaznaczenie(null);
        }

        #region IViewDocumentItemsView Members

        int IViewDocumentItemsView.DocumentId
        {
            set
            {
                documentId = value;
            }
            get
            {
                return CoreObject.GetId(Request); // documentId;
            }
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.DocumentItemDTO> IViewDocumentItemsView.Items
        {
            set
            {
                this.listaPlikow.DataSource = value;
                listaPlikow.DataBind();
            }
        }

        event EventHandler<ExecutingCommandEventArgs> IViewDocumentItemsView.ExecutingCommand
        {
            add { obslugaPolecen += value; }
            remove { obslugaPolecen -= value; }
        }

        string IViewDocumentItemsView.previewImageUrl
        {
            set
            {
                imagePreview.ImageUrl = value + string.Format("?{0}", Guid.NewGuid());
                temporaryFileName = Path.GetFileName(value);
            }
        }

        bool IViewDocumentItemsView.IsInListMode
        {
            set
            {
                blokListy.Visible = value;
                blokNarzedzi.Visible = !value;
                imagePreviewContent.Visible = !value;
            }
        }

        event EventHandler IViewDocumentItemsView.ReturnToFileList
        {
            add { powrotDoListy.Click += value; }
            remove { powrotDoListy.Click -= value; }
        }

        int IViewDocumentItemsView.CurrentPage
        {
            get
            {
                int nr;
                if (!int.TryParse(numerAktualnejStrony.Text, out nr)) throw new ArgumentException(string.Format("Niepoprawny numr strony! {0}", numerAktualnejStrony.Text));
                return nr;
            }
            set
            {
                numerAktualnejStrony.Text = value.ToString();

            }
        }

        int IViewDocumentItemsView.CurrentScale
        {
            get
            {
                int nr;
                if (!int.TryParse(aktualnaSkala.Text, out nr)) throw new ArgumentException("Niepoprawna wartoœæ skali!");
                return nr;
            }
            set
            {
                aktualnaSkala.Text = value.ToString();
            }
        }

        int IViewDocumentItemsView.PageCount
        {
            set { liczbaStron.InnerText = string.Format(" z {0}", value); }
        }

        #endregion

        protected void listaPlikow_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DocumentItemDTO item = e.Item.DataItem as DocumentItemDTO;

                if (item != null)
                {
                    if (item.Category == DocumentItemCategory.Created)
                    {
                        e.Item.FindControl("Image1").Visible = false;
                        e.Item.FindControl("Image2").Visible = false;
                        e.Item.FindControl("Image3").Visible = true;
                        e.Item.FindControl("imageSave").Visible = false;
                        e.Item.FindControl("openWord").Visible = true;

                        foreach (Control c in e.Item.FindControl("blok").Controls)
                            if (c is LinkButton)
                            {
                                if ((c as LinkButton).CommandName == "newVersion")
                                    c.Visible = false;
                                if ((c as LinkButton).CommandName == "toggleVersions")
                                    c.Visible = (item.PreviousVersions.Count > 0);
                                if ((c as LinkButton).CommandName == "openFile")
                                    c.Visible = false;
                            }
                    }
                    else
                    {
                        e.Item.FindControl("Image1").Visible = (item.MimeType == "image/tiff");
                        e.Item.FindControl("Image2").Visible = (item.MimeType != "image/tiff");
                        e.Item.FindControl("Image3").Visible = false;
                        e.Item.FindControl("openWord").Visible = false;

                        e.Item.FindControl("saveAsPDF").Visible = (item.MimeType == "image/tiff");

                        foreach (Control c in e.Item.FindControl("blok").Controls)
                            if (c is LinkButton)
                            {
                                if ((c as LinkButton).CommandName == "newVersion")
                                    c.Visible = (item.OriginalItemID == Guid.Empty);
                                if ((c as LinkButton).CommandName == "toggleVersions")
                                    c.Visible = (item.PreviousVersions.Count > 0);
                                if ((c as LinkButton).CommandName == "openFile")
                                    c.Visible = (item.Browsable);
                            }
                    }
                }
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            docPreview.DocId = presenter.DocumentId;
        }

        protected void nowyWord_Click(object sender, EventArgs e)
        {
            documentItemUploaderContent.Visible = false;
            documentItemWordUploaderContetnt.Visible = true;
            wyczyscZaznaczenie(null);
        }      

        private void RedirectToLegalAct()
        {
            if (!String.IsNullOrEmpty(Page.Request["law"]) && !String.IsNullOrEmpty(Page.Request["id"]))
            {
                string url = String.Format("AktPrawny.aspx?id={0}", Page.Request["id"].ToString());

                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "1;url=" + url;
                Page.Header.Controls.Add(meta);
            }
        }
       
        #region IViewDocumentItemsView Members

        public bool IsMSOTemplateVisible
        {
            set { nowyWord.Visible = value; }
        }
     
        public string Message
        {
            set { lblMessage.Text = string.Empty; }
        }

        #endregion
    }
}