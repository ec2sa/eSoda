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
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class OperacjeSprawy : System.Web.UI.UserControl
    {
        private string currentItem
        {
            get
            {
                return (Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] == null) ? string.Empty : Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"].ToString();
            }
            set
            {
                Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = value;
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
            //if (!IsPostBack)
            //    currentItem = currentItem;
            //else
            //    currentItem = lnkCaseDetails.ID;

            // ustawienie id obiektu dla linków
            int caseId = CoreObject.GetId(Request);

            lnkCaseDetails.CommandArgument = "~/Sprawy/Sprawa.aspx?id="+caseId.ToString();
            lnkCaseDocuments.CommandArgument = "~/Sprawy/DokumentySprawy.aspx?id=" + caseId.ToString();
            lnkCaseHistory.CommandArgument = "~/Sprawy/HistoriaSprawy.aspx?id=" + caseId.ToString();
            lnkCaseActions.CommandArgument = "~/Sprawy/AkcjeSprawy.aspx?id=" + caseId.ToString();
            lnkCaseRegistry.CommandArgument = "~/Sprawy/RejestrySprawy.aspx?id=" + caseId.ToString();
            lnkCaseMetrics.NavigateUrl = "~/Aplikacje/Raporty/MetrykaSprawy.aspx?id=" + caseId.ToString();

            string currentOption = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf('/') + 1);
            currentOption = currentOption.Substring(0, currentOption.LastIndexOf('.'));

            switch (currentOption.ToLower())
            {
                case "sprawa": currentItem = lnkCaseDetails.ID; break;
                case "dokumentysprawy": currentItem = lnkCaseDocuments.ID; break;
                case "historiasprawy": currentItem = lnkCaseHistory.ID; break;
                case "akcjesprawy": currentItem = lnkCaseActions.ID; break;
                case "rejestrysprawy": currentItem = lnkCaseRegistry.ID; break;
                default: currentItem = lnkCaseDetails.ID; break;
            }

            if (Request.Url.AbsoluteUri.Contains("/Akcje/"))
                currentItem = lnkCaseActions.ID;

        }
        protected void wykonaj(object sender, CommandEventArgs e)
        {
            currentItem = (sender as Control).ID;
            if (currentItem.Contains("CaseBack") && Session["PreviousPageUrl"] != null)
                Response.Redirect(Session["PreviousPageUrl"].ToString());
            else
            {
                Session.Remove("PreviousPageUrl");
                Response.Redirect(e.CommandArgument.ToString());
            }
        }
    }
}