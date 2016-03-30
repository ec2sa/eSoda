using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using Pemi.eSoda.CustomForms;
using System.Xml;

namespace Pemi.Esoda.Web.UI
{
    public partial class Formularz : BaseContentPage, IViewDocumentFormView
    {
        private ViewDocumentFormPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new ViewDocumentFormPresenter(this, new WebSessionProvider());

            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                presenter.Initialize();
            }
            presenter.OnViewLoaded();
            CustomFormWrapperControl1.ContentSaving += new EventHandler(CustomFormWrapperControl1_ContentSaving);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            presenter.OnXmlDataLoad();
        }
       
        void CustomFormWrapperControl1_ContentSaving(object sender, EventArgs e)
        {
            presenter.OnSaveFormData();            
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            CustomFormWrapperControl1.DisplayMode = CustomFormDisplayMode.Edit;
        }

        #region IViewDocumentFormView Members

        public int DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        public bool EditButtonVisible
        {
            set { CustomFormWrapperControl1.IsEditable = value; }
        }

        public string Message
        {
            set { lblMessage.Text = value; }
        }

        public string XmlData
        {
            get
            {
                return CustomFormWrapperControl1.GetFormContent().OuterXml;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(value);
                    CustomFormWrapperControl1.SetFormContent(doc);
                }
            }
        }

        #region todel
        //private string fileName;
        //public string FileName
        //{
        //    get
        //    {
        //        return fileName;
        //    }
        //    set
        //    {
        //        fileName = value;
        //    }
        //}

        //private string className;
        //public string ClassName
        //{
        //    get
        //    {
        //        return className;
        //    }
        //    set
        //    {
        //        className = value;
        //    }
        //}

        //private string originalFileName;
        //public string OriginalFileName
        //{
        //    get
        //    {
        //        return originalFileName.ToLower().Replace(".dll", "");
        //    }
        //    set
        //    {
        //        originalFileName = value;
        //    }
        //}
        #endregion

        public CustomFormDisplayMode CurrentMode
        {
            get
            {
                return CustomFormWrapperControl1.DisplayMode;
            }
            set
            {
                CustomFormWrapperControl1.DisplayMode = value;
            }
        }

        public void LoadFormWrapper(string assemblyName, string resourceName, string formHash)
        {
            
                CustomFormWrapperControl1.LoadContent(assemblyName, resourceName, formHash);
            
            

        }

        public bool IsFormWrapperContentValid
        {
            get
            {
                return CustomFormWrapperControl1.IsFormContentValid;
            }
        }

        public bool WarningVisible
        {
            set { lblWarning.Visible = value; }
        }

        #endregion

        #region IViewDocumentFormView Members


        public bool HideCustomFormWrapper
        {
            set { CustomFormWrapperControl1.Visible = !value; }
        }

        #endregion
    }
}
