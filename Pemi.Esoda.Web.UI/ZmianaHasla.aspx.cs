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
using Pemi.Esoda.Web.UI.Controls;

namespace Pemi.Esoda.Web.UI
{
    public partial class ZmianaHasla : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pinEdit.AdminMode(false);
            pinEdit.EditMode(false);
            pinEdit.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        }

        protected void ChangePassword1_ChangingPassword(object sender, LoginCancelEventArgs e)
        {
            if (ConfigurationManager.AppSettings["codename"].ToLower().IndexOf("demo") > 0)
            {
                BaseContentPage.SetError("W tej wersji aplikacji has³o nie mo¿e byæ zmienione.", "~/OczekujaceZadania.aspx");
                e.Cancel = true;
            }              
        }
    }
}
