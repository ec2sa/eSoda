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
using Pemi.Esoda.Tools;
using System.Xml;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Collections.Generic;
using Pemi.Esoda.Presenters;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class edycjaDokumentu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (Session["idDokumentu"] != null)
                //{
                //    int docId;
                //    if (!int.TryParse(Session["idDokumentu"].ToString(), out docId))
                //    {
                //        BaseContentPage.SetError("nie wybrano dokumentu do edycji", "~/oczekujaceZadania.aspx");
                //        return;
                //    }
                //    loadDocumentData(docId);
                //}
                int docId = CoreObject.GetId(Request);
                if (docId >0)
                {
                    loadDocumentData(docId);
                    DisableCategoryAndType(docId);
                }
                else
                {
                    BaseContentPage.SetError("nie wybrano dokumentu do edycji", "~/oczekujaceZadania.aspx");
                    return;
                }
            }

            if (customer != null)
            {
                customer.SearchCustomers += new EventHandler<SearchCustomersEventArgs>(customersList.FindCustomers);
                customer.SearchCustomers += new EventHandler<SearchCustomersEventArgs>(customer_SearchCustomers);
                customer.SearchListVisible += new EventHandler<SearchCustomersEventArgs>(customersList.SearchListVisible);
                customer.OnCustomerAdded += new EventHandler(customer_OnCustomerAdded);
                customersList.AddNewCustomer += new EventHandler(customer.AddNewCustomer);
                customersList.EditCustomer += new EventHandler(customer.EditCustomer);
                customersList.SelectCustomer += new EventHandler(customer.SelectCustomer);
                customersList.SelectCustomer += new EventHandler(onSelectCustomer);
            }
            customersList.AdminMode = false;
            customersList.PageSize = 5;
        }

        protected void saveClick(object sender, EventArgs e)
        {
            saveChanges();
            Response.Redirect("~/Dokumenty/akcjeDokumentu.aspx?id=" + CoreObject.GetId(Request).ToString(), false);
        }

        protected void cancelClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Dokumenty/akcjeDokumentu.aspx?id=" + CoreObject.GetId(Request).ToString(), false);
        }

        private void DisableCategoryAndType(int docId)
        {
            bool isFilled = new DocumentDAO().IsCustomFormFilled(docId);

            if (isFilled)
            {
                this.kategoria.Enabled = false;
                this.rodzaj.Enabled = false;
            }
        }

        private void loadDocumentData(int docId)
        {
            try
            {
                DaneDokumentuTableAdapters.DaneDokumentuDAO dao = new Pemi.Esoda.Web.UI.Akcje.DaneDokumentuTableAdapters.DaneDokumentuDAO();
                DaneDokumentu.DaneDokumentuDataTable dt = dao.GetData(docId);
                XPathDocument xpd = new XPathDocument(new StringReader(dt[0].metadane));
                XPathNavigator xpn = xpd.CreateNavigator();
                ViewState["dmd"] = dt;
                ViewState["docId"] = docId;
                //interesant.DataBind();
                //interesant.SelectedValue = xpn.SelectSingleNode("/metadane/nadawca/@id").Value;
                hfCustomerId.Value = xpn.SelectSingleNode("/metadane/nadawca/@id").Value;
                lblInteresant.Text = xpn.SelectSingleNode("/metadane/nadawca").Value;
                status.SelectedValue = dt[0].idStatusu.ToString();
                znakPisma.Text = xpn.SelectSingleNode("/metadane/numerPisma").Value;
                if (xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/kategoria/@id").Value != "0"
                    && xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/kategoria/@id").Value != "-1"
                    && xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/kategoria/@id").Value != "")
                kategoria.SelectedValue = xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/kategoria/@id").Value;
                if(xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/rodzaj/@id").Value!="0"
                    && xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/rodzaj/@id").Value != "-1"
                    && xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/rodzaj/@id").Value != "")
                rodzaj.SelectedValue = xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/rodzaj/@id").Value;
                txtDataPisma.Text = xpn.SelectSingleNode("/metadane/dataPisma").Value;
                txtOpis.Text = xpn.SelectSingleNode("/metadane/opis").Value;
                kategoria.DataBind();
                rodzaj.DataBind();
            }
            catch
            {
                throw new Exception("B³¹d ³adowania danych dokumentu");
            }

        }
        
        private void saveChanges()
        {
            DaneDokumentu.DaneDokumentuDataTable dt = (DaneDokumentu.DaneDokumentuDataTable)ViewState["dmd"];
            XmlDocument xpd = new XmlDocument();
            xpd.Load(new StringReader(dt[0].metadane));
            XPathNavigator xpn = xpd.CreateNavigator();
            //xpn.SelectSingleNode("/metadane/nadawca/@id").SetValue(interesant.SelectedValue);
           
                xpn.SelectSingleNode("/metadane/nadawca/@id").SetValue(hfCustomerId.Value);
                int typeId, catId;
                //(new UserDAO()).GetCustomerTypeCat(int.Parse(interesant.SelectedValue), out typeId, out catId);
                (new UserDAO()).GetCustomerTypeCat(int.Parse(hfCustomerId.Value), out typeId, out catId);
                xpn.SelectSingleNode("/metadane/nadawca/@typ").SetValue(typeId.ToString());
                xpn.SelectSingleNode("/metadane/nadawca/@kategoria").SetValue(catId.ToString());

                //xpn.SelectSingleNode("/metadane/nadawca").SetValue(interesant.SelectedItem.Text);
                xpn.SelectSingleNode("/metadane/nadawca").SetValue(lblInteresant.Text);           
            
            //if(znakPisma.Text.Length>0)
            xpn.SelectSingleNode("/metadane/numerPisma").SetValue(znakPisma.Text);
            xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/kategoria/@id").SetValue(kategoria.SelectedIndex==-1?"0":kategoria.SelectedValue);
            xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/kategoria").SetValue(kategoria.SelectedIndex == -1 ? "nieokreœlona" : kategoria.SelectedItem.Text);
            xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/rodzaj/@id").SetValue(rodzaj.SelectedIndex==-1?"0":rodzaj.SelectedValue);
            xpn.SelectSingleNode("/metadane/klasyfikacjaDokumentu/rodzaj").SetValue(rodzaj.SelectedIndex==-1?"nieokreœlony": rodzaj.SelectedItem.Text);
            string newMetadata = xpn.SelectSingleNode("/metadane").OuterXml;
            DaneDokumentuTableAdapters.DaneDokumentuDAO dao = new Pemi.Esoda.Web.UI.Akcje.DaneDokumentuTableAdapters.DaneDokumentuDAO();


            ActionLogger al = new ActionLogger(new ActionContext(new Guid("5B1EDF0C-DE49-4D5C-A116-54A5E25C6FB8"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, new List<string>()));
            al.AppliesToDocuments.Add((int)ViewState["docId"]);
            //al.ActionData.Add("interesant", interesant.SelectedItem.Text);
            
            al.ActionData.Add("interesant", (lblInteresant.Text.Length>0)?lblInteresant.Text:"-");
            al.ActionData.Add("status", status.SelectedItem.Text);
            al.ActionData.Add("znakPisma", (znakPisma.Text.Length>0)?znakPisma.Text:"-");
            al.ActionData.Add("kategoria",kategoria.SelectedIndex==-1?"0":kategoria.SelectedItem.Text);
            al.ActionData.Add("rodzaj", rodzaj.SelectedIndex==-1?"0":rodzaj.SelectedItem.Text);
            al.Execute();

            int res = dao.Update((int)ViewState["docId"], newMetadata, int.Parse(status.SelectedValue));
            if (res == 0)
                BaseContentPage.SetError("oops...", "~/Akcje/EdycjaDokumentu.aspx");
            //Session["context"] = null;      
        }

        protected void interesant_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem("- nieokreœlony -", "-1"));
        }

        protected void kategoria_DataBound(object sender, EventArgs e)
        {

        }

        void customer_SearchCustomers(object sender, SearchCustomersEventArgs e)
        {
            customer.Visible = false;
            lnkSearchAgain.Visible = true;
        }

        void customer_OnCustomerAdded(object sender, EventArgs e)
        {
            lnkSelectCustomer.Visible = (customer.IdInteresanta > 0);
        }

        private void onSelectCustomer(object sender, EventArgs e)
        {
            if (customer.IdInteresanta > 0)
            {
                //(frmSprawa.FindControl("hfCustomerId") as HiddenField).Value = customer.IdInteresanta.ToString();
                //(frmSprawa.FindControl("lblOdKogo") as Label).Text = (customer.Nazwa.Length > 0) ? customer.Nazwa : customer.Imie + " " + customer.Nazwisko;
                hfCustomerId.Value = customer.IdInteresanta.ToString();                
                lblInteresant.Text = (customer.Nazwa.Length > 0) ? customer.Nazwa : customer.Imie + " " + customer.Nazwisko;
                customersList.Visible = false;
                customer.Visible = false;
                lnkSearchAgain.Visible = false;
                lnkSelectCustomer.Visible = false;
            }
        }

        protected void lnkSearchAgain_Click(object sender, EventArgs e)
        {
            lnkSearchAgain.Visible = false;
            customersList.Visible = false;
            customer.Visible = true;
            customer.SearchMode();
        }

        protected void lnkSelectCustomer_Click(object sender, EventArgs e)
        {
            onSelectCustomer(sender, e);
        }

        protected void btnZmien_Click(object sender, EventArgs e)
        {
            customer.Visible = true;
            lnkSelectCustomer.Visible = (customer.IdInteresanta > -1);
        }    
    }
}
