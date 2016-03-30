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
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI.Akcje
{
	public partial class PrzypisanieDokumentuDoSprawy : BaseContentPage, IAssignDocumentToCaseView
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AssignDocumentToCasePresenter presenter = new AssignDocumentToCasePresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {
                presenter.Initialize();
                hfCustomerId.Value = "-1";
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

        void customer_OnCustomerAdded(object sender, EventArgs e)
        {
            lnkSelectCustomer.Visible = (customer.IdInteresanta > 0);
        }

        void customer_SearchCustomers(object sender, SearchCustomersEventArgs e)
        {
            customer.Visible = false;
            lnkSearchAgain.Visible = true;
        }

        private void onSelectCustomer(object sender, EventArgs e)
        {
            if (customer.IdInteresanta > 0)
            {
                hfCustomerId.Value = customer.IdInteresanta.ToString();
                lblInteresant.Text = (customer.Nazwa.Length > 0) ? customer.Nazwa : customer.Imie + " " + customer.Nazwisko;
                customersList.Visible = false;
                customer.Visible = false;
                lnkSearchAgain.Visible = false;
                lnkSelectCustomer.Visible = false;
                cmpCustomerId.Text = "";
            }
        }
        
        event EventHandler OnAssignToCase;
        event EventHandler OnAssignToNewCase;
        public event EventHandler YearChanged;

        private void OnYearChanged(EventArgs e)
        {
            if (YearChanged != null)
                YearChanged(this, e);
        }

		#region IAssignDocumentToCaseView Members

        int IAssignDocumentToCaseView.DocumentId
        {
            get { return CoreObject.GetId(Request); }
        }

		Collection<SimpleLookupDTO> IAssignDocumentToCaseView.BriefcaseList
		{
			set { 
				teczka.DataSource = value;
				teczka.DataValueField = "ID";
				teczka.DataTextField = "Description";
				teczka.DataBind();
			}
		}

		Collection<SimpleLookupDTO> IAssignDocumentToCaseView.CaseNumbers
		{
			set {
				numerSprawy.DataSource = value;
				numerSprawy.DataValueField = "ID";
				numerSprawy.DataTextField = "Description";
				numerSprawy.DataBind();
			}
		}

		Collection<SimpleLookupDTO> IAssignDocumentToCaseView.CaseKind
		{
			set {
				rodzajSprawy.DataSource = value;
				rodzajSprawy.DataValueField = "ID";
				rodzajSprawy.DataTextField = "Description";
				rodzajSprawy.DataBind();
			}
		}
        
        public Collection<SimpleLookupDTO> AvailableBriefcasesYears
        {
            set 
            {
                dostepneLata.DataSource = value;
                dostepneLata.DataBind();
                dostepneLata.SelectedValue = DateTime.Today.Year.ToString();
            }
        }
        
		int IAssignDocumentToCaseView.SelectedBriefcase
		{
			get
			{
				return int.Parse(teczka.SelectedValue);
			}
			set
			{
				teczka.SelectedValue = value.ToString();
			}
		}

		int IAssignDocumentToCaseView.SelectedCaseNumber
		{
			get
			{
				return numerSprawy.SelectedValue!=""?int.Parse(numerSprawy.SelectedValue):0;
			}
			set
			{
				numerSprawy.SelectedValue = value.ToString();
			}
		}

		int IAssignDocumentToCaseView.SelectedCaseKind
		{
			get
			{
				return int.Parse(rodzajSprawy.SelectedValue);
			}
			set
			{
				rodzajSprawy.SelectedValue = value.ToString();
			}
		}

		string IAssignDocumentToCaseView.SelectedCaseDescription
		{
			get
			{
				return opisSprawy.Text;
			}
			set
			{
				opisSprawy.Text = value;
			}
		}

		DateTime? IAssignDocumentToCaseView.DocumentDate
		{
			get
			{
				DateTime t;
				if(dataPisma.Text.Length>0 && !DateTime.TryParse(dataPisma.Text,out t))
                    throw new FormatException(string.Format("Niepoprawny format daty! {0}", dataPisma.Text));
				if (dataPisma.Text.Length > 0 && DateTime.TryParse(dataPisma.Text, out t)) 
				return t;
			return null;
			}
			set
			{
				dataPisma.Text = value.Value.ToString("yyyy-MM-dd");
			}
		}

		string IAssignDocumentToCaseView.DocumentReferenceNumber
		{
			get
			{
				return znakPisma.Text;
			}
			set
			{
				znakPisma.Text = value;
			}
		}

		event EventHandler IAssignDocumentToCaseView.AssigningToNewCase
		{
			add { OnAssignToNewCase += value; }
			remove { OnAssignToNewCase -= value; }
		}

		event EventHandler IAssignDocumentToCaseView.AssigningToSelectedCase
		{
			add { OnAssignToCase += value; }
			remove { OnAssignToCase += value; }
		}

		event EventHandler IAssignDocumentToCaseView.CaseTypeSelected
		{
			add { teczka.SelectedIndexChanged += value; }
			remove { teczka.SelectedIndexChanged -= value; }
		}

		string IAssignDocumentToCaseView.DocumentSender
		{
			get
			{
                //if (ddlInteresant.SelectedIndex > -1)
                //    return ddlInteresant.SelectedValue;
                return customer.IdInteresanta.ToString();
                //if (!hfCustomerId.Value.Equals(""))
                //    return hfCustomerId.Value;
                //else
                //    return string.Empty;
			}
			set
			{
				//interesant.Text = value;
                lblInteresant.Text = value;
			}
		}
        	string IAssignDocumentToCaseView.DocumentSenderID
		{
			get
			{
                //if (ddlInteresant.SelectedIndex > -1)
                //    return ddlInteresant.SelectedValue;
                //if (!hfCustomerId.Value.Equals(""))
                //    return hfCustomerId.Value;
                //else
                //    return string.Empty;
                return customer.IdInteresanta.ToString();
			}
			set
			{
               hfCustomerId.Value = value;
                int id;
                if (int.TryParse(value, out id))
                    customer.IdInteresanta = id;
                
			}
		}

       

		int IAssignDocumentToCaseView.CaseId
		{
			set {
                Response.Redirect("~/sprawy/DokumentySprawy.aspx?id="+value.ToString(),false);
			}
		}

        bool IAssignDocumentToCaseView.IsNewCase
        {
            set
            {
                przypiszDoNowej.Visible = value;
                przypiszDoIstniejacej.Visible = !value;
                opcjeNowejSprawy.Visible = value;
                numerSprawy.Visible = !value;
                Label1.Visible = !value;
                rfvSprawa.Enabled = !value;
            }
        }

        #endregion

        protected void ddlInteresant_DataBinding(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Clear();
            ((DropDownList)sender).Items.Insert(0, new ListItem("-- wybierz --", string.Empty));
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            //calDataPisma.Visible = true;
            //lnkHideCal.Visible = true;
        }

        protected void lnkHideCal_Click(object sender, EventArgs e)
        {
            //calDataPisma.Visible = false;
            //lnkHideCal.Visible = false;
        }

        protected void calDataPisma_SelectionChanged(object sender, EventArgs e)
        {
            //dataPisma.Text = calDataPisma.SelectedDate.ToShortDateString();
            //calDataPisma.Visible = false;
            //lnkHideCal.Visible = false;
        }

        protected void przypiszDoIstniejacej_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (OnAssignToCase != null)
                    OnAssignToCase(sender, e);
            }
        }

        protected void przypiszDoNowej_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (OnAssignToNewCase != null)
                    OnAssignToNewCase(sender, e);
            }
        }

        protected void lnkZmien_Click(object sender, EventArgs e)
        {
            customer.Visible = true;
            lnkSelectCustomer.Visible = (customer.IdInteresanta > -1);
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

        protected void cmpCustomerId_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = customer.IdInteresanta > 0;
        }



        #region IAssignDocumentToCaseView Members


        public int SelectedYear
        {
            get { return int.Parse(dostepneLata.SelectedValue); }
        }
        
        #endregion

        protected void dostepneLata_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnYearChanged(e);
        }
    }
}