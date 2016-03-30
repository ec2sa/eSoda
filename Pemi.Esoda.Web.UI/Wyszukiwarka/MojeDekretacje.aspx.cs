using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Presenters.Interfaces;

namespace Pemi.Esoda.Web.UI.Wyszukiwarka
{
    public partial class MojeDekretacje : BaseContentPage, IViewMyDecretationsView
    {
        private ViewMyDecretationsSearchPresenter presenter;

        public event EventHandler<PagerEventArgs> PageChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            presenter = new ViewMyDecretationsSearchPresenter(this, new WebSessionProvider());

            if (!Page.IsPostBack)
            {
                presenter.Initialize();
            }
            presenter.OnViewLoaded();

        }

        protected void search_Click(object sender, EventArgs e)
        {
            presenter.OnSearch();
        }

        protected void OnPageChanged(object sender, PagerEventArgs e)
        {
            if (PageChanged != null)
                PageChanged(this, e);
        }

        protected void zmianaNumeruStrony(object sender, EventArgs e)
        {
            CurrentPage = 1;
            OnPageChanged(sender, new PagerEventArgs(PagerPage.GoToPage));
        }
        
        protected void zmianaStrony(object sender, CommandEventArgs e)
        {
            PagerPage eventType = PagerPage.GoToPage;

            switch (e.CommandArgument.ToString().ToLower())
            {
                case "prev":
                    eventType = PagerPage.Previous;
                    break;
                case "next":
                    eventType = PagerPage.Next;
                    break;
                case "first":
                    eventType = PagerPage.First;
                    break;
                case "last":
                    eventType = PagerPage.Last;
                    break;
            }
            OnPageChanged(sender, new PagerEventArgs(eventType));
        } 

        public MyDecretationsSearchConditions SeachConditions
        {
            get
            {
                MyDecretationsSearchConditions conditions = new MyDecretationsSearchConditions();

                //conditions.ClientID = int.Parse(clients.SelectedValue);
                conditions.SenderName = txtClient.Text;
                int rn;
                if (int.TryParse(txtNr.Text, out rn))
                    conditions.RegistryNumber = rn;
                DateTime dt;
                if (DateTime.TryParse(startDate.Text, out dt))
                    conditions.StartDate = dt;
                if (DateTime.TryParse(endDate.Text, out dt))
                    conditions.EndDate = dt;
                conditions.StartPage = CurrentPage;
                conditions.PageSize = PageSize;
                return conditions;
            }
            set
            {
                if (value != null)
                {
                    //clients.SelectedValue = value.ClientID.ToString();
                    txtClient.Text = value.SenderName;
                    txtNr.Text = value.RegistryNumber.HasValue ? value.RegistryNumber.ToString() : string.Empty;
                    startDate.Text = value.StartDate.HasValue ? value.StartDate.Value.ToShortDateString() : string.Empty;
                    endDate.Text = value.EndDate.HasValue ? value.EndDate.Value.ToShortDateString() : string.Empty;
                }
            }
        }

        public IList<MyDecretationsSearchResult> SearchResults
        {
            set
            {
                searchResults.DataSource = value;
                searchResults.DataBind();
            }
        }

        public string Message
        {
            set { lblMessage.Text = value; }
        }

        public IList<SimpleLookupDTO> Clients
        {
            set
            {
                //clients.DataSource = value;
                //clients.DataBind();
            }
        }

        public IList<SimpleLookupDTO> Departments
        {
            set
            {
                ;
            }
        }

        public int CurrentPage
        {
            get
            {
                int nr;
                if (!int.TryParse(nrStrony.SelectedValue, out nr))
                    return 1;
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

        public int PageCount
        {
            get
            {
                return nrStrony.Items.Count;
            }
            set
            {
                int cp = ((IViewMyDecretationsView)this).CurrentPage;
                nrStrony.SelectedIndexChanged -= zmianaNumeruStrony;
                nrStrony.Items.Clear();
                for (int i = 1; i <= value; i++)
                    nrStrony.Items.Add(i.ToString());
                if (cp <= value)
                    nrStrony.SelectedValue = cp.ToString();
                nrStrony.SelectedIndexChanged += zmianaNumeruStrony;
                liczbaStron.Text = value.ToString();
            }
        }

        //ile rekordow na stronie
        public int PageSize
        {
            get
            {
                return int.Parse(rozmiarStrony.SelectedValue);
            }
            set
            {
                rozmiarStrony.SelectedValue = value.ToString();
            }
        }

 
        protected void searchResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int documentID = int.Parse(e.CommandArgument.ToString());

            Page.Response.Redirect(String.Format("~/Dokumenty/Dokument.aspx?id={0}", documentID));
        }

        protected void zmianaStronyAktualnej(object sender, EventArgs e)
        {            
            OnPageChanged(sender, new PagerEventArgs(PagerPage.GoToPage));
        }
    }
}
