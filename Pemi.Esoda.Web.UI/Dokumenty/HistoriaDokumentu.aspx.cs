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
using Pemi.Esoda.Tools;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI
{
    public partial class HistoriaDokumentu : BaseContentPage, IViewDocumentHistoryView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DocumentHistoryPresenter presenter = new DocumentHistoryPresenter(this, new WebSessionProvider());
            Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "LinkButton3";
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
                presenter.Initialize();
            }
        }

        protected void execCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "details")
            {
                Session["returnFromActionDetails"] = "~/Dokumenty/HistoriaDokumentu.aspx?id="+CoreObject.GetId(Request).ToString();
                Response.Redirect("~/Akcje/SzczegolyAkcji.aspx?id=" + e.CommandArgument.ToString());
            }
        }

        #region IDocumentHistoryView Members

        int IViewDocumentHistoryView.DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        public string HistoryItems
        {
            set
            {
                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                lista.DataSource = null;
                if (xds.Data != "")
                {
                    xds.XPath = "/historia/akcja";
                    lista.DataSource = xds;
                }
                lista.DataBind();
            }
        }
        #endregion
    }
}