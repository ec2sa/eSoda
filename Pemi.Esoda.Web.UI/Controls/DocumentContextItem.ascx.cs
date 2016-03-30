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
using System.Xml.XPath;
using System.IO;
using Pemi.Esoda.DataAccess;
using System.Xml;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DTO;
using System.Text;

namespace Pemi.Esoda.Web.UI.Controls
{
	public partial class DocumentContextItem : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			//if (IsPostBack) return;

            /*XPathDocument xpd;
            XPathNavigator xpn;
            if (Session["context"] == null)
            {
                UserDAO ud = new UserDAO();
                xpd = new XPathDocument(new StringReader(ud.GetAssignedItems((Guid)Membership.GetUser().ProviderUserKey)));
                xpn = xpd.CreateNavigator();

                XPathNodeIterator xpni = xpn.Select(string.Format("/zadania/zadanie[id='{0}']", Session["idDokumentu"].ToString()));
                if (xpni.MoveNext())
                    Session["context"] = xpni.Current.OuterXml;
                xpd = new XPathDocument(new StringReader(Session["context"].ToString()));
            }
            else
                xpd = new XPathDocument(new StringReader(Session["context"].ToString()));
                     
            xpn = xpd.CreateNavigator();           
            
            if (xpn.SelectSingleNode("/zadanie/typ").Value!= "Dokument") return;*/



            //if (Session["idDokumentu"] != null)
            //{
            //    using (XmlReader xr = (new DocumentDAO()).GetDocumentDataXML(int.Parse(Session["idDokumentu"].ToString())))
            //    {
            //        if (xr.Read())
            //        {
            //            XmlDataSource xds = new XmlDataSource();
            //            xds.EnableCaching = false;
            //            xds.Data = xr.ReadOuterXml();
            //            xds.XPath = "/zadania/zadanie";

            //            opisZadania.DataSource = xds;
            //            opisZadania.DataBind();
            //        }
            //    }
            //}
            int docId = CoreObject.GetId(Request);
            if (docId > 0)
            {
                using (XmlReader xr = (new DocumentDAO()).GetDocumentDataXML(docId))
                {
                    if (xr.Read())
                    {
                        XmlDataSource xds = new XmlDataSource();
                        xds.EnableCaching = false;
                        xds.Data = xr.ReadOuterXml();
                        xds.XPath = "/zadania/zadanie";

                        opisZadania.DataSource = xds;
                        opisZadania.DataBind();
                    }
                }
            }
            ShowSearchCriteria();
            
		}

        private void ShowSearchCriteria()
        {
            if (Session["DocumentSearchCriteria"] != null)
            {
                if (Request.QueryString["c"] != null && Request.QueryString["c"].Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    ((SearchCriteriasState)Session["DocumentSearchCriteria"]).FoundInContent = true;
                if (Request.QueryString["d"] != null && Request.QueryString["d"].Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    ((SearchCriteriasState)Session["DocumentSearchCriteria"]).FoundInDescription = true;
                if (Request.QueryString["w"] != null)
                    ((SearchCriteriasState)Session["DocumentSearchCriteria"]).WhereInContent = Request.QueryString["w"];

                SearchCriteriasState state = (SearchCriteriasState)Session["DocumentSearchCriteria"];
                StringBuilder sb = new StringBuilder();
                sb.Append("<div style=\"border:solid 1px red;padding:3px;\">KRYTERIA:   ");
                if (!string.IsNullOrEmpty(state.Category))
                    sb.AppendFormat("<b>Kategoria dok.:</b> {0}, ", state.Category);
                if (!string.IsNullOrEmpty(state.Type))
                    sb.AppendFormat("<b>Rodzaj dok.:</b> {0}, ", state.Type);
                if (!string.IsNullOrEmpty(state.DocumentNumber))
                    sb.AppendFormat("<b>Nr z dz. kanc.:</b> {0}, ", state.DocumentNumber);
                if (!string.IsNullOrEmpty(state.SystemNumber))
                    sb.AppendFormat("<b>Nr systemowy:</b> {0}, ", state.SystemNumber);
                if (!string.IsNullOrEmpty(state.ClientName))
                    sb.AppendFormat("<b>Interesant:</b> {0}, ", state.ClientName);
                if (!string.IsNullOrEmpty(state.Mark))
                    sb.AppendFormat("<b>Znak pisma:</b> {0}, ", state.Mark);
                if (!string.IsNullOrEmpty(state.DocumentStartDate))
                    sb.AppendFormat("<b>Data od:</b> {0}, ", state.DocumentStartDate);
                if (!string.IsNullOrEmpty(state.DocumentEndDate))
                    sb.AppendFormat("<b>Data do:</b> {0}, ", state.DocumentEndDate);
                if (!string.IsNullOrEmpty(state.Status))
                    sb.AppendFormat("<b>Aktualny status:</b> {0}, ", state.Status);
                if (!string.IsNullOrEmpty(state.Text))
                    sb.AppendFormat("<b>Szukana fraza:</b> {0}, ", state.Text);
                if (state.FoundInDescription)
                    sb.AppendFormat("<b>Frazê wyszukano w opisie</b>, ");
                if (state.FoundInContent)
                    sb.AppendFormat("<b>Frazê wyszukano w skanach</b> {0}, ", state.WhereInContent.Replace("tak.","") );


                sb.Remove(sb.Length - 2, 2);//removes comma
                sb.Append("</div>");
                ltrSearchCriteria.Text = sb.ToString();
            }
        }
        
	}
}