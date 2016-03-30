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
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Web.UI
{
    public partial class EdycjaSkanowPozycjiDziennika : BaseContentPage, IViewItemScansView
    {
        public bool IsInvoice
        {
            get
            {
                return Request.QueryString["rf"] != null;
                
            }
        }

        private ViewItemScansPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["parentPage"] = Request.QueryString["pp"];
            presenter = new ViewItemScansPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
            {
                presenter.Initialize();
            }
        }

        protected void obslugaListySkanow(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "wybierzSkan":
                    presenter.SelectScan(e.CommandArgument.ToString());
                    opisElementu.Text = (e.Item.FindControl("opisSkanu") as Literal).Text;
                    isMainItem.Checked = (e.Item.FindControl("czyGlowny") as Literal).Visible;
                    break;

            }
        }

        protected void zapiszZmiany(object sender, EventArgs e)
        {
            if (Session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"] != null)
                presenter.SaveChanges(opisElementu.Text, isMainItem.Checked);
            else
                Pemi.Esoda.Tools.WebMsgBox.Show(this, "Trzeba wybraæ skan!");

        }

        protected void usunSkan(object sender, EventArgs e)
        {
            if (Session["{BB59DB5B-4DDD-4436-87FD-ABEB282D4BE1}"] != null)
                presenter.ReleaseScan();
            else
                Pemi.Esoda.Tools.WebMsgBox.Show(this, "Trzeba wybraæ skan!");
        }

        protected void dodajNowySkan(object sender, EventArgs e)
        {
            presenter.AddNewScan();
        }

        public void GoBack(object sender, EventArgs e)
        {
            string parentPage = (string)Session["parentPage"];
            if (parentPage == "a")
            {
                Response.Redirect("PrzegladDziennika.aspx");
            }
            else if (parentPage == "s")
            {
                Response.Redirect("PrzegladDziennikaSimple.aspx");
            }
        }

        #region IViewItemScansView Members

        string IViewItemScansView.ItemContent
        {
            set
            {

                XmlDataSource xds = new XmlDataSource();
                xds.Data = value;
                xds.EnableCaching = false;
                wybranaPozycja.DataSource = null;
                if (xds.Data != "")
                {
                    xds.TransformFile = "~/xslt/elements2attributes.xslt";
                    wybranaPozycja.DataSource = xds;
                }
                wybranaPozycja.DataBind();
            }
        }

        Collection<DocumentItemDTO> IViewItemScansView.ScanListItems
        {
            set
            {
                listaSkanow.DataSource = value;
                listaSkanow.DataBind();
            }
        }

        string IViewItemScansView.PreviewImage
        {
            set { podgladSkanu.ImageUrl = value; }
        }



        bool IViewItemScansView.IsScanSelected
        {
            get
            {
                return opcjeWybranejPozycji.Visible;
            }
            set
            {
                opcjeWybranejPozycji.Visible = value;
            }
        }


        string IViewItemScansView.RedirectTo
        {
            set
            {
                string rf = IsInvoice ? "?rf=1" : "";
                switch (value.ToLower())
                {
                    case "addnewscan":
                        Response.Redirect("DodawanieSkanow.aspx"+rf);
                        break;
                }
            }
        }


        int IViewItemScansView.ItemID
        {
            set { 
                
                numerPozycji.Text = string.Format("[numer pozycji w rejestrze {1}: {0}]", value,IsInvoice?"faktur":""); }
        }

        #endregion

        #region IViewItemScansView Members


        public bool IsDailyLogItemAccessDenied
        {
            set
            {
                contentPanel.Visible = !value;
                lblDailyLogItemAccessDeniedInfo.Visible = value;
            }
        }

        #endregion
    }
}
