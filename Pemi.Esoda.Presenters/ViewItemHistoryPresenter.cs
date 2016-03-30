using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
 public	class ViewItemHistoryPresenter:BasePresenter
	{
	 private IViewItemHistoryView view;
	 private IViewItemHistoryTask service;
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

	 public ViewItemHistoryPresenter(IViewItemHistoryView view, ISessionProvider session)
	 {
		 this.view = view;
		 this.session = session;
		 this.service = new ViewItemHistoryTask();
	 }

	 public override void Initialize()
	 {
		 view.HistoryItems = service.GetHistoryItems(registryId, itemId);
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
