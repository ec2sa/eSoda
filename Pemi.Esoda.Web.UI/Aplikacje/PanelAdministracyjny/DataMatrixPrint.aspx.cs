using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class DataMatrixPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Dictionary<string, string> configData = new EsodaConfigParametersDAO().GetConfig();

                txtPageHeight.Text = configData["printPageHeight"];
                txtPageWidth.Text = configData["printPageWidth"];
                txtPositionLeft.Text = configData["codePosLeft"];
                txtPositionTop.Text = configData["codePosTop"];
            }
        }

        protected void Zapisz_click(object sender, EventArgs e)
        {
            string printPageHeight = txtPageHeight.Text;
            string printPageWidth = txtPageWidth.Text;
            string codePosLeft = txtPositionLeft.Text;
            string codePosTop = txtPositionTop.Text;

            try
            {
                EsodaConfigParametersDAO dao = new EsodaConfigParametersDAO();
                dao.SetConfigParam("printPageHeight", printPageHeight);
                dao.SetConfigParam("printPageWidth", printPageWidth);
                dao.SetConfigParam("codePosLeft", codePosLeft);
                dao.SetConfigParam("codePosTop", codePosTop);

                msg.Text = "Konfiguracja zapisana poprawnie";
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                msg.Attributes["style"] = "color:red;";
                msg.Text = "Błąd w trakcie zapisu konfiguracji";
            }
        }
    }
}
