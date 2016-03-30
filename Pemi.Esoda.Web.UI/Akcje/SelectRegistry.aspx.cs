using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.Tools;
using Pemi.Esoda.DTO;
using System.Collections.Generic;


namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class SelectRegistry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAvailableYears();
                LoadAvailableRegistries();
            }
        }

        private int SelectedYear
        {
            get
            {
                return int.Parse(dostepneLata.SelectedValue);
            }
        }

        private void LoadAvailableYears()
        {            
            dostepneLata.DataSource = new RegistryDAO().GetRegistriesAvailableYears();
            dostepneLata.DataBind();
            dostepneLata.SelectedValue = DateTime.Today.Year.ToString();

            select.Enabled = false;
            message.Visible = false;
        }

        private void LoadAvailableRegistries()
        {
            
                IDataReader dr = null;
                if (Session["idAkcji"].ToString() == "2820653C-06BA-4704-AE1C-47D667E9F352")
                {
                    //dr = new RegistryDAO().GetAvailableRegistries((Guid)Membership.GetUser().ProviderUserKey, int.Parse(Session["idSprawy"].ToString()), false);
                    dr = new RegistryDAO().GetAvailableRegistries((Guid)Membership.GetUser().ProviderUserKey, CoreObject.GetId(Request), false, SelectedYear);
                }
                if (Session["idAkcji"].ToString() == "ED10E89A-365B-4034-9710-1E58BB93F5E9")
                {
                    //dr = new RegistryDAO().GetAvailableRegistries((Guid)Membership.GetUser().ProviderUserKey, int.Parse(Session["idDokumentu"].ToString()), true);
                    dr = new RegistryDAO().GetAvailableRegistries((Guid)Membership.GetUser().ProviderUserKey, CoreObject.GetId(Request), true, SelectedYear);
                }
                if (dr != null)
                {
                    rejestry.DataSource = dr;
                    rejestry.DataTextField = "nazwa";
                    rejestry.DataValueField = "id";
                    rejestry.DataBind();
                    select.Enabled = rejestry.Items.Count > 0;
                    message.Visible = !select.Enabled;
                }
            
        }

        protected void anuluj_Click(object sender, EventArgs e)
        {
            if (Session["idAkcji"].ToString() == "2820653C-06BA-4704-AE1C-47D667E9F352")
            {
                Response.Redirect("~/sprawy/akcjeSprawy.aspx?id=" + CoreObject.GetId(Request), false);
            }
            if (Session["idAkcji"].ToString() == "ED10E89A-365B-4034-9710-1E58BB93F5E9")
            {
                Response.Redirect("~/dokumenty/akcjeDokumentu.aspx?id=" + CoreObject.GetId(Request), false);
            }
        }

        protected void select_Click(object sender, EventArgs e)
        {
          
            Response.Redirect(string.Format("~/akcje/edycjaRejestru.aspx?regid={0}&id={1}",rejestry.SelectedValue,CoreObject.GetId(Request)),false);
        }

        protected void dostepneLata_SelectedIndexChanged(object sender, EventArgs e)
        {            
            LoadAvailableRegistries();            
        }
    }
}
