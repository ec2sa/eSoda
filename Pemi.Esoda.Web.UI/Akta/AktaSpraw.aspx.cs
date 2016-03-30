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
using System.Xml;
using System.Xml.XPath;
using System.IO;

namespace Pemi.Esoda.Web.UI.Akta
{
    public partial class AktaSpraw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int briefcaseId = -1;

            if (!IsPostBack)
            {                
                Session.Remove("idDokumentu");
                Session.Remove("idSprawy");
                Session.Remove("idTeczki");
                Session["entryPoint"] = "AS";
                LoadYears();
                if (Request.QueryString["idTeczki"] != null)
                {
                    preselectBriefcase();
                }
                if (User.IsInRole("Administratorzy") && Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"].ToString(), out briefcaseId))
                {
                    LoadBriefcase(briefcaseId);
                    //listaTeczek.Visible = false;
                    //listaTeczek.Enabled = false;
                    rblOwnerSelect.Visible = false;
                    lnkReturn.Visible = true;
                }           
                  
            }      
            //LoadBriefcaseTree();
            if (Request.QueryString["source"] != null && Request.QueryString["source"].ToLower() == "admin")
            {
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"].ToString(), out briefcaseId))
                {
                   Guid? userId = new BriefcaseDAO().GetBriefcaseOwner(briefcaseId);
                    if (userId != null)
                        LoadBriefcaseTree(userId.Value, bool.FalseString);
                }
            }
            else
            {
                LoadBriefcaseTree(new Guid(Membership.GetUser().ProviderUserKey.ToString()), rblOwnerSelect.SelectedValue);
            }
            if (!IsPostBack)
            {
                drzewoTeczek.CollapseAll();
            }
        }

        private void LoadYears()
        {
            BriefcaseDAO dao = new BriefcaseDAO();
            rok.DataSource = dao.GetBriefcaseYears();
            rok.DataTextField = "rok";
            rok.DataBind();
            rok.SelectedValue = DateTime.Today.Year.ToString();

        }

        private void LoadBriefcaseTree(Guid userId, string toEmployee)
        {
            drzewoTeczek.Nodes.Clear();
            //string selected = "";
            //if (drzewoTeczek.SelectedNode != null)
            //    selected = drzewoTeczek.SelectedNode.ValuePath;
               
            
            string year = rok.SelectedValue;
            string from = (ddlShowFrom.SelectedValue != string.Empty) ? ddlShowFrom.SelectedValue : null;
       
            xmldsGrupyTeczek.Data = (new BriefcaseDAO()).GetBriefcaseGroups(userId, year, from, toEmployee,showArchive.Checked);
            if (xmldsGrupyTeczek.Data != "")
            {
                drzewoTeczek.DataSource = xmldsGrupyTeczek;
                drzewoTeczek.DataBind();
            }

            //if (drzewoTeczek.FindNode(selected) != null)
            //    drzewoTeczek.FindNode(selected).Select();
        }
        
        protected void listaTeczek_SelectedIndexChanged(object sender, EventArgs e)
        {
            int briefcaseId;
            if (listaTeczek.SelectedDataKey != null && listaTeczek.SelectedDataKey["id"] != null)
            {
                if (!int.TryParse(listaTeczek.SelectedDataKey["id"].ToString(), out briefcaseId))
                    throw new ArgumentException(string.Format("Nie ma takiej teczki {0}", listaTeczek.SelectedDataKey["id"].ToString()));

                Session["nazwaTeczki"] = (listaTeczek.SelectedRow.Cells[2].Controls[0] as LinkButton).Text;
                LoadBriefcase(briefcaseId);
            }
        }

        void LoadBriefcase(int briefcaseId)
        {
            Session["idTeczki"] = briefcaseId;

            XmlDataSource xdsh = new XmlDataSource();
            XmlDataSource xds = new XmlDataSource();

            XmlReader xrh = new BriefcaseDAO().GetBriefcaseInfo(briefcaseId);
            XmlReader xr = new BriefcaseDAO().GetCasesFromBriefcase(briefcaseId);

            naglowekTeczki.DataSource = null;
            listaSpraw.DataSource = null;           


            if (xrh.Read())
            {
                xdsh.Data = xrh.ReadOuterXml();
                xdsh.EnableCaching = false;
                xdsh.XPath = "/teczka";
                naglowekTeczki.DataSource = xdsh;
            }

            if (xr.Read())
            {
                xds.Data = xr.ReadOuterXml();
                xds.EnableCaching = false;
                xds.XPath = "/sprawy/sprawa";
                listaSpraw.DataSource = xds;
            }

            briefcaseStatusMessage.Text = (listaSpraw.DataSource == null) ? "Teczka jest pusta" : "";
            briefcaseHeaderStatusMessage.Text = (naglowekTeczki.DataSource == null) ? "Brak informacji o teczce" : "";
            naglowekTeczki.DataBind();
            listaSpraw.DataBind();
        }

