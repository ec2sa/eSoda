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

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class OperacjeInteresantow : System.Web.UI.UserControl
    {
        private string currentItem
        {
            get
            {
                return (Session["{AB6EB5B1-450B-481a-A2DC-C691788D6665}"] == null) ? string.Empty : Session["{AB6EB5B1-450B-481a-A2DC-C691788D6665}"].ToString();
            }
            set
            {
                Session["{AB6EB5B1-450B-481a-A2DC-C691788D6665}"] = value;
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
            if (!IsPostBack)
                currentItem = currentItem;
            else
                currentItem = string.Empty;
        }
        protected void wykonaj(object sender, CommandEventArgs e)
        {
            currentItem = (sender as Control).ID;
            Response.Redirect(e.CommandArgument.ToString());
        }
    }
}