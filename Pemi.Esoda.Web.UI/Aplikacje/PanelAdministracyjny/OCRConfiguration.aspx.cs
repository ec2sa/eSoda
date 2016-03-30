using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using System.ServiceProcess;
using System.Drawing;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class OCRConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckServiceStatus();

            if (!IsPostBack)
            {
                Dictionary<string, string> configData = new EsodaConfigParametersDAO().GetConfig();

                tbOCRStart.Text = configData["ocrStartHour"];
                tbOCREnd.Text = configData["ocrEndHour"];

                OCRLogsDAO dao = new OCRLogsDAO();
                gvLogs.DataSource = dao.GetLogs();
                gvLogs.DataBind();
            }
        }

        private void CheckServiceStatus()
        {
            ServiceController sc = new ServiceController("ESodaOCR");

            try
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    lblServiceRunning.ForeColor = Color.Green;
                    lblServiceRunning.Text = "Usługa eSoda OCR jest uruchomiona.";
                }
                else
                {
                    throw new Exception("Service is not running.");
                }
            }
            catch (Exception)
            {
                lblServiceRunning.ForeColor = Color.Red;
                lblServiceRunning.Text = "Usługa eSoda OCR nie jest uruchomiona.";
            }
        }

        protected void lbtnShow_Click(object sender, EventArgs e)
        {
            int days;
            if(int.TryParse(tbDays.Text, out days)){
                OCRLogsDAO dao = new OCRLogsDAO();
                gvLogs.DataSource = dao.GetLogs(days);
                gvLogs.DataBind();
            }
        }

        protected void Zapisz_click(object sender, EventArgs e)
        {
            string ocrStartHour= tbOCRStart.Text;
            string ocrEndHour = tbOCREnd.Text;

            try
            {
                if (ValidateHour(ocrStartHour) && ValidateHour(ocrEndHour))
                {
                    EsodaConfigParametersDAO dao = new EsodaConfigParametersDAO();
                    dao.SetConfigParam("ocrStartHour", ocrStartHour);
                    dao.SetConfigParam("ocrEndHour", ocrEndHour);

                    msg.Attributes["style"] = "color:Green;";
                    msg.Text = "Konfiguracja zapisana poprawnie";
                }
                else
                {
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Podaj poprawny format godziny";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                msg.Attributes["style"] = "color:red;";
                msg.Text = "Błąd w trakcie zapisu konfiguracji";
            }
        }

        private bool ValidateHour(string hour)
        {
            if (hour.Contains(":"))
            {
                string[] split = hour.Split(new char[]{':'});
                if (split.Length == 2 && IsHour(split[0]) && IsMinute(split[1]))
                    return true;
            }
            else
            {
                if (IsHour(hour))
                    return true;
            }
            return false;
        }

        private bool IsHour(string hour)
        {
            int h;
            if (int.TryParse(hour, out h) && (h >= 0 && h <= 24))
                return true;
            return false;
        }

        private bool IsMinute(string hour)
        {
            int h;
            if (int.TryParse(hour, out h) && (h >= 0 && h <= 59))
                return true;
            return false;
        }
    }
}
