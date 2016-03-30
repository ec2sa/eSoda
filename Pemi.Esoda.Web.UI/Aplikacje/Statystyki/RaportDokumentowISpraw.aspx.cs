using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Aplikacje.Statystyki
{
    public partial class RaportDokumentowISpraw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void generujRaport(object sender, EventArgs e)
        {
            StatystykiDAC dao = new StatystykiDAC();
            DateTime? dataOd=null;
            DateTime? dataDo=null;

            DateTime tmpdate;
            if(DateTime.TryParse(txtDataOd.Text,out tmpdate))
                dataOd=tmpdate;
             if(DateTime.TryParse(txtDataDo.Text,out tmpdate))
                dataDo=tmpdate;

             raportGrid.DataSource = dao.DokumentyISprawy(int.Parse(ddlWydzialy.SelectedValue), int.Parse(ddlPracownicy.SelectedValue), dataOd, dataDo);
             raportGrid.DataBind();
        }
    }
}
