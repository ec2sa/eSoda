using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Data.SqlClient;

namespace Pemi.Esoda.Presenters
{
    public class ViewDocumentSearchPresenter : BasePresenter
    {
        private IViewDocumentSearchView view;
        private IViewDocumentSearchTask service;
		private ISessionProvider session;

        private int totalRowCount;

        public ViewDocumentSearchPresenter(IViewDocumentSearchView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
            this.service = new ViewDocumentSearchTask();
			subscribeToEvents();
			//(view as IView).ViewTitle = "Moje dekretacje";
		}

		public override void Initialize()
		{
            try
            {
                view.Categories = service.GetCategories();
                view.Statuses = service.GetStatuses();

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

        public void OnCategoryChange(int categoryID)
        {
            try
            {
                view.Types = service.GetTypes(categoryID);
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
                if (!string.IsNullOrEmpty(view.SearchConditions.Text) && view.SearchConditions.SearchContent == false && view.SearchConditions.SearchDescription == false)
                    throw new Exception("Zaznacz gdzie wyszukać podaną frazę.");
                
                view.SearchResults = service.GetDocuments(view.SearchConditions, out totalRowCount);
                view.PageCount = totalRowCount / view.PageSize + (totalRowCount % view.PageSize > 0 ? 1 : 0);
                view.SaveSearchState();
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
