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

namespace Pemi.Esoda.Web.UI.Shared
{
  public partial class error2 : System.Web.UI.Page
  {
    protected string message
    {
      get
      {
          if (Session["errorMessage"] != null)
              return Session["errorMessage"].ToString();
          else
              return "(brak komunikatu o b³êdzie)";
      }
    }
    protected string returnUrl
    {
      get
      {
          return Session["returnUrl"]!=null?Session["returnUrl"].ToString():"~/";
      }
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
      errorMessage.InnerText = message;
    }

    protected void returnFromError_Click(object sender, EventArgs e)
    {
      string ru = returnUrl;
      Session.Remove("errorMessage");
      Session.Remove("returnUrl");
      Response.Redirect(ru);
    }
  }
}
