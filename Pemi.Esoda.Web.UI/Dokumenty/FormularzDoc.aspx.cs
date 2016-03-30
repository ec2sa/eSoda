using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System.Xml;
using System.IO;
using System.IO.Packaging;
using Pemi.Esoda.DataAccess;
using System.Text;
using Pemi.Esoda.Tools;
using DocumentFormat.OpenXml;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Presenters;
using Pemi.Esoda.Presenters.Interfaces;
using Pemi.Esoda.Tools.MSOIntegrationHelper;

namespace Pemi.Esoda.Web.UI.Dokumenty
{
    public partial class FormularzDoc : BaseContentPage, IViewDocumentFormDocView
    {
        ViewDocumentFormDocPresenter presenter;

        #region Page_Load
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new ViewDocumentFormDocPresenter(this, new WebSessionProvider());

            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                presenter.Initialize();
            }

            presenter.OnViewLoaded();

            try
            {
                if (CopyFile() && !string.IsNullOrEmpty(FileName))
                {
                    if (presenter.ProcessDocument())
                    {
                        DownloadFile();
                        DeleteFile();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Wystąpił wyjątek podczas przetwarzania pliku Word: " + ex.Message;
            }
        }
        #endregion

        #region lbtnBack_Click
        /// <summary>
        /// Handles the Click event of the lbtnBack control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dokumenty/FormularzWidokGlowny.aspx?id=" + DocumentID);
        }
        #endregion

        #region CopyFile
        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <returns></returns>
        private bool CopyFile()
        {
            string fileName = presenter.GetTemplateFileName();
            string tempFileName = Server.MapPath(ConfigurationManager.AppSettings["katalogRoboczy"] + "\\" + Guid.NewGuid().ToString() + Path.GetExtension(fileName));

            if (!string.IsNullOrEmpty(fileName))
            {
                try
                {
                    File.Copy(fileName, tempFileName);
                    if (string.IsNullOrEmpty(tempFileName) || !File.Exists(tempFileName))
                    {
                        ErrorMessage = "Błąd kopiowania pliku - podany plik nie istnieje: " + tempFileName;
                        return false;
                    }

                    FileName = tempFileName;
                    return true;
                }
                catch (IOException ex)
                {
                    ErrorMessage = "Nastąpił wyjątek podczas kopiowania pliku: " + ex.Message;
                    return false;
                }
            }

            return false;
        }
        #endregion

        #region DeleteFile
        /// <summary>
        /// Deletes the file.
        /// </summary>
        private void DeleteFile()
        {
            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
            }
            catch (IOException ioe)
            {
                lblMessage.Text = "Błąd usuwania pliku tymczasowego: " + ioe.Message;
            }
        }
        #endregion

        #region DownloadFile
        /// <summary>
        /// Downloads the file.
        /// </summary>
        private void DownloadFile()
        {
            try
            {
                string extension = Path.GetExtension(FileName);
                byte[] buff = File.ReadAllBytes(FileName);

                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Length", buff.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachment; filename=\"Formularz Dokumentu " + DocumentID + extension + "\"");
                Response.ContentType = "application/vnd.ms-word";
                Response.OutputStream.Write(buff, 0, buff.Length);
                Response.Flush();
                Response.Close();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Błąd ściągania pliku: " + ex.Message;
            }
        }
        #endregion

        #region IViewDocumentFormDocView members

        #region DocumentID
        /// <summary>
        /// Gets the document ID.
        /// </summary>
        /// <value>The document ID.</value>
        public int DocumentID
        {
            get { return CoreObject.GetId(Request); }
        }
        #endregion

        #region Mode
        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public Modes Mode
        {
            get
            {
                string mode = Request.QueryString["mode"];
                Modes m = Modes.NotDefined;
                if (mode != null)
                {
                    switch (mode)
                    {
                        case "c":
                            m = Modes.Create;
                            break;
                        case "e":
                            m = Modes.Edit;
                            break;
                        default:
                            m = Modes.NotDefined;
                            break;
                    }
                }
                return m;
            }
        }
        #endregion

        #region FileName
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }
        #endregion

        #region WithSchema
        /// <summary>
        /// Gets or sets a value indicating whether [with schema].
        /// </summary>
        /// <value><c>true</c> if [with schema]; otherwise, <c>false</c>.</value>
        public bool WithSchema { get; set; }
        #endregion

        #region ErrorMessage
        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage
        {
            set { lblMessage.Text = value; }
        }
        #endregion

        #endregion
    }
}
