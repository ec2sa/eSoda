using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.DTO;
using System.Data.SqlClient;
using Pemi.eSoda.CustomForms;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Presenters
{
	public class ViewDocumentFormHistoryPresenter:BasePresenter
	{
		private IViewDocumentFormHistoryView view;
		private IViewDocumentFormTask service;
		private ISessionProvider session;

        public ViewDocumentFormHistoryPresenter(IViewDocumentFormHistoryView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new ViewDocumentFormTask();
			subscribeToEvents();
			(view as IView).ViewTitle = "Historia wpisów formularza";
		}

		public override void Initialize()
		{
            
		}

        private void GetHistoryList()
        {           
            view.HistoryList = service.GetCustomFormHistoryList(view.DocumentId);             
        }
        private void GetHistoryList(bool isLegalAct)
        {
            view.HistoryList = service.GetCustomFormHistoryList(view.DocumentId,isLegalAct);
        }

        public void OnViewLoaded()
        {
            OnViewLoaded(false);
        }

        public void OnViewLoaded(bool isLegalAct)
        {
            try
            {
                GetHistoryList(isLegalAct);                                                     
            }            
            catch (SqlException ex)
            {
                view.Message = ex.Message;
            }
            catch (Exception ex)
            {
                view.Message = ex.Message;
            }
        }

		protected override void subscribeToEvents()
		{
			
		}

        protected override void redirectToPreviousView()
        {
            throw new NotImplementedException();
        }

        public void LoadXmlData(int itemID,bool isLegalAct)
        {
            //view.HistoryData = service.GetCustomFormHistoryData(itemID);
            view.HistoryItem = service.GetCustomFormHistoryItem(itemID,isLegalAct);
        }
    }
}
