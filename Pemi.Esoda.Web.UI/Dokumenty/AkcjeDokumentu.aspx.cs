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
    public partial class AkcjeDokumentu : BaseContentPage, IViewDocumentActionsView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ViewDocumentActionsPresenter presenter = new ViewDocumentActionsPresenter(this, new WebSessionProvider());
            Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "LinkButton4";
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

        protected void wykonajAkcje(object sender, CommandEventArgs e)
        {
            int docId = CoreObject.GetId(Request);
            switch (e.CommandArgument.ToString())
            {
                case "450423F0-4819-4BE3-8BF4-E42C17815B59":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/PrzekierowanieDokumentu.aspx?id="+docId.ToString(), false); 
                    break;
                case "1E476BE2-1FA1-46AF-A45C-A5C0F3882E8E":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Session["nowaSprawa"] = false;
                    Response.Redirect("~/Akcje/PrzypisanieDokumentuDoSprawy.aspx?id=" + docId.ToString(), false);
                    break;
                case "363F872A-CC31-4FA7-8B30-C9917E185544":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Session["nowaSprawa"] = true;
                    Response.Redirect("~/Akcje/PrzypisanieDokumentuDoSprawy.aspx?id=" + docId.ToString(), false);
                    break;
                case "10672122-E570-4A04-9D5D-A6667C27FE3D":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/DekretacjaDokumentuWielokrotna.aspx?id=" + docId.ToString(), false);
                    break;
                case "5B1EDF0C-DE49-4D5C-A116-54A5E25C6FB8":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/EdycjaDokumentu.aspx?id=" + docId.ToString(), false);
                    break;
                case "ED10E89A-365B-4034-9710-1E58BB93F5E9":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Session["idObiektu"] = Session["idDokumentu"];
                    Session["fromCase"] = null;
                    Session["fromDoc"] = true;
                    Response.Redirect("~/Akcje/SelectRegistry.aspx?id=" + docId.ToString(), false);
                    break;
                case "ADBAD8B1-ABE0-41DA-8E17-32817C4FAE74":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/WykonanieCzynnosci.aspx?id=" + docId.ToString(), false);
                    break;    
            }
        }

        #region IViewDocumentActionsView Members

        int IViewDocumentActionsView.DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        string IViewDocumentActionsView.Items
        {
            set
            {
                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                lista.DataSource = null;
                if (xds.Data != "")
                {
                    xds.XPath = "/akcje/akcja";
                    lista.DataSource = xds;
                }
                lista.DataBind();
            }
        }

        Guid IViewDocumentActionsView.SelectedItem
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        Guid IViewDocumentActionsView.UserId
        {
            get { return (Guid)Membership.GetUser().ProviderUserKey; }
        }

        #endregion
    }
}