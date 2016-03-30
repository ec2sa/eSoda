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
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Text;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class SzczegolyAkcji : System.Web.UI.Page
    {
        private int actionID
        {
            get
            {
                if (Request.QueryString["id"] == null)
                    BaseContentPage.SetError("Niepoprawne wywo³anie. Nieokreœlone id akcji.", "~/oczekujaceZadania.aspx");
                return int.Parse(Request.QueryString["id"]);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ActionDAO dao = new ActionDAO();
                string[] data = dao.GetDataAndXslt(actionID);
                if (data[0] == string.Empty || data[1] == string.Empty)
                {
                    emptyData.Visible = true;
                    return;
                }

                xml.DocumentContent = data[1];
                XslTransform actionXslt = new XslTransform();
                actionXslt.Load(XmlReader.Create(new StringReader(data[0])));
                xml.Transform = actionXslt;

                //// compiled xslt transform
                //XslCompiledTransform actionXsltc = new XslCompiledTransform();
                //actionXsltc.Load(XmlReader.Create(new StringReader(data[0])));
                //XmlReader xr = XmlReader.Create(new StringReader(data[1]));
                //StringBuilder sb = new StringBuilder();
                //XmlWriter xw = XmlWriter.Create(sb);

                //actionXsltc.Transform(xr, xw);
                //xr.Close();
                //xw.Close();
                //xml.DocumentContent = sb.ToString();   
            }
            catch
            {
                BaseContentPage.SetError("Nie uda³o siê zobrazowaæ saczegó³ów akcji", "~/OczekujaceZadania.xslt");
            }
        }

        protected void returnToCaller(object sender, EventArgs e)
        {
            if (Session["returnFromActionDetails"] != null)
                Response.Redirect(Session["returnFromActionDetails"].ToString());
            else
                Response.Redirect("~/oczekujaceZadania.aspx");
        }
    }
}