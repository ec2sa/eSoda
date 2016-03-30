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
using System.Text;
using Pemi.Esoda.Web.UI.ePUAP.Skrytka.Service;
using System.Drawing;
using Pemi.Esoda.Web.UI.Classes;
using System.Xml;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class OperacjeFormularza : System.Web.UI.UserControl
    {
        public int DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        public string APath
        {
            get
            {
                return Page.Request.ApplicationPath == "/" ? "" : Page.Request.ApplicationPath;
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

            DocumentDAO dao = new DocumentDAO();

            CustomFormVisibilityDTO formVisibility = dao.GetCustomFormVisibility(DocumentId);

            if (formVisibility != null)
            {
                lblESodaForm.Visible = formVisibility.CustomFormVisible;
                lblWordEditForm.Visible = formVisibility.WordFormEditVisible;
                lblWordForm.Visible = formVisibility.WordFormVisible;
                lblXml.Visible = formVisibility.XmlVisible;
                lblHistory.Visible = formVisibility.HistoryVisible;
                lblepuap.Visible = formVisibility.SendToEPUAPVisible;
            }

            lblESodaForm.CommandArgument = "~/Dokumenty/Formularz.aspx?id=" + DocumentId;
            //lblXml.CommandArgument = "~/Dokumenty/FormularzXml.aspx?id=" + DocumentId;
            lblXml.CommandArgument = "~/Dokumenty/FormularzWidokGlowny.aspx?id=" + DocumentId + "&mode=xml";
            lblWordForm.CommandArgument = "~/Dokumenty/FormularzDoc.aspx?id=" + DocumentId + "&mode=c";
            lblWordForm.CommandName = "NewWordForm";
            lblWordEditForm.CommandArgument = "~/Dokumenty/FormularzWidokGlowny.aspx?id=" + DocumentId + "&mode=e";
            //lblWordEditForm.CommandArgument = "~/Dokumenty/FormularzDoc.aspx?id=" + DocumentId + "&mode=e";            
            lblHistory.CommandArgument = "~/Dokumenty/FormularzHistoria.aspx?id=" + DocumentId;

            string currentOption = Request.Url.AbsoluteUri.Substring(Request.Url.AbsoluteUri.LastIndexOf('/') + 1);
            currentOption = currentOption.Substring(0, currentOption.LastIndexOf('.'));

            switch (currentOption.ToLower())
            {
                case "formularz": currentItem = lblESodaForm.ID; break;
                case "formularzxml": currentItem = lblXml.ID; break;
                case "formularzhistoria": currentItem = lblHistory.ID; break;
            }
            lblMessage.Text = string.Empty;
        }

        protected void wykonaj(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "NewWordForm")
            {
                CustomFormDTO customForm = new CustomFormDAO().GetCustomFormData(DocumentId, false);
                if (customForm != null && !String.IsNullOrEmpty(customForm.XmlData))
                    Response.Redirect("~/Dokumenty/FormularzWidokGlowny.aspx?id=" + DocumentId);
                else
                    Response.Redirect(e.CommandArgument.ToString());
            }
            else
                if (e.CommandName == "nadajepuap")
                {
                    CustomFormDTO customForm = new CustomFormDAO().GetCustomFormData(DocumentId, false);
                    string responseAddress = new DocumentDAO().GetDocumentEPUAPResponseAddress(DocumentId);
                    ePUAPHelper helper = new ePUAPHelper();

                    try
                    {
                        ePUAPUPP upp = helper.SendResponse(responseAddress, Encoding.UTF8.GetBytes(customForm.XmlData));

                        if (upp.ResponseStatus.Code == StatusCode.Success)
                        {
                            lblMessage.ForeColor = Color.Green;
                            lblMessage.Text = "Dokument zosta³ nadany na skrytkê ePUAP.";
                        }
                        else
                        {
                            lblMessage.ForeColor = Color.Red;
                            lblMessage.Text = upp.ResponseStatus.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        lblMessage.ForeColor = Color.Red;
                        lblMessage.Text = "Próba nadania dokumentu na skrytkê ePUAP nie powiod³a siê" + ex.Message;
                    }
                }
                else
                {
                    Response.Redirect(e.CommandArgument.ToString());
                }
        }
    }
}