using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pemi.Esoda.Web.UI
{
    public partial class TestPreview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int docid;
            int.TryParse(Request.QueryString["id"].ToString(), out docid);
            ScanPreviewControl1.DocumentID = docid;
        }
    }
}
