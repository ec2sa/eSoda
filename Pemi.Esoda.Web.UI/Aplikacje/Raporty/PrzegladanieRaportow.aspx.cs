using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Web.UI.ReportingServices;
using Pemi.Esoda.Web.UI.Classes;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Web.Security;

namespace Pemi.Esoda.Web.UI.Aplikacje.Raporty
{
    public partial class PrzegladanieRaportow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                UserDAO dao = new UserDAO();
                var depts = dao.GetDepartments();
                depts.RemoveAt(0);
                depts.Sort(delegate(SimpleLookupDTO d1, SimpleLookupDTO d2)
                {
                    return d1.Description.CompareTo(d2.Description);
                }
                    );

                D1OUID.DataSource = depts;
                D1OUID.DataTextField = "description";
                D1OUID.DataValueField = "ID";
                D1OUID.DataBind();

                D2OUID.DataSource = depts;
                D2OUID.DataTextField = "description";
                D2OUID.DataValueField = "ID";
                D2OUID.DataBind();

                D3OUID.DataSource = depts;
                D3OUID.DataTextField = "description";
                D3OUID.DataValueField = "ID";
                D3OUID.DataBind();

                D5OUID.DataSource = depts;
                D5OUID.DataTextField = "description";
                D5OUID.DataValueField = "ID";
                D5OUID.DataBind();

                D6OUID.DataSource = depts;
                D6OUID.DataTextField = "description";
                D6OUID.DataValueField = "ID";
                D6OUID.DataBind();
                
                var ctx = new ESodaDataContext();
                    var ouid=ctx.GetOrganizationalUnitID((Guid)Membership.GetUser().ProviderUserKey);
                    D1OUID.SelectedValue = D2OUID.SelectedValue = D3OUID.SelectedValue = D5OUID.SelectedValue = D6OUID.SelectedValue = ouid.ToString();

                if (Roles.IsUserInRole("Naczelnicy") && !Roles.IsUserInRole("Raporty"))
                {            
                    D1OUID.Enabled = D2OUID.Enabled = D3OUID.Enabled = D5OUID.Enabled=D6OUID.Enabled= false;            
                }

                List<string> reportTypes = new List<string>();
                reportTypes.Add("PDF");
                reportTypes.Add("Excel");

                D1ReportType.DataSource = reportTypes;
                D1ReportType.DataBind();
                
                D2ReportType.DataSource = reportTypes;
                D2ReportType.DataBind();
                
                D3ReportType.DataSource = reportTypes;
                D3ReportType.DataBind();

                D4ReportType.DataSource = reportTypes;
                D4ReportType.DataBind();

                D5ReportType.DataSource = reportTypes;
                D5ReportType.DataBind();

