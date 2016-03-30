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
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using System.Text;

namespace Pemi.Esoda.Web.UI
{
	public partial class Dokument : BaseContentPage,IViewDocumentView
	{
		private Collection<DocumentDTO> dokument;

		private ViewDocumentPresenter presenter;

		private void generujMetadane(object sender, EventArgs e)
		{
			
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new ViewDocumentPresenter(this, new WebSessionProvider());
           
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Page.Request["mode"]) && !String.IsNullOrEmpty(Page.Request["id"]))
                {                    
                    string url = "FormularzDoc.aspx?id=" + Page.Request["id"].ToString() + "&mode=" + Page.Request["mode"].ToString();

                    //if (!ClientScript.IsStartupScriptRegistered(this.GetType(), "scriptWord"))
                    //{
                    //    string script = @"window.location.href='" + url + "';";
                    //    ClientScript.RegisterStartupScript(this.GetType(), "scriptWord", script, true);
                    //}

                    HtmlMeta meta = new HtmlMeta();
                    meta.HttpEquiv = "Refresh";
                    meta.Content = "1;url=" + url;
                    Page.Header.Controls.Add(meta);
                }  

                Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "LinkButton1";
                Session.Remove("idSkojarzonejSprawy");
                //if (Request.QueryString["id"] != null)
                //{
                //    Session["idDokumentu"] = Request.QueryString["id"];
                    
                //    if (!Membership.GetUser().UserName.Equals("admin") && !(new DocumentDAO()).IsDocVisibleForUser(int.Parse(Session["idDokumentu"].ToString()), new Guid(Membership.GetUser().ProviderUserKey.ToString())))
                //    {
                //        BaseContentPage.SetError("Nie masz uprawnieñ do tego dokumentu", "~/OczekujaceZadania.aspx");
                //    }
                //}
                int docId = CoreObject.GetId(Request);
                if (docId >0)
                {
                    if (!Page.User.IsInRole("Administratorzy") && !(new DocumentDAO()).IsDocVisibleForUser(docId, new Guid(Membership.GetUser().ProviderUserKey.ToString())))
                    {
                        BaseContentPage.SetError("Nie masz uprawnieñ do tego dokumentu", "~/OczekujaceZadania.aspx");
                    }
                }
                presenter.Initialize();
            }
        }
        
        protected void wyborOpcji(object sender, CommandEventArgs e)
		{
			string wynik=presenter.ExecuteCommand(e.CommandName, e.CommandArgument.ToString());

			Response.Redirect(string.Format("{0}.aspx", wynik)); // CHECK
		}

        public bool IsDocVisibleForUser(int docId, Guid userID)
        {
            return (new DocumentDAO()).IsDocVisibleForUser(docId, userID);
        }


		#region IViewDocumentView Members

        int IViewDocumentView.DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

		Pemi.Esoda.DTO.DocumentDTO IViewDocumentView.DocumentData
		{
			set {
                //if (value != null)
                //{
                    dokument = new Collection<DocumentDTO>();
                    dokument.Add(value);
                    daneDokumentu.DataSource = dokument;
                    daneDokumentu.DataBind();

                    int ids;
                    if (value.Metadata.ContainsKey("idSprawy"))
                        if (int.TryParse(value.Metadata["idSprawy"], out ids))
                            opcjeDokumentu1.IdSprawy = ids;
                //}
                //else
                //    BaseContentPage.SetError(string.Format("Nie ma takiego dokumentu {0}", Request.QueryString["id"].ToString()), "~/OczekujaceZadania.aspx");
            }
		}

		#endregion
	}
}
