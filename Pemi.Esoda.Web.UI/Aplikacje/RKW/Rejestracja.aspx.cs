using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BarcodeToolkit;
using System.Data;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Aplikacje.RKW
{
    public partial class Rejestracja : System.Web.UI.Page
    {
        protected int docID;

        protected void Page_Load(object sender, EventArgs e)
        {

            if(!int.TryParse(Request.QueryString["docID"],out docID)){
                Response.Redirect("~/Aplikacje/RKW/Przegladanie.aspx");
            }

            
            using (IDataReader dr = new RegistryDAO().GetRKWDataForRegistration(docID)) { 
            if (dr.Read())
            {
                ldataPrzekazania.Text = dr["DataPrzekazania"].ToString();
              //  lTypKorespondencji.Text = AddidtionsList.GetConcatenatedLabels((int)dr["TypKorespondencjiZakodowany"]);
                lZnakPisma.Text = dr["ZnakPisma"].ToString();
                lWydzial.Text = dr["Wydzial"].ToString();
                lPracownik.Text = dr["Pracownik"].ToString();
                lTypDokumentu.Text = dr["TypDokumentu"].ToString();
                lNazwaAdresata.Text = dr["NazwaAdresata"].ToString();
                lUlicaAdresata.Text = dr["UlicaAdresata"].ToString();
                lKodIMiastoAdresata.Text = dr["KodIMiastoAdresata"].ToString();
              //  tbUwagi.Text = dr["Uwagi"].ToString();
                if(!IsPostBack)
                bindBarcodeValue((int)dr["TypKorespondencjiZakodowany"]);
                
            }
            else
            {
                Response.Redirect("~/Aplikacje/RKW/Przegladanie.aspx");
            }
        }
        }

        private void bindBarcodeValue(int codeValue)
        {
            IList<AdditionItem> additionsItems = AddidtionsList.GetAll(codeValue);
            int zone = (codeValue >> 16);
            if (zone > 0)
            {
                ddlZone.Enabled = true;
                ddlZone.SelectedValue = (zone & 3).ToString();
            }

            cbAdditions.DataSource = additionsItems;
            cbAdditions.DataTextField = "Label";
            cbAdditions.DataValueField = "FlagValue";
            cbAdditions.DataBind();
            foreach (AdditionItem item in additionsItems)
            {
                ListItem li = cbAdditions.Items.FindByValue(item.FlagValue.ToString());
                if (li != null)
                {
                    li.Selected = item.Checked;
                }
            }
        }

        protected void registerDocument(object sender, EventArgs e)
        {
            RegistryDAO dao = new RegistryDAO();
              int additionFlags = 0;
          
            foreach (ListItem item in cbAdditions.Items)
            {
                if (item.Selected)
                {
                    additionFlags |= (int)Enum.Parse(typeof(Additions), item.Value);
                if((Additions)Enum.Parse(typeof(Additions), item.Value)==Additions.DOD_ZAGRANICZNY){
                    int zone = int.Parse(ddlZone.SelectedValue) << 16;
                    additionFlags += zone;
                }
                }
            }
            if (dao.RegisterDocumentInRKW(docID, AddidtionsList.GetConcatenatedLabels(additionFlags), additionFlags, tbUwagi.Text))
            {
                Response.Redirect("~/Aplikacje/RKW/Przegladanie.aspx");
            }
        }
    }
}