        protected void listaSpraw_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "viewCase")
            {
                //Session["idSprawy"] = e.CommandArgument;
                //////////

                /*int briefcaseId = -1;
                bool success = Int32.TryParse(Session["idTeczki"].ToString(), out briefcaseId);
                if (!success)
                    return;
                string assignedItems = Session["{3BA3EA44-0A5E-4b27-94A7-0C6AD10889EC}"].ToString();
                XPathDocument xpd = new XPathDocument(new StringReader(assignedItems));
                XPathNavigator xpn = xpd.CreateNavigator();
                XPathNodeIterator xpni = xpn.Select(string.Format("/zadania/zadanie[id='{0}']", e.CommandArgument));
                if (xpni.MoveNext())
                    Session["context"] = xpni.Current.OuterXml;*/


                //////////
                Response.Redirect("~/Sprawy/Sprawa.aspx?id="+e.CommandArgument.ToString());
            }
        }

        protected void listaTeczek_DataBound(object sender, EventArgs e)
        {            
            naglowekTeczki.DataSource = null;
            listaSpraw.DataSource = null;
            naglowekTeczki.DataBind();
            listaSpraw.DataBind();
            briefcaseHeaderStatusMessage.Text = "Nie wybrano teczki";
            briefcaseStatusMessage.Text = "Nie wybrano teczki";
            listaTeczek.SelectedIndex = -1;
            if (Session["idTeczki"] != null)
            {
                LoadBriefcase(int.Parse(Session["idTeczki"].ToString()));
            }
        }

        private void preselectBriefcase()
        {

            int briefcaseId;
      
                
                if (!int.TryParse(Request.QueryString["idTeczki"].ToString(), out briefcaseId))
                    throw new ArgumentException(string.Format("Nie ma takiej teczki {0}", drzewoTeczek.SelectedValue));
               
              //  Session["nazwaTeczki"] = drzewoTeczek.SelectedNode.ToolTip;
                LoadBriefcase(briefcaseId);
           
            //if (Request.QueryString["idTeczki"] != null)
            //{
            //    int i = 0;
            //    int pk = int.Parse(Request.QueryString["idTeczki"].ToString());
            //    while (i < listaTeczek.Rows.Count)
            //    {
            //        if ((int)(listaTeczek.DataKeys[i].Value) == pk)
            //        {
            //            Session["idTeczki"] = Request.QueryString["idTeczki"];
            //            listaTeczek.SelectedIndex = i;
            //            listaTeczek_SelectedIndexChanged(null, null);

            //            break;
            //        }
            //        i++;
            //    }
            //}
        }

        protected void listaTeczek_DataBinding(object sender, EventArgs e)
        {
            if (ObjectDataSource1.SelectParameters["userId"] != null)
            {
                ObjectDataSource1.SelectParameters["userId"].DefaultValue = Membership.GetUser().ProviderUserKey.ToString();
            }
            else
            {
                ObjectDataSource1.SelectParameters.Add("userId", Membership.GetUser().ProviderUserKey.ToString());
            }
        }

        private void ExpandParent(TreeNode node)
        {
            if (node.Parent != null)
            {
                node.Parent.Expand();
                ExpandParent(node.Parent);
            }
        }

        protected void drzewoTeczek_SelectedNodeChanged(object sender, EventArgs e)
        {
            int briefcaseId;
            if (drzewoTeczek.SelectedNode != null) // && drzewoTeczek.SelectedValue != "-1")
            {
                drzewoTeczek.SelectedNode.Expand();
                if (!int.TryParse(drzewoTeczek.SelectedNode.Value.ToString(), out briefcaseId))
                    throw new ArgumentException(string.Format("Nie ma takiej teczki {0}", drzewoTeczek.SelectedValue));
                ExpandParent(drzewoTeczek.SelectedNode);
                Session["nazwaTeczki"] = drzewoTeczek.SelectedNode.ToolTip;
                LoadBriefcase(briefcaseId);
            }
        }
    }
}
