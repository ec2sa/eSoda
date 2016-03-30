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
using System.Data.SqlClient;
using Pemi.Esoda.DTO;
using Pemi.Esoda.Tools;


namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class Komunikat : System.Web.UI.Page
    {
        private int? NoticeID
        {
            get
            {
                if (ViewState["NoticeID"] != null)
                {
                    return (int)ViewState["NoticeID"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["NoticeID"] = value;
            }
        }

        private NoticeDAO dao = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            dao = new NoticeDAO();
            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                LoadNotice();
            }
        }

        private void LoadNotice()
        {
            try
            {
                NoticeDTO notice = dao.GetNotice(NoticeID);
                if (notice != null)
                {
                    NoticeID = notice.NoticeID;
                    tbNotice.Text = notice.Notice;
                    tbEndDate.Text = notice.EndDate.HasValue ? notice.EndDate.Value.ToShortDateString() : string.Empty;
                    tbStartDate.Text = notice.StartDate.HasValue ? notice.StartDate.Value.ToShortDateString() : string.Empty;
                    cbIsActive.Checked = notice.IsActive;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnSaveNotice_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    DateTime startDate;
                    if (!DateTime.TryParse(tbStartDate.Text, out startDate))
                        startDate = DateTime.MinValue;

                    DateTime endDate;
                    if (!DateTime.TryParse(tbEndDate.Text, out endDate))
                        endDate = DateTime.MinValue;

                    NoticeID = dao.SetNotice(NoticeID, tbNotice.Text, startDate, endDate, cbIsActive.Checked);

                    LoadNotice();

                    WebMsgBox.Show(this, "Dane zosta³y zapisane.");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }            
        }
    }
}

