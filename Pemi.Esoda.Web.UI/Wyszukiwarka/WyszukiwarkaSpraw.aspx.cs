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
using System.Collections.ObjectModel;
using Pemi.Esoda.DataAccess;
using System.Collections.Generic;

namespace Pemi.Esoda.Web.UI
{
    public partial class WyszukiwarkaSpraw : BaseContentPage, IViewCaseSearchView
    {
        private ViewCaseSearchPresenter presenter;

        public event EventHandler<PagerEventArgs> PageChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            presenter = new ViewCaseSearchPresenter(this, new WebSessionProvider());

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

        protected void zmianaStronyAktualnej(object sender, EventArgs e)
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

        #region IViewCaseSearchView Members

        public SearchCaseConditions SearchConditions
        {
            get
            {
                SearchCaseConditions conditions = new SearchCaseConditions();

                //conditions.ClientID = int.Parse(clients.SelectedValue);
                conditions.ClientName = txtClient.Text;
                conditions.DepartmentID = int.Parse(departments.SelectedValue);

                DateTime dt;
                if (DateTime.TryParse(startDate.Text, out dt)) conditions.StartDate = dt;
                if (DateTime.TryParse(endDate.Text, out dt)) conditions.EndDate = dt;

                conditions.StartPage = CurrentPage;
                conditions.PageSize = PageSize;

                return conditions;
            }
            set
            {
                if (value != null)
                {
                    //clients.SelectedValue = value.ClientID.ToString();
                    txtClient.Text = value.ClientName;
                    departments.SelectedValue = value.DepartmentID.ToString();
                    startDate.Text = value.StartDate.HasValue ? value.StartDate.Value.ToShortDateString() : string.Empty;
                    endDate.Text = value.EndDate.HasValue ? value.EndDate.Value.ToShortDateString() : string.Empty;
                }
            }
        }

        public IList<SearchCaseResultItem> SearchResults
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
                departments.DataSource = value;
                departments.DataBind();
            }
        }

        public int CurrentPage
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

        public int PageCount
        {
            get
            {
                return nrStrony.Items.Count;
            }
            set
            {
                int cp = ((IViewCaseSearchView)this).CurrentPage;
                nrStrony.SelectedIndexChanged -= zmianaNumeruStrony;
                nrStrony.Items.Clear();
                for (int i = 1; i <= value; i++)
                    nrStrony.Items.Add(i.ToString());
                if (cp <= value) nrStrony.SelectedValue = cp.ToString();
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

        #endregion

        protected void searchResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            int caseID = int.Parse(e.CommandArgument.ToString());

            Page.Response.Redirect(String.Format("~/Sprawy/Sprawa.aspx?id={0}", caseID));
            
        }
    }
}
