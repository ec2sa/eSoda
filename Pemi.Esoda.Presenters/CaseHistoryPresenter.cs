using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
    public class CaseHistoryPresenter:BasePresenter
	{
		private IViewCaseHistoryView view;
		private ISessionProvider session;
		private ICaseHistoryTask service;

        private int _caseId;

		public CaseHistoryPresenter(IViewCaseHistoryView view, ISessionProvider session)
		{
			this.view = view;
			this.service = new ViewCaseHistoryTask();
			this.session = session;
			(view as IView).ViewTitle = "Historia sprawy";
		}

		public override void Initialize()
		{
			//int documentId=0;
            //string idds = session["idSprawy"] != null ? session["idSprawy"].ToString() : string.Empty;
            //if(!int.TryParse(idds,out documentId))
            //    throw new ArgumentException(string.Format("Nie ma takiej sprawy {0} {1} {2}", session["idSprawy"], idds, documentId));
			
            view.HistoryItems = service.GetCaseHistory(view.CaseId);
			
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
