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
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Collections.Generic;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class OperacjeDokumentu : System.Web.UI.UserControl
    {
        public int IdSprawy
        {
            set
            {
                Session["idSkojarzonejSprawy"] = value;
                lnkDocCase.ToolTip = "Przejœcie do sprawy, z któr¹ jest skojarzony dokument";
                lnkDocCase.Enabled = true;
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
            
                if (Session["idSkojarzonejSprawy"] != null)
                    lnkDocCase.Enabled = true;

                // przypisanie ID w nawigacji
                int docId = CoreObject.GetId(Request);

                // ukrycie/pokazanie linka formularz
                  DocumentDAO dao=new DocumentDAO();
                lnkCustomForm.Visible = dao.IsCustomFormVisible(docId);
                lnkLegalAct.Visible = dao.CanCreateLegalAct(docId);
                lnkDocConfirmRead.Visible = dao.IsConfirmationNeeded(docId);
               
                    lnkCustomForm.CommandArgument = "~/Dokumenty/FormularzWidokGlowny.aspx?id=" + docId.ToString();
                    lnkDocDetails.CommandArgument = "~/Dokumenty/Dokument.aspx?id=" + docId.ToString();
                    lnkDocInnerItems.CommandArgument = "~/Dokumenty/SkladnikiDokumentu.aspx?id=" + docId.ToString();
                    lnkDocHistory.CommandArgument = "~/Dokumenty/HistoriaDokumentu.aspx?id=" + docId.ToString();
                    lnkDocActions.CommandArgument = "~/Dokumenty/AkcjeDokumentu.aspx?id=" + docId.ToString();
                    lnkDocRegisters.CommandArgument = "~/Dokumenty/RejestryDokumentu.aspx?id=" + docId.ToString();
                    lnkDocCodes.CommandArgument = "~/Dokumenty/KodyDokumentu.aspx?id=" + docId.ToString();
                    lnkLegalAct.CommandArgument = "~/Dokumenty/AktyPrawne.aspx?id=" + docId.ToString();
                    //lnkDocCase" Text="sprawa" onCommand="przejdzDoSprawy" Enabled="false"  /></li>

                    string currentOption = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf('/') + 1);
                    currentOption = currentOption.Substring(0, currentOption.LastIndexOf('.'));

                    switch (currentOption.ToLower())
                    {
                        case "formularzwidokglowny": currentItem = lnkCustomForm.ID; break;
                        case "formularzxml": currentItem = lnkCustomForm.ID; break;
                        case "formularzhistoria": currentItem = lnkCustomForm.ID; break;
                        case "formularz": currentItem = lnkCustomForm.ID; break;
                        case "formularzdoc": currentItem = lnkCustomForm.ID; break;
                        case "skladnikdoc": currentItem = lnkDocInnerItems.ID; break;
                        case "dokument": currentItem = lnkDocDetails.ID; break;
                        case "skladnikidokumentu": currentItem = lnkDocInnerItems.ID; break;
                        case "historiadokumentu": currentItem = lnkDocHistory.ID; break;
                        case "akcjedokumentu": currentItem = lnkDocActions.ID; break;
                        case "kodydokumentu":
                            currentItem = lnkDocCodes.ID;
                            break;
                        case "rejestrydokumentu":
                            currentItem = lnkDocRegisters.ID;
                            break;
                        case "aktyprawne": currentItem = lnkLegalAct.ID;break;
                        default: currentItem = lnkDocDetails.ID; break;
                    }

                    if (Request.Url.AbsoluteUri.Contains("/Akcje/"))
                        currentItem = lnkDocActions.ID;
                
        }

        protected void przejdzDoSprawy(object sender, EventArgs e)
        {
            if (Session["idSkojarzonejSprawy"] != null) // CHECK
                Response.Redirect("~/Sprawy/Sprawa.aspx?id=" + Session["idSkojarzonejSprawy"].ToString());
        }

        protected void wykonaj(object sender, CommandEventArgs e)
        {
            currentItem = (sender as LinkButton).ID;
            if (currentItem.Contains("DocBack") && Session["PreviousPageUrl"] != null)
                Response.Redirect(Session["PreviousPageUrl"].ToString());
            else
            {
                Session.Remove("PreviousPageUrl");
                Response.Redirect(e.CommandArgument.ToString());
            }
        }

        protected void potwierdzZapoznanie(object sender, CommandEventArgs e)
        {
            int docId = CoreObject.GetId(Request);

            if (new DocumentDAO().ConfirmReading(docId))
            {
                List<string> parameters = new List<string>();
                parameters.Add(docId.ToString());
                ActionLogger al = new ActionLogger(new ActionContext(new Guid("4453A151-F6EF-40C7-9056-F8D62F2E53D8"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, parameters));
                al.AppliesToDocuments.Add(docId);
                al.Execute();
                Response.Redirect(e.CommandArgument.ToString());
            }
            else
            {
                Session["errorMessage"] = "Nie uda³o siê potwierdziæ zapoznania siê z dokumentem.";
                Session["returnUrl"] = Request.Url.ToString();
                Response.Redirect("~/shared/error2.aspx");
            }

            
        }
    }
}