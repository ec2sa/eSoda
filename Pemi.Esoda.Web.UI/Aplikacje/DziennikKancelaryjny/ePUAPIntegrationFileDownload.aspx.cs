using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Pemi.Esoda.DTO;
using System.Text;

namespace Pemi.Esoda.Web.UI.Aplikacje.DziennikKancelaryjny
{
    public partial class ePUAPIntegrationFileDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["dcontent"] != null)
                {
                    ePUAPIntegrationDTO document = (ePUAPIntegrationDTO)Session["dcontent"];
                    if (document != null)
                    {
                        string content = Encoding.UTF8.GetString(document.DocumentContent);
                        try
                        {
                            XmlDocument xmlContent = new XmlDocument();
                            xmlContent.LoadXml(content);
                            content = xmlContent.OuterXml;
                        }
                        catch (XmlException)
                        {
                        }
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", document.DocumentName));
                        Response.ContentType = "text/xml";
                        Response.Write(content);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Response.Flush();
                Response.Close();
            }
        }
    }
}
