using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.Web.UI.Aplikacje.RKW
{
    public partial class Przegladanie : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                docIdToRegister.Focus();
                bindResults();
                bindDictionary();
            }
        }

        private void bindResults(RKWFilter filter)
        {
            int totalPages;
            rkwGrid.DataSource = new RegistryDAO().GetRKWPage(filter, out totalPages);
            filter.TotalPages = totalPages;
            rkwGrid.DataBind();
            Session["rkwFilter"] = filter;

            nrStrony.Items.Clear();

            for (int i = 1; i <= filter.TotalPages; i++)
                nrStrony.Items.Add(new ListItem(i.ToString(), i.ToString()));
            nrStrony.SelectedValue = filter.PageNumber.ToString();
            liczbaStron.Text = filter.TotalPages.ToString();
            rozmiarStrony.SelectedValue = filter.PageSize.ToString();
        }

        private void bindResults(){
            bindResults(new RKWFilter());
        }
       

        private void bindDictionary()
        {
            Collection<SimpleLookupDTO> lista = new RegistryDAO().GetOrganizationalUnits();
            lista.Insert(0, new SimpleLookupDTO(0, "- dowolny -"));
            fOrganizationalUnit.DataSource = lista;
            fOrganizationalUnit.DataTextField = "Description";
            fOrganizationalUnit.DataValueField = "ID";
            fOrganizationalUnit.DataBind();
        }

        protected void registerDocument(object sender, CommandEventArgs e)
        {
            if (Page.IsValid)
                Response.Redirect("~/Aplikacje/RKW/Rejestracja.aspx?docID=" + docIdToRegister.Text, true);
        }

        protected void checkDocument(object source, ServerValidateEventArgs args)
        {
            using (IDataReader dr = new RegistryDAO().GetRKWDataForRegistration(int.Parse(args.Value)))
            {
                if (dr.Read())
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
        }

        protected void applyFilters(object sender, EventArgs e)
        {
            RKWFilter filter = Session["rkwFilter"] as RKWFilter;
            if (filter == null)
                filter = new RKWFilter();

            filter.EntryNumber = int.Parse(string.IsNullOrEmpty(fEntryNumber.Text) ? "0" : fEntryNumber.Text);
            if (!string.IsNullOrEmpty(fDateFrom.Text))
                filter.DateFrom = DateTime.Parse(fDateFrom.Text);
            if (!string.IsNullOrEmpty(fDateTo.Text))
                filter.DateTo = DateTime.Parse(fDateTo.Text);
            filter.Remarks = fRemarks.Text;
            filter.OrganizationalUnit = fOrganizationalUnit.SelectedValue == "0" ? null : fOrganizationalUnit.SelectedItem.Text;
            filter.NewerFirst = fNewestFirst.SelectedValue == "1";
            filter.PageNumber = 1;
            bindResults(filter);
        }

        protected void zmianaRozmiaruStrony(object sender, EventArgs e)
        {
            RKWFilter f = Session["rkwFilter"] as RKWFilter;
            if (f == null)
                f = new RKWFilter();
            f.PageSize = int.Parse(rozmiarStrony.SelectedValue);
            bindResults(f);
        }

        protected void zmianaNumeruStrony(object sender, EventArgs e)
        {
            RKWFilter f = Session["rkwFilter"] as RKWFilter;
            if (f == null)
                f = new RKWFilter();
            f.PageNumber = int.Parse(nrStrony.SelectedValue);
            bindResults(f);
        }


        protected void zmianaStrony(object sender, CommandEventArgs e)
        {
            RKWFilter f = Session["rkwFilter"] as RKWFilter;
            if (f == null)
                f = new RKWFilter();
            int currentPage = f.PageNumber;
            
            switch (e.CommandArgument.ToString().ToLower())
            {
                case "prev": currentPage--; break;
                case "next": currentPage++; break;
                case "first": currentPage=1; break;
                case "last": currentPage=f.TotalPages; break;
            }

            if (currentPage < 1)
                currentPage = 1;
            if (currentPage > f.TotalPages)
                currentPage = f.TotalPages;

            f.PageNumber = currentPage;
            bindResults(f);
        }
    }
}