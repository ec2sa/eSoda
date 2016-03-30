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
using Pemi.Esoda.Presenters;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class ListaInteresantow : System.Web.UI.UserControl
    {
        public event EventHandler AddNewCustomer;
        public event EventHandler EditCustomer;
        public event EventHandler SelectCustomer;

        public bool AdminMode;

        private int _pageSize;
        
        public int PageSize
        {
            set { this._pageSize = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (_pageSize <=0)
                _pageSize = 10;
            ViewState["PageSize"] = _pageSize;
            gvListaInteresantow.PageSize = _pageSize;
        }

        protected void lnkAddNewCustomer_Click(object sender, EventArgs e)
        {
            if (AddNewCustomer != null)
               AddNewCustomer(sender, e);
        }

        public void SearchListVisible(object sender, Pemi.Esoda.Presenters.SearchCustomersEventArgs e)
        {
            this.Visible = e.SearchListVisible;
        }      

        public void FindCustomer(int idTypu, int idKategorii, string imie, string nazwisko, string nazwa,
            string miejscowosc, string kod, string ulica, string budynek, string lokal,string nip,string poczta,string numerSMS)
        {
            gvListaInteresantow.DataSource = (new CustomerDAO()).FindCustomer(idTypu, idKategorii, imie, nazwisko, nazwa, miejscowosc, kod, ulica, budynek, lokal, "",nip,poczta,numerSMS);
            gvListaInteresantow.PageSize = _pageSize;
            gvListaInteresantow.DataBind();
        }

        public void FindCustomers(object sender, SearchCustomersEventArgs e)
        {
            this.Visible = e.SearchListVisible;
            odsListaInteresantow.SelectParameters.Clear();

            odsListaInteresantow.SelectParameters.Add("idTypu",e.IdTypu.ToString());
            odsListaInteresantow.SelectParameters.Add("idKategorii",e.IdKategorii.ToString());
            odsListaInteresantow.SelectParameters.Add("imie",e.Imie);
            odsListaInteresantow.SelectParameters.Add("nazwisko",e.Nazwisko);
            odsListaInteresantow.SelectParameters.Add("nazwa",e.Nazwa);
            odsListaInteresantow.SelectParameters.Add("miejscowosc",e.Miasto);
            odsListaInteresantow.SelectParameters.Add("kod",e.Kod);
            odsListaInteresantow.SelectParameters.Add("ulica",e.Ulica);
            odsListaInteresantow.SelectParameters.Add("budynek",e.Budynek);
            odsListaInteresantow.SelectParameters.Add("lokal",e.Lokal);
            odsListaInteresantow.SelectParameters.Add("sortParam","");
            odsListaInteresantow.SelectParameters.Add("nip", e.Nip);
            odsListaInteresantow.SelectParameters.Add("poczta", e.Poczta);
            odsListaInteresantow.SelectParameters.Add("numerSMS", e.NumerSMS);
                        
            //gvListaInteresantow.DataSource = (new CustomerDAO()).FindCustomer(e.IdTypu, e.IdKategorii, e.Imie, e.Nazwisko, e.Nazwa, e.Miasto, e.Kod, e.Ulica, e.Budynek, e.Lokal, "");
            //gvListaInteresantow.DataSource = (new CustomerDAO()).FindCustomer(, , , , , , , , , , "");
            gvListaInteresantow.PageSize = _pageSize;
            gvListaInteresantow.DataBind();
        }

        protected void gvListaInteresantow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch(e.CommandName)
            {
                case "EditCustomer": if (EditCustomer != null) EditCustomer(e.CommandArgument, e); break;
                case "SelectCustomer": if (SelectCustomer != null) SelectCustomer(e.CommandArgument, e); break;
                default:
                    break;
            }
        }

        protected void gvListaInteresantow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                (e.Row.FindControl("lnkEdit") as LinkButton).Visible = AdminMode;
                (e.Row.FindControl("lnkSelect") as LinkButton).Visible = !AdminMode;
            }
        }
    }
}