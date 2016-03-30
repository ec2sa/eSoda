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
using System.Xml;
using Pemi.Esoda.Presenters;
using Pemi.Esoda.Web.UI.Controls;
using System.Collections.Generic;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class edycjaSprawy : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                int caseId = CoreObject.GetId(Request);
                if(caseId > 0)
                {
                    CaseDAO cd = new CaseDAO();
                    frmSprawa.ChangeMode(FormViewMode.Edit);
                    XmlDataSource xds = new XmlDataSource();
                    using (XmlReader xr = cd.GetCase(caseId))
                    {
                        if (!xr.Read())
                            xds.Data = string.Empty;
                        else
                            xds.Data = xr.ReadOuterXml();
                    }
                    xds.EnableCaching = false;
                    if (xds.Data != "")
                    {
                        xds.XPath = "/sprawa";
                        frmSprawa.DataSource = xds;
                    }
                    else
                        frmSprawa.DataSource = null;

                    frmSprawa.DataBind();
                }
            }

            if (customer != null)
            {
                customer.SearchCustomers += new EventHandler<SearchCustomersEventArgs>(customersList.FindCustomers);
                customer.SearchCustomers += new EventHandler<SearchCustomersEventArgs>(customer_SearchCustomers);
                customer.SearchListVisible += new EventHandler<SearchCustomersEventArgs>(customersList.SearchListVisible);
                customer.OnCustomerAdded += new EventHandler(customer_OnCustomerAdded);
                customersList.AddNewCustomer += new EventHandler(customer.AddNewCustomer);
                customersList.EditCustomer += new EventHandler(customer.EditCustomer);
                customersList.SelectCustomer += new EventHandler(customer.SelectCustomer);
                customersList.SelectCustomer += new EventHandler(onSelectCustomer);
            }
            customersList.AdminMode = false;
            customersList.PageSize = 5;            
        }

        void customer_OnCustomerAdded(object sender, EventArgs e)
        {
            lnkSelectCustomer.Visible = (customer.IdInteresanta > 0);
        }

        void customer_SearchCustomers(object sender, SearchCustomersEventArgs e)
        {
            customer.Visible = false;
            lnkSearchAgain.Visible = true;
        }

        private void onSelectCustomer(object sender, EventArgs e)
        {
            if (customer.IdInteresanta > 0)
            {
                (frmSprawa.FindControl("hfCustomerId") as HiddenField).Value = customer.IdInteresanta.ToString();
                (frmSprawa.FindControl("lblOdKogo") as Label).Text = (customer.Nazwa.Length>0) ? customer.Nazwa : customer.Imie + " " + customer.Nazwisko ;
                customersList.Visible = false;
                customer.Visible = false;
                lnkSearchAgain.Visible = false;
                lnkSelectCustomer.Visible = false;
            }
        }

        protected void ustawDate(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "wybierz":
                    frmSprawa.FindControl(string.Format("cal{0}Wrapper", e.CommandArgument.ToString())).Visible = true;
                    DateTime data;
                    if (DateTime.TryParse((frmSprawa.FindControl(e.CommandArgument.ToString()) as TextBox).Text, out data))
                    {
                        (frmSprawa.FindControl(string.Format("cal{0}", e.CommandArgument.ToString())) as Calendar).SelectedDate = data;
                        (frmSprawa.FindControl(string.Format("cal{0}", e.CommandArgument.ToString())) as Calendar).VisibleDate = data;
                    }
                    else
                    {
                        (frmSprawa.FindControl(string.Format("cal{0}", e.CommandArgument.ToString())) as Calendar).SelectedDate = DateTime.Today;
                        (frmSprawa.FindControl(string.Format("cal{0}", e.CommandArgument.ToString())) as Calendar).VisibleDate = DateTime.Today;
                        (frmSprawa.FindControl(e.CommandArgument.ToString()) as TextBox).Text = "";
                    }

                    break;
                case "wstaw":
                    (frmSprawa.FindControl(e.CommandArgument.ToString()) as TextBox).Text = (frmSprawa.FindControl(string.Format("cal{0}", e.CommandArgument.ToString())) as Calendar).SelectedDate.ToString("yyyy-MM-dd");
                    frmSprawa.FindControl(string.Format("cal{0}Wrapper", e.CommandArgument.ToString())).Visible = false;
                    break;
                case "anuluj":
                    frmSprawa.FindControl(string.Format("cal{0}Wrapper", e.CommandArgument.ToString())).Visible = false;
                    break;
            }
        }

        protected void zakonczAkcje(object sender, CommandEventArgs e)
        {            
            if (e.CommandName == "Update")
            {
                if (Page.IsValid)
                {
                    int id = -1;
                    CaseDAO dao = new CaseDAO();
                    id = int.Parse((frmSprawa.FindControl("hfCaseId") as HiddenField).Value);
                    string opis = (frmSprawa.FindControl("txtOpis") as TextBox).Text;
                    string znakPisma = (frmSprawa.FindControl("txtZnakPisma") as TextBox).Text;

                    DateTime dataRozpoczecia;
                    if (!DateTime.TryParse((frmSprawa.FindControl("dataRozpoczecia") as TextBox).Text, out dataRozpoczecia))
                        dataRozpoczecia = DateTime.MinValue;

                    DateTime dataPisma;
                    if (!DateTime.TryParse((frmSprawa.FindControl("txtDataPisma") as TextBox).Text, out dataPisma))
                        dataPisma = DateTime.MinValue;

                    DateTime dataZakonczenia;
                    if (!DateTime.TryParse((frmSprawa.FindControl("dataZakonczenia") as TextBox).Text, out dataZakonczenia))
                        dataZakonczenia = DateTime.MinValue;

                    string uwagi = (frmSprawa.FindControl("txtUwagi") as TextBox).Text;

                    int status = int.Parse((frmSprawa.FindControl("ddlStatus") as DropDownList).SelectedValue);
                    int nadawca = int.Parse((frmSprawa.FindControl("hfCustomerId") as HiddenField).Value);

             

                    List<string> parameters = new List<string>();
                    parameters.Add(Membership.GetUser().Comment);
                    ActionLogger al = new ActionLogger(new ActionContext(new Guid("05555FAA-A86A-40C1-9A69-6512276C7098"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, parameters));
                    al.AppliesToCases.Add(id);
                    al.Execute();

                    dao.UpdateCase(id, opis, znakPisma, dataRozpoczecia, dataPisma, dataZakonczenia, uwagi, status, nadawca, -1, new Guid(Membership.GetUser().ProviderUserKey.ToString()));
                    
                    Response.Redirect("~/Sprawy/AkcjeSprawy.aspx?id=" + id.ToString(), false);
                }
            }
            else if (e.CommandName == "Cancel")
            {
                Response.Redirect("~/Sprawy/AkcjeSprawy.aspx?id=" + CoreObject.GetId(Request));
            }
        }

        protected void frmSprawa_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {

        }

        protected void frmSprawa_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }

        protected void lnkZmien_Click(object sender, EventArgs e)
        {
            customer.Visible = true;
            lnkSelectCustomer.Visible = (customer.IdInteresanta > -1);
        }

        protected void lnkSearchAgain_Click(object sender, EventArgs e)
        {
            lnkSearchAgain.Visible = false;
            customersList.Visible = false;
            customer.Visible = true;
            customer.SearchMode();
        }

        protected void lnkSelectCustomer_Click(object sender, EventArgs e)
        {
            onSelectCustomer(sender, e);            
        }     
    }
}