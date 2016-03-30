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
using Pemi.Esoda.Tools;
using Pemi.Esoda.DataAccess;
using System.Text.RegularExpressions;
// DB_HASH: 47fed9b78b28cc3e15331e19052a09a1fa3d66d5
namespace Pemi.Esoda.Web.UI
{
    public partial class Logon : System.Web.UI.Page
    {
        public string MessageOfTheDay
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    lblMessageOfTheDay.Text = "<pre>" + value + "</pre>";
                    pnlMessageOfTheDay.Visible = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton1.Visible = ConfigurationManager.AppSettings["codename"].ToLower().IndexOf("dev") >= 0;
            MessageOfTheDay = new NoticeDAO().GetCurrentNotice();
        }

        protected void logonBox_LoggedIn(object sender, EventArgs e)
        {

            if (Request.QueryString["ReturnUrl"] == null)
            {
                Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"] = "LinkButton0";
                if (ConfigurationManager.AppSettings["mode"].Equals("dokumenty"))
                {
                    LoginNotifier.Log_Success(logonBox.UserName, Request.UserHostAddress, LoginNotifier.Zone.Dokumentomat);
                    Response.Redirect("~/OdbiorDokumentow.aspx");
                }
                else
                {
                    LoginNotifier.Log_Success(logonBox.UserName, Request.UserHostAddress, LoginNotifier.Zone.ESoda);
                    Response.Redirect("~/OczekujaceZadania.aspx");
                }
            }

            if (ConfigurationManager.AppSettings["mode"].Equals("dokumenty"))
            {
                LoginNotifier.Log_Success(logonBox.UserName, Request.UserHostAddress, LoginNotifier.Zone.Dokumentomat);
            }
            else
            {
                LoginNotifier.Log_Success(logonBox.UserName, Request.UserHostAddress, LoginNotifier.Zone.ESoda);
            }

        }

        protected void logonBox_LoginError(object sender, EventArgs e)
        {

            if (Membership.FindUsersByName(logonBox.UserName).Count > 0)
            {
                if (ConfigurationManager.AppSettings["mode"].Equals("dokumenty"))
                {
                    LoginNotifier.Log_BadPassword(logonBox.UserName, logonBox.Password, Request.UserHostAddress, LoginNotifier.Zone.Dokumentomat);
                }
                else
                {
                    LoginNotifier.Log_BadPassword(logonBox.UserName, logonBox.Password, Request.UserHostAddress, LoginNotifier.Zone.ESoda);
                }
            }
            else
            {
                if (ConfigurationManager.AppSettings["mode"].Equals("dokumenty"))
                {
                    LoginNotifier.Log_BadUsername(logonBox.UserName, Request.UserHostAddress, LoginNotifier.Zone.Dokumentomat);
                }
                else
                {
                    LoginNotifier.Log_BadUsername(logonBox.UserName, Request.UserHostAddress, LoginNotifier.Zone.ESoda);
                }
            }


        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie("admin", false);
            if (ConfigurationManager.AppSettings["mode"].Equals("dokumenty"))
            {
                Response.Redirect("~/OdbiorDokumentow.aspx");
            }
            else
                Response.Redirect("~/OczekujaceZadania.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        protected void logonBox_LoggingIn(object sender, LoginCancelEventArgs e)
        {
            if (new UserDAO().IsCoverDublerLogin(logonBox.UserName))
            {
                string hash = (new OfficeDAO()).CheckDB().ToLower();
                if (hash.Equals("cc9961f4f74308d7bd4ca52fce7aa33646c317d8"))
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    BaseContentPage.SetError("Niezgodnoœæ wersji aplikacji i bazy danych.", "~/Logon.aspx");
                }
            }            
            else
            {
                e.Cancel = true;
                BaseContentPage.SetError("Nie mo¿esz zalogowaæ siê do systemu, gdy¿ posiadasz zastêpstwo!", "~/Logon.aspx");                
            }
        }
    }
}