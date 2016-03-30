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
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using Pemi.Esoda.Web.UI.Controls;
using System.Data.Common;
using Pemi.Esoda.DataAccess;


namespace Pemi.Esoda.Web.UI
{
    public partial class EdycjaPozycjiDziennika : BaseContentPage, IEditRegistyItemView
    {
        private bool _isInvoice;

       

       
        private EditRegistyItemPresenter presenter;

        private string ZawartoscPola(string id)
        {
            if (pozycja.FindControl(id) != null)
                return ((pozycja.FindControl(id) as TextBox).Text);
            else
                return string.Empty;
        }

        private int WybraneID(string sid)
        {
            int id=0;

            if (pozycja.FindControl(sid) != null)
            {
                if (!int.TryParse((pozycja.FindControl(sid) as ListControl).SelectedValue, out id))
                    return 0;
            }

            return id;
        }

        private string WybranyOpis(string id)
        {
            ListControl list = pozycja.FindControl(id) as ListControl;
            if (list != null)
            {
                if (list.SelectedItem != null)
                    return list.SelectedItem.Text;
                else
                    return string.Empty;
            }
            else
                return string.Empty;               
        }

        protected void podpiecieDat(object sender, EventArgs e)
        {
            
            if (pozycja.CurrentMode == FormViewMode.Edit)
            {
                XPathDocument xpd = new XPathDocument(new StringReader((pozycja.DataSource as XmlDataSource).Data));
                XPathNavigator xpnav = xpd.CreateNavigator();
                /*bool dekretacja = xpnav.SelectSingleNode("/pozycja/@dekretowanie").Value == "1" ? true : false;
                
                DropDownList ddlReferentWydzial = pozycja.FindControl("fwydzialZnakReferenta") as DropDownList;
                if(ddlReferentWydzial != null)
                {
                    ddlReferentWydzial.Enabled = dekretacja;
                }
                DropDownList ddlReferentPracownik = pozycja.FindControl("fpracownikZnakReferenta") as DropDownList;
                if(ddlReferentPracownik != null )
                {
                    ddlReferentPracownik.Enabled = dekretacja;
                }*/

                try
                {
                    (pozycja.FindControl("txtDataWplywu") as TextBox).Text = xpnav.SelectSingleNode("/pozycja/dataWplywu").Value;
                   
                    
                    (pozycja.FindControl("txtDataPisma") as TextBox).Text = xpnav.SelectSingleNode("/pozycja/dataPisma").Value;
                    //(pozycja.FindControl("fdataWplywu") as Calendar).SelectedDate = DateTime.Parse(xpnav.SelectSingleNode("/pozycja/dataWplywu").Value).Date;
                    //(pozycja.FindControl("fdataPisma") as Calendar).SelectedDate = DateTime.Parse(xpnav.SelectSingleNode("/pozycja/dataPisma").Value).Date;
                }
                catch { }
            }
            if (string.IsNullOrEmpty((pozycja.FindControl("txtDataWplywu") as TextBox).Text))
                (pozycja.FindControl("txtDataWplywu") as TextBox).Text = DateTime.Today.ToShortDateString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {       
            Session["parentPage"] = Request.QueryString["pp"];
            Session["isRFEdited"] = Request.QueryString["rf"];
            presenter = new EditRegistyItemPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
                presenter.Initialize();
            lblMessage.Text = "";

            Timer1.Enabled = true;            

            Interesant customer = (Interesant)pozycja.FindControl("customer");
            if (customer != null)
            {
                customer.SearchCustomers += new EventHandler<SearchCustomersEventArgs>(customersList.FindCustomers);
                customer.SearchListVisible += new EventHandler<SearchCustomersEventArgs>(customersList.SearchListVisible);
                customersList.AddNewCustomer += new EventHandler(customer.AddNewCustomer);
                customersList.EditCustomer += new EventHandler(customer.EditCustomer);
                customersList.SelectCustomer += new EventHandler(customer.SelectCustomer);
            }
            customersList.AdminMode = false;
            customersList.PageSize = 10;
        }

        private EventHandler<ExecutingCommandEventArgs> itemCommand;

        protected void OnItemCommand(ExecutingCommandEventArgs e)
        {
            
                if (itemCommand != null)
                    switch (e.CommandName)
                    {
                        /*case "zapiszNowegoInteresanta":
                            presenter.SaveNewCustomer(WybraneID("rodzajNowegoInteresanta"), ZawartoscPola("nowyInteresantImie"), ZawartoscPola("nowyInteresantNazwisko"), ZawartoscPola("nowyInteresantNazwa"),
                                ZawartoscPola("nowyKod"), ZawartoscPola("nowyMiejscowosc"), ZawartoscPola("nowyUlica"), ZawartoscPola("nowyBudynek"), ZawartoscPola("nowyLokal"), ZawartoscPola("nowyPoczta"));
                            break;
                        case "listaInteresantow":
                            pozycja.FindControl("nowyInteresant").Visible = false;
                            pozycja.FindControl("interesant").Visible = true;
                            break;*/
                        case "zapisz":
                            if (Page.IsValid)
                            {
                                if ((pozycja.FindControl("customer") as Interesant).IdInteresanta > 0)
                                {
                                    string dodatkoweMaterialy = (pozycja.FindControl("fdodatkoweMaterialy") as TextBox).Text;
                                    //if (!string.IsNullOrEmpty(dodatkoweMaterialy.Trim()) && !IsAdditionalDescriptionValid(dodatkoweMaterialy)
                                    //{
                                     

                                    //}
                                    presenter.SaveItem(generujPozycje(), (Session["ESPDocId"] != null) ? Session["ESPDocId"].ToString() : string.Empty);
                                    Session["ESPDocId"] = null;

                                    string parentPage = (string)Session["parentPage"];
                                    if (parentPage == "a")
                                    {
                                        Response.Redirect("PrzegladDziennika.aspx");
                                    }
                                    else //if (parentPage == "s")
                                    {
                                        Response.Redirect("PrzegladDziennikaSimple.aspx");
                                    }

                                }
                                else
                                {
                                    //Session["StateBeforeError"] = this.SaveViewState();
                                    lblMessage.Text = "Nale¿y wybraæ interesanta";
                                }
                            }
                            break;

                        default:
                            itemCommand(this, e);
                            break;
                    
            }
        }

        protected void obslugaZdarzen(object sender, FormViewCommandEventArgs e)
        {
            OnItemCommand(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        protected void obslugaZmianyTypuInteresanta(object sender, EventArgs e)
        {
            OnItemCommand(new ExecutingCommandEventArgs("ustawSlownik", "customer"));
        }

        protected void obslugaZmianyTypuNowegoInteresanta(object sender, EventArgs e)
        {
            int ct = (pozycja.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedIndex;
            pozycja.FindControl("daneOsoby").Visible = (ct == 0 || ct > 2);
            pozycja.FindControl("daneFirmy").Visible = (ct > 0 && ct < 3);
        }

        protected void obslugaZmianyWydzialu(object sender, EventArgs e)
        {
            OnItemCommand(new ExecutingCommandEventArgs("ustawSlownik", "employee"));
        }

        protected void obslugaZmianyKategoriiDokumentu(object sender, EventArgs e)
        {
            OnItemCommand(new ExecutingCommandEventArgs("ustawSlownik", "documentTypes"));
        }

        private string NazwaNadawcy()
        {
            Interesant customer = (pozycja.FindControl("customer") as Interesant);
            if (customer != null)
            {
                if (customer.Nazwa.Length > 0)
                    return customer.Nazwa;
                else
                    return customer.Nazwisko + " " + customer.Imie;
            }
            else
                return string.Empty;
        }

        private string generujPozycje()
        {
            MembershipUser user = Membership.GetUser();
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            xw.WriteStartDocument();
            xw.WriteStartElement("wpis");
            xw.WriteAttributeString("data", DateTime.Now.ToString());
            xw.WriteAttributeString("idPracownika", user.ProviderUserKey.ToString());

            //string dw = (pozycja.FindControl("fdataWplywu") as Calendar).SelectedDate.ToString("yyyy-MM-dd");
            //string dp = (pozycja.FindControl("fdataPisma") as Calendar).SelectedDate.ToString("yyyy-MM-dd");
            string dw = (pozycja.FindControl("txtDataWplywu") as TextBox).Text;
            string dp = (pozycja.FindControl("txtDataPisma") as TextBox).Text;
            if (dw == "0001-01-01") dw = "";
            if (dp == "0001-01-01") dp = "";
            xw.WriteElementString("dataWplywu", dw);
            xw.WriteElementString("dataPisma", dp);
            xw.WriteElementString("numerPisma", ZawartoscPola("fnumerPisma"));
            xw.WriteStartElement("nadawca");
            xw.WriteAttributeString("typ", (pozycja.FindControl("customer") as Interesant).IdTypu.ToString());
            xw.WriteAttributeString("kategoria", (pozycja.FindControl("customer") as Interesant).IdKategorii.ToString());

            xw.WriteAttributeString("id", (pozycja.FindControl("customer") as Interesant).IdInteresanta.ToString());

            xw.WriteString(NazwaNadawcy());
            xw.WriteEndElement();//nadawca
            xw.WriteElementString("opis", ZawartoscPola("fopis"));

            xw.WriteStartElement("klasyfikacjaDokumentu");
            xw.WriteStartElement("kategoria");
            xw.WriteAttributeString("id", WybraneID("fkategoria").ToString());
            xw.WriteString(WybranyOpis("fkategoria"));
            xw.WriteEndElement();//kategoria
            xw.WriteStartElement("rodzaj");
            xw.WriteAttributeString("id", WybraneID("frodzajDokumentu").ToString());
            xw.WriteString(WybranyOpis("frodzajDokumentu"));
            xw.WriteEndElement();//rodzaj
            xw.WriteElementString("wartosc", ZawartoscPola("frodzajWartosc"));
            xw.WriteEndElement();//klasyfikacjaDokumentu

            xw.WriteStartElement("typKorespondencji");
            xw.WriteAttributeString("id", WybraneID("ftypKorespondencji").ToString());
            xw.WriteElementString("rodzaj", WybranyOpis("ftypKorespondencji"));
            xw.WriteElementString("wartosc", ZawartoscPola("ftypKorespondencjiWartosc"));
            
            xw.WriteEndElement();//typKorespondencji
            xw.WriteElementString("uwagi", ZawartoscPola("fuwagi"));
            xw.WriteStartElement("znakReferenta");
            xw.WriteStartElement("wydzial");
            xw.WriteAttributeString("id", WybraneID("fwydzialZnakReferenta").ToString());
            xw.WriteString(WybranyOpis("fwydzialZnakReferenta"));
            xw.WriteEndElement();//wydzial
            xw.WriteStartElement("pracownik");
            xw.WriteAttributeString("id", WybraneID("fpracownikZnakReferenta").ToString());
            xw.WriteString(WybranyOpis("fpracownikZnakReferenta"));
            xw.WriteEndElement();//pracownik
            xw.WriteEndElement();//znakReferenta
            xw.WriteElementString("kwota", ZawartoscPola("fKwotaFaktury"));

            xw.WriteStartElement("dodatkoweMaterialy");//dodatkoweMaterialy
            bool zawieraDM = (pozycja.FindControl("cbDodatkoweMaterialy") as CheckBox).Checked;
            string opisDM = (pozycja.FindControl("fdodatkoweMaterialy") as TextBox).Text;
            if (zawieraDM)
                xw.WriteAttributeString("zawiera", "tak");
            xw.WriteString(opisDM);
            xw.WriteEndElement();//dodatkoweMaterialy
            xw.WriteEndElement();//wpis
            xw.WriteEndDocument();
            xw.Close();            
            //return sb.ToString().Substring(sb.ToString().LastIndexOf("<wpis"));
            return sb.ToString();
        }

        protected void pozycja_PageIndexChanging(object sender, FormViewPageEventArgs e)
        {

        
        }
        #region IEditRegistyItemView Members

        string IEditRegistyItemView.ItemContent
        {
            get { return (pozycja.DataSource as XmlDataSource).Data; }
            set
            {

                IsInvoice = value.Contains("<faktura>tak</faktura>");

                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                xds.XPath = "/pozycja";
                pozycja.DataSource = null;
                if (xds.Data != "")
                {
                    pozycja.DataSource = xds;
                }
                pozycja.DataBind();
                setCategories(IsInvoice);
            }
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistyItemView.Customers
        {
            set
            {
                //DropDownList lista = pozycja.FindControl("fnadawca") as DropDownList;
                //lista.DataSource = value;
                //lista.DataTextField = "Description";
                //lista.DataValueField = "ID";
                //lista.DataBind();
            }
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistyItemView.OrganizationalUnits
        {
            set
            {
                DropDownList lista = pozycja.FindControl("fwydzialZnakReferenta") as DropDownList;
                if (lista != null)
                {
                    lista.DataSource = value;
                    lista.DataTextField = "Description";
                    lista.DataValueField = "ID";
                    lista.DataBind();
                }
            }
        }

        System.Collections.ObjectModel.Collection<Pemi.Esoda.DTO.SimpleLookupDTO> IEditRegistyItemView.DocumentTypes
        {
            set
            {
                DropDownList lista = pozycja.FindControl("frodzajDokumentu") as DropDownList;
                if (lista != null)
                {
                    lista.DataSource = value;
                    lista.DataTextField = "Description";
                    lista.DataValueField = "ID";
                    lista.DataBind();
                }
            }
        }

        bool IEditRegistyItemView.IsInPreviewState
        {
            set { }
        }

        bool IEditRegistyItemView.IsInEditState
        {
            set { pozycja.ChangeMode(FormViewMode.Edit); }
        }

        bool IEditRegistyItemView.IsInInsertState
        {
            set
            {
                pozycja.ChangeMode(FormViewMode.Insert);
            }
        }

        bool IEditRegistyItemView.IsInCustomerInsertState
        {
            set
            {
                if (value)
                {
                    int ct = (pozycja.FindControl("rodzajInteresanta") as RadioButtonList).SelectedIndex;
                    pozycja.FindControl("nowyInteresant").Visible = true;
                    pozycja.FindControl("opcjeNowegoInteresanta").Visible = true;
                    pozycja.FindControl("daneOsoby").Visible = (ct == 0 || ct > 2);
                    pozycja.FindControl("daneFirmy").Visible = (ct > 0 && ct < 3);
                    pozycja.FindControl("interesant").Visible = false;
                    (pozycja.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedIndex = ct < 3 ? ct : 0;
                }
                else
                {
                    int ct = (pozycja.FindControl("rodzajNowegoInteresanta") as RadioButtonList).SelectedIndex;
                    pozycja.FindControl("nowyInteresant").Visible = false;
                    pozycja.FindControl("opcjeNowegoInteresanta").Visible = false;
                    pozycja.FindControl("daneOsoby").Visible = false;
                    pozycja.FindControl("daneFirmy").Visible = false;
                    pozycja.FindControl("interesant").Visible = true;
                    (pozycja.FindControl("rodzajInteresanta") as RadioButtonList).SelectedIndex = ct < 3 ? ct : 0;
                }
            }
        }

        int IEditRegistyItemView.ItemID
        {
            set
            {
                if(!IsInvoice)
                    numerPozycji.Text = string.Format("[numer pozycji w rejestrze: {0}]", value);
                else
                    numerPozycji.Text = string.Format("[numer pozycji w rejestrze faktur: {0}]", value);

                if (pozycja.CurrentMode == FormViewMode.Insert)
                    (pozycja.FindControl("zarezerwowanyNumer") as Literal).Text = value.ToString();
            }
        }

        event EventHandler<ExecutingCommandEventArgs> IEditRegistyItemView.ItemCommand
        {
            add { itemCommand += value; }
            remove { itemCommand -= value; }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IEditRegistyItemView.Employees
        {
            set
            {
                DropDownList lista = pozycja.FindControl("fpracownikZnakReferenta") as DropDownList;
                if (lista != null)
                {
                    lista.DataSource = value;
                    lista.DataTextField = "Description";
                    lista.DataValueField = "ID";
                    lista.DataBind();
                }
            }
        }


        int IEditRegistyItemView.CustomerType
        {
            get
            {
                RadioButtonList rbl = (pozycja.FindControl("rodzajInteresanta") as RadioButtonList);
                if (rbl == null)
                    rbl = (pozycja.FindControl("rodzajNowegoInteresanta") as RadioButtonList);
                return int.Parse(rbl.SelectedValue);
            }
            set
            {
                (pozycja.FindControl("customer") as Interesant).LoadCustomerData(value);
                OnItemCommand(new ExecutingCommandEventArgs("ustawSlownik", "customer"));
            }
        }

        int IEditRegistyItemView.CustomerId
        {
            get
            {
                return (pozycja.FindControl("customer") as Interesant).IdInteresanta;  //WybraneID("fnadawca");
            }
            set
            {
                (pozycja.FindControl("customer") as Interesant).IdInteresanta = value;
            }
        }

        int IEditRegistyItemView.DocumentType
        {
            get
            {
                return WybraneID("frodzajDokumentu");
            }
            set
            {
                (pozycja.FindControl("frodzajDokumentu") as DropDownList).SelectedValue = value.ToString();
            }
        }

        int IEditRegistyItemView.EmployeeId
        {
            get
            {
                return int.Parse((pozycja.FindControl("fpracownikZnakReferenta") as DropDownList).SelectedValue);
            }
            set
            {
                (pozycja.FindControl("fpracownikZnakReferenta") as DropDownList).SelectedValue = value.ToString();
            }
        }

        int IEditRegistyItemView.OrganizationalUnitId
        {
            get
            {
                return WybraneID("fwydzialZnakReferenta");
            }
            set
            {
                (pozycja.FindControl("fwydzialZnakReferenta") as DropDownList).SelectedValue = value.ToString();
                OnItemCommand(new ExecutingCommandEventArgs("ustawSlownik", "employee"));
            }
        }

        int IEditRegistyItemView.CorrespondenceType
        {
            get
            {
                return WybraneID("ftypKorespondencji");
            }
            set
            {
                (pozycja.FindControl("ftypKorespondencji") as DropDownList).SelectedValue = value.ToString();
            }
        }

        int IEditRegistyItemView.DocumentCategory
        {
            get
            {
                return WybraneID("fkategoria");
            }
            set
            {
                (pozycja.FindControl("fkategoria") as DropDownList).SelectedValue = value.ToString();
                setCategories(IsInvoice);
            }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IEditRegistyItemView.CorrespondenceTypes
        {
            set
            {
                DropDownList lista = pozycja.FindControl("ftypKorespondencji") as DropDownList;
                if (lista != null)
                {
                    lista.DataSource = value;
                    lista.DataTextField = "Description";
                    lista.DataValueField = "ID";
                    lista.DataBind();
                }
            }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IEditRegistyItemView.DocumentCategories
        {
            set
            {
                DropDownList lista = pozycja.FindControl("fkategoria") as DropDownList;
                if (lista != null)
                {
                    lista.DataSource = value;
                    lista.DataTextField = "Description";
                    lista.DataValueField = "ID";
                    lista.DataBind();
                }
            }
        }

        string IEditRegistyItemView.UserName
        {
            get { return Membership.GetUser().UserName; }
        }

        string IEditRegistyItemView.UserFullName
        {
            get { return Membership.GetUser().Comment; }
        }

        #endregion

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            if (Session["ESPDocId"] != null)
            {
                docPreview.DocGuid = new Guid(Session["ESPDocId"].ToString());
            }
            else
            {
                if (Session["idDokumentu"] != null)
                {
                    docPreview.DocId = int.Parse(Session["idDokumentu"].ToString());
                }
                else
                {
                    int itId;
                    if (int.TryParse(numerPozycji.Text, out itId))
                    {
                        docPreview.ItemId = itId;
                    }
                    else
                        docPreview.ItemId = presenter.ItemID;
                }
            }
        }

        //protected void WydzialValidate(object source, ServerValidateEventArgs args)
        //{
        //    int num;

        //    if (int.TryParse(args.Value, out num))
        //    {
        //        args.IsValid = (num == 0 ? false : true);
        //    }
        //    else
        //        args.IsValid = false;
        //}
        protected void GoBack(object sender, EventArgs e)
        {
            string parentPage = (string)Session["parentPage"];
            if (parentPage == "a")
            {
                Response.Redirect("PrzegladDziennika.aspx");
            }
            else //if (parentPage == "s")
            {
                Response.Redirect("PrzegladDziennikaSimple.aspx");
            }
        }

        
        #region IEditRegistyItemView Members


        public bool IsDailyLogItemAccessDenied
        {
            set 
            {
                contentPanel.Visible = !value;
                lblDailyLogItemAccessDeniedInfo.Visible = value;
            }
        }



        public bool IsInvoice
        {
            get
            {
                return Session["isRF"] != null || Request.QueryString["rf"] != null;
            }
            set
            {
                _isInvoice = value;
               
            }
        }

        private void setCategories(bool isRF)
        {
            if (!isRF)
                return;
            (pozycja.FindControl("fkategoria") as DropDownList).SelectedValue = new RegistryDAO().GetBuiltinDocumentCategoryId("faktury").ToString();
            (pozycja.FindControl("fkategoria") as DropDownList).Enabled = false;
        }

       

        #endregion
    }
}