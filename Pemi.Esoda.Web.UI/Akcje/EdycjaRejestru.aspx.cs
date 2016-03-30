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
using System.Xml;
using Pemi.Esoda.DataAccess;
using System.Xml.XPath;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class EdycjaRejestru : System.Web.UI.Page
    {
        protected string ObjectType;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            

            if (!IsPostBack)
            {                
                int itemID;
                int registryId = 0;

                if (Request["itemId"] != null && int.TryParse(Request["itemId"].ToString(), out itemID))
                {
                    registryId = new RegistryDAO().GetRegistryIDByItem(itemID);
                }

                if (registryId == 0 && (Request["regid"] == null || !int.TryParse(Request["regid"].ToString(), out registryId)))
                    return;

                RegistryDAO dao = new RegistryDAO();

                if (!dao.IsInMainDepartment(registryId, (Guid)Membership.GetUser().ProviderUserKey))
                {
                    lblMessage.Text = "Brak uprawnień do edycji/zapisu.";
                    lblSave.Visible = false;
                    lblCancel.Visible = false;
                    fs.Visible = false;
                    return;     
                }

                using (IDataReader idr = dao.GetRegistryDefinitionByRegistryId(registryId))
                {
                    if (idr.Read())
                    {
                        Session["registryDefinition"] = idr["definicja"].ToString();
                    }
                }
            }
            generateFields();
          
        }

        private void generateFields()
        {
            string xmlItem = string.Empty;
            string objectType;
            int objectID;

            Dictionary<string, string> values = new Dictionary<string, string>();
            
            if (Session["registryDefinition"] == null) return;
            int idPozycji = 0;
            if (Request.QueryString["itemId"] != null && int.TryParse(Request.QueryString["itemId"].ToString(), out idPozycji))
            {
                xmlItem = new RegistryDAO().GetRegistryItem(idPozycji);
                new RegistryDAO().GetObjectIDRegistry(idPozycji, out objectType, out objectID);

                switch (objectType)
                {
                    case "DOC":
                        ObjectDetailsViewC.DocumentID = objectID;
                        ObjectType = "dokumentu";
                        ScanPreviewControl1.DocumentID = objectID;
                        break;
                    case "CASE":
                        ObjectDetailsViewC.CaseID = objectID;
                        ObjectType = "sprawy";
                        break;
                }

                if (!string.IsNullOrEmpty(xmlItem))
                {
                    XPathDocument xmld = new XPathDocument(new StringReader(xmlItem));
                    XPathNavigator xmln = xmld.CreateNavigator();
                    XPathNodeIterator xmli = xmln.Select("/wpis//*");

                    while (xmli.MoveNext())
                    {
                        values.Add(xmli.Current.Name, xmli.Current.Value);
                    }
                }
            }
            else
            {
                int regID=0;

                int.TryParse(Request["regid"].ToString(), out regID);

                RegistryDAO dao = new RegistryDAO();

                objectType = dao.GetRegistryType(regID);
                objectID = CoreObject.GetId(Request);

                switch (objectType)
                {
                    case "DOC":
                        ObjectDetailsViewC.DocumentID = objectID;
                        ObjectType = "dokumentu";
                        ScanPreviewControl1.DocumentID = objectID;
                        break;
                    case "CASE":
                        ObjectDetailsViewC.CaseID = objectID;
                        ObjectType = "sprawy";
                        break;
                }

            }
            XPathDocument xpd = new XPathDocument(new StringReader(Session["registryDefinition"].ToString()));
            XPathNavigator xpn = xpd.CreateNavigator();
            XPathNodeIterator xpni = xpn.Select("//pole");
            while (xpni.MoveNext())
            {
                bool isHeader = xpni.Current.SelectDescendants("pole", "", false).Count > 0;
                int level = xpni.Current.SelectAncestors("pole", "", false).Count+1;
                if(isHeader)
                    level-=1;
                if (level < 0) level = 0;
                string name = xpni.Current.SelectSingleNode("@nazwa").Value;
                string label = xpni.Current.SelectSingleNode("@etykieta").Value;


                if (isHeader)
                    fieldContainer.Controls.Add(RegistryHelper.CreateField(isHeader, "0", level, name, label,null,false,null));
                else
                {
                    string dataType = xpni.Current.SelectSingleNode("@typ").Value;
                    bool required = xpni.Current.SelectSingleNode("@wymagane").Value == "1" ? true : false;
                    string position = xpni.CurrentPosition.ToString();
                   
                    fieldContainer.Controls.Add(RegistryHelper.CreateField(isHeader, position, level, name, label, dataType, required,values.Count>0?values[name]:null));
                }
            }
        }

        protected void saveItem(object sender, EventArgs e)
        {
            if (Session["registryDefinition"] == null) return;
            XPathDocument xpd = new XPathDocument(new StringReader(Session["registryDefinition"].ToString()));
            XPathNavigator xpn = xpd.CreateNavigator();
            XPathNodeIterator xpni = xpn.Select("//pole[not(*)]");
            //int i = 1;
         
            List<string> values = new List<string>();

            foreach (Control c in fieldContainer.Controls)
            {

                if (c is Panel)
                {

                    foreach (Control c2 in c.Controls)
                    {
                        if (c2 is TextBox)
                        {
                            TextBox tb = c2 as TextBox;
                            values.Add(tb.Text);

                            break;
                        }
                    }
                }

            }

            int objectId=0;
            
            //if (Session["idObiektu"] == null || !int.TryParse(Session["idObiektu"].ToString(), out objectId))



            if (Request.QueryString["itemId"] != null)
                int.TryParse(Request.QueryString["itemId"].ToString(), out objectId);
            else
            {
                if (CoreObject.GetId(Request) <= 0)
                    BaseContentPage.SetError("Nie udało się zapisać pozycji rejestru - brak skojarzonego dokumentu lub sprawy", "~/OczekujaceZadania.aspx");
                objectId = CoreObject.GetId(Request);
            }

            int registryId=0;
            int itemID = 0;
            if (Request["itemId"] != null && int.TryParse(Request["itemId"].ToString(), out itemID))
                {
                    registryId = new RegistryDAO().GetRegistryIDByItem(itemID);
                    Session["idAkcji"] = "ED10E89A-365B-4034-9710-1E58BB93F5E9";
                }

            if (registryId == 0 && (Request["regid"] == null || !int.TryParse(Request["regid"].ToString(), out registryId)))
                BaseContentPage.SetError("Nie udało się zapisać pozycji rejestru - brak wybranego rejestru", "~/OczekujaceZadania.aspx");

             string itemContent=RegistryHelper.CreateRegistryItem(Session["registryDefinition"].ToString(), values);
             int pozId = (new RegistryDAO()).SaveRegistryItem((Guid)Membership.GetUser().ProviderUserKey, registryId,objectId,itemContent,itemID==0);

             List<string> parameters = new List<string>();
             using (DbDataReader dr = (DbDataReader)(new RegistryDAO()).GetRegistry(registryId))
             {
                 if (dr.Read())
                 {
                     parameters.Add(dr["nazwa"].ToString());
                     parameters.Add(objectId.ToString());
                     ActionLogger al = new ActionLogger(new ActionContext(new Guid(Session["idAkcji"].ToString()), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, parameters));
                     if (dr["wpisy"].ToString().Contains("doc"))
                         al.AppliesToDocuments.Add(objectId);
                     else
                         al.AppliesToCases.Add(objectId);

                     if (Request.QueryString["itemId"] != null)
                        al.ActionData.Add("WykonanaAkcja", "Edycja wpisu do rejestru");
                     else
                         al.ActionData.Add("WykonanaAkcja", "Dodanie wpisu do rejestru");
                     al.ActionData.Add("Data", DateTime.Now.ToString());
                     al.ActionData.Add("Nazwa", "Wpis do rejestru " + dr["nazwa"].ToString());
                     al.ActionData.Add("Uzytkownik", Membership.GetUser().UserName);
                     al.ActionData.Add("NazwaRejestru", dr["nazwa"].ToString());
                     al.ActionData.Add("NrPozycjiWRejestrze", pozId.ToString());
                     al.ActionData.Add("TypWpisu", (dr["wpisy"].ToString().Contains("doc")) ? "dokument" : "sprawa");
                     al.Execute();
                 }
             }

             Response.Redirect("~/Rejestry/ListaRejestrow.aspx?regid=" + registryId.ToString());
        }

        protected void lblCancel_Click(object sender, EventArgs e)
        {
            int regId = 0;
            
            if (String.IsNullOrEmpty(Page.Request["regId"].ToString()))
            {
                if (Session["fromDoc"] == null && Session["fromCase"] != null)
                {
                    Session["fromDoc"] = Session["fromCase"] = null;
                    Response.Redirect("~/Sprawy/Sprawa.aspx?id=" + CoreObject.GetId(Request).ToString()); // CHECK
                }
                if (Session["fromCase"] == null && Session["fromDoc"] != null)
                {
                    Response.Redirect("~/Dokumenty/Dokument.aspx?id=" + CoreObject.GetId(Request).ToString()); // CHECK
                    Session["fromDoc"] = Session["fromCase"] = null;
                }
                if (Session["fromDoc"] == null && Session["fromCase"] == null)
                {
                    Response.Redirect("~/OczekujaceZadania.aspx");
                }
            }
            else
            {
                if (int.TryParse(Page.Request["regId"].ToString(),out regId))
                    Response.Redirect("~/Rejestry/ListaRejestrow.aspx?regid=" + regId);
            }
        }
    }
}

