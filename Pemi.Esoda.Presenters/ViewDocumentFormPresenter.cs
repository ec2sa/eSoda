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
	public class ViewDocumentFormPresenter:BasePresenter
	{
		private IViewDocumentFormView view;
		private IViewDocumentFormTask service;
		private ISessionProvider session;

        private CustomFormDTO customForm = null;

		public ViewDocumentFormPresenter(IViewDocumentFormView view, ISessionProvider session)
		{
			this.view = view;
			this.session = session;
			this.service = new ViewDocumentFormTask();
			subscribeToEvents();
			(view as IView).ViewTitle = "Formularz dokumentu";
		}

		public override void Initialize()
		{
            
		}

        private void GetCustomFormData()
        {
            customForm = service.GetCustomFormData(view.DocumentId);
        }

        public void OnViewLoaded()
        {
            try
            {
                GetCustomFormData();
               
                view.LoadFormWrapper(customForm.OriginalFilename.ToLower().Replace(".dll", ""), customForm.ClassName, customForm.FormHash);
                               
            }
            catch (ArgumentException ex)
            {
                view.HideCustomFormWrapper = true;
                view.Message = ex.Message;
            }
            catch (SqlException exs)
            {
                view.Message = exs.Message;
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

        public void OnSaveFormData()
        {
            if (view.IsFormWrapperContentValid)
            {
                new CustomFormDAO().SetCustomFormData(view.DocumentId, customForm.DocumentTypeID, view.XmlData);
                view.CurrentMode = CustomFormDisplayMode.Browse;
            }
            else
            {
                view.Message = String.Format("{0}", "Formularz zawiera niepoprawne dane!");
            }
            
        }

        public void OnXmlDataLoad()
        {
            try
            {
                
                GetCustomFormData();
                
                view.XmlData = customForm.XmlData;

                view.EditButtonVisible = false;

                if (customForm.IsCFActive && !string.IsNullOrEmpty(customForm.XmlData))
                {
                    if (view.CurrentMode != CustomFormDisplayMode.Edit)
                        view.CurrentMode = CustomFormDisplayMode.Browse;
                    view.EditButtonVisible = true;
                    view.WarningVisible = (customForm.FormHash != customForm.DataHash);
                }
                else if (customForm.IsCFActive && string.IsNullOrEmpty(customForm.XmlData))
                    view.CurrentMode = CustomFormDisplayMode.Insert;
                else
                {
                    view.CurrentMode = CustomFormDisplayMode.Browse;
                    if (!string.IsNullOrEmpty(customForm.XmlData))
                    view.WarningVisible = (customForm.FormHash != customForm.DataHash);
                }
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
    }
}
