using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using System.Data.Linq;
using System.Web.Security;

namespace Pemi.Esoda.Web.UI.Rejestry
{
    public partial class RejestrCentralny : System.Web.UI.Page
    {

        protected SearchCriteria currentSearchCriteria
        {
            get
            {
                if((Session["currentSearchCriteria"] as SearchCriteria)==null){
                 Session["currentSearchCriteria"]=new SearchCriteria();
                }
                return Session["currentSearchCriteria"] as SearchCriteria;
            }
            set
            {
                Session["currentSearchCriteria"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!IsPostBack)
            {
                registryYear.Text = DateTime.Today.Year.ToString();
                Session.Remove("assignedDocID");
                if (Session["rcid"] != null)
                {
                    registryType.SelectedValue = Session["rcid"].ToString();
                    registryType.DataBind();
                    Session.Remove("rcid");
                }
                synchronizeRegistryView();
            }

            registryDS.Inserting += new EventHandler<LinqDataSourceInsertEventArgs>(registryDS_Inserting);
            GridView1.RowDataBound += new GridViewRowEventHandler(bindSearchCriteria);
        }

        protected void bindSearchCriteria(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header)
                return;
            
            if (e.Row.FindControl("filterPositionNumber") != null)
                (e.Row.FindControl("filterPositionNumber") as TextBox).Text = currentSearchCriteria.PositionNumber>0? currentSearchCriteria.PositionNumber.ToString():string.Empty;

            if (e.Row.FindControl("filterDateFrom") != null) (e.Row.FindControl("filterDateFrom") as TextBox).Text = currentSearchCriteria.DateFrom.ToString();
            if (e.Row.FindControl("filterSubject") != null) (e.Row.FindControl("filterSubject") as TextBox).Text = currentSearchCriteria.Subject;
            if (e.Row.FindControl("filteRemarks") != null) (e.Row.FindControl("filteRemarks") as TextBox).Text = currentSearchCriteria.Remarks;
            if (e.Row.FindControl("filterSide") != null) (e.Row.FindControl("filterSide") as TextBox).Text = currentSearchCriteria.Side;
            if (e.Row.FindControl("filterReferenceNumber") != null) (e.Row.FindControl("filterReferenceNumber") as TextBox).Text = currentSearchCriteria.ReferenceNumber;
            if (e.Row.FindControl("filterOrganizationalUnit") != null) (e.Row.FindControl("filterOrganizationalUnit") as TextBox).Text = currentSearchCriteria.OrganizationalUnit;

        }

        protected void changeRegistry(object sender, EventArgs e)
        {
            currentSearchCriteria = new SearchCriteria();
            synchronizeRegistryView();

        }

        protected void resetFilters(object sender, EventArgs e)
        {
            currentSearchCriteria = new SearchCriteria();
            synchronizeRegistryView();

        }        

        protected void registryDS_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            var ctx = new ESodaDataContext();
            var criteria = currentSearchCriteria;

            IQueryable<CentralRegistry> results;

            results = ctx.CentralRegistries
                .Where(r => r.RegistryTypeID.ToString() == (registryType.SelectedValue == "" ? "1" : registryType.SelectedValue))
                .Where(r => r.RegistryYear.ToString() == registryYear.Text || string.IsNullOrEmpty(registryYear.Text));

            if (registryOrder.SelectedValue == "asc")
                results = results.OrderBy(r => r.PositionNumber);
            else
                results = results.OrderByDescending(r => r.PositionNumber);

            if (criteria.AreEmpty)
            {
                e.Result = results;
                return;
            }

            if (criteria.PositionNumber > 0)
                results = results.Where(r => r.PositionNumber == criteria.PositionNumber);

            if (criteria.DateFrom.HasValue)
                results = results.Where(r => r.ItemDate >= criteria.DateFrom.Value.Date);

            if (criteria.DateTo.HasValue)
                results = results.Where(r => r.ItemDate <= criteria.DateTo.Value.Date);

