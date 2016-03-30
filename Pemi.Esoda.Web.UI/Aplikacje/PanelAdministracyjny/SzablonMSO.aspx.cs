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
using System.Text;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class SzablonMSO : System.Web.UI.Page
    {
        private MSOTemplateDAO dao = null;

        private int? WordTemplateID
        {
            get
            {
                if (ViewState["WordTemplateID"] != null)
                {
                    return (int)ViewState["WordTemplateID"];
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                ViewState["WordTemplateID"] = value;
            }
        }

        private int? WordSecureTemplateID
        {
            get
            {
                if (ViewState["WordSecureTemplateID"] != null)
                {
                    return (int)ViewState["WordSecureTemplateID"];
                }
                else
                {
                    return 2;
                }
            }
            set
            {
                ViewState["WordSecureTemplateID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dao = new MSOTemplateDAO();
            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                LoadWordTemplate();
                LoadSecureWordTemplate();
                LoadTicketDuration();
                LoadTicketLifeTime();
                LoadLegalActsSettings();
            }
        }

        private void LoadWordTemplate()
        {
            try
            {
                MSOTemplateDTO wordTemplate = dao.GetMSOTemplate(WordTemplateID);
                if (wordTemplate != null)
                {
                    WordTemplateID = wordTemplate.TemplateID;
                    isWordTemplateActive.Checked = wordTemplate.IsActive;
                    lblWordOriginalFileName.Text = wordTemplate.OriginalFileName;
                    hfWordFileName.Value = wordTemplate.FileName;

                    if (!String.IsNullOrEmpty(wordTemplate.OriginalFileName))
                    {
                        currentWordTemplate.Visible = true;
                    }
                    else
                    {
                        currentWordTemplate.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadSecureWordTemplate()
        {
            try
            {
                MSOTemplateDTO wordSecureTemplate = dao.GetMSOTemplate(WordSecureTemplateID, true, null, null);
                if (wordSecureTemplate != null)
                {
                    WordSecureTemplateID = wordSecureTemplate.TemplateID;
                    if (!String.IsNullOrEmpty(wordSecureTemplate.OriginalFileName))
                    {
                        lblWordSecureOriginalFileName.Text = wordSecureTemplate.OriginalFileName;
                        currentWordSecureTemplate.Visible = true;
                    }
                    else
                    {
                        lblWordSecureOriginalFileName.Text = string.Empty;
                        currentWordSecureTemplate.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadTicketDuration()
        {
            ticketDuration.Text = EsodaConfigurationParameters.TicketDuration.ToString();
        }

        private void LoadLegalActsSettings()
        {
            LegalActsSettingsDTO legalActsSettings = dao.GetLegalActsSettings();
            if (legalActsSettings != null)
            {
                lblLegalActsSettingsVersion.Text = "Wersja: " + legalActsSettings.Version;
                lnkCurrentSchema.Visible = true;
                lnkCurrentXslt.Visible = true;
            }
            else
            {
                lblLegalActsSettingsVersion.Text = "Brak bie¿¹cych ustawieñ.";
                lnkCurrentSchema.Visible = false;
                lnkCurrentXslt.Visible = false;
            }
        }

        private void LoadTicketLifeTime()
        {
            ticketLifeTime.Text = EsodaConfigurationParameters.TicketLifeTime.ToString();
        }

        private string SaveFile(FileUpload fu)
        {
            string ext = Path.GetExtension(fu.PostedFile.FileName);

            string temporaryFileName = string.Empty;

            try
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["katalogDokumentow"] + "\\MSOIntegration\\");

                temporaryFileName = String.Concat(Guid.NewGuid().ToString(), ext);
                fu.SaveAs(String.Concat(ConfigurationManager.AppSettings["katalogDokumentow"] + "\\MSOIntegration\\", temporaryFileName));
            }
            catch (HttpException ex)
            {
                throw new Exception(ex.Message);
            }

            return temporaryFileName;
        }

        private string GetLegalActsSchema(FileUpload fu)
        {
            string ext = Path.GetExtension(fu.PostedFile.FileName);
            if (ext != ".xsd")
            {
                throw new Exception("Nieprawid³owy plik XSD.");
            }

            try
            {
                StreamReader schemaReader = new StreamReader(fu.FileContent);
                if (schemaReader != null)
                {
                    string schema = schemaReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(schema))
                    {
                        return schema;
                    }
                }
            }
            catch (HttpException ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        private string GetLegalActsXslt(FileUpload fu)
        {
            string ext = Path.GetExtension(fu.PostedFile.FileName);
            if (ext != ".xsl")
            {
                throw new Exception("Nieprawid³owy plik XSL.");
            }

            try
            {
                StreamReader xsltReader = new StreamReader(fu.FileContent);
                if (xsltReader != null)
                {
                    string xslt = xsltReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(xslt))
                    {
                        return xslt;
                    }
                }
            }
            catch (HttpException ex)
            {
                throw new Exception(ex.Message);
            }

            return null;
        }

        protected void lnkAddWordTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string wordTemplateFileName = hfWordFileName.Value; //string.Empty;
                    string wordTemplateOriginalFileName = lblWordOriginalFileName.Text; //string.Empty;

                    if (wordTemplate.HasFile)
                    {
                        wordTemplateOriginalFileName = Path.GetFileName(wordTemplate.PostedFile.FileName);
                        wordTemplateFileName = SaveFile(wordTemplate);
                    }

                    WordTemplateID = dao.SetTemplate(WordTemplateID, wordTemplateFileName, wordTemplateOriginalFileName, false, isWordTemplateActive.Checked);

                    LoadWordTemplate();
                    LoadSecureWordTemplate();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }


        }

        protected void lnkAddWordSecureTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string wordSecTemplateFileName = string.Empty;
                    string wordSecTemplateOriginalFileName = string.Empty;

                    if (wordSecureTemplate.HasFile)
                    {
                        wordSecTemplateOriginalFileName = Path.GetFileName(wordSecureTemplate.PostedFile.FileName);
                        wordSecTemplateFileName = SaveFile(wordSecureTemplate);
                    }

                    WordSecureTemplateID = dao.SetTemplate(WordSecureTemplateID, wordSecTemplateFileName, wordSecTemplateOriginalFileName, true, true);

                    LoadSecureWordTemplate();
                    LoadWordTemplate();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void lnkSaveLegalActsSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    if (fuLegalActsXslt.HasFile && fuLegalActsSchema.HasFile)
                    {
                        string schema = GetLegalActsSchema(fuLegalActsSchema);
                        string xslt = GetLegalActsXslt(fuLegalActsXslt);
                        
                        LegalActsSettingsDTO legalActsSettings = dao.GetLegalActsSettings();
                        if (legalActsSettings != null)
                            dao.SaveLegalActSettings(legalActsSettings.ID, schema, xslt);
                        else
                            dao.AddLegalActSettings("1.0", schema, xslt);
                        LoadLegalActsSettings();
                    }
                    else
                    {
                        lblMessage.Text = "W celu zapisania ustawieñ aktów prawnych nale¿y podaæ pliki XSD oraz XSL.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void lnkCurrentXslt_Click(object sender, EventArgs e)
        {
            LegalActsSettingsDTO legalActsSettings = dao.GetLegalActsSettings();
            if (legalActsSettings != null)
            {
                byte[] buff = Encoding.UTF8.GetBytes(legalActsSettings.Xslt);

                Response.ClearContent();
                Response.ClearHeaders(); 
                Response.AddHeader("Content-Length", buff.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=\"akt-prawny.xsl\"");
                Response.ContentType = "text/xml";
                Response.OutputStream.Write(buff, 0, buff.Length);
                Response.Flush();
                Response.Close();
            }
        }

        protected void lnkCurrentSchema_Click(object sender, EventArgs e)
        {
            LegalActsSettingsDTO legalActsSettings = dao.GetLegalActsSettings();
            if (legalActsSettings != null)
            {
                byte[] buff = Encoding.UTF8.GetBytes(legalActsSettings.Schema);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Length", buff.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=\"akt-prawny.xsd\"");
                Response.ContentType = "text/xml";
                Response.OutputStream.Write(buff, 0, buff.Length);
                Response.Flush();
                Response.Close();
            }
        }

        protected void lblSaveTicketDuration_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    new EsodaConfigParametersDAO().SetConfigParam("ticketDuration", ticketDuration.Text);
                    //EsodaConfigurationParameters.Refresh();
                    LoadTicketDuration();

                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }

        protected void lblSaveTicketLifeTime_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    new EsodaConfigParametersDAO().SetConfigParam("ticketLifeTime", ticketLifeTime.Text);
                    //EsodaConfigurationParameters.Refresh();
                    LoadTicketLifeTime();

                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }
    }
}
