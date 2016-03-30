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
    public partial class DaneUrzedu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Saves new information about current agency.
        /// </summary>
        protected void frmDaneUrzedu_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                string PelnaNazwa = e.NewValues["PelnaNazwa"].ToString();
                string TypUrzedu = e.NewValues["TypUrzedu"].ToString();
                string OrganKierujacy = e.NewValues["OrganKierujacy"].ToString();

                string NIP = e.NewValues["NIP"].ToString();
                string REGON = e.NewValues["REGON"].ToString();

                string Miasto = e.NewValues["Miasto"].ToString();
                string Ulica = e.NewValues["Ulica"].ToString();
                string Budynek = e.NewValues["Budynek"].ToString();
                string Lokal = e.NewValues["Lokal"].ToString();

                string Telefon = e.NewValues["Telefon"].ToString();
                string Fax = e.NewValues["Fax"].ToString();
                string WWW = e.NewValues["WWW"].ToString();
                string BIP = e.NewValues["BIP"].ToString();
                string Email = e.NewValues["Email"].ToString();

                (new OfficeDAO()).UpdateOfficeData(PelnaNazwa, TypUrzedu, OrganKierujacy, NIP, REGON, Miasto,
                    Ulica, Budynek, Lokal, Telefon, Fax, WWW, BIP, Email);
            }
        }

        protected void odsOfficeData_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.Cancel = true;
        }

        protected void frmDaneUrzedu_ModeChanging(object sender, FormViewModeEventArgs e)
        {

        }
    }
}
