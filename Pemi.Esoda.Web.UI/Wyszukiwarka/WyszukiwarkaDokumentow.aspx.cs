using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.Presenters;
using Pemi.Esoda.DTO;
using System.Web.Security;

namespace Pemi.Esoda.Web.UI
{
    public partial class WyszukiwarkaDokumentow : BaseContentPage, IViewDocumentSearchView
    {
        private ViewDocumentSearchPresenter presenter;
        public event EventHandler<PagerEventArgs> PageChanged;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            presenter = new ViewDocumentSearchPresenter(this, new WebSessionProvider());

            if (!Page.IsPostBack)
            {
                presenter.Initialize();
            }
            presenter.OnViewLoaded();
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            presenter.OnSearch();
        }

        protected void ddlCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.OnCategoryChange(int.Parse(ddlCategories.SelectedValue));
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

        #region IViewDocumentSearchView Members

        public SearchDocumentConditions SearchConditions
        {
            get
            {
                SearchDocumentConditions conditions = new SearchDocumentConditions();

                conditions.UserGuid = (Guid)Membership.GetUser().ProviderUserKey;
                conditions.HasExtendedSearchRole = Roles.IsUserInRole("WyszukiwanieZaawansowane");

                int dca, dty;
                conditions.DocumentCategory = int.TryParse(ddlCategories.SelectedValue, out dca)==true?dca:-1;
                conditions.DocumentType = int.TryParse(ddlTypes.SelectedValue, out dty)==true?dty:-1;
                
                conditions.DocumentNumber = tbDocumentNumber.Text;

                int sn;
                conditions.SystemNumber = int.TryParse(tbSystemNumber.Text, out sn) == true ? sn : -1;
                conditions.Mark = tbMark.Text;
                conditions.ClientName = tbClientName.Text;

                int sts;
                conditions.Text = tbText.Text;
                conditions.SearchContent = cbSearchContent.Checked;
                conditions.SearchDescription = cbSearchDescription.Checked;

                if (cblStatuses.Items != null)
                {
                    for (int i = 0; i < cblStatuses.Items.Count; i++)
                    {
                        if (cblStatuses.Items[i].Selected)
                        {
                            conditions.Status += string.Format("{0},", cblStatuses.Items[i].Value);
                        }
                    }
                    if (conditions.Status != null && conditions.Status.EndsWith(","))
                        conditions.Status = conditions.Status.Substring(0, conditions.Status.Length - 1);
                }
                DateTime dt;
                if (DateTime.TryParse(tbDateFrom.Text, out dt)) conditions.DocumentStartDate = dt;
                if (DateTime.TryParse(tbDateTo.Text, out dt)) conditions.DocumentEndDate = dt;

                conditions.StartPage = CurrentPage;
                conditions.PageSize = PageSize;

                return conditions;
            }
            set
            {
                if (value != null)
                {
                    ddlCategories.SelectedValue = value.DocumentCategory.ToString();
                    ddlTypes.SelectedValue = value.DocumentType.ToString();

                    tbDocumentNumber.Text = value.DocumentNumber;
                    tbSystemNumber.Text = value.SystemNumber.ToString();
                    tbMark.Text = value.Mark;
                    tbClientName.Text = value.ClientName;

                    tbText.Text = value.Text;
                    cbSearchDescription.Checked = value.SearchDescription;
                    cbSearchContent.Checked = value.SearchContent;
                    
                    tbDateFrom.Text = value.DocumentStartDate.HasValue ? value.DocumentStartDate.Value.ToShortDateString() : string.Empty;
                    tbDateTo.Text = value.DocumentEndDate.HasValue ? value.DocumentEndDate.Value.ToShortDateString() : string.Empty;
                }
            }
        }

        public SearchCriteriasState SearchCriteriaState
        {
            get
            {
                SearchCriteriasState state = new SearchCriteriasState();
                state.Category = GetDropDownText(ddlCategories);
                state.Type = GetDropDownText(ddlTypes);

                if (cblStatuses.Items != null)
                {
                    for (int i = 0; i < cblStatuses.Items.Count; i++)
                    {
                        if (cblStatuses.Items[i].Selected)
                        {
                            state.Status += string.Format("{0},", cblStatuses.Items[i].Text);
                        }
                    }
                    if (state.Status != null && state.Status.EndsWith(","))
                        state.Status = state.Status.Substring(0, state.Status.Length - 1);
                }
                
                state.ClientName = tbClientName.Text;
                state.DocumentNumber = tbDocumentNumber.Text;
                state.Mark = tbMark.Text;
                state.DocumentStartDate = tbDateFrom.Text;
                state.DocumentEndDate = tbDateTo.Text;
                state.SystemNumber = tbSystemNumber.Text;
                state.Text = tbText.Text;

                return state;
            }
        }

        public IList<SearchDocumentResultItem> SearchResults
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

        public IList<SimpleLookupDTO> Statuses
        {
            set
            {
                //ddlStatuses.DataTextField = "Description";
                //ddlStatuses.DataValueField = "ID";
                //ddlStatuses.DataSource = value;
                //ddlStatuses.DataBind();

                cblStatuses.DataSource = value;
                cblStatuses.DataTextField = "Description";
                cblStatuses.DataValueField = "ID";
                cblStatuses.DataBind();
            }
        }

        public IList<SimpleLookupDTO> Categories
        {
            set
            {
                ddlCategories.DataTextField = "Description";
                ddlCategories.DataValueField = "ID";
                ddlCategories.DataSource = value;
                ddlCategories.DataBind();
            }
        }

        public IList<SimpleLookupDTO> Types
        {
            set
            {
                ddlTypes.DataTextField = "Description";
                ddlTypes.DataValueField = "ID";
                ddlTypes.DataSource = value;
                ddlTypes.DataBind();
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
                int cp = ((IViewDocumentSearchView)this).CurrentPage;
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

        public void SaveSearchState()
        {
            Session["DocumentSearchCriteria"] = SearchCriteriaState;
        }

        #endregion

        private string GetDropDownText(DropDownList ddl)
        {
            if (ddl.SelectedValue != null && ddl.SelectedValue != "-1" && ddl.SelectedItem != null && ddl.SelectedItem.Text != "-- dowolny --")
            {
                return ddl.SelectedItem.Text;
            }
            return null;
        }

        protected void searchResults_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int documentID = int.Parse(e.CommandArgument.ToString());

            Page.Response.Redirect(String.Format("~/Dokumenty/Dokument.aspx?id={0}", documentID));
        }

        protected string FormatDocURL(int docID, bool isInContent, bool isInDesc, string where)
        {
            where = HttpUtility.UrlEncode(where);
            
            string link = string.Format("/Dokumenty/Dokument.aspx?ID={0}&c={1}&d={2}&w={3}",docID, isInContent, isInDesc, where);

            int pos = Request.Url.AbsoluteUri.IndexOf("/Wyszukiwarka/WyszukiwarkaDokumentow.aspx");
            return Request.Url.AbsoluteUri.Substring(0,pos) + link;
        }
        
    }
}
