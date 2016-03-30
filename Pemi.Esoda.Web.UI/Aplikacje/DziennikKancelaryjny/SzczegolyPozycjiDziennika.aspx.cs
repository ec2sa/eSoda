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

namespace Pemi.Esoda.Web.UI
{
    public partial class SzczegolyPozycjiDziennika : BaseContentPage,IRegistryItemDetailsView
    {

        public bool IsInvoice
        {
            get
            {
                return Request.QueryString["rf"] != null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            RegistryItemDetailsPresenter presenter = new RegistryItemDetailsPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {
                presenter.Initialize();
                
            }
            Session["parentPage"] = Request.QueryString["pp"];
        }

        #region IRegistryItemDetailsView Members

        string IRegistryItemDetailsView.ItemContent
        {
            set {
                szczegolyPozycji.DocumentContent = value;
                szczegolyPozycji.TransformSource = "~/xslt/registryItem.xslt";
            }
        }

        string IRegistryItemDetailsView.HistoryItems
        {
            set {
                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                historiaPozycji.DataSource = null;
                if (xds.Data != "")
                {
                    xds.TransformFile = "~/xslt/historyItem.xslt";
                    xds.XPath = "/historia/wpis";
                    historiaPozycji.DataSource = xds;
                }
                historiaPozycji.DataBind();
            }
        }

        int IRegistryItemDetailsView.ItemID
        {
            set {
                if(!IsInvoice)
                numerPozycji.Text = string.Format("[numer pozycji w rejestrze: {0}]", value);
                else
                    numerPozycji.Text = string.Format("[numer pozycji w rejestrze faktur: {0}]", value);
            }
        }

        #endregion

        public void GoBack(object sender, EventArgs e)
        {
            string parentPage = (string)Session["parentPage"];
            if (parentPage == "a")
            {
                Response.Redirect("PrzegladDziennika.aspx");
            }
            else if (parentPage == "s")
            {
                Response.Redirect("PrzegladDziennikaSimple.aspx");
            }
        }

        public void Edit(object sender, EventArgs e)
        {
            string parentPage = (string)Session["parentPage"];
            string rf = IsInvoice ? "&rf=1" : "";
            if (parentPage == "a")
            {
                Response.Redirect("EdycjaPozycjiDziennika.aspx?pp=a" + rf);
            }
            else //if (parentPage == "s")
            {
                Response.Redirect("EdycjaPozycjiDziennika.aspx?pp=s" + rf);
            }
        }

        #region IRegistryItemDetailsView Members


        public bool IsDailyLogItemAccessDenied
        {
            set
            {
                contentPanel.Visible = !value;
                lblDailyLogItemAccessDeniedInfo.Visible = value;
            }
        }

        #endregion
    }
}
