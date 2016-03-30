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
using System.Xml;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Sprawy
{
    public partial class AkcjeSprawy : BaseContentPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "LinkButton4";
            if (CoreObject.GetId(Request) <= 0)
            {
                BaseContentPage.SetError("Sprawa o podanym numerze nie istnieje", "~/Akta/AktaSpraw.aspx");
            }
            if (!IsPostBack)
            {
                int caseId = CoreObject.GetId(Request);
                if (caseId > 0)
                {
                    if (!Page.User.IsInRole("Administratorzy") && !(new CaseDAO()).IsCaseVisibleForUser(caseId, new Guid(Membership.GetUser().ProviderUserKey.ToString())))
                    {
                        BaseContentPage.SetError("Nie masz uprawnieñ do tej sprawy", "~/OczekujaceZadania.aspx");
                    }
                }
                ActionDAO dao = new ActionDAO();
                string xmlData = string.Empty;
                try
                {
                    //using (XmlReader xr = dao.GetAvailableActions(int.Parse(Session["idSprawy"].ToString()), (Guid)Membership.GetUser().ProviderUserKey, ActionMask.Case,ActionType.CalledFromList))
                    using (XmlReader xr = dao.GetAvailableActions(CoreObject.GetId(Request), (Guid)Membership.GetUser().ProviderUserKey, ActionMask.Case, ActionType.CalledFromList))
                    {
                        if (xr.Read())
                            xmlData = xr.ReadOuterXml();
                    }

                    XmlDataSource xds = new XmlDataSource();
                    xds.Data = xmlData;
                    xds.EnableCaching = false;
                    lista.DataSource = null;
                    if (xds.Data != "")
                    {
                        xds.XPath = "/akcje/akcja";
                        lista.DataSource = xds;
                    }
                    lista.DataBind();
                }
                catch //(Exception ex)
                {
                    BaseContentPage.SetError("Nie uda³o siê odnaleŸæ sprawy", "~/Akta/AktaSpraw.aspx");
                }
            }
        }

        protected void wykonajAkcje(object sender, CommandEventArgs e)
        {
            int caseId = CoreObject.GetId(Request);
            switch (e.CommandArgument.ToString())
            {
                case "E2E8D217-1E83-4F5B-BA3A-31412D771BF1":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/NowyDokumentLogiczny.aspx", false);
                    break;
                case "05555FAA-A86A-40C1-9A69-6512276C7098":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/EdycjaSprawy.aspx?id="+caseId.ToString(), false);
                    break;
                case "1F00EEAE-D0DB-4267-A62A-07A95FDF8E25":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/PowiadomienieSMS.aspx?id=" + caseId.ToString(), false);
                    break;
                case "386D76E7-FAA0-4264-A722-BFB60ACCBD46":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Response.Redirect("~/Akcje/PrzekazanieSprawy.aspx?id=" + caseId.ToString(), false);
                    break;
                case "2820653C-06BA-4704-AE1C-47D667E9F352":
                    Session["szablon"] = e.CommandName;
                    Session["idAkcji"] = e.CommandArgument.ToString();
                    Session["idObiektu"] = Session["idSprawy"]; // TODO
                    Session["fromCase"] = true;
                    Session["fromDoc"] = null;
                    Response.Redirect("~/Akcje/SelectRegistry.aspx?id=" + caseId.ToString(), false);
                    break;
                case "AD9DEECC-96A0-4478-9891-90B1E96085E6":
                    Response.Redirect("~/Akcje/NowyDokumentLogiczny.aspx?idSprawy=" + caseId.ToString(), false);
                    break;
                
            }
        }
    }
}
