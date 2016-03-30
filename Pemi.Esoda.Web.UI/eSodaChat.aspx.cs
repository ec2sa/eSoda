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
using System.Collections.Generic;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI
{
    public partial class eSodaChat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageTitle = String.Format("{0}", "Komunikator");

            if (!Page.IsPostBack)
            {
                if (Cache["MSG"] == null)
                {                    
                    Cache.Insert("MSG", new List<MessageCls>());
                }
               
                if (!String.IsNullOrEmpty(Page.Request["toguid"]))
                {
                    toGuid.Value = Page.Request["toguid"].ToString();
                    try
                    {
                        MembershipUser toUser = Membership.GetUser(new Guid(toGuid.Value));                       
                        pageTitle = String.Format("{0} - {1}", "Komunikator", toUser.UserName);
                    }
                    catch { }
                }
            }

            username.Value = Membership.GetUser().UserName;
            fromGuid.Value = Membership.GetUser().ProviderUserKey.ToString();

            this.Header.Title = pageTitle;
            
        }
    }
}
