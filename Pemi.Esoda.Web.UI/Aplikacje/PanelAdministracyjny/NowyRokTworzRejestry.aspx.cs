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
    public partial class NowyRokTworzRejestry : System.Web.UI.Page
    {
        private List<RegistryItemDTO> registryList = new List<RegistryItemDTO>();

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
            ddlAvailableYears.DataSource = new RegistryDAO().GetRegistriesAvailableYears();
            ddlAvailableYears.DataBind();
            ddlAvailableYears.SelectedValue = DateTime.Today.Year.ToString();
        }

        private void LoadRegistriesForDepartment(int departmentID, int year)
        {
            try
            {
                registryList = new RegistryDAO().GetRegistriesForDepartment(departmentID, year);

                registries.DataSource = registryList;
                registries.DataBind();

                if (registryList.Count > 0)
                {
                    availableRegistries.Visible = true;
                    noAvailableRegistries.Visible = false;
                }
                else
                {
                    availableRegistries.Visible = false;
                    noAvailableRegistries.Visible = true;
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

            LoadRegistriesForDepartment(departmentID, CurrentYear);
            
            rightPanel.Visible = true;
        }

        protected void registries_PreRender(object sender, EventArgs e)
        {
            foreach (ListItem i in ((CheckBoxList)sender).Items)
            {
                i.Selected = true;
            }

            foreach (RegistryItemDTO bi in registryList)
            {
                if (bi.IsNewYearCopy)
                {
                    ListItem i = FindRegistryCheckBox(bi.ID.ToString());
                    if (i != null)
                    {
                        i.Enabled = false;
                        i.Selected = false;
                    }
                }
            }
        }

        protected void create_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    CopyRegistries();
                    LoadDepartments();
                    LoadAvailableYears();
                    newYear.Text = string.Empty;
                    rightPanel.Visible = false;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }

        private void CopyRegistries()
        {
            try
            {
                foreach (ListItem i in registries.Items)
                {
                    if (i.Selected)
                        new RegistryDAO().CreateNewYearRegistry(int.Parse(i.Value), int.Parse(newYear.Text));
                }
            }
            catch (Exception e)
            {
                lblMessage.Text = e.Message;
            }
        }

        private ListItem FindRegistryCheckBox(string id)
        {
            foreach (ListItem i in registries.Items)
            {
                if (i.Value == id)
                    return i;
            }
            return null;
        }

        protected void ddlAvailableYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRegistriesForDepartment(CurrentDepartmentID, int.Parse(((DropDownList)sender).SelectedValue));
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

