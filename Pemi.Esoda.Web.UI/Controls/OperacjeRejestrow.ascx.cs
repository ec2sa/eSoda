using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class OperacjeRejestrow : System.Web.UI.UserControl
    {      
        private string currentItem
        {
            get
            {
                return (Session["{85680734-789F-4e0d-A4DA-4182E73C3B0E}"] == null) ? string.Empty : Session["{85680734-789F-4e0d-A4DA-4182E73C3B0E}"].ToString();
            }
            set
            {
                Session["{85680734-789F-4e0d-A4DA-4182E73C3B0E}"] = value;
                LinkButton lb = this.FindControl(value) as LinkButton;
                if (lb == null) return;

                foreach (Control c in this.Controls)
                {
                    if (c is LinkButton)
                        (c as LinkButton).CssClass = string.Empty;
                }
                lb.CssClass = "currentOption";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegistryDAO rd = new RegistryDAO();
            if (Session["idRejestru"] != null)
            {
                int regId = int.Parse(Session["idRejestru"].ToString());
                lnkRegistryDefinition.Visible = !rd.RegHasData(regId);
            }
           
            string currentOption = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf('/') + 1);

            switch (currentOption.ToLower())
            {
                case "edycjarejestru.aspx": currentItem = lnkRegistryDetails.ID; break;
                case "budowadefinicjirejestru.aspx": currentItem = lnkRegistryDefinition.ID; break;               
                default:
                   break;
            }            
        }

        protected void wykonaj(object sender, CommandEventArgs e)
        {
            currentItem = (sender as Control).ID;
            if (Session["idDefinicji"] != null)
                Response.Redirect(e.CommandArgument.ToString());
            else
                WebMsgBox.Show(this.Page, "Najpierw stwórz definicję rejestru");
        }
    }
}