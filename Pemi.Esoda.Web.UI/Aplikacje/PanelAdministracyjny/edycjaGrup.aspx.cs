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

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class edycjaGrup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                LoadGroupsList();
            }
        }

        private void LoadGroupsList()
        {
           gvGroupsList.DataBind();
        }

        protected void lnkAddGroup_Click(object sender, EventArgs e)
        {
            frmGroup.ChangeMode(FormViewMode.Insert);
            frmGroup.Visible = true;
        }

        protected void lnkAddRole_Click(object sender, EventArgs e)
        {
            frmRole.ChangeMode(FormViewMode.Insert);
            frmRole.Visible = true;
        }

        /// <summary>
        /// Adds new users group.
        /// </summary>
        protected void frmGroup_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {                
                GroupDAO gd = new GroupDAO();

                string nazwa = ((TextBox)frmGroup.FindControl("txtNazwa")).Text;
                string skrot = ((TextBox)frmGroup.FindControl("txtSkrot")).Text;
                int idRodzica = int.Parse(((DropDownList)frmGroup.FindControl("ddlParentGroup")).SelectedItem.Value);

                gd.CreateGroup(nazwa, skrot, true, idRodzica);
                gvGroupsList.DataBind();
                frmGroup.ChangeMode(FormViewMode.ReadOnly);
                frmGroup.Visible = false;
            }
        }

        /// <summary>
        /// Adds new user role.
        /// </summary>
        protected void frmRole_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                GroupDAO gd = new GroupDAO();

                string nazwa = ((TextBox)frmRole.FindControl("txtNazwa")).Text;
                string skrot = ((TextBox)frmRole.FindControl("txtSkrot")).Text;

                gd.CreateGroup(nazwa, skrot, false, 0);
                gvGroupsList.DataBind();
                frmRole.ChangeMode(FormViewMode.ReadOnly);
                frmRole.Visible = false;
            }
        }

        /// <summary>
        /// Modifies selected user group.
        /// </summary>
        protected void frmGroup_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
               GroupDAO gd = new GroupDAO();

                int groupId = int.Parse(gvGroupsList.SelectedDataKey.Value.ToString());
                string nazwa = ((TextBox)frmGroup.FindControl("txtNazwa")).Text;
                string skrot = ((TextBox)frmGroup.FindControl("txtSkrot")).Text;
                int idRodzica = int.Parse(((DropDownList)frmGroup.FindControl("ddlParentGroup")).SelectedItem.Value);
               
                gd.UpdateGroup(groupId, idRodzica, nazwa, skrot, true);
                gvGroupsList.DataBind();
                frmGroup.ChangeMode(FormViewMode.ReadOnly);
                frmGroup.Visible = false;
            }
        }

        /// <summary>
        /// Modifies selected user role.
        /// </summary>
        protected void frmRole_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                GroupDAO gd = new GroupDAO();

                int groupId = int.Parse(gvGroupsList.SelectedDataKey.Value.ToString());
                string nazwa = ((TextBox)frmRole.FindControl("txtNazwa")).Text;
                string skrot = ((TextBox)frmRole.FindControl("txtSkrot")).Text;

                gd.UpdateGroup(groupId, 0, nazwa, skrot, false);
                gvGroupsList.DataBind();
                frmRole.ChangeMode(FormViewMode.ReadOnly);
                frmRole.Visible = false;
            }
        }

        /// <summary>
        /// Loads data for selected group or role.
        /// </summary>
        protected void gvGroupsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvGroupsList.SelectedIndex > -1)
            { 
                int groupId = int.Parse(gvGroupsList.SelectedDataKey.Value.ToString());

                if (ddlGroupSelector.SelectedValue.Equals("True"))
                {
                    frmGroup.ChangeMode(FormViewMode.Edit);                   
                    frmGroup.DataSource = (new GroupDAO()).GetGroup(groupId);
                    frmGroup.DataBind();
                    frmGroup.Visible = true;
                }
                else
                {
                    frmRole.ChangeMode(FormViewMode.Edit);
                    frmRole.DataSource = (new GroupDAO()).GetGroup(groupId);
                    frmRole.DataBind();
                    frmRole.Visible = true;
                }
            }
        }

        protected void frmGroup_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            frmGroup.ChangeMode(e.NewMode);
        }

        protected void frmRole_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            frmRole.ChangeMode(e.NewMode);
        }

        protected void frmGroup_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel"))
            {
                frmGroup.ChangeMode(FormViewMode.ReadOnly);
                frmGroup.Visible = false;
            }
        }

        protected void frmRole_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Cancel"))
            {
                frmRole.ChangeMode(FormViewMode.ReadOnly);
                frmRole.Visible = false;
            }
        }

        protected void ddlGroupSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmGroup.ChangeMode(FormViewMode.ReadOnly);
            frmGroup.Visible = false;
            frmRole.ChangeMode(FormViewMode.ReadOnly);
            frmRole.Visible = false;

            lnkAddGroup.Visible = ddlGroupSelector.SelectedValue.Equals("True");
            lnkAddRole.Visible = ddlGroupSelector.SelectedValue.Equals("False");
        }

        /// <summary>
        /// Prevents from showing the top-most user group in case of delete or modification.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGroupsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if(DataBinder.Eval(e.Row.DataItem,"idRodzica")==System.DBNull.Value && ddlGroupSelector.SelectedValue.Equals("True"))
                {
                    e.Row.Visible = false;
                }
            }
        }        
    }
}