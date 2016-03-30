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
using System.Data.SqlClient;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;


namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class NowyRok : System.Web.UI.Page
    {               
        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        protected void execAction_Click(object sender, EventArgs e)
        {
            Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "lblNowyRok";

            int actionID = int.Parse(newYearAvailableAction.SelectedValue);

            switch (actionID)
            {
                case 1:
                    Page.Response.Redirect("NowyRokTworzTeczki.aspx");
                    break;
                case 2:
                    Page.Response.Redirect("NowyRokTworzRejestry.aspx");
                    break;
            }            
        }       
    }
}

