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

namespace Pemi.Esoda.Web.UI.Sprawy
{
	public partial class HistoriaSprawy : BaseContentPage,IViewCaseHistoryView	{

		protected void Page_Load(object sender, EventArgs e)
		{
			CaseHistoryPresenter presenter = new CaseHistoryPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {
                int caseId = CoreObject.GetId(Request);
                if (caseId > 0)
                {
                    if (!Page.User.IsInRole("Administratorzy") && !(new CaseDAO()).IsCaseVisibleForUser(caseId, new Guid(Membership.GetUser().ProviderUserKey.ToString())))
                    {
                        BaseContentPage.SetError("Nie masz uprawnieñ do tej sprawy", "~/OczekujaceZadania.aspx");
                    }
                }
                presenter.Initialize();
            }
		}

        protected void execCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "details")
            {
                Session["returnFromActionDetails"] = "~/Sprawy/HistoriaSprawy.aspx?id=" + CoreObject.GetId(Request);
                Response.Redirect("~/Akcje/SzczegolyAkcji.aspx?id=" + e.CommandArgument.ToString());
            }
        }

		#region IDocumentHistoryView Members

        int IViewCaseHistoryView.CaseId
        {
            get { return CoreObject.GetId(Request); }
        }

		//public string HistoryItems
        string IViewCaseHistoryView.HistoryItems
		{
			set {
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
