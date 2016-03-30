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
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Sprawy
{
	public partial class Sprawa : BaseContentPage, IViewCaseView
	{
        private Collection<CaseDTO> sprawa;

        private ViewCasePresenter presenter;

       protected void Page_Load(object sender, EventArgs e)
		{
            presenter = new ViewCasePresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {
                Session.Remove("DocumentSearchCriteria");
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

        #region IViewCaseView Members

        string IViewCaseView.CaseInfo
        {
            set { ; }
        }

        int IViewCaseView.CaseId
        {
            get
            {
                //int caseId=0;
                //if (Request.QueryString["id"] != null)
                //    int.TryParse(Request.QueryString["id"].ToString(), out caseId);
                              
                //if (Session["idSprawy"] != null)
                //    int.TryParse(Session["idSprawy"].ToString(), out caseId);
                //if (caseId == 0 && Session["idDokumentu"] != null)
                //{
                //    int docId=-1;
                //    if (int.TryParse(Session["idDokumentu"].ToString(), out docId))
                //    {
                //        caseId = (new CaseDAO()).GetCaseNumberFromDocument(docId);
                //        Session["idSprawy"] = caseId;
                //    }
                //}
                //return caseId;

                return CoreObject.GetId(Request);
            }
        }

        Pemi.Esoda.DTO.CaseDTO IViewCaseView.CaseData
        {
            set
            {
                sprawa = new Collection<CaseDTO>();
                sprawa.Add(value);
                daneSprawy.DataSource = sprawa;
                daneSprawy.DataBind();
            }
        }
                     
        public string CaseSignature
        {
            set { ; }
        }

        #endregion
    }
}
