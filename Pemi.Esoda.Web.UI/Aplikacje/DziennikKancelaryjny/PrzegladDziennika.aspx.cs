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
using Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI
{
    public partial class PrzegladDziennika : BaseContentPage, IViewRegistryView
    {
        private event EventHandler<PagerEventArgs> pageChanged;

        private event EventHandler<ExecutingCommandEventArgs> executingCommand;

        protected void OnExecutingCommand(ExecutingCommandEventArgs e)
        {
            if (executingCommand != null)
                executingCommand(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewRegistryPresenter presenter = new ViewRegistryPresenter(this, new WebSessionProvider());
            if (this.IsCurrentDailyLogDef)
            {
                singleColumn.Visible = true;
                dailyLogDefInfo.Visible = false;
            }
            else
            {
                singleColumn.Visible = false;
                dailyLogDefInfo.Visible = true;
            }
            presenter.ExecuteCommand += new EventHandler<ExecutingCommandEventArgs>(ExecuteCommand);
            changeDateRangeLink.Visible = (zakresDat.SelectedIndex == 3);
            if (!IsPostBack)
                presenter.Initialize();
        }

        void ExecuteCommand(object sender, ExecutingCommandEventArgs e)
        {
            if (e.CommandSource != null)
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                Label lblID = (Label)row.FindControl("lblID");
                if (lblID != null)
                    ((IViewRegistryView)this).RegistryID = int.Parse(lblID.Text);//Session["registryId"] = lblID.Text;
            }
            switch (e.CommandName)
            {
                case "itemEdit":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument;
                    Session["ESPDocId"] = null;
                    Response.Redirect("EdycjaPozycjiDziennika.aspx?pp=a");
                    break;
                case "itemScans":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument.ToString();
                    Response.Redirect("EdycjaSkanowPozycjiDziennika.aspx?pp=a");
                    break;
                case "itemHistory":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument.ToString();
                    Response.Redirect("SzczegolyPozycjiDziennika.aspx?pp=a");
                    break;
                case "itemDocHistory":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument.ToString();
                    Response.Redirect("HistoriaDokumentuPozycjiDziennika.aspx?pp=a");
                    break;

                case "itemEditRF":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument;
                    Session["ESPDocId"] = null;
                    Response.Redirect("EdycjaPozycjiDziennika.aspx?pp=a&rf=1");
                    break;
                case "itemScansRF":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument.ToString();
                    Response.Redirect("EdycjaSkanowPozycjiDziennika.aspx?pp=a&rf=1");
                    break;
                case "itemHistoryRF":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument.ToString();
                    Response.Redirect("SzczegolyPozycjiDziennika.aspx?pp=a&rf=1");
                    break;
                case "itemDocHistoryRF":
                    //Session["registryId"] = ((IViewRegistryView)this).RegistryID;
                    Session["itemId"] = e.CommandArgument.ToString();
                    Response.Redirect("HistoriaDokumentuPozycjiDziennika.aspx?pp=a&rf=1");
                    break;
            }
        }

        protected void zmianaNumeruStrony(object sender, EventArgs e)
        {
            OnPageChanged(sender, new PagerEventArgs(PagerPage.GoToPage));
        }

        protected void zmianaStrony(object sender, CommandEventArgs e)
        {
            PagerPage eventType = PagerPage.GoToPage;

            switch (e.CommandArgument.ToString().ToLower())
            {
                case "prev": eventType = PagerPage.Previous; break;
                case "next": eventType = PagerPage.Next; break;
                case "first": eventType = PagerPage.First; break;
                case "last": eventType = PagerPage.Last; break;
            }
            OnPageChanged(sender, new PagerEventArgs(eventType));
        }

        protected void wykonaniePolecenia(object sender, GridViewCommandEventArgs e)
        {
          //  OnExecutingCommand(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
            OnExecutingCommand(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument, e.CommandSource));
        }

        protected void OnPageChanged(object sender, PagerEventArgs e)
        {
            if (pageChanged != null)
                pageChanged(this, e);
        }

        protected void czyscFiltry(object sender, EventArgs e)
        {
            snadawca.Text = string.Empty;
            sdataPisma.Text = string.Empty;
            sdataWplywu.Text = string.Empty;
            snumerPisma.Text = string.Empty;
            wartoscKategorii.Text = string.Empty;
            wartoscTypKorespondencji.Text = string.Empty;
            kategoria.SelectedValue = "-1";
            ddlRodzajDokumentu.SelectedValue = "-1";
            typKorespondencji.SelectedValue = "-1";
            ddlStatus.SelectedValue = "-1";
            ddlWydzial.SelectedValue = "-1";
            ddlPracownik.SelectedValue = "-1";
        }

        protected void toggleFilters(object sender, EventArgs e)
        {
            searchTable.Visible = !searchTable.Visible;
            btnToggleFilters.Text = searchTable.Visible ? "ukryj filtry" : "poka¿ filtry";
        }

        #region IViewRegistryView Members

        int IViewRegistryView.CurrentPage
        {
            get
            {
                int nr;
                if (!int.TryParse(nrStrony.SelectedValue, out nr)) return 1;
                return nr;
            }
            set
            {
                nrStrony.SelectedIndexChanged -= zmianaNumeruStrony;
                if (nrStrony.Items.FindByValue(value.ToString()) != null)
                    nrStrony.SelectedValue = value.ToString();
                else
                    nrStrony.SelectedValue = "1";
                nrStrony.SelectedIndexChanged += zmianaNumeruStrony;
            }
        }

        string IViewRegistryView.CurrentDateRange
        {
            get
            {
                return zakresDat.SelectedValue.ToString();
            }
            set
            {
                zakresDat.SelectedValue = value;
            }
        }

        string IViewRegistryView.CurrentDateRangeDescription
        {
            set
            {
                opisOkresu.Text = string.Format("Wybrany zakres dat: {0}", value);
            }
        }

        int IViewRegistryView.PageCount
        {
            get
            {
                return nrStrony.Items.Count;
            }
            set
            {
                int cp = ((IViewRegistryView)this).CurrentPage;
                nrStrony.SelectedIndexChanged -= zmianaNumeruStrony;
                nrStrony.Items.Clear();
                for (int i = 1; i <= value; i++)
                    nrStrony.Items.Add(i.ToString());
                if (cp <= value) nrStrony.SelectedValue = cp.ToString();
                nrStrony.SelectedIndexChanged += zmianaNumeruStrony;
                liczbaStron.Text = value.ToString();
            }
        }

        string IViewRegistryView.RegistryItems
        {
            set
            {
                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                lista.DataSource = null;
                if (xds.Data != "")
                {
                    xds.XPath = "/rejestr/pozycja";
                    lista.DataSource = xds;
                }
                lista.DataBind();
            }
        }

        bool IViewRegistryView.IsInDateChoosingState
        {
            set { przedzialDat.Visible = value; }
        }

        event EventHandler<PagerEventArgs> IViewRegistryView.ActivePageChanged
        {
            add { this.pageChanged += value; }
            remove { this.pageChanged -= value; }
        }

        event EventHandler IViewRegistryView.Filtering
        {
            add { zastosujFiltr.Click += value; }
            remove { zastosujFiltr.Click -= value; }
        }

        event EventHandler IViewRegistryView.ClearFilter
        {
            add { czyscFiltr.Click += value; }
            remove { czyscFiltr.Click -= value; }
        }

        event EventHandler IViewRegistryView.ActiveDateRangeChanging
        {
            add
            {
                zakresDat.SelectedIndexChanged += value;
                changeDateRangeLink.Click += value;
                zastosujFiltr.Click += value;
                czyscFiltr.Click += value;
            }
            remove
            {
                zakresDat.SelectedIndexChanged -= value;
                zastosujFiltr.Click -= value;
            }
        }

        

        event EventHandler IViewRegistryView.ActiveDateRangeChanged
        {
            add { zatwierdzWyborDat.Click += value; }
            remove { zatwierdzWyborDat.Click -= value; }
        }

        event EventHandler<ExecutingCommandEventArgs> IViewRegistryView.CommandExecuting
        {
            add { this.executingCommand += value; }
            remove { this.executingCommand -= value; }
        }

        int IViewRegistryView.RegistryID
        {
            get
            {
                int registryId=0;
                if (Session["registryId"] != null)
                    int.TryParse(Session["registryId"].ToString(), out registryId);
                return registryId;
            }
            set
            {
                Session["registryId"] = value;
            }
        }

        int IViewRegistryView.PageSize
        {
            get
            {
                //if (ViewState["pageSize"] == null)
                //  ViewState["pageSize"] = 10;
                //return (int)ViewState["pageSize"];
                return int.Parse(rozmiarStrony.SelectedValue);
            }
            set
            {
                rozmiarStrony.SelectedValue = value.ToString();
            }
        }

        DateTime IViewRegistryView.StartDate
        {
            get
            {
                DateTime dataOd = DateTime.MinValue;
                //return dataPoczatkowa.SelectedDate.Date;
                if (DateTime.TryParse(txtDataOd.Text, out dataOd))
                    return dataOd;
                else
                    return DateTime.MinValue;
            }
            set
            {
                //dataPoczatkowa.SelectedDate = value.Date;
                txtDataOd.Text = value.ToString("yyyy-MM-dd");
            }
        }

        DateTime IViewRegistryView.EndDate
        {
            get
            {
                DateTime dataDo = DateTime.MaxValue;
                if (DateTime.TryParse(txtDataDo.Text, out dataDo))
                    return dataDo;
                else
                    return DateTime.MaxValue;

                //return dataKoncowa.SelectedDate.Date;
            }
            set
            {
                txtDataDo.Text = value.ToString("yyyy-MM-dd");
                //dataKoncowa.SelectedDate = value.Date;
            }
        }

        event EventHandler IViewRegistryView.AcquiringItemID
        {
            add { rezerwacja.Click += value; }
            remove { rezerwacja.Click -= value; }
        }

        event EventHandler IViewRegistryView.CorrespondenceCategoriesChanged
        {
            add { kategoria.SelectedIndexChanged += value; }
            remove { kategoria.SelectedIndexChanged -= value; }
        }

        event EventHandler IViewRegistryView.CorrespondenceDeptChanged
        {
            add { ddlWydzial.SelectedIndexChanged += value; }
            remove { ddlWydzial.SelectedIndexChanged -= value; }
        }

        string IViewRegistryView.SearchIncomeDate
        {
            get { return sdataWplywu.Text; }
        }

        string IViewRegistryView.SearchDocumentDate
        {
            get { return sdataPisma.Text; }
        }

        string IViewRegistryView.SearchDocumentNumber
        {
            get { return snumerPisma.Text; }
        }

        string IViewRegistryView.SearchSenderName
        {
            get { return snadawca.Text; }
        }




        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryView.CorrespondenceTypes
        {
            set
            {
                typKorespondencji.DataSource = value;
                typKorespondencji.DataTextField = "Description";
                typKorespondencji.DataValueField = "ID";
                typKorespondencji.DataBind();
            }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryView.CorrespondenceCategories
        {
            set
            {
                kategoria.DataSource = value;
                kategoria.DataTextField = "Description";
                kategoria.DataValueField = "ID";
                kategoria.DataBind();
            }

        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryView.CorrespondenceKinds
        {
            set
            {
                ddlRodzajDokumentu.DataSource = value;
                ddlRodzajDokumentu.DataTextField = "Description";
                ddlRodzajDokumentu.DataValueField = "ID";
                ddlRodzajDokumentu.DataBind();
            }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryView.CorrespondenceStatus
        {
            set
            {
                ddlStatus.DataSource = value;
                ddlStatus.DataTextField = "Description";
                ddlStatus.DataValueField = "ID";
                ddlStatus.DataBind();
            }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryView.CorrespondenceDepts
        {
            set
            {
                ddlWydzial.DataSource = value;
                ddlWydzial.DataTextField = "Description";
                ddlWydzial.DataValueField = "ID";
                ddlWydzial.DataBind();
                ddlPracownik.SelectedValue = "-1";
            }
        }

        System.Collections.ObjectModel.Collection<SimpleLookupDTO> IViewRegistryView.CorrespondenceWorkers
        {
            set
            {
                ddlPracownik.DataSource = value;
                ddlPracownik.DataTextField = "Description";
                ddlPracownik.DataValueField = "ID";
                ddlPracownik.DataBind();
                ddlPracownik.SelectedValue = "-1";
            }
        }

        int IViewRegistryView.SearchCorrespondenceStatus
        {
            get { return int.Parse(ddlStatus.SelectedValue); }
        }

        int IViewRegistryView.SearchCorrespondenceDept
        {
            get
            {
                //if (ddlWydzial.SelectedValue.Equals("-1"))
                //    return string.Empty;
                int wydzId = -1;
                if (ddlWydzial.SelectedValue != null)
                    int.TryParse(ddlWydzial.SelectedValue, out wydzId);
                return wydzId;
            }
        }

        int IViewRegistryView.SearchCorrespondenceWorker
        {
            get
            {
                // if (ddlPracownik.SelectedValue.Equals("-1"))
                //  return string.Empty;
                int pracId = -1;
                if (ddlPracownik.SelectedValue != null)
                    int.TryParse(ddlPracownik.SelectedValue, out pracId);
                return pracId;
                //return ddlPracownik.SelectedValue;
            }
        }

        int IViewRegistryView.SearchCorrespondenceCategory
        {
            get {
                int corrCat = -1;
                if (kategoria.SelectedValue != null)
                    int.TryParse(kategoria.SelectedValue, out corrCat);
                return corrCat; 
            }
        }

        int IViewRegistryView.SearchCorrespondenceKind
        {
            get {
                int corrKind = -1;
                if (ddlRodzajDokumentu.SelectedValue != null)
                    int.TryParse(ddlRodzajDokumentu.SelectedValue, out corrKind);
                return corrKind;
            }
        }

        int IViewRegistryView.SearchCorrespondenceType
        {
            get {
                int corrType = -1;
                if (typKorespondencji.SelectedValue != null)
                    int.TryParse(typKorespondencji.SelectedValue, out corrType);
                return corrType;
            }
        }

        string IViewRegistryView.SearchCategoryValue
        {
            get { return wartoscKategorii.Text; }
        }

        string IViewRegistryView.SearchTypeValue
        {
            get { return wartoscTypKorespondencji.Text; }
        }

        public bool IsCurrentDailyLogDef
        {
            set
            {
                ViewState["IsCurrentDailyLogDef"] = value;
            }

            get
            {
                if (ViewState["IsCurrentDailyLogDef"] != null)
                    return (bool)ViewState["IsCurrentDailyLogDef"];
                else return false;
            }

        }
        #endregion

        protected void esp_Click(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["espticket"] != null)
            {
                Response.Redirect("~/Aplikacje/DziennikKancelaryjny/DokumentyOczekujace.aspx");
            }
            else
                WebMsgBox.Show(this, "Brak autoryzacji dla skzynki podawczej.");
        }

        protected void epuap_Click(object sender, EventArgs e)
        {
            Session["registryId"] = ((IViewRegistryView)this).RegistryID;
            Response.Redirect("~/Aplikacje/DziennikKancelaryjny/ePUAPIntegration.aspx?pp=a");
        }

        protected void rezerwujPozycjeRF(object sender, EventArgs e)
        {
            Session["itemIdRequest"] = Session["registryId"];
            Session["itemRFRequest"] = true;
            ExecuteCommand(this, new ExecutingCommandEventArgs("itemEdit", null));
        }

        protected void toggleItemType(object sender, EventArgs e)
        {

        }
        public bool ShowInvoices
        {
            get
            {
                return rbInvoices.Checked;
            }
            set
            {
                rbInvoices.Checked = value;
                rbDocuments.Checked = !value;
            }
        }
    }
}