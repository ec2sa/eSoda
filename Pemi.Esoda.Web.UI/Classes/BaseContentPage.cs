using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Presenters;

namespace Pemi.Esoda.Web.UI
{
	public class BaseContentPage:Page,IView
	{
	
    public static void SetError(string message,string returnUrl){
      HttpContext.Current.Session["errorMessage"] = message;
      HttpContext.Current.Session["returnUrl"] = returnUrl;
      HttpContext.Current.Response.Redirect("~/shared/error2.aspx",true);
    }
		#region IView Members

		public string ErrorMessage
		{
			set
			{
        //HtmlGenericControl errorMessageBlock = Master.FindControl("errorMessageBlock") as HtmlGenericControl;
        //HtmlGenericControl errorMessage = Master.FindControl("errorMessage") as HtmlGenericControl;
        //if (errorMessageBlock == null || errorMessage==null) throw new ArgumentNullException("Nie uda³o siê odnaleŸæ kontrolki obs³ugi b³êdu");

        //errorMessage.InnerText = value;
        //errorMessageBlock.Visible = (value != string.Empty && value != null && value.Trim() != "");

        //ContentPlaceHolder c = Master.FindControl("main") as ContentPlaceHolder;
        //  if(c!=null)
        //    c.Visible=false;
			}
		}

		//public event EventHandler RedirectToPreviousView;

		public Guid UserID
		{
			get { return (Guid)Membership.GetUser().ProviderUserKey; }
		}

		public void RedirectToPreviousView()
		{
			
		}

		public string ViewTitle
		{
			set { Page.Title = value; }
		}

		#endregion
	}
}
