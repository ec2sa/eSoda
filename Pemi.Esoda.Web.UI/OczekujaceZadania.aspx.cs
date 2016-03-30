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
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI
{
    public partial class OczekujaceZadania : BaseContentPage, IAssignedItemsView
    {
        private event EventHandler<ExecutingCommandEventArgs> wykonaniePolecenia;
        private AssignedItemsPresenter presenter;
        protected void OnWykonaniePolecenia(ExecutingCommandEventArgs e)
        {
            if (wykonaniePolecenia != null)
                wykonaniePolecenia(this, e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
       
            presenter = new AssignedItemsPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {

                string ticket = new RSSDAO().GetTicketforUser((Guid)Membership.GetUser().ProviderUserKey, 1);

                rssLink.Attributes["href"] = string.Format("rss/oczekujaceDokumenty.aspx?ticket={0}", ticket);
                Session.Remove("taskFilter");
                Session.Remove("taskFilter2");
                Session.Remove("idDokumentu");
                Session.Remove("idSprawy");
                Session["entryPoint"] = "OZ";
                Session.Remove("DocumentSearchCriteria");
                //presenter.Initialize();

                Session["taskFilter2"] = "doPracownika=1 and typ <> 'Sprawa'";
                AwaitingTasksBind();

            }
        }

        protected void wykonaj(object sender, CommandEventArgs e)
        {
            OnWykonaniePolecenia(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        #region IAssignedItemsView Members

        int IAssignedItemsView.ObjectId
        {
            set { ; }
            get { return 0; }
        }

        string IAssignedItemsView.Items
        {
            set
            {
        
                if (Session["taskFilter2"] == null)
                    filtruj(null, null);
                else
                {

                    odsAwaitingTasks.SelectParameters.Clear();
                    odsAwaitingTasks.SelectParameters.Add("userId", Membership.GetUser().ProviderUserKey.ToString());
                    odsAwaitingTasks.SelectParameters.Add("sortParam", gvAwaitingTasks.SortExpression);
                    odsAwaitingTasks.SelectParameters.Add("filterParam", (Session["taskFilter2"] != null) ? Session["taskFilter2"].ToString() : "");
                    gvAwaitingTasks.DataBind();
                }
            }
        }

        Guid IAssignedItemsView.UserId
        {
            get
            {
                return (Guid)Membership.GetUser().ProviderUserKey;
            }
        }


        event EventHandler<ExecutingCommandEventArgs> IAssignedItemsView.ExecutingCommand
        {
            add { wykonaniePolecenia += value; }
            remove { wykonaniePolecenia -= value; }
        }

        string IAssignedItemsView.TargetView
        {
            set
            {
                //Response.Redirect(string.Format("~/{0}.aspx", value));
                Response.Redirect(string.Format("~/{0}", value)); // CHECK
            }
        }

        #endregion

        protected void lista_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            object x = e.CommandSource;
            OnWykonaniePolecenia(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        protected void filtruj(object sender, EventArgs e)
        {
      
            //StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            if (!ckbWszystkie.Checked && !cbDoWiadomosci.Checked)
            {
              //  sb.Append(string.Format("[doPracownika='{0}' and doWiadomosci='-1'", ckbMoje.Checked ? "1" : "0"));
                sb2.Append(string.Format("doPracownika={0}  and doWiadomosci<>1", ckbMoje.Checked ? "1" : "0"));
            }
            else if(ckbWszystkie.Checked)
            {
             //   sb.Append(string.Format("[(doPracownika='0' or doPracownika='1') and doWiadomosci!=1"));
                sb2.Append(string.Format("(doPracownika=1 OR doPracownika=0) and doWiadomosci<>1"));
            }
            else if (cbDoWiadomosci.Checked)
            {
              //    sb.Append(" [doPracownika=1 and doWiadomosci=1");
                  sb2.Append(" (doPracownika=1 and doWiadomosci=1)");
            }

           
         

            if (!ckbDokumenty.Checked)
            {
              //  sb.Append(" and typ!='Dokument'");
                sb2.Append(" AND typ <> 'Dokument' ");
            }
            if (!ckbSprawy.Checked)
            {
             //   sb.Append(" and typ!='Sprawa'");
                sb2.Append(" and typ <> 'Sprawa'");
            }
            if (!ckbFaktury.Checked)
            {
              //  sb.Append(" and typ!='Faktura'");
                sb2.Append(" and typ <> 'Faktura'");
            }
            if (ddlTypInteresanta.SelectedValue != "-1")
            {
                sb2.Append(" and idTypuInteresanta=" + ddlTypInteresanta.SelectedValue);
            }
         //   sb.Append("]");
          
            //Session["taskFilter"] = sb.ToString();
            Session["taskFilter2"] = sb2.ToString();
            //presenter.Initialize();
            System.Diagnostics.Debug.WriteLine("************************************************");
            //System.Diagnostics.Debug.WriteLine(sb.ToString());
            System.Diagnostics.Debug.WriteLine(sb2.ToString());
            System.Diagnostics.Debug.WriteLine("************************************************");
            AwaitingTasksBind();
            
        }

        private void AwaitingTasksBind()
        {
            odsAwaitingTasks.SelectParameters.Clear();
            odsAwaitingTasks.SelectParameters.Add("userId", Membership.GetUser().ProviderUserKey.ToString());
            odsAwaitingTasks.SelectParameters.Add("sortParam", gvAwaitingTasks.SortExpression);
            odsAwaitingTasks.SelectParameters.Add("filterParam", (Session["taskFilter2"] != null) ? Session["taskFilter2"].ToString() : "");
            odsAwaitingTasks.SelectParameters.Add("doWiadomosci", cbDoWiadomosci.Checked.ToString());
            gvAwaitingTasks.DataBind();
        }

        protected void ddlPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvAwaitingTasks.PageIndex = int.Parse(((DropDownList)sender).SelectedValue);
            gvAwaitingTasks.DataBind();
        }

        protected void ddlPageSelector_DataBinding(object sender, EventArgs e)
        {

        }

        protected void ddlPageSelector_DataBound(object sender, EventArgs e)
        {
            if (gvAwaitingTasks.PageCount != ((DropDownList)sender).Items.Count)
            {
                for (int i = 0; i < gvAwaitingTasks.PageCount; i++)
                    ((DropDownList)sender).Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                ((DropDownList)sender).SelectedIndex = gvAwaitingTasks.PageIndex;
            }
        }
    }
}