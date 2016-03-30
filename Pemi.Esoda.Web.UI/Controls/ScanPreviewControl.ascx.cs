using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class ScanPreviewControl : System.Web.UI.UserControl
    {

        private int _pageNumber;

        public int PageNumber
        {
            get { int tmpcp;
            _pageNumber = 1;

            if (ViewState["currentPage"] != null && int.TryParse(ViewState["currentPage"].ToString(), out tmpcp))
                _pageNumber = tmpcp;
            currentPage.Text = _pageNumber.ToString();
                return _pageNumber; }
            set { _pageNumber = value;
            ViewState["currentPage"] = value;
            currentPage.Text = _pageNumber.ToString();
            }
        }

        private int _documentID;

        public int DocumentID
        {
            get { return _documentID; }
            set { _documentID = value; loadScanPreview(); }
        }

        public bool PageWasChanged { get; set; }

        public List<Guid> fsGuids = new List<Guid>();
        protected void Page_Load(object sender, EventArgs e)
        {

            PageWasChanged = false;
        }

        private void previewUnavailable()
        {
            imagePreview.ImageUrl = "~/App_themes/StandardLayout/img/noPreview.png";
            noPreviewLabel.Visible = true;
            btnNext.Enabled = false;
            btnPrev.Enabled = false;
        }

        private void loadScanPreview()
        {
            DocumentDAO dao = new DocumentDAO();
            Collection<DocumentItemDTO> items=dao.GetItems(DocumentID);
            if (items.Count == 0)
            {
                previewUnavailable();
                return;
            }
            IEnumerable<DocumentItemDTO> scans = items.OrderBy(item=>item.Browsable).SkipWhile(item=>item.Browsable==false);
         

            if (scans.Count()==0)
            {
                previewUnavailable();
                return;
            }

            foreach (DocumentItemDTO item in scans)
                fsGuids.Add(item.ID);

            ImageHelper helper = new ImageHelper(fsGuids[0]);
            helper.AktualnaSzerokosc =600;
            if (PageNumber > 0 && PageNumber <= helper.LiczbaStron)
                helper.AktualnaStrona = PageNumber;
            else
                if (PageNumber > helper.LiczbaStron)
                    PageNumber = helper.LiczbaStron;
            if (helper.LiczbaStron > 1)
            {
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
            }
            imagePreview.ImageUrl = helper.UrlObrazka+"?"+Guid.NewGuid().ToString();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            PageNumber = PageNumber + 1;
            loadScanPreview();
            PageWasChanged = true;
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (PageNumber > 1)
            {
                PageNumber = PageNumber - 1;
                loadScanPreview();
               
            }
            PageWasChanged = true;

        }
    }
}