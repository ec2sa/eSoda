using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.Common;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class listaRejestrow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["regEdit"] =null;
            Session["regPreview"] = null;
            if (!IsPostBack)
            {
                using (DbDataReader dr = (DbDataReader)(new RegistryDAO()).GetRegistryYears())
                {
                    ddlRok.Items.Clear();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            string rok = dr["rok"].ToString();
                            ddlRok.Items.Add(new ListItem(rok, rok));
                        }
                        ddlRok_DataBound(null, null);
                    }
                }
                if (ddlRok.Items.Count == 0)
                {
                    int rok = DateTime.Now.Year;
                    ddlRok.Items.Add(new ListItem((rok - 1).ToString(), (rok - 1).ToString()));
                    ddlRok.Items.Add(new ListItem(rok.ToString(), rok.ToString()));
                    ddlRok_DataBound(null,null);
                }
            }
        }

        protected void gvRegistersList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "RegEdit":
                    Session["idRejestru"] = e.CommandArgument;
                    Session["idDefinicji"] = null;
                    Session["regEdit"] = true;
                    Session["regPreview"] = null;
                    Response.Redirect("~/Aplikacje/PanelAdministracyjny/edycjaRejestru.aspx");
                    break;

                case "Preview":
                    Session["idRejestru"] = e.CommandArgument;
                    Session["idDefinicji"] = null;
                    Session["regPreview"] = true;
                    Session["regEdit"] = null;
                    Response.Redirect(string.Format("~/Rejestry/ListaRejestrow.aspx?id={0}", e.CommandArgument));                    
                    break;

                default:
                    Session["idRejestru"] = Session["regEdit"] = null;
                    break;
            }
        }

        protected void lnkNewRegister_Click(object sender, EventArgs e)
        {
            Session["idRejestru"] = null;
            Session["RegDefinition"] = null;
            Session["idDefinicji"] = null;
            Response.Redirect("~/Aplikacje/PanelAdministracyjny/BudowaDefinicjiRejestru.aspx");
        }

        protected void ddlRok_DataBound(object sender, EventArgs e)
        {
            ddlRok.Items.Insert(0, new ListItem("- wybierz", "-1", true));
            ddlRok.SelectedIndex = 0;
        }

        protected void gvRegistersList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RegistryDAO rd = new RegistryDAO();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkRegEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                if (lnkRegEdit != null)
                {
                    bool isSysReg = DataBinder.Eval(e.Row.DataItem, "sys").ToString().ToLower().Contains("rue");
                    lnkRegEdit.Visible = !isSysReg;
                }
            }
        }
    }
}