            if (!string.IsNullOrEmpty(criteria.Side))
                results = results.Where(r => r.ItemSide.Contains(criteria.Side));

            if (!string.IsNullOrEmpty(criteria.Subject))
                results = results.Where(r => r.ItemSubject.Contains(criteria.Subject));

            if (!string.IsNullOrEmpty(criteria.ReferenceNumber))
                results = results.Where(r => r.ItemReferenceNumber.Contains(criteria.ReferenceNumber));

            if (!string.IsNullOrEmpty(criteria.OrganizationalUnit))
                results = results.Where(r => r.OrganizationalUnit.Contains(criteria.OrganizationalUnit));

            if (!string.IsNullOrEmpty(criteria.Remarks))
                results = results.Where(r => r.Remarks.Contains(criteria.Remarks));


            e.Result = results;
        }

        protected SearchCriteria getSearchCriteriaFrom(GridView grid)
        {
            if (grid == null)
                return new SearchCriteria();

            var filterPositionNumber = (grid.HeaderRow.FindControl("filterPositionNumber") as TextBox)!=null? (grid.HeaderRow.FindControl("filterPositionNumber") as TextBox).Text:"0";
            if (string.IsNullOrEmpty(filterPositionNumber.Trim()))
                filterPositionNumber = "0";
            var filterDateFrom = (grid.HeaderRow.FindControl("filterDateFrom") as TextBox) != null ? (grid.HeaderRow.FindControl("filterDateFrom") as TextBox).Text : string.Empty;
            var filterSubject = (grid.HeaderRow.FindControl("filterSubject") as TextBox) != null ? (grid.HeaderRow.FindControl("filterSubject") as TextBox).Text : string.Empty;
            var filteRemarks = (grid.HeaderRow.FindControl("filteRemarks") as TextBox) != null ? (grid.HeaderRow.FindControl("filteRemarks") as TextBox).Text : string.Empty;
            var filterSide = (grid.HeaderRow.FindControl("filterSide") as TextBox) != null ? (grid.HeaderRow.FindControl("filterSide") as TextBox).Text : string.Empty;
            var filterReferenceNumber = (grid.HeaderRow.FindControl("filterReferenceNumber") as TextBox) != null ? (grid.HeaderRow.FindControl("filterReferenceNumber") as TextBox).Text : string.Empty;
            var filterOrganizationalUnit = (grid.HeaderRow.FindControl("filterOrganizationalUnit") as TextBox) != null ? (grid.HeaderRow.FindControl("filterOrganizationalUnit") as TextBox).Text : string.Empty;

             return new SearchCriteria()
             {
                 PositionNumber = int.Parse(filterPositionNumber),
                 DateFrom = !string.IsNullOrEmpty(filterDateFrom) ? (DateTime?)DateTime.Parse(filterDateFrom) : null,
                 DateTo = !string.IsNullOrEmpty(filterDateFrom) ? (DateTime?)DateTime.Parse(filterDateFrom) : null,
                 Side = filterSide,
                 Subject = filterSubject,
                 ReferenceNumber = filterReferenceNumber,
                 OrganizationalUnit = filterOrganizationalUnit,
                 Remarks = filteRemarks
             };
        }

        protected void filterRegistry(object sender, EventArgs e)
        {
          var currentGrid = (sender as Control).NamingContainer.NamingContainer as GridView;

          currentSearchCriteria = getSearchCriteriaFrom(currentGrid);
          if (currentGrid != null)
              currentGrid.DataBind();

        }

        void registryDS_Inserting(object sender, LinqDataSourceInsertEventArgs e)
        {
            var item = e.NewObject as CentralRegistry;

            if (item.ItemDate == DateTime.MinValue)
                item.ItemDate = null;

        }

