using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pemi.Esoda.Web.UI.ReportingServices;
using System.Net.Mail;
using System.IO;
using Pemi.Esoda.DataAccess;
using System.Data;
using System.Xml.Linq;


namespace Pemi.Esoda.Web.UI.Classes
{
    public class ReportService
    {
        public byte[] GetReportFromRS(ReportCallContext reportContext, out string mimeType, out string extension)
        {
            reportContext.ReportName = "/eSoda/" + reportContext.ReportName;

            ReportExecutionService ws = new ReportingServices.ReportExecutionService();
            Warning[] warnings;
            string[] sids;
            string encoding;

            ws.Credentials = System.Net.CredentialCache.DefaultCredentials;

            ws.Timeout = 1000 * 600;
            //parameters[0] = new ParameterValue();
            //parameters[0].Name = "params";
            //parameters[0].Value = reportContext.ReportParameters.ToString(SaveOptions.DisableFormatting);

            ExecutionInfo execInfo = new ExecutionInfo();
            ExecutionHeader execHeader = new ExecutionHeader();
            ws.ExecutionHeaderValue = execHeader;
            byte[] reportContent = null;
            try
            {
                execInfo = ws.LoadReport(reportContext.ReportName, null);
                ws.SetExecutionParameters(reportContext.Parameters, "pl-PL");
                string sessionID = ws.ExecutionHeaderValue.ExecutionID;
                reportContent = ws.Render(reportContext.ReportFormat, null, out extension, out mimeType, out encoding, out warnings, out sids);
            }
            catch (Exception ex)
            {
                mimeType = null;
                extension = null;
                System.Diagnostics.Trace.WriteLine(ex.Message);
                throw;
            }
            return reportContent;
        }

        protected bool SendReport(ReportSubscription subscription)
        {
            string mimeType, extension;
            
            ParameterValue[] reportParameters=new ParameterValue[1];

            if (subscription.ReportName == "CasesPending")
            {
                reportParameters = new ParameterValue[5];
                reportParameters[0] = new ParameterValue() { Name = "ouID", Value = subscription.ParamOUID };
                reportParameters[1] = new ParameterValue() { Name = "year", Value = subscription.ParamYear };
                reportParameters[2] = new ParameterValue() { Name = "jrwa", Value = subscription.ParamJRWA };
                reportParameters[3] = new ParameterValue() { Name = "daysFrom", Value = subscription.ParamDaysFrom };
                reportParameters[4] = new ParameterValue() { Name = "daysTo", Value = subscription.ParamDaysTo };

            }
            if (subscription.ReportName == "CasesOutOfDate")
            {
                reportParameters = new ParameterValue[3];
                reportParameters[0] = new ParameterValue() { Name = "ouID", Value = subscription.ParamOUID };
                reportParameters[1] = new ParameterValue() { Name = "year", Value = subscription.ParamYear };
                reportParameters[2] = new ParameterValue() { Name = "jrwa", Value = subscription.ParamJRWA };
            }

            ReportCallContext reportContext = new ReportCallContext()
            {
                ReportName = subscription.ReportName
                ,
                ReportFormat = subscription.Format
                ,
                ReportTitle = subscription.ReportTitle
                ,
                Parameters = reportParameters
            };
            try
            {
                byte[] reportContent = new ReportService().GetReportFromRS(reportContext, out mimeType, out extension);
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("raporty@esoda.pl", "eSoda-Raporty");
                mail.To.Add(subscription.EmailAddress);
                mail.Subject = "Raport z systemu eSoda ";
                mail.Body = "W załączniku znajduje się raport. Został wysłany z systemu eSoda w ramach zdefiniowanej subskrypcji.";

                using (MemoryStream ms = new MemoryStream(reportContent))
                {
                    try
                    {
                        mail.Attachments.Add(new Attachment(ms, "Raport.pdf"));
                        SmtpClient client = new SmtpClient();
                        client.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public bool SendReportsViaEmail()
        {
            List<ReportSubscription> subscriptions = new List<ReportSubscription>();

            using (IDataReader dr = new ReportsDAO().GetSubscriptions())
            {
                //napchac do subscriptions
                while (dr.Read())
                {
                    ReportSubscription s = new ReportSubscription();
                    s.ReportName = dr["ReportName"].ToString();
                    s.ReportTitle = "Raport z systemu eSoda";
                    s.Format = dr["Format"].ToString();
                    s.Filename = "Raport";
                    s.EmailAddress = dr["emailAddress"].ToString();
                    XElement p=XElement.Parse(dr["xmlParams"].ToString());
                    s.ParamOUID =p.Descendants("ouID").First().Value;
                    s.ParamYear = p.Descendants("year").FirstOrDefault()!=null?p.Descendants("year").FirstOrDefault().Value:null;
                    s.ParamJRWA = p.Descendants("jrwa").FirstOrDefault() != null ? p.Descendants("jrwa").FirstOrDefault().Value : null;
                    s.ParamDaysFrom = p.Descendants("daysFrom").FirstOrDefault() != null ? p.Descendants("daysFrom").FirstOrDefault().Value : null;
                    s.ParamDaysTo = p.Descendants("daysTo").FirstOrDefault() != null ? p.Descendants("daysTo").FirstOrDefault().Value : null;

                    if (s.ParamJRWA == "")
                        s.ParamJRWA = null;
                    if (s.ParamDaysFrom == "")
                        s.ParamDaysFrom = null;
                    if (s.ParamDaysTo == "")
                        s.ParamDaysTo = null;
                    subscriptions.Add(s);
                }

            }


            foreach (ReportSubscription subsc in subscriptions)
            {
                SendReport(subsc);
            }

          

            return true;
        }
    }

    public class ReportSubscription
    {
        public string ReportName { get; set; }
        public string ReportTitle { get; set; }
        public string ParamOUID { get; set; }
        public string ParamYear { get; set; }
        public string ParamJRWA { get; set; }
        public string ParamDaysFrom { get; set; }
        public string ParamDaysTo { get; set; }
        public string Format { get; set; }
        public string Filename { get; set; }
        public string EmailAddress { get; set; }
    }
    public class ReportCallContext
    {
        // public XElement ReportParameters { get; set; }
        public string ReportTitle { get; set; }
        public string ReportName { get; set; }
        public string ReportFormat { get; set; }
        public ParameterValue[] Parameters { get; set; }
    }

    
}
