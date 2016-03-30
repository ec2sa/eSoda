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

namespace Pemi.Esoda.Web.UI
{
    public partial class eSodaChatNewMessageNotify : System.Web.UI.Page
    {
        public string APath
        {
            get
            {
                return Page.Request.ApplicationPath == "/" ? "" : Page.Request.ApplicationPath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            toGuid.Value = Membership.GetUser().ProviderUserKey.ToString();
        }
    }
}
