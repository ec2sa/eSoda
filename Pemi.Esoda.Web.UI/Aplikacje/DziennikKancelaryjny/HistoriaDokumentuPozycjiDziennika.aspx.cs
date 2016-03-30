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
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Web.UI
{
    public partial class HistoriaDokumentuPozycjiDziennika : Page
	{
        private int itemID;
        private int registryID;
        private RegistryDAO dao;

        public bool IsInvoice
        {
            get
            {
                return Request.QueryString["rf"] != null;
            }
        }

        public bool IsDailyLogItemAccessDenied
        {
            set
            {
                contentPanel.Visible = !value;
                lblDailyLogItemAccessDeniedInfo.Visible = value;
            }
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            lblError.Text = string.Empty;
            dao = new RegistryDAO();

            Session["parentPage"] = Request.QueryString["pp"];

            if (Session["registryID"] != null)
            {
                int.TryParse(Session["registryID"].ToString(), out registryID);
            }

            if (Session["itemID"] != null)
            {
                if (int.TryParse(Session["itemID"].ToString(), out itemID))
                {
                    HistoryItems = GetDocumentHistory(GetDocumentIDByDailyLogItem(registryID, itemID));
                    numerPozycji.Text =  String.Format("[numer pozycji w rejestrze {1}: {0}]", itemID.ToString(),IsInvoice?"faktur":"");
                }
            }

           

            if (!Page.IsPostBack)
                IsDailyLogItemAccessDenied = dao.IsDailyLogItemAccessDenied(registryID, itemID, new Guid(Membership.GetUser().ProviderUserKey.ToString()));
		}

        private int GetDocumentIDByDailyLogItem(int registryID, int itemID)
        {
            int returnValue = -1;
            try
            {               
                returnValue = dao.GetDocumentIDByDailyLogItem(registryID, itemID,IsInvoice);
            }
            catch (Exception e)
            {
                lblError.Text = e.Message;
            }

            return returnValue;


        }

        private string GetDocumentHistory(int documentId)
        {
            string returnValue = string.Empty;

            try
            {
                DocumentDAO dao = new DocumentDAO();
                using (XmlReader xr = dao.GetDocumentHistory(documentId))
                {
                    if (xr.Read())
                        returnValue = xr.ReadOuterXml();
                }
            }
            catch (Exception e)
            {
                lblError.Text = e.Message;
            }

            return returnValue;
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
                Response.Redirect("EdycjaPozycjiDziennika.aspx?pp=a"+rf);
            }
            else //if (parentPage == "s")
            {
                Response.Redirect("EdycjaPozycjiDziennika.aspx?pp=s"+rf);
            }
        }
    }
}
