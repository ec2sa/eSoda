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
using System.Data.Common;
using Pemi.Esoda.DataAccess;
using System.Data.SqlClient;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class EdycjaKategoriiInteresantow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCustomerCategoriesList();
                //customerType.TypeChanged += new EventHandler(customerType_TypeChanged);
            }
        }

        void customerType_TypeChanged(object sender, EventArgs e)
        {
            LoadCustomerCategoriesList();
        }

        private void LoadCustomerCategoriesList()
        {
            CustomerDAO cd = new CustomerDAO();
            int typ;

            if (rblTypInteresanta.SelectedIndex > -1)
                typ = int.Parse(rblTypInteresanta.SelectedItem.Value);
            else
                typ = -1;
            gvKategorieInteresantow.DataSource = cd.GetCustomersCategoriesByType(typ);
            gvKategorieInteresantow.DataBind();
        }

        protected void rblTypInteresanta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblTypInteresanta.SelectedIndex > -1)
            {
                LoadCustomerCategoriesList();
            }
        }

        protected void rblTypInteresanta_DataBound(object sender, EventArgs e)
        {
            ListItem item = new ListItem("Wszyscy", "-1");
            item.Selected = true;
            ((RadioButtonList)sender).Items.Insert(0, item);
            ((RadioButtonList)sender).Items.FindByValue("1").Enabled = false;
        }

        protected void lnkAddCategory_Click(object sender, EventArgs e)
        {
            frmKategoria.ChangeMode(FormViewMode.Insert);
            frmKategoria.Visible = true;
            rblTypInteresanta.Items.FindByValue("-1").Enabled = false;
            rblTypInteresanta.Items.FindByValue("-1").Selected = false;
            if(rblTypInteresanta.SelectedIndex<=0)
                rblTypInteresanta.Items[1].Selected = true;
        }

        protected void frmKategoria_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                string name = (frmKategoria.FindControl("txtKategoria") as TextBox).Text;
                int typeCat = int.Parse(rblTypInteresanta.SelectedItem.Value);
                (new CustomerDAO()).AddCustomerCategory(name, typeCat);
                LoadCustomerCategoriesList();
                frmKategoria.ChangeMode(FormViewMode.ReadOnly);
                frmKategoria.Visible = false;
                rblTypInteresanta.Items.FindByValue("-1").Enabled = true;
                rblTypInteresanta.DataBind();
            }
        }

        protected void frmKategoria_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                string name = (frmKategoria.FindControl("txtKategoria") as TextBox).Text;
                int typeCat = int.Parse(rblTypInteresanta.SelectedItem.Value);
                int id = int.Parse(gvKategorieInteresantow.SelectedDataKey.Value.ToString());
                (new CustomerDAO()).UpdateCustomerCategory(id, typeCat, name);
                LoadCustomerCategoriesList();
                frmKategoria.ChangeMode(FormViewMode.ReadOnly);
                frmKategoria.Visible = false;
                rblTypInteresanta.DataBind();
            }
        }

        protected void frmKategoria_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            if (e.NewMode == FormViewMode.ReadOnly)
                frmKategoria.ChangeMode(FormViewMode.ReadOnly);
            rblTypInteresanta.DataBind();
        }

        protected void gvKategorieInteresantow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvKategorieInteresantow.SelectedIndex > -1)
            {
                frmKategoria.ChangeMode(FormViewMode.Edit);
                int id = int.Parse(gvKategorieInteresantow.SelectedDataKey.Value.ToString());
                frmKategoria.DataSource = (new CustomerDAO()).GetCustomerCategory(id);
                frmKategoria.DataBind();
                SqlDataReader dr = (SqlDataReader)(new CustomerDAO()).GetCustomerCategory(id);
                if (dr.Read())
                {
                    rblTypInteresanta.SelectedValue = dr["idTypuUzytkownika"].ToString();
                }
                dr.Close();

                frmKategoria.Visible = true;
                rblTypInteresanta.Items.FindByValue("-1").Enabled = false;
            }
        }

        protected void gvKategorieInteresantow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Remove":
                    int id = int.Parse(e.CommandArgument.ToString());
                    (new CustomerDAO()).RemoveCustomerCategory(id);
                    LoadCustomerCategoriesList();
                    break;

                default:
                    break;
            }
        }

        protected void frmKategoria_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Cancel":
                    frmKategoria.ChangeMode(FormViewMode.ReadOnly);
                    frmKategoria.Visible = false;
                    break;
                default:
                    break;
            }
        }
    }
}