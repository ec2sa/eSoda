using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DataAccess;
using System.Web.Security;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class WykonanieCzynnosci : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int docId = CoreObject.GetId(Request);
                ViewState["docID"] = docId;
            }
        }

        protected void saveClick(object sender, EventArgs e)
        {
            string[] parameters = { description.Text };

            ActionLogger al = new ActionLogger(new ActionContext(new Guid("ADBAD8B1-ABE0-41DA-8E17-32817C4FAE74"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, parameters));
            al.AppliesToDocuments.Add((int)ViewState["docID"]);
            al.ActionData.Add("Opis", description.Text);
            
            al.Execute();

            Response.Redirect("~/Dokumenty/akcjeDokumentu.aspx?id=" + CoreObject.GetId(Request).ToString(), false);
        }

        protected void cancelClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Dokumenty/akcjeDokumentu.aspx?id=" + CoreObject.GetId(Request).ToString(), false);
        }
    }
}