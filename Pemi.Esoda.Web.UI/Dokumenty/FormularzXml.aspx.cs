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
using System.IO;

namespace Pemi.Esoda.Web.UI
{
    public partial class FormularzXml : BaseContentPage, IViewDocumentFormXmlView
    {
        private ViewDocumentFormXmlPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new ViewDocumentFormXmlPresenter(this, new WebSessionProvider());

            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                presenter.Initialize();
            }
            presenter.OnViewLoaded();

            if (customForm != null)
            {
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xml", "formularz"));//Path.GetFileNameWithoutExtension(customForm.OriginalFilename)
                Response.ContentType = "text/xml";
                Response.Write(customForm.XmlData);
                Response.Flush();
                Response.Close();
            }                        
        }

        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Dokumenty/FormularzWidokGlowny.aspx?id=" + DocumentId);
        }

        #region IViewDocumentFormXmlView Members

        public int DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

        public string Message
        {
            set { lblMessage.Text = value; }
        }
        private CustomFormDTO customForm;
        
        public CustomFormDTO CustomForm
        {
            set { customForm = value; }
        }

        #endregion
    }
}
