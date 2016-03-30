using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
	public class ViewDocumentPresenter:BasePresenter
	{
		private IViewDocumentView view;
		private IViewDocumentTask service;
		private ISessionProvider session;

		public ViewDocumentPresenter(IViewDocumentView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new ViewDocumentTask();
			subscribeToEvents();
			(view as IView).ViewTitle = "Informacje o dokumencie";
		}

		public override void Initialize()
		{
            //int id = 0;

            //if (session["idDokumentu"] != null && !int.TryParse(session["idDokumentu"].ToString(), out id))
            //    throw new Exception(string.Format("Brak dokumentu {0} {1}", session["idDokumentu"], id));

            //view.DocumentData = service.GetDocumentData(id);            
                
            view.DocumentData = service.GetDocumentData(view.DocumentId);
		}

		protected override void subscribeToEvents()
		{
			
		}

		protected override void redirectToPreviousView()
		{
		
		}

		public string ExecuteCommand(string commandName, string commandArgument)
		{
			string wynik = null;
			switch (commandName)
			{
				case "szczegoly":
					wynik = "Dokument";
					break;
				case "skany":
					wynik = "skladnikiDokumentu";
					break;
				case "historia":
					wynik = "historiaDokumentu";
					break;
				case "akcje":
					wynik = "akcjeDokumentu";
					break;
			}
			return wynik;
		}
	}
}
