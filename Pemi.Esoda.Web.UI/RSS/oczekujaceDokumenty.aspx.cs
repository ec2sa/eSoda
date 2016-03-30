using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Text;

namespace Pemi.Esoda.Web.UI.RSS
{
    public partial class oczekujaceDokumenty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid ticket;
            if (Request.QueryString["ticket"] == null)
                return;
            ticket = new Guid(Request.QueryString["ticket"].ToString());
            RSSDAO dao = new RSSDAO();
            using (XmlReader reader = dao.GetAwaitingDocuments(ticket))
            {
                if (reader.Read())
                {
                   
                    Response.ContentType = "text/xml";
                   
                    
                    string feed = reader.ReadOuterXml();
                    byte[] resp = Encoding.UTF8.GetBytes(feed);

                 
                        Response.ClearContent();
                    Response.OutputStream.Write(resp, 0, resp.Length);
                    Response.End();

                }
            }
        }
    }
}
