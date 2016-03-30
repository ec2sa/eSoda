using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class listaDefinicjiRejestrow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void gvListaDefinicji_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DefEdit":
                    Session["idDefinicjiRejestru"] = e.CommandArgument;
                    Response.Redirect("~/Aplikacje/PanelAdministracyjny/BudowaDefinicjiRejestru.aspx");
                    break;

                default:
                    Session["idDefinicjiRejestru"] = null;
                    Session["RegDefinition"] = null;
                    break;
            }
        }
    }
}
