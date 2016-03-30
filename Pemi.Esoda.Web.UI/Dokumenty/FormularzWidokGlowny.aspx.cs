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
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using Pemi.eSoda.CustomForms;
using System.Xml;

namespace Pemi.Esoda.Web.UI
{
    public partial class FormularzWidokGlowny : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string mode = string.Empty;
            string id = string.Empty;
            string url = string.Empty;

            if (!String.IsNullOrEmpty(Page.Request["mode"]) && !String.IsNullOrEmpty(Page.Request["id"]))
            {
                mode = Page.Request["mode"].ToString();
                id = Page.Request["id"].ToString();
                
                switch (mode)
                {
                    case "xml":
                        url = String.Format("FormularzXml.aspx?id={0}", id);
                        break;                    
                    default:
                        url = String.Format("FormularzDoc.aspx?id={0}&mode={1}", id, mode);
                        break;
                }
                                       
                //if (!ClientScript.IsStartupScriptRegistered(this.GetType(), "scriptWord"))
                //{
                //    string script = @"window.location.href='" + url + "';";
                //    ClientScript.RegisterStartupScript(this.GetType(), "scriptWord", script, true);
                //}
                
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "1;url=" + url;
                Page.Header.Controls.Add(meta); 
            }
        }
    }
}
