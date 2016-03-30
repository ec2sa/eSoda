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
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class AktualneZastepstwaPrzelacz : System.Web.UI.UserControl
    {
        private UserDAO uDAO = null;
        private Guid userGuid;

        protected void Page_Load(object sender, EventArgs e)
        {
            uDAO = new UserDAO();

            MembershipUser user = Membership.GetUser();

            if (user != null)
            {
                userGuid = new Guid(user.ProviderUserKey.ToString());
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (uDAO.IsAvailableCover(userGuid))
            {                
                availableCover.Visible = true;
            }
            else
            {
                availableCover.Visible = false;
            }
        }
    }
}