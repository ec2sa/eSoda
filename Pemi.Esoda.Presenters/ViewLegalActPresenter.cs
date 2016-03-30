using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.Presenters.Interfaces;
using Pemi.Esoda.Tasks;
using DocumentFormat.OpenXml.Packaging;
using System.Xml;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools.MSOIntegrationHelper;
using Pemi.Esoda.DTO;
using System.Data.SqlClient;
using System.IO;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Presenters
{
    public class ViewLegalActPresenter : BasePresenter
    {
        private IViewLegalActView view;
        private IViewDocumentFormTask service;
        private ISessionProvider session;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDocumentFormDocPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="session">The session.</param>
        public ViewLegalActPresenter(IViewLegalActView view, ISessionProvider session)
        {
            this.view = view;
            this.session = session;
            this.service = new ViewDocumentFormTask();
            subscribeToEvents();
            (view as IView).ViewTitle = "Akt Prawny";
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

                XmlDocument eSodaData = new MSOIntegrationDAO().GetMSOServiceContextLAW(view.DocumentID, EsodaConfigurationParameters.TicketDuration); //?

                MSOProcessingHelper helper = new MSOProcessingHelper();

                customXmlPartsIndexes = helper.GetCustomXmlPartsIndexes(mainPart, null, null);

                if (customXmlPartsIndexes.Keys.Contains<string>("context"))
                    index = customXmlPartsIndexes["context"];

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
                MSOTemplateDTO msoTemplateDto = new MSOTemplateDAO().GetLAWTemplate(view.DocumentID);
                if (msoTemplateDto != null)
                {
                    fileName = msoTemplateDto.FileName;
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        originalFileName = "Akt prawny" + Path.GetExtension(fileName);
                        fileName = Path.Combine(MSOProcessingHelper.MSODirectoryPath, fileName);
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
