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
	public class ViewDocumentFormXmlPresenter:BasePresenter
	{
		private IViewDocumentFormXmlView view;
		private IViewDocumentFormTask service;
		private ISessionProvider session;        

        public ViewDocumentFormXmlPresenter(IViewDocumentFormXmlView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new ViewDocumentFormTask();
			subscribeToEvents();
			(view as IView).ViewTitle = "Widok XML formularza";
		}

		public override void Initialize()
		{
            
		}

        private void GetCustomFormData(bool isLegalAct)
        {
            view.CustomForm = service.GetCustomFormData(view.DocumentId,isLegalAct);                  
        }

        public void OnViewLoaded(){
            OnViewLoaded(false);
        }

        public void OnViewLoaded(bool isLegalAct)
        {
            try
            {
                GetCustomFormData(isLegalAct);                                                              
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
    }
}
