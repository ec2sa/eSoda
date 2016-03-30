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
using System.Data.SqlClient;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;
using System.Collections.Generic;


namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class NowyRokTworzTeczki : System.Web.UI.Page
    {
        private List<BriefcaseItemDTO> briefcaseList = new List<BriefcaseItemDTO>();

        private int CurrentDepartmentID
        {
            get
            {
                if (ViewState["CurrentDepartmentID"] != null)
                    return (int)ViewState["CurrentDepartmentID"];
                else
                    return -1;
            }
            set
            {
                ViewState["CurrentDepartmentID"] = value;
            }
        }

        private int CurrentYear
        {
            get
            {
                return int.Parse(ddlAvailableYears.SelectedValue);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                LoadDepartments();
                LoadAvailableYears();
            }
        }

        private void LoadDepartments()
        {
            try
            {
                GroupDAO gd = new GroupDAO();
                DataSet dsGroups = gd.GetGroupsListDataSet();

                dsGroups.DataSetName = "Groups";
                dsGroups.Tables[0].TableName = "Group";

                DataRelation dRel = new DataRelation("parentGroups", dsGroups.Tables["Group"].Columns["id"], dsGroups.Tables["Group"].Columns["idRodzica"], true);
                dRel.Nested = true;
                dsGroups.Relations.Add(dRel);

                dsxmlGroups.Data = dsGroups.GetXml();
                tvGroups.DataBind();
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
            }
        }

        private void LoadAvailableYears()
        {
            ddlAvailableYears.DataSource = new BriefcaseDAO().GetBriefcasesAvailableYears();
            ddlAvailableYears.DataBind();
        }

        private void LoadBriefcaseTree(int departmentID, int year)
        {
            try
            {
                briefcasesTree.Nodes.Clear();

                briefcaseList = new BriefcaseDAO().GetBriefcasesForDepartment(departmentID, year);

                xmldsbriefcasesTree.Data = new BriefcaseDAO().GetBriefcasesForDepartmentXML(departmentID, year);

                if (xmldsbriefcasesTree.Data != "")
                {
                    briefcasesTree.DataSource = xmldsbriefcasesTree;
                    briefcasesTree.DataBind();
                }

                if (briefcaseList.Count > 0)
                {
                    availableBriefcases.Visible = true;
                    noAvailableBriefcases.Visible = false;
                }
                else
                {
                    availableBriefcases.Visible = false;
                    noAvailableBriefcases.Visible = true;
                }
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
            }
        }

        protected void tvGroups_SelectedNodeChanged(object sender, EventArgs e)
        {
            int departmentID = int.Parse(((TreeView)sender).SelectedValue);
            CurrentDepartmentID = departmentID;

            LoadBriefcaseTree(departmentID, CurrentYear);

            rightPanel.Visible = true;
        }

        private void CopyBriefcasesTree()
        {

            try
            {
                foreach (TreeNode i in briefcasesTree.Nodes)
                {
                    if (i.Checked)
                    {
                        new BriefcaseDAO().CreateNewYearBriefcaseWithChildren(int.Parse(i.Value), int.Parse(newYear.Text));
                    }
                }
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
            }

        }

        protected void create_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    CopyBriefcasesTree();
                    LoadDepartments();
                    LoadAvailableYears();
                    //LoadBriefcaseTree(CurrentDepartmentID, CurrentYear);
                    newYear.Text = string.Empty;
                    rightPanel.Visible = false;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }

        private TreeNode FindBriefcaseNode(string id)
        {
            foreach (TreeNode i in briefcasesTree.Nodes)
            {
                if (i.Value == id)
                    return i;
            }
            return null;
        }

        protected void briefcasesTree_PreRender(object sender, EventArgs e)
        {
            foreach (TreeNode i in ((TreeView)sender).Nodes)
            {
                i.Checked = true;
                foreach (TreeNode cn in i.ChildNodes)
                {
                    cn.Checked = true;
                    cn.ShowCheckBox = false;
                }
            }

            foreach (BriefcaseItemDTO bi in briefcaseList)
            {
                if (bi.IsNewYearCopy)
                {
                    TreeNode i = FindBriefcaseNode(bi.ID.ToString());
                    if (i != null)
                    {
                        i.ShowCheckBox = false;
                        i.Checked = false;
                        foreach (TreeNode cn in i.ChildNodes)
                        {
                            cn.ShowCheckBox = false;
                            cn.Checked = false;
                        }
                    }
                }
            }
        }

        protected void ddlAvailableYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBriefcaseTree(CurrentDepartmentID, int.Parse(((DropDownList)sender).SelectedValue));
        }

        protected void NewYearValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(args.Value) == CurrentYear)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}

