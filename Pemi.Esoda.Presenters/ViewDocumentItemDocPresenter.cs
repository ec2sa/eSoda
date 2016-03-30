using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.Tasks;
using DocumentFormat.OpenXml.Packaging;
using System.Xml;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.IO;
using System.Data.SqlClient;
using Pemi.Esoda.Tools.MSOIntegrationHelper;
using System.Configuration;

namespace Pemi.Esoda.Presenters
{
    public class ViewDocumentItemDocPresenter : BasePresenter
    {
        private IViewDocumentItemDocView view;
        private IViewDocumentFormTask service;
        private ISessionProvider session;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDocumentFormDocPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="session">The session.</param>
        public ViewDocumentItemDocPresenter(IViewDocumentItemDocView view, ISessionProvider session)
        {
            this.view = view;
            this.session = session;
            this.service = new ViewDocumentFormTask();
            subscribeToEvents();
            (view as IView).ViewTitle = "Dokument Word";
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize()
        {

        }
        #endregion

        #region OnViewLoaded
        /// <summary>
        /// Called when [view loaded].
        /// </summary>
        public void OnViewLoaded()
        {
            try
            {
                if (view.DocumentID <= 0)
                {
                    view.ErrorMessage = "Nieprawidłowy identyfikator dokumentu.";
                    return;
                }

                if (view.Mode == Modes.NotDefined)
                {
                    view.ErrorMessage = "Nieprawidłowy identyfikator trybu.";
                    return;
                }
            }
            catch (Exception ex)
            {
                view.ErrorMessage = ex.Message;
            }
        }
        #endregion

        #region ProcessDocument
        /// <summary>
        /// Processes the document.
        /// </summary>
        public void ProcessDocument()
        {
            int index = -1;
            Dictionary<string, int> customXmlPartsIndexes;

            using (WordprocessingDocument document = WordprocessingDocument.Open(view.FileName, true))
            {
                MainDocumentPart mainPart = document.MainDocumentPart;

                XmlDocument eSodaData = new MSOIntegrationDAO().GetMSOServiceContextBin(view.DocumentID, EsodaConfigurationParameters.TicketDuration, string.IsNullOrEmpty(view.ItemGuid) ? null : ((Guid?)new Guid(view.ItemGuid)), view.Description, null);

                MSOProcessingHelper helper = new MSOProcessingHelper();

                //customXmlPartsIndexes = helper.GetCustomXmlPartsIndexes(mainPart, null);
                customXmlPartsIndexes = helper.GetCustomXmlPartsIndexes(mainPart, null, null);

                if (customXmlPartsIndexes.Keys.Contains<string>("context"))
                    index = customXmlPartsIndexes["context"];

                //helper.SetEsodaData(true, false, view.DocumentID, view.ItemGuid, view.Description, index, ref mainPart);
                helper.SetEsodaData(eSodaData, index, ref mainPart);
            }
        }
        #endregion

        #region GetTemplateFileName
        /// <summary>
        /// Gets the name of the template file.
        /// </summary>
        public string GetTemplateFileName(out string originalFileName)
        {
            string fileName = null;
            originalFileName = string.Empty;

            try
            {
                if (view.Mode == Modes.Create)
                {
                    MSOTemplateDTO msoTemplateDto = new MSOTemplateDAO().GetCurrentMSOTemplate(false);
                    if (msoTemplateDto != null)
                    {
                        fileName = msoTemplateDto.FileName;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            originalFileName = "Nowy dokument eSody" + Path.GetExtension(fileName);
                            fileName = Path.Combine(MSOProcessingHelper.MSODirectoryPath, fileName);
                        }
                    }
                }
                else if (view.Mode == Modes.Edit)
                {
                    if (string.IsNullOrEmpty(view.ItemGuid))
                    {
                        view.ErrorMessage = "Nie można odnaleźć elementu z powodu braku jego identyfikatora.";
                        return null;
                    }
                    DocumentItemDTO documentItemDto = new DocumentDAO().GetItem(new Guid(view.ItemGuid));
                    if (documentItemDto != null)
                        fileName = documentItemDto.FSGUID.ToString();

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileName = Path.Combine(ConfigurationManager.AppSettings["katalogDokumentow"], fileName);

                        if (!string.IsNullOrEmpty(documentItemDto.OriginalName))                           
                            originalFileName = documentItemDto.OriginalName;
                    }
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    view.ErrorMessage = "Nie udało się pobrać nazwy pliku template'u.";
                    return null;
                }

                if (!File.Exists(fileName))
                {
                    view.ErrorMessage = "Podany plik nie istnieje: " + fileName;
                    return null;
                }
            }
            catch (SqlException se)
            {
                view.ErrorMessage = "Wystąpił wyjątek podczas pobierania nazwy pliku template'u: " + se.Message;
            }

            return fileName;
        }
        #endregion

        #region BasePresenter members

        #region subscribeToEvents
        /// <summary>
        /// Subscribes to events.
        /// </summary>
        protected override void subscribeToEvents()
        {

        }
        #endregion

        #region redirectToPreviousView
        /// <summary>
        /// Redirects to previous view.
        /// </summary>
        protected override void redirectToPreviousView()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion
    }
}
