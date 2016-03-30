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
using Pemi.Esoda.Presenters;
using System.Reflection;
using Pemi.Esoda.DataAccess;
namespace Pemi.Esoda.Web.UI.MasterPages
{
    public partial class SingleColumn : System.Web.UI.MasterPage
    {

        protected string aktywnaOpcja
        {
            get
            {
                if (Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"] == null)
                    Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"] = "";
                return Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"].ToString();
            }
            set
            {            
                Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"] = value;
            }
        }

        public string APath
        {
            get
            {
                return Page.Request.ApplicationPath == "/" ? "" : Page.Request.ApplicationPath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CaseNav1.Visible = Membership.GetUser() != null;
            lblLoadTime.Visible = ConfigurationManager.AppSettings["LoadTimeVisible"] != null ||
                                ConfigurationManager.AppSettings["codename"].ToLower().IndexOf("dev") >= 0;
            LinkButton akt = form1.FindControl(aktywnaOpcja) as LinkButton;
           //eSodaChatNewMessageNotify.Attributes.Add("src",String.Format("{0}/{1}", this.APath, "eSodaChatNewMessageNotify.aspx"));
            if (akt != null)
                akt.CssClass = "current";
            if (!IsPostBack)
            {
                versionInfo.Text = string.Format("Wersja: [{0}]. Data: {1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), GetBuildDate());
                Version v=System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                versionInfoHeader.Text = string.Format("{0}.{1}.{2}", v.Major,v.Minor,v.Build);
            }

            string mode = ConfigurationManager.AppSettings["mode"];

            if (Membership.GetUser() != null)
            {
                string[] role = (new GroupDAO()).GetGroupsForUser((Guid)Membership.GetUser().ProviderUserKey);

                if (role.Length > 3)
                    userInfo.Text = string.Format("({0}{1})", string.Join(", ", role, 0, 3), "[...]");
                else
                    userInfo.Text = string.Format("({0})", string.Join(", ", role));

                SetVisibleFields(true);
            }
            else
                SetVisibleFields(false);

            if (mode != null)
            {
                switch (mode)
                {
                    case "dokumenty":
                        LinkButton0.Visible = LinkButton1.Visible = LinkButton2.Visible = LinkButton4.Visible = LinkButton3.Visible = false;
                        lnkChangeUserPassword.Visible = false;                        
                     //   lPanel.Visible = false;
                        //chatnotify.Visible = false;
                        break;

                    case "esoda":
                        LinkButton0.Visible = LinkButton1.Visible = LinkButton2.Visible = LinkButton4.Visible = LinkButton3.Visible = true;
                        lnkChangeUserPassword.Visible = true && (Membership.GetUser() != null);                        
                   //     lPanel.Visible = true;
                       // chatnotify.Visible = true;
                        break;

                    default:
                        break;
                }
            }
        }



        DateTime startTime = DateTime.Now;

        protected override void OnPreRender(EventArgs e)
        {

            base.OnPreRender(e);

            this.lblLoadTime.Text = "Load time: " + ((DateTime.Now - startTime).TotalMilliseconds/1000.0f).ToString() + " s.";

        

        }

        void SetVisibleFields(bool visible)
        {
            lnkChangeUserPassword.Visible = visible;            
            litBAR.Visible = visible;
          
            lblyala.Visible = visible;
        
        }

        protected void wylogowanie(object sender, EventArgs e)
        {
            lnkChangeUserPassword.Visible = false;            
            aktywnaOpcja = null;
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
        }

        protected void wykonaj(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "zadania":
                    aktywnaOpcja = (sender as LinkButton).ID;
                    Response.Redirect("~/OczekujaceZadania.aspx");
                    break;
                case "aplikacje":
                    aktywnaOpcja = (sender as LinkButton).ID;
                    Response.Redirect("~/Aplikacje/ListaAplikacji.aspx");
                    break;
                case "akta":
                    aktywnaOpcja = (sender as LinkButton).ID;
                    Response.Redirect("~/Akta/AktaSpraw.aspx");
                    break;
                case "rejestry":
                    aktywnaOpcja = (sender as LinkButton).ID;
                    Response.Redirect("~/Rejestry/ListaRejestrow.aspx");
                    break;
                case "szukanie":
                    aktywnaOpcja = (sender as LinkButton).ID;
                    Response.Redirect("~/Wyszukiwarka/WyszukiwarkaLista.aspx");
                    break;
            }
        }

        private DateTime GetBuildDate()
        {
            AssemblyName an = Assembly.GetExecutingAssembly().GetName();
            DateTime date = new DateTime(2000, 1, 1, 0, 0, 0);
            date += TimeSpan.FromDays(an.Version.Build) +
            TimeSpan.FromSeconds(an.Version.Revision * 2);
            return date;
        }        
    }
}
