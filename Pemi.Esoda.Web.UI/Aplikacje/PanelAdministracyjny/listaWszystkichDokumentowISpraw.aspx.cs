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
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using Pemi.Esoda.Presenters;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class listaWszystkichDokumentowISpraw : BaseContentPage, IAssignedItemsView
    {
        private event EventHandler<ExecutingCommandEventArgs> wykonaniePolecenia;
        private AssignedItemsPresenter presenter;
        protected void OnWykonaniePolecenia(ExecutingCommandEventArgs e)
        {
            if (wykonaniePolecenia != null)
                wykonaniePolecenia(this, e);
        }

        int IAssignedItemsView.ObjectId
        {
            get { return 0;}
            set { ;}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter = new AssignedItemsPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {
                Session["PreviousPageUrl"] = null;
                Session.Remove("DocCaseFilter");
                presenter.Initialize();
            }
                //LoadDocCaseList();
        }

        //private void LoadDocCaseList()
        //{
        //    XmlDataSource xds = new XmlDataSource();
        //    ActionDAO ad = new ActionDAO();

        //    using (XmlReader xdr = ad.GetDocCaseFullList())
        //    {
        //        if (xdr.Read())
        //            xds.Data = xdr.ReadOuterXml();
        //        else
        //            xds.Data = string.Empty;
        //        xds.EnableCaching = false;
        //        gvCaseDocList.DataSource = null;
        //        if (xds.Data != "")
        //        {
        //            xds.XPath = "/zadania/zadanie";
        //            if (Session["DocCaseFilter"] != null)
        //                xds.XPath += Session["DocCaseFilter"].ToString();
        //            gvCaseDocList.DataSource = xds;
        //        }
        //    }
        //    gvCaseDocList.DataBind();
        //}

        protected void filtruj(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            if (!ckbDokumenty.Checked)
            {
                sb.Append("[typ!='Dokument'");
                sb2.Append("typ <> 'Dokument'");
            }
            if (!ckbSprawy.Checked)
            {
                if (sb.Length > 0 && sb2.Length > 0)
                {
                    sb.Append(" and ");
                    sb2.Append(" AND ");
                }
                else
                {
                    sb.Append("[");
                }

                sb.Append("typ!='Sprawa'");
                sb2.Append(" typ <> 'Sprawa'");
            }
            if(sb.Length>0)
                sb.Append("]");
            Session["DocCaseFilter"] = sb2.ToString();

            //////////////////////////////////////////////////////

                  
            //if (!ckbWszystkie.Checked)
            //{
            //    sb.Append(string.Format("[doPracownika='{0}'", ckbMoje.Checked ? "1" : "0"));
            //    sb2.Append(string.Format("doPracownika={0}", ckbMoje.Checked ? "1" : "0"));
            //}
            //else
            //{
            //    sb.Append(string.Format("[(doPracownika='0' or doPracownika='1')"));
            //    sb2.Append(string.Format("(doPracownika=1 OR doPracownika=0)"));
            //}
            //if (!ckbDokumenty.Checked)
            //{
            //    sb.Append(" and typ!='Dokument'");
            //    sb2.Append(" AND typ <> 'Dokument' ");
            //}
            //if (!ckbSprawy.Checked)
            //{
            //    sb.Append(" and typ!='Sprawa'");
            //    sb2.Append(" and typ <> 'Sprawa'");
            //}
            //if (ddlTypInteresanta.SelectedValue != "-1")
            //{
            //    sb2.Append(" and idTypuInteresanta=" + ddlTypInteresanta.SelectedValue);
            //}
            //sb.Append("]");

            //Session["taskFilter"] = sb.ToString();
            //Session["taskFilter2"] = sb2.ToString();
            //presenter.Initialize();

            //////////////////////////////////////////////////////

            //LoadDocCaseList();
            presenter.Initialize();
            
        }

        //protected void podglad(object sender, EventArgs e)
        //{
        //    string commandName = ((LinkButton)sender).CommandName;
        //    string commandArgument = ((LinkButton)sender).CommandArgument;
        //    string sDocCaseList = string.Empty;
        //    if (commandName.ToLower() == "dokument" || commandName.ToLower() == "sprawa")
        //    {                
        //        ActionDAO ad = new ActionDAO();
        //        using (XmlReader xdr = ad.GetDocCaseFullList())
        //        {
        //            if (xdr.Read())
        //                sDocCaseList = xdr.ReadOuterXml();
        //        }
                
        //        XPathDocument xpd = new XPathDocument(new StringReader(sDocCaseList));
        //        XPathNavigator xpn = xpd.CreateNavigator();
        //        XPathNodeIterator xpni = xpn.Select(string.Format("/zadania/zadanie[id='{0}']", commandArgument));
        //        if (xpni.MoveNext())
        //            Session["context"] = xpni.Current.OuterXml;
        //        if (commandName.ToLower() == "dokument")
        //            Session["idDokumentu"] = commandArgument.ToString();
        //        if (commandName.ToLower() == "sprawa")
        //            Session["idSprawy"] = commandArgument.ToString();
        //        pnlFullDocCaseList.Visible = false;
        //        pnlDocCaseDetails.Visible = true;
        //        //view.TargetView = string.Format("{1}/{0}", e.CommandName, e.CommandName == "Sprawa" ? "Sprawy" : "Dokumenty");
        //    }
        //}

        protected void podglad(object sender, CommandEventArgs e)
        {
            Session["PreviousPageUrl"] = Request.Url.AbsoluteUri;
            OnWykonaniePolecenia(new ExecutingCommandEventArgs(e.CommandName, e.CommandArgument));
        }

        #region IAssignedItemsView Members

        string IAssignedItemsView.Items
        {
            set
            {
                /*XmlDataSource xds = new XmlDataSource();
                ActionDAO ad = new ActionDAO();

                using (XmlReader xdr = ad.GetDocCaseFullList())
                {
                    if (xdr.Read())
                        xds.Data = xdr.ReadOuterXml();
                    else
                        xds.Data = string.Empty;
                    xds.EnableCaching = false;
                    gvCaseDocList.DataSource = null;
                    if (xds.Data != "")
                    {
                        xds.XPath = "/zadania/zadanie";
                        if (Session["DocCaseFilter"] != null)
                            xds.XPath += Session["DocCaseFilter"].ToString();
                        gvCaseDocList.DataSource = xds;
                    }
                }
                gvCaseDocList.DataBind();*/

                odsDocCaseList.SelectParameters.Clear();
                //odsDocCaseList.SelectParameters.Add("userId", Membership.GetUser().ProviderUserKey.ToString());
                odsDocCaseList.SelectParameters.Add("sortParam", gvCaseDocList.SortExpression);
                odsDocCaseList.SelectParameters.Add("filterParam", (Session["DocCaseFilter"] != null) ? Session["DocCaseFilter"].ToString() : "");
                gvCaseDocList.DataBind();
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
                Response.Redirect(string.Format("~/{0}", value)); // CHECK
            }
        }

        #endregion

        protected void ddlPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCaseDocList.PageIndex = int.Parse(((DropDownList)sender).SelectedValue);
            gvCaseDocList.DataBind();
        }

        protected void gvCaseDocList_DataBound(object sender, EventArgs e)
        {
            
        }

        protected void ddlPageSelector_DataBinding(object sender, EventArgs e)
        {
            
        }

        protected void ddlPageSelector_DataBound(object sender, EventArgs e)
        {
            if(gvCaseDocList.PageCount != ((DropDownList)sender).Items.Count)
            {
                for (int i = 0; i < gvCaseDocList.PageCount; i++)
                    ((DropDownList)sender).Items.Add(new ListItem((i+1).ToString(), i.ToString() ));
                ((DropDownList)sender).SelectedIndex = gvCaseDocList.PageIndex;
            }
        }
    }
}