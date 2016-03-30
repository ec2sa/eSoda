using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class SMSConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SMSServiceDAO dao = new SMSServiceDAO();
                SMSServiceConfiguration config = dao.GetSMSConfig();
                bindConfig(config);
                msg.Text = string.Empty;
            }
        }

        private void bindConfig(SMSServiceConfiguration config)
        {
            if (!(config is SMSServiceConfiguration))
                return;
            txtSender.Text = config.Sender;
            txtUser.Text = config.Username;
            txtPassword.Text = config.Password;
            txtTemplate.Text = config.MessageTemplate;
            cbIsFlash.Checked = config.IsFlash;
            cbIsTest.Checked = config.IsTest;

        }

        protected void Zapisz_click(object sender, EventArgs e)
        {
            SMSServiceConfiguration newConfig = new SMSServiceConfiguration()
            {
                Sender = txtSender.Text
                ,
                Username = txtUser.Text
                ,
                Password = txtPassword.Text
                ,
                IsFlash = cbIsFlash.Checked
                ,
                IsTest = cbIsTest.Checked
                ,
                MessageTemplate=txtTemplate.Text
            };

            if (new SMSServiceDAO().SaveSMSConfig(newConfig))
            {
                msg.Text = "Konfiguracja zapisana poprawnie";
            }
            else
            {
                msg.Attributes["style"] = "color:red;";
                msg.Text = "Błąd w trakcie zapisu konfiguracji";
            }

        }
    }
}
