using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.DTO;
using Pemi.Esoda.DataAccess;
using System.Data.SqlClient;

namespace Pemi.Esoda.Presenters
{
	public class ViewCaseSearchPresenter : BasePresenter
	{
		private IViewCaseSearchView view;
        private IViewCaseSearchTask service;
		private ISessionProvider session;

        private int totalRowCount;

        public ViewCaseSearchPresenter(IViewCaseSearchView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
            this.service = new ViewCaseSearchTask();
			subscribeToEvents();
			(view as IView).ViewTitle = "Moje dekretacje";
		}

		public override void Initialize()
		{
            try
            {
                //view.Clients = service.GetClients();
                view.Departments = service.GetDepartments();               
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
        
        public void OnViewLoaded()
        {
            try
            {
                                                                  
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

        public void OnSearch()
        {
            try
            {                
                view.SearchResults = service.GetCases(view.SearchConditions, out totalRowCount);
                view.PageCount = totalRowCount / view.PageSize + (totalRowCount % view.PageSize > 0 ? 1 : 0);             
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
            view.PageChanged += new EventHandler<PagerEventArgs>(view_PageChanged);
		}

        void view_PageChanged(object sender, PagerEventArgs e)
        {
            switch (e.EventType)
            {
                case PagerPage.GoToPage: view.CurrentPage = view.CurrentPage; break;
                case PagerPage.First: view.CurrentPage = 1; break;
                case PagerPage.Previous: if (view.CurrentPage > 1) view.CurrentPage -= 1; break;
                case PagerPage.Next: if (view.CurrentPage < view.PageCount) view.CurrentPage += 1; break;
                case PagerPage.Last: view.CurrentPage = view.PageCount; break;
            }
            OnSearch();
        }

        protected override void redirectToPreviousView()
        {
            throw new NotImplementedException();
        }      
    }    
}
