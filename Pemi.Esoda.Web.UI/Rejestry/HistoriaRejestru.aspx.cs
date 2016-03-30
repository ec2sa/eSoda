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
using System.Collections.Generic;
using System.Xml.XPath;
using System.Data.SqlClient;

namespace Pemi.Esoda.Web.UI.Rejestry
{
    public partial class HistoriaRejestru : BaseContentPage
    {
        private int regItemID;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                ViewState["ReferrerUrl"] = Request.UrlReferrer.ToString();

                if (int.TryParse(Page.Request.QueryString["id"], out regItemID))
                {
                    try
                    {
                        XslCompiledTransform xslTransform = new XslCompiledTransform();

                        xslTransform.Load(Server.MapPath("~/xslt/registryView.xslt"));

                        XmlReader xr = new RegistryDAO().GetRegistryItemHistory(regItemID, (Guid)Membership.GetUser().ProviderUserKey);

                        XsltArgumentList args = new XsltArgumentList();
                        args.AddParam("history", "", "true");
                        StringWriter sw = new StringWriter();
                        xslTransform.Transform(xr, args, sw);

                        regContent.InnerHtml = sw.ToString();
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.Text = ex.Message;
                        fsRegistryHistory.Visible = false;
                    }
                    catch (Exception)
                    {
                        lblMessage.Text = String.Format("Brak historii!");
                        fsRegistryHistory.Visible = false;
                    }
                }
            }            
        }

        protected void lblGoBack_Click(object sender, EventArgs e)
        {
            if (ViewState["ReferrerUrl"] != null)
            {
                Response.Redirect(ViewState["ReferrerUrl"].ToString());
            }
        }        
    }
}

