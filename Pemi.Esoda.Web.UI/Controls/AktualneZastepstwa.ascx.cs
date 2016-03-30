using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class AktualneZastepstwa : System.Web.UI.UserControl
    {
        private UserDAO uDAO = null;
        private Guid userGuid;

        private string SelectedCover
        {
            get
            {
                if (ViewState["SelectedCover"] != null)
                    return ViewState["SelectedCover"].ToString();
                else
                    return "-1";
            }
            set
            {
                ViewState["SelectedCover"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            uDAO = new UserDAO();
            
            MembershipUser user = Membership.GetUser();

            if (user != null)
            {
                 userGuid = new Guid(user.ProviderUserKey.ToString());
            }

            if (Page.IsPostBack)
            {
                SelectedCover = ddlAvailableCover.SelectedValue;
            }        
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (uDAO.IsAvailableCover(userGuid))
            {
                LoadAvailableCover();
                availableCover.Visible = true;
            }
            else
            {
                availableCover.Visible = false;
            }
        }

        private void LoadAvailableCover()
        {
            ddlAvailableCover.DataSource = uDAO.GetAvailableCover(userGuid);
            ddlAvailableCover.DataBind();
           
        }

        protected void switch_Click(object sender, EventArgs e)
        {
            if (SelectedCover != "-1")
            {
                FormsAuthentication.SetAuthCookie(SelectedCover, false);

                if (ConfigurationManager.AppSettings["mode"].Equals("dokumenty"))
                {
                    Response.Redirect("~/OdbiorDokumentow.aspx");
                }
                else
                    Response.Redirect("~/OczekujaceZadania.aspx");
            }
        }
    }
}