using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.DataAccess;
using System.Data.Common;
using System.Xml.Xsl;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Sprawy
{
    public partial class RejestrySprawy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int caseId = CoreObject.GetId(Request);
                if (caseId > 0)
                {
                    if (!Page.User.IsInRole("Administratorzy") && !(new CaseDAO()).IsCaseVisibleForUser(caseId, new Guid(Membership.GetUser().ProviderUserKey.ToString())))
                    {
                        BaseContentPage.SetError("Nie masz uprawnień do tej sprawy", "~/OczekujaceZadania.aspx");
                    }
                }
               
                if(caseId <= 0)
                    BaseContentPage.SetError("Nie ma takiej sprawy", "~/OczekujaceZadania.aspx");
                CaseDAO cd = new CaseDAO();
                rejestry.DataSource = cd.GetCaseRegistryItems(caseId);
                rejestry.DataBind();
            }
        }

        protected void rejestry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Xml wt = (e.Item.FindControl("registryContent") as Xml);
                if (wt == null) return;
                wt.DocumentContent = (e.Item.DataItem as DbDataRecord)["zawartosc"].ToString();
                
                XslTransform itemsXslt = new XslTransform();
                itemsXslt.Load(Server.MapPath("~/xslt/registryView.xslt"));
                //itemsXslt.Load(XmlReader.Create(new StringReader((e.Item.DataItem as DbDataRecord)["transformacjaWpisu"].ToString())));
                
                wt.Transform = itemsXslt;
            }
        }
    }
}
