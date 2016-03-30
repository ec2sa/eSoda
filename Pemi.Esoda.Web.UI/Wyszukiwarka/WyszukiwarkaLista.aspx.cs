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
using System.Text.RegularExpressions;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI
{
    public partial class WyszukiwarkaLista : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            try
            {
                SearchDAO dao = new SearchDAO();
                myDecretaction.Visible = dao.isShowDecretactionLink(new Guid(Membership.GetUser().ProviderUserKey.ToString()));
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}
