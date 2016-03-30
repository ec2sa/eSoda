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
using Pemi.Esoda.DTO;
using System.Xml;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class EdycjaInteresantow : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WczytajListeInteresantow(int.Parse(rodzajInteresanta.SelectedValue));
            }
        }

        public int IdInteresanta
        {
            get { return int.Parse(this.ddlSendersList.SelectedValue); }
        }

        public string NazwaInteresanta
        {
            get { return this.ddlSendersList.Text; }
        }

        private void WczytajListeInteresantow(int typInteresanta)
        {
            RegistryDAO rd = new RegistryDAO();
            ddlSendersList.DataSource = rd.GetCustomers(typInteresanta);
            lnkEdit.Visible = (ddlSendersList.DataSource as ICollection).Count != 0;
            ddlSendersList.DataValueField = "ID";
            ddlSendersList.DataTextField = "Description";
            ddlSendersList.DataBind();
        }

        protected void lnkAddSender_Click(object sender, EventArgs e)
        {
            frmSender.ChangeMode(FormViewMode.Insert);
            frmSender.Visible = true;
            v1.Visible = false;
          if (int.Parse(rodzajInteresanta.SelectedValue) >0)
              (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedValue = rodzajInteresanta.SelectedValue;
            else
              (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedValue = "1";
            obslugaZmianyTypuNowegoInteresanta(null, null);
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            RegistryDAO rd = new RegistryDAO();
            frmSender.ChangeMode(FormViewMode.Edit);
            frmSender.DataSource = rd.GetCustomer(ddlSendersList.SelectedValue);
            frmSender.DataBind();
            if (int.Parse(rodzajInteresanta.SelectedValue) > 0)
                (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedValue = rodzajInteresanta.SelectedValue;
            else
                (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedValue = "1";
            obslugaZmianyTypuNowegoInteresanta(null, null);
            frmSender.Visible = true;
            v1.Visible = false;
        }

        protected void frmSender_ModeChanging(object sender, FormViewModeEventArgs e)
        {
           // frmSender.ChangeMode(e.NewMode);
        }

        protected void frmSender_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                RegistryDAO rd = new RegistryDAO();

                int id = int.Parse((frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedItem.Value);
                string typ = (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedItem.Text; 
                
                CustomerDTO customer = new CustomerDTO(id,typ);
                                
                customer.FirstName = (frmSender.FindControl("txtImie") as TextBox).Text;
                customer.LastName = (frmSender.FindControl("txtNazwisko") as TextBox).Text;
                customer.Name = (frmSender.FindControl("txtNazwa") as TextBox).Text;

                if(frmSender.FindControl("txtNip")!=null)
                customer.Nip = (frmSender.FindControl("txtNip") as TextBox).Text;
                customer.NumberSMS = (frmSender.FindControl("txtNumerSMS") as TextBox).Text;
                AddressDTO addr = new AddressDTO();

                addr.Building = (frmSender.FindControl("txtBudynek") as TextBox).Text;
                addr.City = (frmSender.FindControl("txtMiasto") as TextBox).Text;
                addr.Flat = (frmSender.FindControl("txtLokal") as TextBox).Text;
                addr.Post = (frmSender.FindControl("txtPoczta") as TextBox).Text;
                addr.PostalCode = (frmSender.FindControl("txtKod") as TextBox).Text;
                addr.Street = (frmSender.FindControl("txtUlica") as TextBox).Text;

                customer.Address = addr;

                // wstawienie interesanta
                rd.CreateCustomer(customer);
                frmSender.ChangeMode(FormViewMode.ReadOnly);
                frmSender.Visible = false;
                v1.Visible = true;
                WczytajListeInteresantow(int.Parse(rodzajInteresanta.SelectedValue));
            }
        }

        protected void frmSender_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                RegistryDAO rd = new RegistryDAO();

                int id = int.Parse((frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedItem.Value);
                string typ = (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedItem.Text;

                CustomerDTO customer = new CustomerDTO(id, typ);

                customer.ID = int.Parse(ddlSendersList.SelectedValue);
                customer.FirstName = (frmSender.FindControl("txtImie") as TextBox).Text;
                customer.LastName = (frmSender.FindControl("txtNazwisko") as TextBox).Text;
                customer.Name = (frmSender.FindControl("txtNazwa") as TextBox).Text;

                if (frmSender.FindControl("txtNip") != null)
                    customer.Nip = (frmSender.FindControl("txtNip") as TextBox).Text;
                customer.NumberSMS = (frmSender.FindControl("txtNumerSMS") as TextBox).Text;

                AddressDTO addr = new AddressDTO();

                addr.Building = (frmSender.FindControl("txtBudynek") as TextBox).Text;
                addr.City = (frmSender.FindControl("txtMiasto") as TextBox).Text;
                addr.Flat = (frmSender.FindControl("txtLokal") as TextBox).Text;
                addr.Post = (frmSender.FindControl("txtPoczta") as TextBox).Text;
                addr.PostalCode = (frmSender.FindControl("txtKod") as TextBox).Text;
                addr.Street = (frmSender.FindControl("txtUlica") as TextBox).Text;

                customer.Address = addr;

                // aktualizacja interesanta
                if (customer.FirstName != null && customer.LastName != null)
                    customer.Name = null;

                rd.UpdateCustomer(customer);
                frmSender.ChangeMode(FormViewMode.ReadOnly);
                frmSender.Visible = false;
                v1.Visible = true;
                WczytajListeInteresantow(int.Parse(rodzajInteresanta.SelectedValue));
            }
        }

        protected void obslugaZmianyTypuInteresanta(object sender, EventArgs e)
        {
          
            WczytajListeInteresantow(int.Parse(rodzajInteresanta.SelectedValue));
        }

        protected void obslugaZmianyTypuNowegoInteresanta(object sender, EventArgs e)
        {
            int ct = (frmSender.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedIndex;
            frmSender.FindControl("daneOsoby").Visible = (ct == 0 || ct > 2);
            frmSender.FindControl("daneFirmy").Visible = (ct > 0 && ct < 3);
        }

        protected void frmSender_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel"))
            {
               // frmSender.ChangeMode(FormViewMode.ReadOnly);
                frmSender.Visible = false;
                v1.Visible = true;
            }
        }

        protected void frmSender_DataBound(object sender, EventArgs e)
        {
         //   obslugaZmianyTypuNowegoInteresanta(sender, e);
            if (frmSender.DataItem != null)
            {
                object obj;
                string adresy; //,data;
                obj = DataBinder.Eval(frmSender.DataItem, "adresy");
                if (obj != null)
                {
                    adresy = obj.ToString();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(adresy);

                    XmlNode xNode;

                    xNode = doc.SelectSingleNode("/adresy/adres/kodPocztowy");
                    if (xNode != null)
                        (frmSender.FindControl("txtKod") as TextBox).Text = (xNode.InnerText != null) ? xNode.InnerText : "";
                    
                    xNode = doc.SelectSingleNode("/adresy/adres/miejscowosc");
                    if (xNode != null)
                        (frmSender.FindControl("txtMiasto") as TextBox).Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                    xNode = doc.SelectSingleNode("/adresy/adres/ulica");
                    if (xNode != null)
                        (frmSender.FindControl("txtUlica") as TextBox).Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                    xNode = doc.SelectSingleNode("/adresy/adres/budynek");
                    if (xNode != null)
                        (frmSender.FindControl("txtBudynek") as TextBox).Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                    xNode = doc.SelectSingleNode("/adresy/adres/lokal");
                    if (xNode != null)
                        (frmSender.FindControl("txtLokal") as TextBox).Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                    xNode = doc.SelectSingleNode("/adresy/adres/poczta");
                    if (xNode != null)
                        (frmSender.FindControl("txtPoczta") as TextBox).Text = (xNode.InnerText != null) ? xNode.InnerText : "";
                }
            }
        }        
    }
}