using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;


namespace Pemi.Esoda.Tools
{
    public class WebMsgBox
    {
        public static void Show(Page page,string message)
        {   
              
            // with AJAX
          message = message.Replace("\'", @"\'");
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "webmsgbox", string.Format("alert('{0}');", message), true);
        }

        public static void ShowNoAjax(Page page, string message)
        {
            // wo AJAX
            message = message.Replace("\'", @"\'");
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "webmsgbox", string.Format("alert('{0}');",message), true);
        }
    }
}
