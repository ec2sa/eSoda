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
using System.Collections.ObjectModel;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI
{
    public partial class PrzekierowanieDokumentu : BaseContentPage, IRedirectDocumentView
    {
        public event EventHandler ActionExecuted;

        protected void Page_Load(object sender, EventArgs e)
        {
            RedirectDocumentPresenter presenter = new RedirectDocumentPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
                presenter.Initialize();
        }

        protected void obslugaZmianyWydzialu(object sender, EventArgs e)
        {
        }

        protected void OnActionExecuted(EventArgs e)
        {
            if (ActionExecuted != null)
                ActionExecuted(this, e);
        }

        #region IRedirectDocumentView Members

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IRedirectDocumentView.OrganizationalUnits
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

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IRedirectDocumentView.Employees
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

        int IRedirectDocumentView.OrganizationalUnitId
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



        //event EventHandler IRedirectDocumentView.ActionExecuted
        //{            
        //    add { wykonaj.Click += value; }
        //    remove { wykonaj.Click -= value; }
        //}


        event EventHandler IRedirectDocumentView.OrganizationalUnitChanged
        {
            add { wydzial.SelectedIndexChanged += value; }
            remove { wydzial.SelectedIndexChanged -= value; }
        }


        Guid IRedirectDocumentView.UserId
        {
            get { return (Guid)Membership.GetUser().ProviderUserKey; }
        }

      


        int IRedirectDocumentView.EmployeeId
        {
            get { return int.Parse(pracownik.SelectedValue); }
        }


        int IRedirectDocumentView.DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        string IRedirectDocumentView.ReturnTo
        {
            set { Response.Redirect(string.Format("{0}", value)); }
        }
        string IRedirectDocumentView.UserName
        {
            get { return Membership.GetUser().UserName; }
        }

        string IRedirectDocumentView.UserFullName
        {
            get { return Membership.GetUser().Comment; }
        }

        bool IRedirectDocumentView.WorkOnPaper
        {
            get { return false; }
        }

        string IRedirectDocumentView.Note
        {
            get { return txtNote.Text; }
        }
  

        string IRedirectDocumentView.OUName
        {
            get { return wydzial.SelectedItem.Text; }
        }

        string IRedirectDocumentView.EmpName
        {
            get { return pracownik.SelectedItem.Text; }
        }


        #endregion

        protected void wykonaj_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                OnActionExecuted(e);
            }
        }
    }
}