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
	public partial class DokumentySprawy : BaseContentPage,IViewCaseDocumentsView
	{
        protected event EventHandler<ExecutingCommandEventArgs> executeCommand;

        protected void OnEcexuteCommand(ExecutingCommandEventArgs e)
        {
            if (executeCommand != null)
                executeCommand(this,e);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            ViewCaseDocumentsPresenter presenter = new ViewCaseDocumentsPresenter(this,new WebSessionProvider());
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

        protected void wykonaniePolecenia(object sender, GridViewCommandEventArgs e)
        {
            OnEcexuteCommand(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        #region IViewCaseDocumentsView Members

        string IViewCaseDocumentsView.Items
        {
            set {
                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                listaDokumentow.DataSource = null;
                if (xds.Data != "")
                {
                    xds.XPath = "/dokumenty/dokument";
                    listaDokumentow.DataSource = xds;
                }
                listaDokumentow.DataBind();
            }
        }

        event EventHandler<Pemi.Esoda.Presenters.ExecutingCommandEventArgs> IViewCaseDocumentsView.ViewDetails
        {
            add { executeCommand += value; }
            remove { executeCommand -= value; }
        }

        string IViewCaseDocumentsView.NextView
        {
            set 
            {
                Response.Redirect(value); // CHECK
            }
        }

        int IViewCaseDocumentsView.CaseId
        {
            get { return CoreObject.GetId(Request); }
        }

        #endregion
    }
}