        protected void currentRegistryChanged(object sender, EventArgs e)
        {
            synchronizeRegistryView();

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "assignDataToEdit")
            {
                int docID;

                var currentRow = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;

                var docIDField = (currentRow.FindControl("DocumentID") as TextBox);

                if (docIDField == null)
                    return;

                if (string.IsNullOrEmpty(docIDField.Text))
                {
                    (e.CommandSource as LinkButton).CssClass = "findDoc";
                    return;
                }
                if (!int.TryParse(docIDField.Text, out docID))
                {
                    (e.CommandSource as LinkButton).CssClass = "notFoundDoc";
                    Session.Remove("assignedDocID");
                    return;
                }

                var ctx = new ESodaDataContext();

                DataForCentralRegistry data = ctx.GetDataForCentralRegistry(docID).FirstOrDefault();

                if (data == null)
                {
                    (e.CommandSource as LinkButton).CssClass = "notFoundDoc";
                    Session.Remove("assignedDocID");
                    return;
                }

                var targetDateField = (currentRow.Cells[1].Visible) ? currentRow.Cells[1].Controls[0] as TextBox : null;
                var targetOrganizationalUnitField = (currentRow.Cells[5].Visible) ? currentRow.Cells[5].Controls[0] as TextBox : null;
                var targetReferenceNumberField = (currentRow.Cells[3].Visible) ? currentRow.Cells[3].Controls[0] as TextBox : null;


                if (targetDateField != null)
                    targetDateField.Text = data.ItemDate;
                if (targetOrganizationalUnitField != null)
                    targetOrganizationalUnitField.Text = data.OrganizationalUnit;
                if (targetReferenceNumberField != null)
                    targetReferenceNumberField.Text = data.ItemReferenceNumber;

                Session["assignedDocID"] = docID;
                (e.CommandSource as LinkButton).CssClass = "foundDoc";

            }
        }

        protected void assignData(object sender, CommandEventArgs e)
        {
            var fv = (sender as Control).NamingContainer as FormView;

            int docID;
            var docIDField = (fv.FindControl("fDocumentID") as TextBox);
            var showDocLink = (fv.FindControl("showDocLink") as HyperLink);

            if (showDocLink != null)
            {
                showDocLink.NavigateUrl = "";
                showDocLink.Visible = false;
            }

            if (docIDField == null)
                return;

            if (string.IsNullOrEmpty(docIDField.Text))
            {
                (sender as LinkButton).CssClass = "findDoc";
                return;
            }

            if (!int.TryParse(docIDField.Text, out docID))
            {
                (sender as LinkButton).CssClass = "notFoundDoc";
                Session.Remove("assignedDocID");
                return;
            }

            var ctx = new ESodaDataContext();

            DataForCentralRegistry data = ctx.GetDataForCentralRegistry(docID).FirstOrDefault();

            if (data == null)
            {
                (sender as LinkButton).CssClass = "notFoundDoc";
                Session.Remove("assignedDocID");
                return;
            }

            if (showDocLink != null)
            {
                showDocLink.NavigateUrl = string.Format("~/Dokumenty/SkladnikiDokumentu.aspx?id={0}", docID);
                showDocLink.Visible = true;
            }

            var targetDateField = fv.FindControl("ItemDateTextBox") as TextBox;
            var targetOrganizationalUnitField = fv.FindControl("ItemOrganizationalUnitTextBox") as TextBox;
            var targetReferenceNumberField = fv.FindControl("ItemReferenceNumberTextBox") as TextBox;

            if (targetDateField != null)
                targetDateField.Text = data.ItemDate;
            if (targetOrganizationalUnitField != null)
                targetOrganizationalUnitField.Text = data.OrganizationalUnit;
            if (targetReferenceNumberField != null)
                targetReferenceNumberField.Text = data.ItemReferenceNumber;

            Session["assignedDocID"] = docID;
            (sender as LinkButton).CssClass = "foundDoc";
        }

        private void synchronizeRegistryView()
        {
            Session.Remove("assignedDocID");
            insertWrapper.Visible = Roles.IsUserInRole("RejestryCentralne");
           

            switch (registryType.SelectedValue)
            {
                case "":
                case "1": GridView1.Visible = true;
                    GridView2.Visible = false;
                    GridView3.Visible = false;
                    GridView4.Visible = false;
                    formView1.Visible = true;
                    formView2.Visible = false;
                    formView3.Visible = false;
                    formView4.Visible = false;
                    GridView1.Columns[GridView1.Columns.Count - 1].Visible = Roles.IsUserInRole("RejestryCentralne");
                    GridView1.DataBind();
                    break;
                case "2": GridView1.Visible = false;
                    GridView2.Visible = true;
                    GridView3.Visible = false;
                    GridView4.Visible = false;
                    formView1.Visible = false;
                    formView2.Visible = true;
                    formView3.Visible = false;
                    formView4.Visible = false;
                    GridView2.Columns[GridView2.Columns.Count - 1].Visible = Roles.IsUserInRole("RejestryCentralne");
                    GridView2.DataBind();
                    break;
                case "3": GridView1.Visible = false;
                    GridView2.Visible = false;
                    GridView3.Visible = true;
                    GridView4.Visible = false;
                    formView1.Visible = false;
                    formView2.Visible = false;
                    formView3.Visible = true;
                    formView4.Visible = false;
                    GridView3.Columns[GridView3.Columns.Count - 1].Visible = Roles.IsUserInRole("RejestryCentralne");
                    GridView3.DataBind();
                    break;
                case "4": GridView1.Visible = false;
                    GridView2.Visible = false;
                    GridView3.Visible = false;
                    GridView4.Visible = true;
                    formView1.Visible = false;
                    formView2.Visible = false;
                    formView3.Visible = false;
                    formView4.Visible = true;
                    GridView4.Columns[GridView4.Columns.Count - 1].Visible = Roles.IsUserInRole("RejestryCentralne");
                    GridView4.DataBind();
                    break;
            }
        }

        protected void lnkNewEntry_click(object sender, EventArgs e)
        {
            var fv = (sender as Control).NamingContainer as FormView;
            if (fv != null)
                fv.ChangeMode(FormViewMode.Insert);

        }

        protected void formView_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            synchronizeRegistryView();
            //if (GridView1.Visible) GridView1.DataBind();
            //if (GridView2.Visible) GridView2.DataBind();
            //if (GridView3.Visible) GridView3.DataBind();
            //if (GridView4.Visible) GridView4.DataBind();
        }

        //void registryDS_Inserted(object sender, LinqDataSourceStatusEventArgs e)
        //{
        //    Session["currentCentralRegistryType"] = registryType.SelectedValue;
        //    Session["currentCentralRegistryYear"] = registryYear.Text;
        //    Session.Remove("assignedDocID");
        //  //  Response.Redirect(Request.Url.AbsoluteUri, true);
        //}

        protected void formView_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            e.Values.Add("RegistryYear", registryYear.Text);
            e.Values.Add("RegistryTypeID", registryType.SelectedValue);
            e.Values.Add("PositionNumber", 1);
            if (Session["assignedDocID"] != null)
                e.Values.Add("DocumentID", Session["assignedDocID"].ToString());
        }

        protected class SearchCriteria
        {
            public int PositionNumber { get; set; }
            public DateTime? DateFrom { get; set; }
            public DateTime? DateTo { get; set; }
            public string Subject { get; set; }
            public string Side { get; set; }
            public string ReferenceNumber { get; set; }
            public string OrganizationalUnit { get; set; }
            public string Remarks { get; set; }

            public bool AreEmpty
            {
                get
                {
                    return PositionNumber == 0 && !DateFrom.HasValue && !DateTo.HasValue && string.IsNullOrEmpty((Subject + Side + ReferenceNumber + OrganizationalUnit + Remarks).Trim());
                }
            }
        }
    }
}
