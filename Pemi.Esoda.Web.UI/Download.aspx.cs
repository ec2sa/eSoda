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

namespace Pemi.Esoda.Web.UI
{
    public partial class Download : System.Web.UI.Page, IDownloadFileView
    {
        private int pageNumber;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["pn"] != null)
            {
                int pn;
                if (!int.TryParse(Request["pn"].ToString(), out pn))
                    pageNumber = 0;
                else
                    pageNumber = pn;
            }
            Response.Clear();
            DownloadFilePresenter presenter = new DownloadFilePresenter(this);
            presenter.Initialize();
            Response.Flush();
        }

        #region IDownloadFileView Members

        string IDownloadFileView.MimeType
        {
            set { Response.ContentType = value; }
        }

        Guid IDownloadFileView.FileID
        {
            get
            {
                Guid id = Guid.Empty;
                if (Request["id"] == null) return id;
                try
                {
                    id = new Guid(Request["id"]);
                }
                catch
                {

                }
                return id;
            }
        }

        string IDownloadFileView.FileName
        {
            get
            {
                string fn = string.Empty;
                if (Request["fid"] == null) return fn;
                fn = Request["fid"];
                return fn;
            }
        }

        string IDownloadFileView.DownloadedFileName
        {
            set
            {
                Response.Charset = "UTF-8";
                if (Request.Browser.VBScript)
                    Response.AppendHeader("content-disposition", string.Format("attachment; filename=\"{0}\"", HttpUtility.UrlEncode(value).Replace("+", " ")));
                else
                    Response.AppendHeader("content-disposition", string.Format("attachment; filename=\"{0}\"", value));
            }
        }

        System.IO.Stream IDownloadFileView.ContentStream
        {
            get { return Response.OutputStream; }
        }

        #endregion

        #region IDownloadFileView Members


        public int PageNumber
        {
            get { return this.pageNumber; }
        }

        public bool ToPDF{
            get 
            {
                if (Request["p"] == null) 
                    return false;
                return Request["p"].Equals("1");
            }
        }

        #endregion
    }
}
