using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Presenters
{
	public class ViewDocumentActionsPresenter:BasePresenter
	{
		private IViewDocumentActionsView view;
		private IViewDocumentActionsTask service;
		private ISessionProvider session;

		public ViewDocumentActionsPresenter(IViewDocumentActionsView view, ISessionProvider session)
		{
			this.view = view;
			this.service = new ViewDocumentActionsTask();
			this.session = session;
			((IView)view).ViewTitle = "Akcje dla dokumentu";
		}


		public override void Initialize()
		{
            //int documentId;
            //string idds = session["idDokumentu"] != null ? session["idDokumentu"].ToString() : string.Empty;
            //if (!int.TryParse(idds, out documentId))
            //    throw new ArgumentException(string.Format("Nie ma takiego dokumentu {0} {1} {2}", session["idDokumentu"], idds, documentId));
            //view.Items = service.GetAvailableActions(documentId, view.UserId,ActionType.CalledFromList);
            view.Items = service.GetAvailableActions(view.DocumentId, view.UserId, ActionType.CalledFromList);
			
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
