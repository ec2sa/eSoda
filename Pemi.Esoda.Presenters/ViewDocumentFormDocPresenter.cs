using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Data.SqlClient;
using System.Xml;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools.MSOIntegrationHelper;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using Pemi.Esoda.DataAccess;
using System.Web;
using System.Linq;
using Pemi.Esoda.Tools;
using System.Collections;
using System.Xml.Schema;

namespace Pemi.Esoda.Presenters
{
    public class ViewDocumentFormDocPresenter : BasePresenter
    {
        private IViewDocumentFormDocView view;
        private IViewDocumentFormTask service;
        private ISessionProvider session;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewDocumentFormDocPresenter"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="session">The session.</param>
        public ViewDocumentFormDocPresenter(IViewDocumentFormDocView view, ISessionProvider session)
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
        public bool ProcessDocument()
        {
            try
            {
                using (WordprocessingDocument document = WordprocessingDocument.Open(view.FileName, true))
                {
                    MainDocumentPart mainPart = document.MainDocumentPart;

                    XmlDocument xmlData = null;
                    XmlDocument eSodaData = new MSOIntegrationDAO().GetMSOServiceContext(view.DocumentID, EsodaConfigurationParameters.TicketDuration, view.WithSchema);
                   
                    int index = -1;
                    string storeItemID = null;
                    Dictionary<string, int> customXmlPartsIndexes;

                    MSOProcessingHelper processingHelper = new MSOProcessingHelper();
                    MSOGenerationHelper generationHelper = new MSOGenerationHelper();
                    List<CustomXmlElement> customXmlElements = generationHelper.BuildCustomXmlStructure(mainPart);

                    storeItemID = generationHelper.GetStoreItemID(customXmlElements);

                    //customXmlPartsIndexes = processingHelper.GetCustomXmlPartsIndexes(mainPart, storeItemID);
                    customXmlPartsIndexes = processingHelper.GetCustomXmlPartsIndexes(mainPart, storeItemID, processingHelper.GetTargetNamespaces(eSodaData));

                    if (customXmlPartsIndexes.Keys.Contains<string>("xmlData"))
                        index = customXmlPartsIndexes["xmlData"];

                    xmlData = processingHelper.SetDocumentData(view.Mode, view.DocumentID, index, out storeItemID, ref mainPart);

                    if (xmlData != null)
                    {
                        XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlData.NameTable);
                        int prefixIndex = 0;

                        generationHelper.SetNamespaces(xmlData.DocumentElement, ref prefixIndex, ref nsManager);

                        if (customXmlElements.Count > 0)
                        {
                            for (int i = 0; i < customXmlElements.Count; ++i)
                            {
                                CustomXmlElement customXmlElement = customXmlElements[i];
                                generationHelper.GenerateAttributes(xmlData.DocumentElement, nsManager, ref customXmlElement);
                                generationHelper.GenerateDataBindings(xmlData.DocumentElement, nsManager, storeItemID, ref customXmlElement);
                                //generationHelper.GenerateCustomControls(xmlData.DocumentElement, nsManager, storeItemID, ref customXmlElement);
                            }
                        }
                        #region For future releases
                        //else
                        //{
                        //    OpenXmlElement customXmlBlock = new CustomXmlBlock();
                        //    ((CustomXmlBlock)customXmlBlock).Element = new StringValue(xmlData.DocumentElement.LocalName);
                        //    if (mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Count() > 0)
                        //    {
                        //        mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last().AppendChild(customXmlBlock);
                        //    }
                        //    else
                        //    {
                        //        mainPart.Document.Body.PrependChild(customXmlBlock);
                        //    }
                        //    generationHelper.GenerateCustomXmlBlocks(xmlData.DocumentElement, null, nsManager, ref  customXmlBlock);
                        //}
                        #endregion

                        mainPart.Document.Save();
                    }

                    index = -1;

                    if (customXmlPartsIndexes.Keys.Contains<string>("context"))
                    {
                        index = customXmlPartsIndexes["context"];
                    }

                    //processingHelper.SetEsodaData(false, view.WithSchema, view.DocumentID, null, null, index, ref mainPart);
                    processingHelper.SetEsodaData(eSodaData, index, ref mainPart);
                }
            }
            catch (Exception e)
            {
                view.ErrorMessage = "Błąd przetwarzania dokumentu Word: " + e.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region GetTemplateFileName
        /// <summary>
        /// Gets the name of the template file.
        /// </summary>
        public string GetTemplateFileName()
        {
            string fileName = null;
            view.WithSchema = true;

            try
            {
                if (view.Mode == Modes.Create)
                {
                    fileName = new MSOIntegrationDAO().GetMSOTemplateFileName(view.DocumentID);
                }
                else if (view.Mode == Modes.Edit)
                {
                    MSOIntegrationDAO msoIntegrationDao = new MSOIntegrationDAO();
                    fileName = msoIntegrationDao.GetMSOFileName(view.DocumentID);
                    if (string.IsNullOrEmpty(fileName))
                        fileName = msoIntegrationDao.GetMSOTemplateFileName(view.DocumentID);
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    MSOTemplateDTO msoTemplateDto = new MSOTemplateDAO().GetCurrentMSOTemplate(true);
                    if (msoTemplateDto != null)
                        fileName = msoTemplateDto.FileName;
                    view.WithSchema = false;
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    view.ErrorMessage = "Nie udało się pobrać nazwy pliku template'u.";
                    view.WithSchema = false;
                    return null;
                }

                fileName = Path.Combine(MSOProcessingHelper.MSODirectoryPath, fileName);

                if (!File.Exists(fileName))
                {
                    view.ErrorMessage = "Podany plik nie istnieje: " + fileName;
                    view.WithSchema = false;
                    return null;
                }
            }
            catch (SqlException se)
            {
                view.ErrorMessage = "Wystąpił wyjątek podczas pobierania nazwy pliku template'u: " + se.Message;
                view.WithSchema = false;
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
