using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
	public class ViewItemScansPresenter:BasePresenter
	{
		private IViewItemScansView view;
		private IViewItemScansTask service;
		private ISessionProvider session;

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

		private void getCurrentRegistryItem()
		{
			view.ItemContent = service.GetRegistryItem(itemId, registryId,view.IsInvoice);
		}

		public ViewItemScansPresenter(IViewItemScansView view, ISessionProvider session)
		{
			this.view = view;
			this.service = new ViewItemScansTask();
			this.session = session;
			subscribeToEvents();
		}

		public override void Initialize()
		{
			session.Remove("{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}");
			getCurrentRegistryItem();
			view.ScanListItems = service.GetItemsScans(itemId, registryId,view.IsInvoice);
			view.ItemID = itemId;
			((IView)view).ViewTitle = "Skany skojarzone z pozycj¹ dziennika kancelaryjnego";

            view.IsDailyLogItemAccessDenied = service.IsGetDailyLogItemAccessDenied(registryId, itemId, ((IView)view).UserID);
		}

		protected override void subscribeToEvents()
		{
			
		}

		protected override void redirectToPreviousView()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void SelectScan(string scanGuid){
			session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"] = scanGuid;
			view.IsScanSelected = true;
			view.PreviewImage = string.Format("~/Image.aspx?id={0}&type=preview",scanGuid);
		}

		public void SaveChanges(string description,bool isMain)
		{
			if (session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"] == null) throw new ArgumentException("nie wybrano skanu!");
			service.SaveChanges(session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"].ToString(), description, isMain);
			view.IsScanSelected = false;
			this.Initialize();
		}

		public void ReleaseScan()
		{
			if (session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"] == null) throw new ArgumentException("nie wybrano skanu!");
			service.ReleaseScan(session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"].ToString());
			view.IsScanSelected = false;
			this.Initialize();
		}

		public void AddNewScan()
		{
			session["addingScanToItem"] = itemId;
			session["addingScanToRegistry"] = registryId;
			view.RedirectTo="addNewScan";
		}
	}
}
