using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Web.UI.ReportingServices;
using Pemi.Esoda.Web.UI.Classes;

namespace Pemi.Esoda.Web.UI.Aplikacje.Raporty
{
    public partial class MetrykaSprawy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            int id;
            if (Request.QueryString["id"] == null || !int.TryParse(Request.QueryString["id"], out id))
            {
                Response.End();
                return;
            }

            ParameterValue[] parameters = new ParameterValue[3];
            parameters[0] = new ParameterValue() { Name = "idSprawy", Value = id.ToString() };

            string format = "PDF";
            DownloadReport("CaseMetrics", parameters, format, "MetrykaSprawy", "MetrykaSprawy");
        }

        protected void DownloadReport(string reportName, ParameterValue[] parameters, string format, string reportTitle, string filename)
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
    }
}