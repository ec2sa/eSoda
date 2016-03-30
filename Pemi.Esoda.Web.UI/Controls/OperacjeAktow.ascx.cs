using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class OperacjeAktow : System.Web.UI.UserControl
    {
        public int DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
                DocumentDAO dao = new DocumentDAO();

                CustomFormVisibilityDTO formVisibility = dao.GetCustomFormVisibility(DocumentId);

                if (formVisibility != null)
                {
                    lblXml.Visible = formVisibility.LegalActXmlVisible;
                    lblHistory.Visible = formVisibility.LegalActHistoryVisible;
                }

                lblHistory.CommandArgument = "~/Dokumenty/AktPrawnyHistoria.aspx?id=" + DocumentId;

                string currentOption = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf('/') + 1);
                currentOption = currentOption.Substring(0, currentOption.LastIndexOf('.'));

                switch (currentOption.ToLower())
                {
                    case "aktprawnyhistoria":
                        currentItem = lblHistory.ID;
                        break;
                    case "aktprawnyxml":
                        currentItem = lblXml.ID;
                        break;
                }
        }

        private string currentItem
        {
            get
            {
                return (Session["{A9369F29-4E10-48ab-9E52-C4D415CF402A}"] == null) ? string.Empty : Session["{A9369F29-4E10-48ab-9E52-C4D415CF402A}"].ToString();
            }
            set
            {
                Session["{A9369F29-4E10-48ab-9E52-C4D415CF402A}"] = value;
                LinkButton lb = this.FindControl(value) as LinkButton;
                if (lb == null)
                    return;

                foreach (Control c in this.Controls)
                {
                    if (c is LinkButton)
                        (c as LinkButton).CssClass = string.Empty;
                }
                lb.CssClass = "currentOption";
            }
        }

        protected void wykonaj(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "EditLegalAct")
            {

                Response.Redirect("~/Dokumenty/AktPrawny.aspx?id=" + DocumentId);
            }
            if (e.CommandName == "GetHistory")
            {
                Response.Redirect("~/Dokumenty/AktPrawnyHistoria.aspx?id=" + DocumentId);
            }
            if (e.CommandName == "GetXml")
            {
                Response.Redirect("~/Dokumenty/AktPrawnyXml.aspx?id=" + DocumentId);
            }
        }
    }
}