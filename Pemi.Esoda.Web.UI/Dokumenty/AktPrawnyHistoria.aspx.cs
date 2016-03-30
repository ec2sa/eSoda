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
    public partial class AktPrawnyHistoria : BaseContentPage, IViewDocumentFormHistoryView
    {
        private ViewDocumentFormHistoryPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new ViewDocumentFormHistoryPresenter(this, new WebSessionProvider());

            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                presenter.Initialize();
            }
            presenter.OnViewLoaded(true);           
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
               
        public System.Collections.Generic.IList<CustomFormHistoryItemDTO> HistoryList
        {
            set 
            {
                historyList.DataSource = value;
                historyList.DataBind();
            }
        }

        #endregion

        protected void historyList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int itemID;
            if (e.CommandName == "selectedItem")
            {
                itemID = int.Parse(e.CommandArgument.ToString());
                presenter.LoadXmlData(itemID,true);
            }

        }

        #region IViewDocumentFormHistoryView Members


        public string HistoryData
        {
            set
            {
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xml", "formularz"));
                Response.ContentType = "text/xml";
                Response.Write(value);
                Response.Flush();
                Response.Close();
            }
        }

       
        public CustomFormHistoryItemDTO HistoryItem
        {
            set
            {
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename=formularz_{0}.xml", value.Date.ToString()));
                Response.ContentType = "text/xml";
                Response.Write(value.XmlData);
                Response.Flush();
                Response.Close();
            }
        }

        #endregion
    }
}
