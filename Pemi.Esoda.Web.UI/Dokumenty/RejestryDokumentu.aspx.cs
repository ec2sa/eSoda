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
using System.Data.Common;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Dokumenty
{
  public partial class RejestryDokumentu : BaseContentPage
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      Session["{A9369F29-4E10-48ab-9E52-B4D415CF402A}"] = "LinkButton5";
      if (!IsPostBack)
      {

          int docId = CoreObject.GetId(Request);
          if (docId > 0)
          {
              if (!Page.User.IsInRole("Administratorzy") && !(new DocumentDAO()).IsDocVisibleForUser(docId, new Guid(Membership.GetUser().ProviderUserKey.ToString())))
              {
                  BaseContentPage.SetError("Nie masz uprawnieñ do tego dokumentu", "~/OczekujaceZadania.aspx");
              }
          }

        DocumentDAO dao = new DocumentDAO();
        rejestry.DataSource = dao.GetDocumentRegistryItems(CoreObject.GetId(Request));
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
        bool isInvoiceRegistry = (e.Item.DataItem as DbDataRecord)["nazwa"].ToString() == "Rejestr faktur";
        if (isInvoiceRegistry)
            (e.Item.FindControl("registryLink")).Visible = false;

      
        XslTransform itemsXslt = new XslTransform();
        itemsXslt.Load(Server.MapPath("~/xslt/registryView.xslt"));
       //itemsXslt.Load(XmlReader.Create(new StringReader((e.Item.DataItem as DbDataRecord)["transformacjaWpisu"].ToString())));

       
        wt.Transform = itemsXslt;

      }
    }
  }
}