                D6ReportType.DataSource = reportTypes;
                D6ReportType.DataBind();
            }
        }

        public void DownloadReport(string reportName, ParameterValue[] parameters,string format,string reportTitle,string filename)
        {
            string extension, mimeType;

            ReportCallContext reportContext = new ReportCallContext()
            {
                ReportName = reportName
                ,
                ReportFormat = format
                ,
                ReportTitle = reportTitle
                ,
                Parameters = parameters
            };

            byte[] reportContent = new ReportService().GetReportFromRS(reportContext, out mimeType, out extension);

            string fileName = string.Format("{0}_{1}.{2}", filename, DateTime.Now.ToString("yyyy-MM-dd"), extension);

            Response.ContentType = mimeType;

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);

            Response.BinaryWrite(reportContent);

            Response.End();
        }

        protected void ReportD1_Click(object sender, EventArgs e)
        {
            ParameterValue[] parameters = new ParameterValue[3];
            parameters[0] = new ParameterValue() { Name = "ouID", Value = D1OUID.SelectedValue };
          
            parameters[1] = new ParameterValue() { Name = "daysFrom", Value = string.IsNullOrEmpty(D1DaysFrom.Text) ? null : D1DaysFrom.Text };
            parameters[2] = new ParameterValue() { Name = "daysTo", Value =string.IsNullOrEmpty(D1DaysTo.Text)?null:D1DaysTo.Text };

            string format = D1ReportType.SelectedValue;
            DownloadReport("DocumentsUnasignedIncoming", parameters, format, "Lista dokumentów nieprzypisanych do spraw", "NieprzypisaneDokumenty");
        }

        protected void ReportD2_Click(object sender, EventArgs e)
        {
            ParameterValue[] parameters = new ParameterValue[5];
            parameters[0] = new ParameterValue() { Name = "ouID", Value = D2OUID.SelectedValue };
            parameters[1] = new ParameterValue() { Name = "year", Value =string.IsNullOrEmpty(D2Year.Text)?DateTime.Today.Year.ToString():D2Year.Text};
            parameters[2] = new ParameterValue() { Name = "jrwa", Value = string.IsNullOrEmpty(D2JRWA.Text)?null:D2JRWA.Text };
            parameters[3] = new ParameterValue() { Name = "daysFrom", Value = string.IsNullOrEmpty(D2DaysFrom.Text) ? null : D2DaysFrom.Text };
            parameters[4] = new ParameterValue() { Name = "daysTo", Value = string.IsNullOrEmpty(D2DaysTo.Text) ? null : D2DaysTo.Text };

            string format = D2ReportType.SelectedValue;
            DownloadReport("CasesPending", parameters, format, "Lista spraw z kończącym się terminem realizacji", "SprawyPrzedTerminem");
        }

        protected void ReportD3_Click(object sender, EventArgs e)
        {
            ParameterValue[] parameters = new ParameterValue[5];
            parameters[0] = new ParameterValue() { Name = "ouID", Value = D3OUID.SelectedValue };
            parameters[1] = new ParameterValue() { Name = "year", Value = string.IsNullOrEmpty(D3Year.Text) ? DateTime.Today.Year.ToString() : D3Year.Text };
            parameters[2] = new ParameterValue() { Name = "jrwa", Value = string.IsNullOrEmpty(D3JRWA.Text) ? null : D3JRWA.Text };

            string format = D3ReportType.SelectedValue;
            DownloadReport("CasesOutOfDate", parameters, format, "Lista spraw przeterminowanych", "SprawyPrzeterminowane");
        }

        protected void ReportD4_Click(object sender, EventArgs e)
        {
            ParameterValue[] parameters = new ParameterValue[5];
            parameters[0] = new ParameterValue() { Name = "dateFrom", Value = string.IsNullOrEmpty(D4DateFrom.Text) ? null: D4DateFrom.Text };
            parameters[1] = new ParameterValue() { Name = "dateTo", Value = string.IsNullOrEmpty(D4DateTo.Text) ? null: D4DateTo.Text };
            parameters[2] = new ParameterValue() { Name = "registryID", Value = null };

            string format = D4ReportType.SelectedValue;
            DownloadReport("RegistryEntriesWithAdditionalMaterials", parameters, format, "Lista pozycji dziennika z niezdigitalizowanymi materiałami", "NiezdigitalizowaneMaterialy");
        }

        protected void ReportD5_Click(object sender, EventArgs e)
        {
            ParameterValue[] parameters = new ParameterValue[3];
            parameters[0] = new ParameterValue() { Name = "ouID", Value = D5OUID.SelectedValue };
            parameters[1] = new ParameterValue() { Name = "days", Value = string.IsNullOrEmpty(D5Days.Text) ? null : D5Days.Text };
            
            string format = D5ReportType.SelectedValue;
            DownloadReport("LastUserLogons", parameters, format, "Lista ostatnich logowań", "OstatnieLogowania");
        }

        protected void ReportD6_Click(object sender, EventArgs e)
        {
            ParameterValue[] parameters = new ParameterValue[3];
            parameters[0] = new ParameterValue() { Name = "ouID", Value = D6OUID.SelectedValue };
            parameters[1] = new ParameterValue() { Name = "days", Value = string.IsNullOrEmpty(D6Days.Text) ? null : D6Days.Text };

            string format = D6ReportType.SelectedValue;
            DownloadReport("LastCreatedCases", parameters, format, "Ostatnio utworzone sprawy", "OstatnioUtworzoneSprawy");
        }
 
    }
}
