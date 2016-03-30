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
using Pemi.Esoda.DTO;
using System.Xml;
using System.Data.SqlClient;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Presenters;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class Interesant : System.Web.UI.UserControl
    {
        public event EventHandler<SearchCustomersEventArgs> SearchListVisible;
        public event EventHandler<SearchCustomersEventArgs> SearchCustomers;
        public event EventHandler OnCustomerAdded;

        public int IdInteresanta
        {
            get
            {
                int id;
                if (int.TryParse(hfCustomerID.Value, out id))
                    return id;
                else
                    return -1;
            }
            set
            {
                hfCustomerID.Value = value.ToString();
                LoadCustomerData(value);
               // ViewMode();
                rblTypInteresanta.Enabled = false;
            }
        }

        public int IdTypu
        {
            get
            {
                int idTypu;
                if (int.TryParse(rblTypInteresanta.SelectedValue, out idTypu))
                    return idTypu;
                else
                    return -1;
            }
                
        }

        public int IdKategorii
        {
            get
            {
                int idKategorii;
                if (int.TryParse(ddlKategoria.SelectedValue, out idKategorii))
                    return idKategorii;
                else
                    return -1;
            }
        }

        public string Imie
        {
            get { return txtImie.Text; }
        }

        public string Nazwisko
        {
            get { return txtNazwisko.Text; }
        }

        public string Nazwa
        {
            get { return txtNazwa.Text; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCategories();
                if (hfCustomerID.Value != null && hfCustomerID.Value != string.Empty)
                    ViewMode();
                else
                    SearchMode();
            }
        }

        private void CustomersListVisible(bool visible)
        {
            if (SearchListVisible != null)
                SearchListVisible(this, new SearchCustomersEventArgs(visible));
        }

        void InsertMode()
        {
            SetLabelsVisble(false);
            lnkAddCustomer.Visible = true;
            lnkUpdateCustomer.Visible = false;
            lnkSearchMode.Visible = true;
            lnkEdit.Visible = false;
            lnkFind.Visible = false;
            //customersList.Visible = false;
            CustomersListVisible(false);
            lblModeDesc.Text = "Dodawanie interesana";
            rblTypInteresanta.DataBind();
            LoadCategories();
        }

        public void SearchMode()
        {
            SetLabelsVisble(false);
            lnkAddCustomer.Visible = false;
            lnkUpdateCustomer.Visible = false;
            lnkSearchMode.Visible = false;
            lnkEdit.Visible = false;
            lnkFind.Visible = true;
            //customersList.Visible = false;
            CustomersListVisible(false);
            lblModeDesc.Text = "Wyszukiwanie interesantów";
            rblTypInteresanta.DataBind();
            LoadCategories(true);
        }

        void UpdateMode()
        {
            SetLabelsVisble(false);
            ClearForm();
            lblModeDesc.Text = "Edycja interesanta";
            rblTypInteresanta.Enabled = true;
            lnkAddCustomer.Visible = false;
            lnkUpdateCustomer.Visible = true;
            lnkFind.Visible = false;
            lnkEdit.Visible = false;
            //customersList.Visible = true;
            CustomersListVisible(false);
            rblTypInteresanta.DataBind();
            LoadCategories();
            lnkSearchMode.Visible = true;
        }

        void ViewMode()
        {
            lblModeDesc.Text = "Wybrany interesant";
            CopyDataToLabels();
            SetLabelsVisble(true);
            lnkEdit.Visible = true;
            lnkSearchMode.Visible = true;
            lnkFind.Visible = false;                       
            lnkAddCustomer.Visible = false;
            lnkUpdateCustomer.Visible = false;
        }

        public void AddNewCustomer(object sender, EventArgs e)
        {
            //ClearForm();
            this.Visible = true;
            string selected = rblTypInteresanta.SelectedValue;
            InsertMode();
            if(rblTypInteresanta.Items.FindByValue(selected) != null)
                rblTypInteresanta.SelectedValue = selected;
            rblTypInteresanta_SelectedIndexChanged(sender, e);
        }

        public void EditCustomer(object sender, EventArgs e)
        {
            ClearForm();
            UpdateMode();
            LoadCustomerData(int.Parse(sender.ToString()));
            rblTypInteresanta.Enabled = true;
        }

        public void SelectCustomer(object sender, EventArgs e)
        {
            ClearForm();
            SearchMode(); 
            //UpdateMode();
            LoadCustomerData(int.Parse(sender.ToString()));
            CustomersListVisible(false);
            ViewMode();
            rblTypInteresanta.Enabled = false;
        }

        public void LoadCustomerData(int id)
        {
            SqlDataReader dr = (SqlDataReader)(new CustomerDAO()).GetCustomer(id);
            if (dr.Read())
            {
                hfCustomerID.Value = id.ToString();

                rblTypInteresanta.SelectedValue = dr["idTypuUzytkownika"].ToString();
                rblTypInteresanta_SelectedIndexChanged(this, null);
                if (dr["idKategorii"] != System.DBNull.Value && !rblTypInteresanta.SelectedValue.Equals("1") && !dr["idKategorii"].ToString().Equals("-1")) // osoba fizyczna
                    ddlKategoria.SelectedValue = dr["idKategorii"].ToString();

                txtImie.Text = txtImie.ToolTip = dr["imie"].ToString();                 
                txtNazwisko.Text = txtNazwisko.ToolTip = dr["nazwisko"].ToString();
                if (txtImie.Text.Length == 0 && txtNazwisko.Text.Length == 0)
                    txtNazwa.Text = txtNazwa.ToolTip = dr["nazwa"].ToString();
                else
                    txtNazwa.Text = txtNazwa.ToolTip = "";

                txtNIP.Text = dr["nip"].ToString();
                txtNumerSMS.Text = dr["numerSMS"].ToString();
                string adresy = dr["adresy"].ToString();

                ///////////////////////////////////

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(adresy);

                XmlNode xNode;

                //string text = "";

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

                xNode = doc.SelectSingleNode("/adresy/adres/poczta");
                if (xNode != null)
                    txtPoczta.Text = (xNode.InnerText != null) ? xNode.InnerText : "";

                ///////////////////////////////////
            }
        }        

        void ClearForm()
        {
            ddlKategoria.SelectedIndex = -1;
            hfCustomerID.Value = "-1";
            rblTypInteresanta.SelectedIndex = -1;
            rblTypInteresanta.DataBind();
            txtBudynek.Text = txtImie.Text = txtKod.Text = txtLokal.Text = txtMiasto.Text = txtNazwa.Text = txtNazwisko.Text = txtUlica.Text = txtNIP.Text=txtPoczta.Text="";
        }

        void SetLabelsVisble(bool visible)
        {
            lblTxtImie.Visible = lblTxtNazwisko.Visible = lblTxtNazwa.Visible = lblTxtKategoria.Visible = visible;
            lblTxtMiasto.Visible = lblTxtKod.Visible = lblTxtUlica.Visible = lblTxtBudynek.Visible = lblTxtLokal.Visible = visible;
            lblTxtNIP.Visible = lblTxtPoczta.Visible = lblTxtNumerSMS.Visible= visible;

            ddlKategoria.Visible = txtImie.Visible = txtNazwisko.Visible = txtNazwa.Visible = !visible;
            txtMiasto.Visible = txtKod.Visible = txtUlica.Visible = txtBudynek.Visible = txtLokal.Visible = !visible;
            txtNIP.Visible = txtPoczta.Visible=txtNumerSMS.Visible= !visible;
        }

        void CopyDataToLabels()
        {
            lblTxtImie.Text = lblTxtImie.ToolTip = txtImie.Text;
            lblTxtNazwisko.Text = lblTxtNazwisko.ToolTip = txtNazwisko.Text;
            lblTxtNazwa.Text = lblTxtNazwa.ToolTip = txtNazwa.Text;
            lblTxtKategoria.Text = ddlKategoria.SelectedItem!=null? ddlKategoria.SelectedItem.Text:"???";
            lblTxtMiasto.Text = txtMiasto.Text;
            lblTxtKod.Text = txtKod.Text;
            lblTxtUlica.Text = txtUlica.Text;
            lblTxtBudynek.Text = txtBudynek.Text;
            lblTxtLokal.Text = txtLokal.Text;
            lblTxtNIP.Text = txtNIP.Text;
            lblTxtPoczta.Text = txtPoczta.Text;
            lblTxtNumerSMS.Text = txtNumerSMS.Text;
        }

        private void LoadCategories()
        {
            LoadCategories(false);
        }

        private void LoadCategories(bool force)
        {
            if (ddlKategoria.SelectedValue == null || force)
            {
                int typ = -1;
                if (!int.TryParse(rblTypInteresanta.SelectedValue, out typ))
                    typ = -1;
                ddlKategoria.Items.Clear();
                ddlKategoria.DataSource = (new CustomerDAO()).GetCustomersCategoriesByType(typ);
                ddlKategoria.DataTextField = "kategoria";
                ddlKategoria.DataValueField = "id";
                ddlKategoria.DataBind();
            }
        }

        protected void rblTypInteresanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTypInteresanta.SelectedIndex > -1)
            {  
                LoadCategories(true);
                rfvKategoria.Enabled = true;
                switch (rblTypInteresanta.SelectedValue)
                {
                    case "-1":
                        pnlPelnaNazwa.Visible = true;
                        pnlNazwa.Visible = true;
                        ddlKategoria.Enabled = true;                        
                        break;
                    case "1":
                        txtNazwa.Text = string.Empty;
                        pnlPelnaNazwa.Visible = true;
                        pnlNazwa.Visible = false;
                        //ddlKategoria.SelectedValue = "-1";
                        ddlKategoria.Enabled = false;
                        rfvKategoria.Enabled = false;
                        break;
                    default:
                        txtImie.Text = txtNazwisko.Text = string.Empty;
                        pnlPelnaNazwa.Visible = false;
                        pnlNazwa.Visible = true;
                        ddlKategoria.Enabled = true;
                        rfvKategoria.Enabled = true;
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
            //if (!lnkAddCustomer.Visible && !lnkUpdateCustomer.Visible)
            //{
                ListItem item = new ListItem("Wszystkie", "-1");
                //item.Selected = true;
                ((DropDownList)sender).Items.Insert(0, item);
            //}
        }

        protected void lnkAddCustomer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                RegistryDAO rd = new RegistryDAO();

                int id = int.Parse(rblTypInteresanta.SelectedItem.Value);
                string typ = rblTypInteresanta.SelectedItem.Text;

                CustomerDTO customer = new CustomerDTO(id, typ) { Nip = txtNIP.Text,NumberSMS=txtNumerSMS.Text};

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
                addr.PostalCode = txtKod.Text;
                addr.Street = txtUlica.Text;

                customer.Address = addr;

                // wstawienie interesanta
                int custId = rd.CreateCustomer(customer);
                SearchMode();
                //ClearForm();
                // pokazuje dodanego interesanta na liœcie
                //lnkFind_Click(sender, e);
                SelectCustomer(custId.ToString(), null);
                if (OnCustomerAdded != null)
                    OnCustomerAdded(sender, e);
                
                //
            }
        }

        protected void lnkFind_Click(object sender, EventArgs e)
        {
            int idKategorii; // ,idTypu;

            if (!int.TryParse(ddlKategoria.SelectedValue, out idKategorii))
                //idKategorii = idKategorii;
            //else
                idKategorii = -1;

            if(SearchCustomers != null)
                SearchCustomers(this, new SearchCustomersEventArgs(int.Parse(rblTypInteresanta.SelectedItem.Value),
                                       idKategorii,
                                       txtImie.Text,
                                       txtNazwisko.Text,
                                       txtNazwa.Text,
                                       txtMiasto.Text,
                                       txtKod.Text,
                                       txtUlica.Text,
                                       txtBudynek.Text,
                                       txtLokal.Text,
                                       txtNIP.Text,
                                       txtPoczta.Text,
                                       txtNumerSMS.Text));
        }

        protected void lnkUpdateCustomer_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                RegistryDAO rd = new RegistryDAO();

                int id = int.Parse(rblTypInteresanta.SelectedItem.Value);
                string typ = rblTypInteresanta.SelectedItem.Text;

                CustomerDTO customer = new CustomerDTO(id, typ) { Nip = txtNIP.Text, NumberSMS = txtNumerSMS.Text };

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
                addr.Post = txtPoczta.Text;
                customer.Address = addr;

                // wstawienie interesanta
                rd.UpdateCustomer(customer);
                SearchMode();
                ClearForm();
                // pokazuje dodanego interesanta na liœcie
                //lnkFind_Click(sender, e); 
                // wybiera zmienionego
                SelectCustomer(customer.ID.ToString(), null);
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            string custId = hfCustomerID.Value;
            UpdateMode();
            LoadCustomerData(int.Parse(custId));
            rblTypInteresanta.Enabled = true;
        }

        protected void lnkSearchMode_Click(object sender, EventArgs e)
        {
            SearchMode();            
            ClearForm();
            rblTypInteresanta.Enabled = true;
            rblTypInteresanta.SelectedIndex = 0;
            rblTypInteresanta_SelectedIndexChanged((object)rblTypInteresanta, e);
        }

    }
}