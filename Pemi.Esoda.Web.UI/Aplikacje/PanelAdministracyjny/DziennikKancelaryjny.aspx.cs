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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Pemi.Esoda.Tools;
using System.Data.SqlClient;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class DziennikKancelaryjny : System.Web.UI.Page
    {
        private RegistryDTO dailyLog;

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMessage.Text = null;

            if (!Page.IsPostBack)
            {
                LoadDailyLogItemAccessDenied();
                GetDailyLog();
                if (dailyLog.IsXslFoExist)
                {
                    Session["dlXslFo"] = dailyLog.XslFo;
                    lblCustomXslFo.Visible = true;
                    lnkRemoveXslFo.Visible = true;
                }
            }

            

        }

        private void UpdateDailyLog()
        {
            try
            {
                string xslFo = (Session["dlXslFo"] != null) ? Session["dlXslFo"].ToString() : null;

                new RegistryDAO().UpdateDailyLog(xslFo);

                GetDailyLog();

                if (dailyLog.IsXslFoExist)
                {                
                    lblCustomXslFo.Visible = true;
                    lnkRemoveXslFo.Visible = true;
                }

                WebMsgBox.Show(this, "Dane dziennika zosta³y zapisane.");
            }
            catch (SqlException ex)
            {
                WebMsgBox.Show(this, String.Format("{0}:{1}", "Wyst¹pi³ problem z zapisaniem danych dziennika", ex.Message));
            }
            catch (Exception ex)
            {
                WebMsgBox.Show(this, String.Format("{0}:{1}", "Wyst¹pi³ problem z zapisaniem danych dziennika", ex.Message));
            }
        }

        private void GetDailyLog()
        {
            try
            {
                dailyLog = new RegistryDAO().GetDailyLog();                
            }
            catch (SqlException ex)
            {
                WebMsgBox.Show(this, String.Format("{0}:{1}", "Wyst¹pi³ problem z zapisaniem danych dziennika", ex.Message));
            }
            catch (Exception ex)
            {
                WebMsgBox.Show(this, String.Format("{0}:{1}", "Wyst¹pi³ problem z zapisaniem danych dziennika", ex.Message));
            }
        }

        protected void lnkAddXslFo_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (uploadXslFo.HasFile)
            {
                try
                {
                    filePath = Server.MapPath(ConfigurationManager.AppSettings["katalogRoboczy"].ToString()) + "\\" + uploadXslFo.FileName;
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    uploadXslFo.SaveAs(filePath);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XslCompiledTransform regXSLT = new XslCompiledTransform();
                    regXSLT.Load(XmlReader.Create(new StreamReader(filePath)));
                    Session["dlXslFo"] = doc.OuterXml;
                    lblCustomXslFo.Visible = true;
                    lnkRemoveXslFo.Visible = true;
                }
                catch (Exception ex)
                {
                    WebMsgBox.Show(this, String.Format("Nie uda³o siê wczytaæ pliku. SprawdŸ, czy wskazany plik jest poprawnym dokumentem XSLT. {0}", ex.Message));                    
                }
            }
            else
                WebMsgBox.Show(this, "Wybierz plik XSLT do wczytania");
        }

        protected void lnkRemoveXslFo_Click(object sender, EventArgs e)
        {
            if (Session["dlXslFo"] != null)
            {
                Session["dlXslFo"] = null;
                lblCustomXslFo.Visible = false;
                lnkRemoveXslFo.Visible = false;
            }
        }

        protected void lnkSaveChanges_Click(object sender, EventArgs e)
        {
            UpdateDailyLog();
        }

        private void LoadDailyLogItemAccessDenied()
        {
            cbDailyLogItemAccessDenied.Checked = EsodaConfigurationParameters.IsDailyLogItemAccessDenied;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                new EsodaConfigParametersDAO().SetConfigParam("dailyLogItemAccessDenied", cbDailyLogItemAccessDenied.Checked ? "1" : "0");               
                LoadDailyLogItemAccessDenied();

            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
    }
}
