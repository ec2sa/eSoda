using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
    public class ViewCaseDocumentsPresenter:BasePresenter
    {
        private IViewCaseDocumentsView view;
        private ISessionProvider session;
        private IViewCaseDocumentsTask service;

        private int CaseId
        {
            get
            {
                //int caseId = 0;
                //if (!session.Contains("idSprawy")) return 0;
                //if(!int.TryParse(session["idSprawy"].ToString(),out caseId)) return 0;
                //return caseId;

                return view.CaseId;
            }
        }

        public ViewCaseDocumentsPresenter(IViewCaseDocumentsView view, ISessionProvider session)
        {
            this.view = view;
            this.session = session;
            this.service = new ViewCaseDocumentsTask();
            subscribeToEvents();
        }

        public override void Initialize()
        {
            try
            {
                view.Items = service.GetCaseDocuments(view.CaseId);
            }
            catch
            {
                view.NextView = "~/Akta/AktaSpraw.aspx";
            }
        }

        protected override void subscribeToEvents()
        {
            view.ViewDetails += new EventHandler<ExecutingCommandEventArgs>(view_ViewDetails);
        }

        void view_ViewDetails(object sender, ExecutingCommandEventArgs e)
        {
            if (e.CommandName == "dokument")
            {
                //session["idDokumentu"] = int.Parse(e.CommandArgument.ToString());
                view.NextView = "~/Dokumenty/Dokument.aspx?id="+ e.CommandArgument.ToString();
            }

            if (e.CommandName == "usunZeSprawy")
            {
                session["docToRemoveFromCase"] = e.CommandArgument;
                session["caseToRemoveFrom"] = CaseId;
                view.NextView = "~/Akcje/UsuniecieDokumentuZeSprawy.aspx";
            }
        }

        protected override void redirectToPreviousView()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
