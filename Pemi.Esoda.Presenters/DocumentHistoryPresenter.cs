using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
	public class DocumentHistoryPresenter:BasePresenter
	{
		private IViewDocumentHistoryView view;
		private ISessionProvider session;
		private IDocumentHistoryTask service;


		public DocumentHistoryPresenter(IViewDocumentHistoryView view, ISessionProvider session)
		{
			this.view = view;
			this.service = new ViewDocumentHistoryTask();
			this.session = session;
			(view as IView).ViewTitle = "Historia dokumentu";
		}

		public override void Initialize()
		{
            //int documentId;
            //string idds = session["idDokumentu"] != null ? session["idDokumentu"].ToString() : string.Empty;
            //if(!int.TryParse(idds,out documentId))
            //    throw new ArgumentException(string.Format("Nie ma takiego dokumentu {0} {1} {2}", session["idDokumentu"], idds, documentId));
            //view.HistoryItems = service.GetDocumentHistory(documentId);
          
            view.HistoryItems = service.GetDocumentHistory(view.DocumentId);
			
		}

		protected override void subscribeToEvents()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		protected override void redirectToPreviousView()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
