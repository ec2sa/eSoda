using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI.Aplikacje.Raporty
{
    public partial class WysylkaRaportow : System.Web.UI.Page
    {
        private UserDAO dao = new UserDAO();
        private RegistryDAO registryDao = new RegistryDAO();
        ReportsDAO reportsDao = new ReportsDAO();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var depts = dao.GetDepartments();
                depts.RemoveAt(0);
                depts.Sort(delegate(SimpleLookupDTO d1, SimpleLookupDTO d2)
                {
                    return d1.Description.CompareTo(d2.Description);
                }
                    );
                ddlOUID.DataSource = depts;
                ddlOUID.DataTextField = "description";
                ddlOUID.DataValueField = "ID";
                ddlOUID.DataBind();

                ddlDepartment.DataSource = depts;
                ddlDepartment.DataTextField = "description";
                ddlDepartment.DataValueField = "ID";
                ddlDepartment.DataBind();

                int val = int.Parse(ddlDepartment.SelectedValue);
                var employees = registryDao.GetEmployees(val);
                ddlEmployee.DataSource = employees;
                ddlEmployee.DataTextField = "Description";
                ddlEmployee.DataValueField = "ID";
                ddlEmployee.DataBind();
                
                Dictionary<string, string> dict = new Dictionary<string, string>();

                dict.Add("CasesPending", "Lista spraw z kończącym się terminem realizacji");
                dict.Add("CasesOutOfDate", "Lista spraw przeterminowanych");

                ddlReports.DataSource = dict;
                ddlReports.DataTextField = "Value";
                ddlReports.DataValueField = "Key";
                ddlReports.DataBind();

                List<string> reportTypes = new List<string>();
                reportTypes.Add("PDF");
                reportTypes.Add("Excel");

                ddlReportType.DataSource = reportTypes;
                ddlReportType.DataBind();

                FillSubscriptionGrid();
            }
        }

        private void FillSubscriptionGrid()
        {
            var subscriptions = reportsDao.GetSubscriptions();
            
            gvSubscriptions.DataSource = subscriptions;
            gvSubscriptions.DataBind();
        }

        protected void ddlDepartment_SelectedIndexChange(object sender, EventArgs e)
        {
            int val = int.Parse(ddlDepartment.SelectedValue);
            var employees = registryDao.GetEmployees(val);
            ddlEmployee.DataSource = employees;
            ddlEmployee.DataTextField = "Description";
            ddlEmployee.DataValueField = "ID";
            ddlEmployee.DataBind();
        }

        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            string parameters="<params></params>"; 

            if (ddlReports.SelectedValue == "CasesPending")
            {
                parameters = "<params><ouID>{0}</ouID><year>{1}</year><jrwa>{2}</jrwa><daysFrom>{3}</daysFrom><daysTo>{4}</daysTo></params>";
                parameters = string.Format(parameters, ddlOUID.SelectedValue, tbYear.Text, tbJRWA.Text, tbDaysFrom.Text, tbDaysTo.Text);

            }
            else if (ddlReports.SelectedValue == "CasesOutOfDate")
            {
                parameters = "<params><ouID>{0}</ouID><year>{1}</year><jrwa>{2}</jrwa></params>";
                parameters = string.Format(parameters, ddlOUID.SelectedValue, tbYear.Text, tbJRWA.Text);
            }

            reportsDao.CreateSubscription(ddlReports.SelectedValue, parameters, int.Parse(ddlEmployee.SelectedValue), ddlReportType.SelectedValue);
            FillSubscriptionGrid();
        }

        protected void gvSubscriptions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName =="Delete"){

                int subscriptionID = Convert.ToInt32(e.CommandArgument);
                reportsDao.DeleteSubscription(subscriptionID);
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        protected void btnSend_click(object sender, EventArgs e)
        {
            new Pemi.Esoda.Web.UI.Classes.ReportService().SendReportsViaEmail();
        }
    }
}
