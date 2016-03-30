using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.Tasks;
using Pemi.Esoda.Presenters.Interfaces;
using System.Data.SqlClient;
using System.Web.Security;

namespace Pemi.Esoda.Presenters
{
    public class ViewMyDecretationsSearchPresenter : BasePresenter
    {
        private IViewMyDecretationsView view;
        private IViewMyDecretationsTask service;
		private ISessionProvider session;

        private int totalRowCount;

        public ViewMyDecretationsSearchPresenter(IViewMyDecretationsView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
            this.service = new ViewMyDecretationsTask();
			subscribeToEvents();
			(view as IView).ViewTitle = "Wyszukiwarka spraw";
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
                view.SearchResults = service.GetDecretations(view.SeachConditions, out totalRowCount, Membership.GetUser().ProviderUserKey);
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
