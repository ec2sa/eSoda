using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
	public class RegistryItemDetailsPresenter:BasePresenter
	{
		IRegistryItemDetailsView view;
		IRegistryItemDetailsTask service;
		ISessionProvider session;

		private int itemId
		{
			get
			{
				int tmp;
				if (!session.Contains("itemId")) return 0;
				if (!int.TryParse(session["itemId"].ToString(), out tmp)) return 0;
				return tmp;
			}
		}

		private int registryId
		{
			get
			{
				int tmp;
				if (!session.Contains("registryId")) return 0;
				if (!int.TryParse(session["registryId"].ToString(), out tmp)) return 0;
				return tmp;
			}
		}

		public RegistryItemDetailsPresenter(IRegistryItemDetailsView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new RegistryItemDetailsTask();
		}

		public override void Initialize()
		{
			view.ItemContent = service.GetItem(itemId, registryId,view.IsInvoice);
			view.HistoryItems = service.GetItemHistory(registryId, itemId,view.IsInvoice);
			view.ItemID = itemId;
			((IView)view).ViewTitle = "Przegl¹danie szczegó³ów pozycji dziennika kancelaryjnego";

            view.IsDailyLogItemAccessDenied = service.IsGetDailyLogItemAccessDenied(registryId, itemId, ((IView)view).UserID);
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
