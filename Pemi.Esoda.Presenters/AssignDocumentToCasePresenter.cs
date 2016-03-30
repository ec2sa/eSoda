using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Xml.XPath;
using System.IO;

namespace Pemi.Esoda.Presenters
{
	public class AssignDocumentToCasePresenter:BasePresenter
	{
		private IAssignDocumentToCaseView view;
		private ISessionProvider session;
		private IAssignDocumentToCaseTask service;
        
		private int documentId
		{
			get
			{
                return view.DocumentId;
                //int docId; 
                //if (session["idDokumentu"] != null && int.TryParse(session["idDokumentu"].ToString(), out docId))
                //    return docId;
                //return 0;
			}
		}

		public AssignDocumentToCasePresenter(IAssignDocumentToCaseView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new AssignDocumentToCaseTask();
			this.subscribeToEvents();
		}

		public override void Initialize()
		{
            view.AvailableBriefcasesYears = service.GetAvailableYears();

			view.BriefcaseList = service.GetBriefcaseList((view as IView).UserID, view.SelectedYear);
			//view.CaseKind = service.GetCaseKindList();
            
            string xmlData = service.GetDocumentData(documentId);
            
            if (session["nowaSprawa"] != null)
                view.IsNewCase = (bool)session["nowaSprawa"];

            if (xmlData == null || xmlData==string.Empty)
                return;

		

			try
			{
                XPathDocument xpd = new XPathDocument(new StringReader(xmlData));
                XPathNavigator xpn = xpd.CreateNavigator();

				
                view.DocumentSenderID = xpn.SelectSingleNode("/sprawa/interesant/@id").Value;
                view.DocumentSender = xpn.SelectSingleNode("/sprawa/interesant").Value;
                
				view.DocumentReferenceNumber = xpn.SelectSingleNode("/sprawa/znakPisma").Value;
                view.SelectedCaseDescription = xpn.SelectSingleNode("/sprawa/opis").Value;
				view.DocumentDate = xpn.SelectSingleNode("/sprawa/dataPisma").ValueAsDateTime;
                view.SelectedCaseKind = xpn.SelectSingleNode("/sprawa/idRodzajuSprawy").ValueAsInt;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Write("B³¹d przy pobieraniu danych z xml: " + ex.Message);
			}
            
		}

		protected override void subscribeToEvents()
		{
			view.CaseTypeSelected += new EventHandler(view_CaseTypeSelected);
			view.AssigningToSelectedCase+=new EventHandler(view_AssigningToSelectedCase);
			view.AssigningToNewCase += new EventHandler(view_AssigningToNewCase);
            view.YearChanged += new EventHandler(view_YearChanged);
		}

        void view_YearChanged(object sender, EventArgs e)
        {
            view.BriefcaseList = service.GetBriefcaseList((view as IView).UserID, view.SelectedYear);
        }

		void view_AssigningToNewCase(object sender, EventArgs e)
		{
			int caseId;
			caseId = service.AssignDocumentToNewCase((view as IView).UserID, documentId, view.SelectedBriefcase, view.SelectedCaseKind, view.SelectedCaseDescription, view.DocumentDate, view.DocumentReferenceNumber,view.DocumentSenderID);
			//session["idSprawy"] = caseId;
			view.CaseId = caseId;			
		}

		void view_AssigningToSelectedCase(object sender, EventArgs e)
		{
            service.AssignDocumentToCase((view as IView).UserID,documentId, view.SelectedCaseNumber);
            //session["idSprawy"] = view.SelectedCaseNumber; 
            view.CaseId = view.SelectedCaseNumber;
		}

		void view_CaseTypeSelected(object sender, EventArgs e)
		{
			view.CaseNumbers = service.GetCaseNumbersList(view.SelectedBriefcase);
            view.CaseKind = service.GetCaseKindsFromBriefcase(view.SelectedBriefcase);            
		}

		protected override void redirectToPreviousView()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}