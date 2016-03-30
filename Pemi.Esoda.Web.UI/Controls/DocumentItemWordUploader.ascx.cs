using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Pemi.Esoda.Tools;


namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class DocumentItemWordUploader : System.Web.UI.UserControl
    {
        public event EventHandler CancelUpload;

        public void OnCancelUpload(EventArgs e)
        {
            if (CancelUpload != null)
                CancelUpload(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lblCancel_Click(object sender, EventArgs e)
        {
            txtDescription.Text = string.Empty;
            OnCancelUpload(new EventArgs());
        }

        protected void lblOpenFile_Click(object sender, EventArgs e)
        {            
            Session.Add("WordDescription", txtDescription.Text);
            txtDescription.Text = string.Empty;

            Response.Redirect(String.Format("~/Dokumenty/SkladnikiDokumentu.aspx?id={0}&mode={1}", CoreObject.GetId(Request).ToString(), "c"));//c - przy nowym

            //Response.Redirect(String.Format("~/Dokumenty/SkladnikDoc.aspx?id={0}&mode={1}", CoreObject.GetId(Request), "c"));//c - przy nowym
        }
    }
}