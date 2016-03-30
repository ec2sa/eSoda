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
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class PrzekazanieSprawy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void podpieteDane(object sender, EventArgs e)
        {
            pracownik.Items.Insert(0, new ListItem("- wybierz -", "0"));
        }

        protected void executeCommand(object sender, EventArgs e)
        {
            int caseId = CoreObject.GetId(Request);
            if (caseId <= 0)
            {
                BaseContentPage.SetError("Nie wybrano sprawy", "~/Oczekuj¹ceZadania.aspx");
                return;
            }

            //if(Session["idSprawy"]==null || !int.TryParse(Session["idSprawy"].ToString(),out caseId)){
            //  BaseContentPage.SetError("Nie wybrano sprawy","~/Oczekuj¹ceZadania.aspx");
            //  return;
            //}

            CaseDAO dao = new CaseDAO();
            dao.RedirectCase(caseId, new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment,
              txtNote.Text, int.Parse(wydzial.SelectedItem.Value), int.Parse(pracownik.SelectedItem.Value), wydzial.SelectedItem.Text, pracownik.SelectedItem.Text);
            if (IsCaseVisibleToUser(new Guid(Membership.GetUser().ProviderUserKey.ToString()), caseId))
                Response.Redirect("~/Sprawy/HistoriaSprawy.aspx?id="+caseId.ToString()); 
            else
                Response.Redirect("~/OczekujaceZadania.aspx");
        }

        private bool IsCaseVisibleToUser(Guid userId, int caseId)
        {
            return (new CaseDAO()).IsCaseVisibleForUser(caseId, userId);
        }
    }
}