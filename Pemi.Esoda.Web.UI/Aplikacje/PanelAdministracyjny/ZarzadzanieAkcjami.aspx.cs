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
using Pemi.Esoda.DataAccess;
using System.Collections.Generic;
using System.IO;
using Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny.ActionsTableAdapters;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
  public partial class ZarzadzanieAkcjami : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void uploadXslt(object sender, EventArgs e)
    {
      try
      {
        Guid actionId = new Guid(actions.SelectedDataKey[0].ToString());
        if (!uploadControl.HasFile) return;
        using (TextReader tr = new StreamReader(uploadControl.PostedFile.InputStream))
        {
          string xslt = tr.ReadToEnd();
          ListaAkcjiDAO dao = new ListaAkcjiDAO();
          dao.Update(actionId, xslt);
          mv1.ActiveViewIndex = 2;
        }
      }
      catch
      {
        mv1.ActiveViewIndex = 3;
      }
     
    }

    protected void actions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
      if (e.CommandName == "exportXslt")
      {
       
        ActionDAO dao = new ActionDAO();
        string xslt=dao.GetXslt(new Guid(e.CommandArgument.ToString()));


        Response.Clear();
        Response.ContentType = "text/xml";
        Response.AddHeader("Content-Disposition", "attachment; filename=szablon.xslt");
       
        Response.Write(xslt);
        Response.Flush();
        Response.Close();
        
        return;
      }
      mv1.ActiveViewIndex = 1;
    }

    protected void powrotDoListy(object sender, EventArgs e)
    {
      mv1.ActiveViewIndex = 0;
    }
  }
}
