using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.DataAccess;
namespace Pemi.Esoda.Presenters
{
	public class RedirectDocumentPresenter:BasePresenter
	{
		private IRedirectDocumentView view;
		private IEditRegistryItemTask service;
		private ISessionProvider session;

		public RedirectDocumentPresenter(IRedirectDocumentView view, ISessionProvider session)
		{
			this.view = view;
			this.service = new EditRegistryItemTask();
			this.session = session;
		//	(view as IView).ViewTitle = "Przekierowywanie dokumentu";
			subscribeToEvents();
		}

		public override void Initialize()
		{
			view.OrganizationalUnits = service.GetOrganizationalUnits();
			view.Employees = service.GetEmployees(view.OrganizationalUnitId);
		}

		protected override void subscribeToEvents()
		{
			view.ActionExecuted += new EventHandler(view_ActionExecuted);
			view.OrganizationalUnitChanged += new EventHandler(view_OrganizationalUnitChanged);
		}

		void view_OrganizationalUnitChanged(object sender, EventArgs e)
		{
			view.Employees = service.GetEmployees(view.OrganizationalUnitId);
		}

		void view_ActionExecuted(object sender, EventArgs e)
		{
            //IRedirectDocumentTask srv = new RedirectDocumentTask();
            //srv.RedirectDocument(session["idAkcji"].ToString(), int.Parse(session["idDokumentu"].ToString()), view.UserId,view.UserName,view.UserFullName,view.WorkOnPaper,view.Note, view.OrganizationalUnitId, view.EmployeeId,view.OUName,view.EmpName);
            ////if (IsDocVisibleForUser(view.UserId, int.Parse(session["idDokumentu"].ToString())))
            //if (IsDocVisibleForUser(view.UserId, view.DocumentId))
            //    view.ReturnTo = "~/Dokumenty/HistoriaDokumentu";
            //else
            //    view.ReturnTo = "~/OczekujaceZadania";

            IRedirectDocumentTask srv = new RedirectDocumentTask();
            srv.RedirectDocument(session["idAkcji"].ToString(), view.DocumentId, view.UserId, view.UserName, view.UserFullName, view.WorkOnPaper, view.Note, view.OrganizationalUnitId, view.EmployeeId, view.OUName, view.EmpName);
            //if (IsDocVisibleForUser(view.UserId, int.Parse(session["idDokumentu"].ToString())))
            if (IsDocVisibleForUser(view.UserId, view.DocumentId))
                view.ReturnTo = "~/Dokumenty/HistoriaDokumentu.aspx?id="+view.DocumentId.ToString();
            else
                view.ReturnTo = "~/OczekujaceZadania.aspx";
		}

        private bool IsDocVisibleForUser(Guid userId, int docId)
        {
            return (new DocumentDAO()).IsDocVisibleForUser(docId, userId);
        }

		protected override void redirectToPreviousView()
		{
			
		}
	}
}
