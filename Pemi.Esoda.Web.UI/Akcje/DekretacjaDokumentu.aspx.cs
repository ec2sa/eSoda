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
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Akcje
{
  public partial class DekretacjaDokumentu : System.Web.UI.Page, IRedirectDocumentView
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      RedirectDocumentPresenter presenter = new RedirectDocumentPresenter(this, new WebSessionProvider());
      if (!IsPostBack)
        presenter.Initialize();
    }

    protected void obslugaZmianyWydzialu(object sender, EventArgs e)
    {
    }

    #region IRedirectDocumentView Members

    int IRedirectDocumentView.DocumentId
    {
        get { return CoreObject.GetId(Request); }
    }

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



    event EventHandler IRedirectDocumentView.ActionExecuted
    {
      add { wykonaj.Click += value; }
      remove { wykonaj.Click -= value; }
    }



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


    string IRedirectDocumentView.ReturnTo
    {
        set { Response.Redirect(string.Format("{0}", value), false); } // CHECK
    }

    #endregion

    protected void anuluj_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Dokumenty/AkcjeDokumentu.aspx?id="+CoreObject.GetId(Request).ToString(), false);
    }

    #region IRedirectDocumentView Members


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
      get { return cbPaper.Checked; }
    }

    string IRedirectDocumentView.Note
    {
      get { return txtNote.Text; }
    }

    #endregion

    #region IRedirectDocumentView Members


    string IRedirectDocumentView.OUName
    {
      get { return wydzial.SelectedItem.Text; }
    }

    string IRedirectDocumentView.EmpName
    {
      get { return pracownik.SelectedItem.Text; }
    }

    #endregion
  }
}
