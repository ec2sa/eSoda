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
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using System.Xml;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Controls
{
	public partial class CaseContextItem : System.Web.UI.UserControl,IViewCaseView
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            ISessionProvider session = new WebSessionProvider();
            ViewCasePresenter presenter=new ViewCasePresenter(this, session);
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                    session["caseID"] = Request.QueryString["id"];
            }
                presenter.Initialize();
            

                      
		}

		#region IViewCaseView Members

		string IViewCaseView.CaseInfo
		{
			set {

                //XmlReader xr = (new CaseDAO()).GetCase(int.Parse(Session["idSprawy"].ToString()));
                XmlReader xr = (new CaseDAO()).GetCase(CoreObject.GetId(Request));
                if (xr.Read())
                {
                    XmlDataSource xds = new XmlDataSource();
                    xds.Data = xr.ReadOuterXml();
                    xds.EnableCaching = false;
                    opisZadania.DataSource = null;
                    if (xds.Data != "")
                    {
                        xds.XPath = "/sprawa";
                        opisZadania.DataSource = xds;
                    }
                    opisZadania.DataBind();
                }
			}
		}

		#endregion

		#region IViewCaseView Members


		int IViewCaseView.CaseId
		{
			get {
                //int idSprawy = 0;


                 /*if (Session["context"] == null) return 0;
                XPathDocument xpd = new XPathDocument(new StringReader(Session["context"].ToString()));
                XPathNavigator xpn = xpd.CreateNavigator();
                if (xpn.SelectSingleNode("/zadanie/typ").Value != "Sprawa") return 0;
                
                if (!int.TryParse(xpn.SelectSingleNode("/zadanie/id").Value, out idSprawy)) return 0;
                */



                //if (idSprawy != 0) Session["idSprawy"] = idSprawy;
                //else
                //    if(Session["idSprawy"] != null)
                //        int.TryParse(Session["idSprawy"].ToString(), out idSprawy);
                //return idSprawy;

                return CoreObject.GetId(Request);
			}
		}

        CaseDTO IViewCaseView.CaseData
        {
            set
            {
            }
        }

		public string CaseSignature
        {
            set { oznaczenieSprawy.InnerText = value; }
        }

        #endregion
    }
}