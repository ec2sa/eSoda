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
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class DekretacjaDokumentuWielokrotna : System.Web.UI.Page, IRedirectDocumentMultipleView
    {
        public event EventHandler AddToRedirectList;
        private RedirectDocumentMultiplePresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new RedirectDocumentMultiplePresenter(this, new WebSessionProvider());

            lblMessage.Text = string.Empty;

            if (!IsPostBack)
                presenter.Initialize();

            presenter.OnViewLoaded();
            #region todel
            //            string script = @"
//            var h = document.getElementById('" + cbAllHistory.ClientID + @"');
//            var s = document.getElementById('" + cbAllScan.ClientID + @"');
//            if (h.disabled == false) h.disabled = true; else h.disabled = false;            
//            if (s.disabled == false) s.disabled = true; else s.disabled = false;
//            ";

            //            cbPaper.Attributes.Add("onClick", script);
            #endregion
        }
      
        #region IRedirectDocumentView Members

        int IRedirectDocumentMultipleView.DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        protected void OnAddToRedirectList(EventArgs e)
        {
            if (AddToRedirectList != null)
                AddToRedirectList(this, e);
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IRedirectDocumentMultipleView.OrganizationalUnits
        {
            set
            {
                DropDownList lista = wydzial;
                lista.DataSource = value;
                lista.DataTextField = "Description";
                lista.DataValueField = "ID";
                lista.DataBind();
            }
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IRedirectDocumentMultipleView.Employees
        {
            set
            {
                DropDownList lista = pracownik;
                lista.DataSource = value;
                lista.DataTextField = "Description";
                lista.DataValueField = "ID";
                lista.DataBind();
            }
        }

        int IRedirectDocumentMultipleView.OrganizationalUnitId
        {
            get
            {
                return int.Parse(wydzial.SelectedValue);
            }
            set
            {
                wydzial.SelectedValue = value.ToString();
                obslugaZmianyWydzialu(null, null);
            }
        }

        event EventHandler IRedirectDocumentMultipleView.ActionExecuted
        {
            add { wykonaj.Click += value; }
            remove { wykonaj.Click -= value; }
        }

        event EventHandler IRedirectDocumentMultipleView.OrganizationalUnitChanged
        {
            add { wydzial.SelectedIndexChanged += value; }
            remove { wydzial.SelectedIndexChanged -= value; }
        }

        Guid IRedirectDocumentMultipleView.UserId
        {
            get { return (Guid)Membership.GetUser().ProviderUserKey; }
        }

        int IRedirectDocumentMultipleView.EmployeeId
        {
            get { return int.Parse(pracownik.SelectedValue); }
            set { pracownik.SelectedValue = value.ToString(); }
        }

        string IRedirectDocumentMultipleView.ReturnTo
        {
            set { Response.Redirect(string.Format("{0}", value), false); }
        }      

        string IRedirectDocumentMultipleView.UserName
        {
            get { return Membership.GetUser().UserName; }
        }

        string IRedirectDocumentMultipleView.UserFullName
        {
            get { return Membership.GetUser().Comment; }
        }

        bool IRedirectDocumentMultipleView.WorkOnPaper
        {
            get { return cbPaper.Checked; }
            set { cbPaper.Checked = value; }
        }

        string IRedirectDocumentMultipleView.Note
        {
            get { return txtNote.Text; }
            set { txtNote.Text = value; }
        }

        string IRedirectDocumentMultipleView.OUName
        {
            get { return wydzial.SelectedItem.Text; }
        }

        string IRedirectDocumentMultipleView.EmpName
        {
            get { return pracownik.SelectedItem.Text; }
        }

        public System.Collections.Generic.IList<RedirectItem> RedirectList
        {
            set 
            {
                RedirectListGridView.DataSource = value;
                RedirectListGridView.DataBind();
            }
        }
                     
        public bool CommandID
        {
            get
            {
                return cbCommand.Checked;
            }
            set
            {
                cbCommand.Checked = value;
            }
        }

        public bool AllHistory
        {
            get
            {
                return cbAllHistory.Checked;
            }
            set
            {
                cbAllHistory.Checked = value;
            }
        }

        public bool AllScans
        {
            get
            {
                return cbAllScan.Checked;
            }
            set
            {
                cbAllScan.Checked = value;
            }
        }
       
        public int SelectedItemID
        {
            get { if (ViewState["SelectedItemID"] != null) return (int)ViewState["SelectedItemID"]; else return -1; }
            set { ViewState["SelectedItemID"] = value; }
        }
        
        public string AddToRedirectButtonName
        {
            set { lnkAddToRedirect.Text = value; }
        }       

        public bool ShowCancelChangesButton
        {
            set { lnkCancel.Visible = value; }
        }
       
        public bool WorkOnPaperEnable
        {
            set { cbPaper.Enabled = value; }
        }
       
        public bool AllHistoryEnable
        {
            set { cbAllHistory.Enabled = value; }
        }

        public bool AllScanEnable
        {
            set { cbAllScan.Enabled = value; }
        }
      
        public string Message
        {
            set { lblMessage.Text = value; }
        }
       
        string IRedirectDocumentMultipleView.Description
        {
            set { lblDescription.Text = value; }
        }

        string IRedirectDocumentMultipleView.Notice
        {
            set { lblNotice.Text = value; }
        }

        #endregion

        protected void obslugaZmianyWydzialu(object sender, EventArgs e)
        {
        }

        protected void lnkAdvancedOptions_Click(object sender, EventArgs e)
        {
            if (AdvancedOptionsPanel.Visible)
            {
                AdvancedOptionsPanel.Visible = false;
                lnkAdvancedOptions.Text = "Poka¿ opcje zaawansowane";

            }
            else
            {
                AdvancedOptionsPanel.Visible = true;
                lnkAdvancedOptions.Text = "Ukryj opcje zaawansowane";
            }
        }

        protected void lnkAddToRedirect_Click(object sender, EventArgs e)
        {
            OnAddToRedirectList(e);
            presenter.GetRedirectList();
        }

        protected void cbPaper_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                cbAllHistory.Checked = true;
                cbAllHistory.Enabled = false;
                cbAllScan.Checked = true;
                cbAllScan.Enabled = false;
            }
            else
            {
                cbAllHistory.Checked = false;
                cbAllHistory.Enabled = true;
                cbAllScan.Checked = false;
                cbAllScan.Enabled = true;
            }
        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            SelectedItemID = -1;
            presenter.OnCancelChangeSelectedItem();
            presenter.GetRedirectList();
        }

        protected void RedirectListGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int itemID;
            if (e.CommandName == "selectEditItem")
            {
                itemID = int.Parse(e.CommandArgument.ToString());

                SelectedItemID = itemID - 1;//lista numerowana od zera a grid od 1
                presenter.OnSelectedItemIDChanged();
                presenter.GetRedirectList();
            }
            else if (e.CommandName == "selectDeleteItem")
            {
                itemID = int.Parse(e.CommandArgument.ToString());

                SelectedItemID = itemID - 1;
                presenter.OnDeleteSelectedItem();
                presenter.GetRedirectList();
            }
        }

        protected void anuluj_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dokumenty/AkcjeDokumentu.aspx?id="+CoreObject.GetId(Request).ToString(), false);
        }
    }
}
