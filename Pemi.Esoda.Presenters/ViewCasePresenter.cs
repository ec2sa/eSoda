using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
	public class ViewCasePresenter:BasePresenter
	{
		private IViewCaseView view;
		private ISessionProvider session;
		private IViewCaseTask service;
		public ViewCasePresenter(IViewCaseView view,ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new ViewCaseTask();
		}

		public override void Initialize()
		{
			int caseId;
            if (view.CaseId > 0)
            {
                caseId = view.CaseId;
                view.CaseInfo = service.GetCaseInfo(caseId);
                view.CaseData = service.GetCaseData(caseId);
                view.CaseSignature = service.GetCaseSignature(caseId);
                return;
            }
        //    if (session["idSprawy"] != null && int.TryParse(session["idSprawy"].ToString(), out caseId))
        //    {
        //        view.CaseInfo = service.GetCaseInfo(caseId);
        //        view.CaseData = service.GetCaseData(caseId);
        //        view.CaseSignature = service.GetCaseSignature(caseId);
        //    }
        //    else
        //        if (session["caseID"] != null && int.TryParse(session["caseID"].ToString(), out caseId))
        //        {
        //            session["idSprawy"] = caseId;
        //            session["caseID"] = null;
        //            view.CaseInfo = service.GetCaseInfo(caseId);
        //            view.CaseData = service.GetCaseData(caseId);
        //            view.CaseSignature = service.GetCaseSignature(caseId);
        //        }
        //        else
        //            throw new ArgumentException(string.Format("Nie ma takiej sprawy {0} {1}", session["iSPrawy"], session["caseID"] ));
        //
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
