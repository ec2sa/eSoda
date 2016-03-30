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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Pemi.Esoda.Tools;
using System.Data.SqlClient;
using Pemi.Esoda.DTO;
using System.Collections.Generic;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class GrupaTeczki : System.Web.UI.Page
    {
        private int briefcaseID;
        private List<BriefcaseGroupItemDTO> items;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            if (!int.TryParse(Page.Request.QueryString["ID"], out briefcaseID))
            {
                briefcaseID = -1;                
            }

            if (!Page.IsPostBack)
            {
                BriefcaseDataSource.SelectParameters.Add("briefcaseID", briefcaseID.ToString());

                LoadBriefcaseGroup();
                BriefcaseGroup.DataSource = items;
                BriefcaseGroup.DataBind();

                if (items.Count == 0)
                {
                    panelGroup.Visible = false;
                    dBriefcaseGroupEmpty.Visible = true;
                }
            }            
        }

        private void LoadBriefcaseGroup()
        {
            try
            {
                items = new BriefcaseDAO().GetBriefcaseGroup(briefcaseID);
            }
            catch (SqlException ex)
            {
                lblMessage.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void SetChecked()
        {            
            ListItemCollection checkboxitems = BriefcaseGroup.Items;

            if (items != null)
            {
                for (int i = 0; i < checkboxitems.Count; i++)
                {
                    if (items[i].ID == int.Parse(checkboxitems[i].Value) && items[i].IsChecked)
                    {
                        checkboxitems[i].Selected = true;
                    }
                    else
                    {
                        checkboxitems[i].Selected = false;
                    }
                }
            }            
        }

        protected void SaveChanges_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < BriefcaseGroup.Items.Count; i++)
            {
                if (BriefcaseGroup.Items[i].Selected)
                    new BriefcaseDAO().AssingBriefcaseToParent(int.Parse(BriefcaseGroup.Items[i].Value), briefcaseID);
                else
                    new BriefcaseDAO().AssingBriefcaseToParent(int.Parse(BriefcaseGroup.Items[i].Value), null);
            }

            LoadBriefcaseGroup();

            WebMsgBox.Show(this, "Dane zosta³y zapisane.");
        }

        protected void BriefcaseGroup_PreRender(object sender, EventArgs e)
        {
            SetChecked();
        }

        protected void gvBriefcaseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal list = (Literal)e.Row.FindControl("litCaseKindsList");
                if (list != null)
                {
                    string sDoc = DataBinder.Eval(e.Row.DataItem, "rodzajeSpraw").ToString();
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(sDoc);

                        foreach (XmlElement elem in doc.SelectNodes("/rodzajeSpraw/rodzajSprawy"))
                        {
                            if (list.Text.Length > 0)
                                list.Text += "<br/>";
                            list.Text += elem.InnerText;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = ex.Message;
                    }
                }
            }
        }
    }
}
