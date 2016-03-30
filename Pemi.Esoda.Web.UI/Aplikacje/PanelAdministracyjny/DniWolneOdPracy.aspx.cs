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
using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Pemi.Esoda.Tools;
using System.Data.SqlClient;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class DniWolneOdPracy : System.Web.UI.Page
    {
        private HolidayDateDAO dao;

        private int? HolidayDateID
        {
            get
            {
                if (String.IsNullOrEmpty(holidayDateID.Value))
                    return null;
                else
                    return int.Parse(holidayDateID.Value);
            }
            set
            {
                holidayDateID.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dao = new HolidayDateDAO();
            lblMessage.Text = string.Empty;

            if (!Page.IsPostBack)
            {
                try
                {
                    LoadHolidayDateList(DateTime.Today.Year);
                    LoadAvailableYears(DateTime.Today.Year);
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }

        private void LoadHolidayDateList(int year)
        {
            holidayDateList.DataSource = dao.GetHolidayDateList(year);
            holidayDateList.DataBind();
        }

        private void LoadAvailableYears(int selectedYear)
        {
            availableYears.DataSource = dao.GetAvailableYears();
            availableYears.DataBind();
            availableYears.SelectedValue = selectedYear.ToString();
            
        }

        private void LoadHolidayDate(int id)
        {
            HolidayDateDTO hd = dao.GetHolidayDate(id);
            this.HolidayDateID = hd.ID;
            this.holidayDate.Text = hd.Date.ToShortDateString();
            this.description.Text = hd.Description;
        }

        protected void save_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    HolidayDateDTO hd = new HolidayDateDTO(HolidayDateID, DateTime.Parse(holidayDate.Text), description.Text);
                    dao.SetHolidayDate(hd);
                    if (!String.IsNullOrEmpty(availableYears.SelectedValue))
                    {
                        LoadHolidayDateList(int.Parse(availableYears.SelectedValue));
                        LoadAvailableYears(int.Parse(availableYears.SelectedValue));
                    }
                    ClearForm();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                }
            }
        }

        protected void new_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            try
            {
                dao.DeleteHolidayDate(this.HolidayDateID);
                if (!String.IsNullOrEmpty(availableYears.SelectedValue))
                {
                    LoadHolidayDateList(int.Parse(availableYears.SelectedValue));
                    LoadAvailableYears(DateTime.Today.Year); // wybierz
                }
                ClearForm();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }

        }


        private void ClearForm()
        {
            this.HolidayDateID = null;
            this.description.Text = string.Empty;
            this.holidayDate.Text = string.Empty;
        }

        protected void holidayDateList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int selectedRow;
            try
            {
                if (e.CommandName.Equals("hdID"))
                {
                    selectedRow = int.Parse(e.CommandArgument.ToString());
                    LoadHolidayDate(selectedRow);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void availableYears_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHolidayDateList(int.Parse(((DropDownList)sender).SelectedItem.Value));
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
    }
}
