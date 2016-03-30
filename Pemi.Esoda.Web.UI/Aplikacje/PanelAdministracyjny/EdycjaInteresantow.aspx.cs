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
using System.Data.SqlClient;
using System.Xml;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class EdycjaInteresantow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCategories();
                SearchMode();
            } 
            //customersList.AddNewCustomer += new EventHandler(customersList_AddNewCustomer);
            //customersList.EditCustomer += new EventHandler(customersList_EditCustomer);

            customersList.AddNewCustomer += new EventHandler(customer.AddNewCustomer);
            customersList.EditCustomer += new EventHandler(customer.EditCustomer);
            customersList.AdminMode = true;

            customer.SearchCustomers += new EventHandler<Pemi.Esoda.Presenters.SearchCustomersEventArgs>(customersList.FindCustomers);
            customer.SearchListVisible += new EventHandler<Pemi.Esoda.Presenters.SearchCustomersEventArgs>(customersList.SearchListVisible);
        }

         

        void InsertMode()
        {
            lnkAddCustomer.Visible = true;
            lnkUpdateCustomer.Visible = false;
            lnkFind.Visible = false;
            customersList.Visible = false;
            lblModeDesc.Text = "Dodawanie interesana";
            rblTypInteresanta.DataBind();
            LoadCategories();
        }

        void SearchMode()
        {
            lnkAddCustomer.Visible = false;
            lnkUpdateCustomer.Visible = false;
            lnkFind.Visible = true;
            customersList.Visible = false;
            lblModeDesc.Text = "Wyszukiwanie interesantów";
            rblTypInteresanta.DataBind();
            LoadCategories();
        }

        void UpdateMode()
        {
            ClearForm();
            lblModeDesc.Text = "Edycja interesanta";
            lnkAddCustomer.Visible = false;
            lnkUpdateCustomer.Visible = true;
            lnkFind.Visible = false;
            customersList.Visible = true;
            rblTypInteresanta.DataBind();
            LoadCategories();
        }

        void customersList_AddNewCustomer(object sender, EventArgs e)
        {
            ClearForm();
            InsertMode();
        }

        void customersList_EditCustomer(object sender, EventArgs e)
        {
            ClearForm();
            UpdateMode();
            SqlDataReader dr = (SqlDataReader)(new CustomerDAO()).GetCustomer(int.Parse(sender.ToString()));
            if (dr.Read())
            {
                hfCustomerID.Value = sender.ToString();

                rblTypInteresanta.SelectedValue = dr["idTypuUzytkownika"].ToString();
                rblTypInteresanta_SelectedIndexChanged(sender, e);
                if (dr["idKategorii"] != null)
                    ddlKategoria.SelectedValue = dr["idKategorii"].ToString();

                txtImie.Text = dr["imie"].ToString();
                txtNazwisko.Text = dr["nazwisko"].ToString();
                if (txtImie.Text.Length == 0 && txtNazwisko.Text.Length == 0)
                    txtNazwa.Text = dr["nazwa"].ToString();
                else
                    txtNazwa.Text = "";

                string adresy = dr["adresy"].ToString();

                ///////////////////////////////////

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(adresy);

                XmlNode xNode;

                xNode = doc.SelectSingleNode("/adresy/adres/kodPocztowy");
                if (xNode != null)
                    txtKod.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                xNode = doc.SelectSingleNode("/adresy/adres/miejscowosc");
                if (xNode != null)
                    txtMiasto.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                xNode = doc.SelectSingleNode("/adresy/adres/ulica");
                if (xNode != null)
                    txtUlica.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                xNode = doc.SelectSingleNode("/adresy/adres/budynek");
                if (xNode != null)
                    txtBudynek.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                xNode = doc.SelectSingleNode("/adresy/adres/lokal");
                if (xNode != null)
                    txtLokal.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                //xNode = doc.SelectSingleNode("/adresy/adres/poczta");
                //if (xNode != null)
                //    txtPoczta.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                ///////////////////////////////////
            }
        }

        void ClearForm()
        {
            ddlKategoria.SelectedIndex = -1;
            txtBudynek.Text = txtImie.Text = txtKod.Text = txtLokal.Text = txtMiasto.Text = txtNazwa.Text = txtNazwisko.Text = txtUlica.Text = "";
        }

        private void LoadCategories()
        {
            int typ = -1;
            Trace.Warn("przed wyj¹tkiem");
            Trace.Warn(rblTypInteresanta.ToString());
            if (!int.TryParse(rblTypInteresanta.SelectedValue, out typ))
                typ = -1;
            Trace.Warn("Za wyj¹tkiem");
            ddlKategoria.DataSource = (new CustomerDAO()).GetCustomersCategoriesByType(typ);
            ddlKategoria.DataTextField = "kategoria";
            ddlKategoria.DataValueField = "id";
            ddlKategoria.DataBind();
        }

        protected void rblTypInteresanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTypInteresanta.SelectedIndex > -1)
            {
                LoadCategories();
                switch (rblTypInteresanta.SelectedValue)
                {
                    case "-1":
                        pnlPelnaNazwa.Visible = true;
                        pnlNazwa.Visible = true;
                        break;
                    case "1":
                        pnlPelnaNazwa.Visible = true;
                        pnlNazwa.Visible = false;
                        break;
                    default:
                        pnlPelnaNazwa.Visible = false;
                        pnlNazwa.Visible = true;
                        break;
                }
            }
        }

        protected void rblTypInteresanta_DataBound(object sender, EventArgs e)
        {
            if (!lnkAddCustomer.Visible && !lnkUpdateCustomer.Visible)
            {
                ListItem item = new ListItem("Wszyscy", "-1");
                item.Selected = true;
                ((RadioButtonList)sender).Items.Insert(0, item);
            }
            else
                rblTypInteresanta.Items[0].Selected = true;
        }

        protected void ddlKategoria_DataBound(object sender, EventArgs e)
        {
            if (!lnkAddCustomer.Visible && !lnkUpdateCustomer.Visible)
            {
                ListItem item = new ListItem("Wszystkie", "-1");
                item.Selected = true;
                ((DropDownList)sender).Items.Insert(0, item);
            }
        }

        protected void lnkAddCustomer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                RegistryDAO rd = new RegistryDAO();

                int id = int.Parse(rblTypInteresanta.SelectedItem.Value);
                string typ = rblTypInteresanta.SelectedItem.Text;

                CustomerDTO customer = new CustomerDTO(id, typ);

                if (id == 1)
                    customer.CustomerCategory = null;
                else
                    customer.CustomerCategory = int.Parse(ddlKategoria.SelectedValue);
                customer.FirstName = txtImie.Text;
                customer.LastName = txtNazwisko.Text;
                customer.Name = (txtNazwa.Text.Length > 0) ? txtNazwa.Text : null;

                AddressDTO addr = new AddressDTO();

                addr.Building = txtBudynek.Text;
                addr.City = txtMiasto.Text;
                addr.Flat = txtLokal.Text;
                //addr.Post = frmSender.FindControl("txtPoczta") as TextBox).Text;
                addr.PostalCode = txtKod.Text;
                addr.Street = txtUlica.Text;

                customer.Address = addr;

                // wstawienie interesanta
                rd.CreateCustomer(customer);
                SearchMode();
                ClearForm();
                // pokazuje dodanego interesanta na liœcie
                //lnkFind_Click(sender, e);                
            }
        }

        protected void lnkFind_Click(object sender, EventArgs e)
        {
            customersList.Visible = true;
            int idKategorii; // , idTypu;

            if (!int.TryParse(ddlKategoria.SelectedValue, out idKategorii))
                //idKategorii = idKategorii;
            //else
                idKategorii = -1;

            customersList.FindCustomer(int.Parse(rblTypInteresanta.SelectedItem.Value),
                                       idKategorii,
                                       txtImie.Text,
                                       txtNazwisko.Text,
                                       txtNazwa.Text,
                                       txtMiasto.Text,
                                       txtKod.Text,
                                       txtUlica.Text,
                                       txtBudynek.Text,
                                       txtLokal.Text,"","","");
        }

        protected void lnkUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                RegistryDAO rd = new RegistryDAO();

                int id = int.Parse(rblTypInteresanta.SelectedItem.Value);
                string typ = rblTypInteresanta.SelectedItem.Text;

                CustomerDTO customer = new CustomerDTO(id, typ);

                customer.ID = int.Parse(hfCustomerID.Value);
                if (id == 1)
                    customer.CustomerCategory = null;
                else
                    customer.CustomerCategory = int.Parse(ddlKategoria.SelectedValue);
                customer.FirstName = txtImie.Text;
                customer.LastName = txtNazwisko.Text;
                customer.Name = (txtNazwa.Text.Length > 0) ? txtNazwa.Text : null;

                AddressDTO addr = new AddressDTO();

                addr.Building = txtBudynek.Text;
                addr.City = txtMiasto.Text;
                addr.Flat = txtLokal.Text;
                //addr.Post = frmSender.FindControl("txtPoczta") as TextBox).Text;
                addr.PostalCode = txtKod.Text;
                addr.Street = txtUlica.Text;

                customer.Address = addr;

                // wstawienie interesanta
                rd.UpdateCustomer(customer);
                SearchMode();
                ClearForm();
                // pokazuje dodanego interesanta na liœcie
                //lnkFind_Click(sender, e); 
            }
        }

        
    }
}