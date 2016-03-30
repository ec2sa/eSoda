using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class DefinicjaZastepstw : System.Web.UI.UserControl
    {
        private UserDAO uDAO = null;

        public event EventHandler CoverChanged;

        private void OnCoverChanged()
        {
            if (CoverChanged != null)
                CoverChanged(this, null);
        }

        public int DublerID
        {
            set
            {
                dublerID.Value = value.ToString();
                LoadDepartments();
                LoadUsersFromDepartments(-1);
                LoadCoverList();
            }
            get
            {
                if (String.IsNullOrEmpty(dublerID.Value))
                    return -1;
                else
                    return int.Parse(dublerID.Value);
            }
        }

        private int SelectedCoverID
        {
            set
            {
                ViewState["SelectedCoverID"] = value;
            }
            get
            {
                if (ViewState["SelectedCoverID"] != null)
                    return (int)ViewState["SelectedCoverID"];
                else
                    return -1;
            }
        }

        private int UserID
        {
            get
            {
                return int.Parse(ddlUsers.SelectedValue);
            }
        }

        private DateTime StartDate
        {
            get
            {
                return DateTime.Parse(startDate.Text);
            }
            set
            {
                startDate.Text = value.ToShortDateString();
            }
        }

        private DateTime EndDate
        {
            get
            {
                return DateTime.Parse(endDate.Text);
            }
            set
            {
                endDate.Text = value.ToShortDateString();
            }
        }

        private void ClearForm()
        {
            startDate.Text = string.Empty;
            endDate.Text = string.Empty;
            LoadDepartments();
            LoadUsersFromDepartments(-1);

        }

        private void LoadUsersFromDepartments(int departmentID)
        {
            ddlUsers.DataSource = uDAO.GetUsersFromDepartment(departmentID, DublerID);
            ddlUsers.DataBind();
        }

        private void LoadDepartments()
        {
            ddlDepartments.DataSource = uDAO.GetDepartments();
            ddlDepartments.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            uDAO = new UserDAO();
            lblError.Text = string.Empty;
        }

        private void LoadCoverList()
        {
            coverList.DataSource = uDAO.GetCoverList(DublerID);
            coverList.DataBind();
        }

        protected void addCover_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    uDAO.SetCover(UserID, DublerID, StartDate, EndDate);
                    LoadCoverList();
                    ClearForm();
                    OnCoverChanged();
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
        }

        private void OnDeleteCover(int coverID)
        {
            try
            {
                uDAO.DelCover(coverID);
                OnCoverChanged();
            }
            catch (Exception e)
            {
                lblError.Text = e.Message;
            }
        }

        protected void coverList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int coverid = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "delCover")
            {
                OnDeleteCover(coverid);
                LoadCoverList();
            }
        }

        protected void ddlDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsersFromDepartments(int.Parse(ddlDepartments.SelectedValue));
        }        
    }
}