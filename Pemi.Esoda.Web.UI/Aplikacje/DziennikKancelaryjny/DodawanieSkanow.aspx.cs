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
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Pemi.Esoda.Tools;
using System.Xml;
using System.Xml.XPath;
using System.Text;

namespace Pemi.Esoda.Web.UI
{
    public partial class DodawanieSkanow : BaseContentPage, IAddItemScanView
    {
        private AddItemScanPresenter presenter;

        public bool IsInvoice { get { return Request.QueryString["rf"] != null; } }

        protected int aktualnyDokument
        {
            get
            {
                if (Session["{73C84BD0-7552-4b0d-BBD8-1A3299B26327}"] == null)
                    Session["{73C84BD0-7552-4b0d-BBD8-1A3299B26327}"] = 1;
                return (int)Session["{73C84BD0-7552-4b0d-BBD8-1A3299B26327}"];
            }
            set
            {
                Session["{73C84BD0-7552-4b0d-BBD8-1A3299B26327}"] = value;
                pozycjaZalacznika.Text = string.Format("{0} z {1}", aktualnyDokument, wybraneDokumenty.Length);
            }
        }

        protected string[] wybraneDokumenty
        {
            get
            {
                if (Session["{0C9330BA-5C65-4706-BE50-83A46EB5B836}"] == null) return new string[0];
                return Session["{0C9330BA-5C65-4706-BE50-83A46EB5B836}"] as string[];
            }
            set
            {
                Session["{0C9330BA-5C65-4706-BE50-83A46EB5B836}"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Pemi.Esoda.Tools.Configuration.ScannersConfigurationFile))
            {
                BaseContentPage.SetError("Nie uda³o siê odnaleŸæ pliku konfiguracyjnego skanerów", "~/Aplikacje/DziennikKancelaryjny/EdycjaSkanowPozycjiDziennika.aspx");
                return;
            }
            presenter = new AddItemScanPresenter(this, new WebSessionProvider());
            if (!IsPostBack)
                presenter.Initialize();

            liczbaNowychSkanow.Text = MonitorUrzadzen.LiczbaOczekujacychSkanow().ToString();
        }

        protected void filtrujSkany(object sender, EventArgs e)
        {
            presenter.SearchForScans(ddlLokalizacja.SelectedValue, ddlUrzadzenie.SelectedValue, ddlRodzajDokumentu.SelectedValue, ddlZrodloDokumentu.SelectedValue);
            gvListSkanow.DataBind();            
        }

        protected void pobierzSkany(object sender, EventArgs e)
        {
            TiffManagerKonfiguracja konf = new TiffManagerKonfiguracja();
            konf.FormatMiniatury = ImageFormat.Gif;
            konf.FormatPodgladu = ImageFormat.Gif;
            konf.SzerokoscMiniatury = 50;
            konf.WysokoscMiniatury = 70;
            konf.SzerokoscPodgladu = 430;
            konf.WysokoscPodgladu = 600;
            konf.PlikKonfiguracyjnySkanerow = Pemi.Esoda.Tools.Configuration.ScannersConfigurationFile;
            konf.KatalogWyjsciowy = Server.MapPath(Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory);
            TiffManager tm = new TiffManager(konf);

            string[] pliki = System.IO.Directory.GetFiles(MonitorUrzadzen.DomyslnyKatalog, "*.tif?", System.IO.SearchOption.AllDirectories);

            StringBuilder errMsg = new StringBuilder();
            int badFileCount = 0;
            foreach (string nazwaPliku in pliki)
            {
                try
                {
                    tm.WczytajZPliku(nazwaPliku);
                    tm.ZapiszDokumentyLogiczneDoKatalogu();
                    if (tm.NazwaPlikuSkanuZeSciezka != null && tm.NazwaPlikuSkanuZeSciezka.Length > 0)
                        System.IO.File.Delete(tm.NazwaPlikuSkanuZeSciezka);
                }
                catch//(Exception ex)
                {
                    if (!errMsg.ToString().Contains(nazwaPliku))
                    {
                        badFileCount++;
                        if (errMsg.Length > 0)
                            errMsg.Append(", ");
                        errMsg.Append(nazwaPliku);
                    }
                }
            }
            if (errMsg.Length > 0)
            {
                WebMsgBox.Show(this, string.Format("Nie uda³o siê pobraæ wszystkich skanów. {0} spoœród {1} plików nie s¹ poprawnymi plikami tiff lub zawieraj¹ b³êdy nag³ówka. Pliki: {2}", badFileCount, pliki.Length, errMsg.ToString().Replace("\\", "\\\\")));
            }
            liczbaNowychSkanow.Text = MonitorUrzadzen.LiczbaOczekujacychSkanow().ToString();
            gvListSkanow.DataBind();
        }

        protected void listy_SelectedIndexChanged(object sender, EventArgs e) { }

        protected void dokumenty_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "opiszPojedynczy":
                    wybraneDokumenty = new string[] { Server.MapPath(Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory + "/" + System.IO.Path.GetFileNameWithoutExtension(e.CommandArgument.ToString()) + ".tif") };
                    widokListy.Visible = false;
                    widokSzczegolow.Visible = true;
                    widokNawigacjiZalacznikow.Visible = false;
                    opisywanieSkanu.Visible = true;
                    podepnijPodgladuDokumentu(e.CommandArgument.ToString());
                    break;
                case "opiszIstniejacy":
                    wybraneDokumenty = new string[] { e.CommandArgument.ToString() };
                    widokListy.Visible = false;
                    widokSzczegolow.Visible = true;
                    widokNawigacjiZalacznikow.Visible = false;
                    opisywanieSkanu.Visible = true;
                    podepnijPodgladuDokumentu(new Guid(e.CommandArgument.ToString()));

                    break;
            }
        }
        protected void dokumenty_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //blokada edycji
            //int liczbaStron = int.Parse(ixpn.CreateNavigator().SelectSingleNode("liczbaStron").Value);
            //e.Item.FindControl("edycjaDokumentu").Visible = liczbaStron > 1;
            e.Item.FindControl("edycjaDokumentu").Visible = false;

            IXPathNavigable ixpn = e.Item.DataItem as IXPathNavigable;
            XPathNavigator xpn = ixpn.CreateNavigator();
            if (xpn.SelectSingleNode("guid") != null)
            {
                ImageButton imgb = (e.Item.FindControl("btnMiniatura") as ImageButton);
                if (imgb == null) return;

                (e.Item.FindControl("cbDodajDoOpisu") as CheckBox).Enabled = false;

                imgb.ImageUrl = string.Format("~/image.aspx?id={0}&w=430", xpn.SelectSingleNode("guid").Value);
                imgb.CommandName = "opiszIstniejacy";
                imgb.CommandArgument = xpn.SelectSingleNode("guid").Value;
            }
        }

        protected void dodajSkan(object sender, EventArgs e)
        {
            presenter.AssignScanWithRegistryItem(wybraneDokumenty, opisElementu.Text, isMainItem.Checked,IsInvoice);

            powrotDoListy_Click(null, null);
        }

        protected void poprzedniZalacznik(object sender, EventArgs e)
        {
            if (aktualnyDokument == 1) return;
            aktualnyDokument--;
            podepnijPodgladuDokumentu(wybraneDokumenty[aktualnyDokument - 1]);
        }

        protected void nastepnyZalacznik(object sender, EventArgs e)
        {
            if (aktualnyDokument + 1 > wybraneDokumenty.Length) return;
            aktualnyDokument++;
            podepnijPodgladuDokumentu(wybraneDokumenty[aktualnyDokument - 1]);
        }

        private void podepnijPodgladuDokumentu(Guid id)
        {
            podglad.ImageUrl = string.Format("~/image.aspx?id={0}&w=430", id);
        }

        private void podepnijPodgladuDokumentu(string p)
        {
            XPathHelperClass xp = new XPathHelperClass(Server.MapPath(Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory + "/" + System.IO.Path.GetFileNameWithoutExtension(p) + ".xml"));
            string nazwa = Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory + "/" + xp.PobierzWartosc("/dokument/pierwszaStrona");
            podglad.ImageUrl = nazwa;
        }

        protected void powrotDoListy_Click(object sender, EventArgs e)
        {
            widokListy.Visible = true;
            widokSzczegolow.Visible = false;
            opisywanieSkanu.Visible = false;
            isMainItem.Checked = false;
            opisElementu.Text = "";
            filtrujSkany(this, null);
        }

        protected void opisanieGrupySkanow(object sender, EventArgs e)
        {
            List<string> wybrane = new List<string>();
            //foreach (RepeaterItem ri in listaSkanow.Items)
            //{
            //    if ((ri.FindControl("cbDodajDoOpisu") as CheckBox).Checked)
            //    {
            //        wybrane.Add(Server.MapPath(Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory + "/" + System.IO.Path.GetFileNameWithoutExtension((ri.FindControl("btnMiniatura") as ImageButton).CommandArgument.ToString()) + ".tif"));
            //    }
            //}

            foreach (GridViewRow row in gvListSkanow.Rows)
            {
                if ((row.FindControl("cbDodajDoOpisu") as CheckBox).Checked)
                {
                    wybrane.Add(Server.MapPath(Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory + "/" + System.IO.Path.GetFileNameWithoutExtension((row.FindControl("btnMiniatura") as ImageButton).CommandArgument.ToString()) + ".tif"));
                }
            }


            if (wybrane.Count < 2)
            {
                WebMsgBox.Show(this, "¯eby dodaæ grupê trzeba zaznaczyæ co najmniej 2 skany!");
                return;
            }
           
            opisywanieSkanu.Visible = true;
            isMainItem.Enabled = false;
            widokListy.Visible = false;
            widokSzczegolow.Visible = true;
            widokNawigacjiZalacznikow.Visible = true;
            wybraneDokumenty = wybrane.ToArray();
            aktualnyDokument = 1;
            podepnijPodgladuDokumentu(wybrane[0]);
        }

        #region IAddItemScanView Members

        int IAddItemScanView.IncomingScansCount
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }

        void IAddItemScanView.BindScans(string xmlContent, string xpath)
        {
            //XmlDataSource xds = new XmlDataSource();
            //xds.EnableCaching = false;
            //xds.Data = xmlContent;
            //xds.XPath = xpath;
            //listaSkanow.DataSource = xds;
            //listaSkanow.DataBind();

            //gvListSkanow.DataSource = xds;
            //gvListSkanow.DataBind();
        }

        void IAddItemScanView.BindConditions()
        {

            KonfiguratorUrzadzen ku = new KonfiguratorUrzadzen(Pemi.Esoda.Tools.Configuration.ScannersConfigurationFile);
            ddlLokalizacja.DataSource = ku.PobierzLokalizacje();
            ddlLokalizacja.DataTextField = "Text";
            ddlLokalizacja.DataValueField = "Value";
            ddlLokalizacja.DataBind();

            ddlUrzadzenie.DataSource = ku.PobierzUrzadzenia();
            ddlUrzadzenie.DataTextField = "Text";
            ddlUrzadzenie.DataValueField = "Value";
            ddlUrzadzenie.DataBind();

            ddlRodzajDokumentu.DataSource = ku.PobierzRodzajeDokumentow();
            ddlRodzajDokumentu.DataTextField = "Text";
            ddlRodzajDokumentu.DataValueField = "Value";
            ddlRodzajDokumentu.DataBind();

            ddlZrodloDokumentu.DataSource = ku.PobierzZrodlaDokumentow();
            ddlZrodloDokumentu.DataTextField = "Text";
            ddlZrodloDokumentu.DataValueField = "Value";
            ddlZrodloDokumentu.DataBind();
        }

        void IAddItemScanView.BindUnassignedItems(string xmlContent, string xpath)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        int IAddItemScanView.ItemId
        {
            set { numerPozycji.Text = string.Format("[numer pozycji w rejestrze: {0}]", value); }
        }

        #endregion

        protected void gvListSkanow_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "opiszPojedynczy":
                    wybraneDokumenty = new string[] { Server.MapPath(Pemi.Esoda.Tools.Configuration.VirtualTemporaryDirectory + "/" + System.IO.Path.GetFileNameWithoutExtension(e.CommandArgument.ToString()) + ".tif") };
                    widokListy.Visible = false;
                    widokSzczegolow.Visible = true;
                    widokNawigacjiZalacznikow.Visible = false;
                    opisywanieSkanu.Visible = true;
                    podepnijPodgladuDokumentu(e.CommandArgument.ToString());
                    break;
                case "opiszIstniejacy":
                    wybraneDokumenty = new string[] { e.CommandArgument.ToString() };
                    widokListy.Visible = false;
                    widokSzczegolow.Visible = true;
                    widokNawigacjiZalacznikow.Visible = false;
                    opisywanieSkanu.Visible = true;
                    podepnijPodgladuDokumentu(new Guid(e.CommandArgument.ToString()));

                    break;
            }
        }

        protected void gvListSkanow_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.FindControl("edycjaDokumentu").Visible = false;

                //IXPathNavigable ixpn = e.Item.DataItem as IXPathNavigable;
                //XPathNavigator xpn = ixpn.CreateNavigator();
                //if (xpn.SelectSingleNode("guid") != null)
                //{
                //    ImageButton imgb = (e.Item.FindControl("btnMiniatura") as ImageButton);
                //    if (imgb == null) return;

                //    (e.Item.FindControl("cbDodajDoOpisu") as CheckBox).Enabled = false;

                //    imgb.ImageUrl = string.Format("~/image.aspx?id={0}&w=430", xpn.SelectSingleNode("guid").Value);
                //    imgb.CommandName = "opiszIstniejacy";
                //    imgb.CommandArgument = xpn.SelectSingleNode("guid").Value;
                //}

                Guid scanGuid = Guid.Empty;
                object scanId = DataBinder.Eval(e.Row.DataItem, "guid");
                if (scanId != null)
                {
                    scanGuid = new Guid(scanId.ToString());
                }
                else
                    return;
                 
                if (scanGuid != Guid.Empty)
                {                    
                    ImageButton imgb = (e.Row.FindControl("btnMiniatura") as ImageButton);
                    if (imgb == null) return;

                    (e.Row.FindControl("cbDodajDoOpisu") as CheckBox).Enabled = false;

                    imgb.ImageUrl = string.Format("~/image.aspx?id={0}&w=430", scanGuid.ToString());
                    imgb.CommandName = "opiszIstniejacy";
                    imgb.CommandArgument = scanGuid.ToString();
                }
            }
        }

        protected void ddlPageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvListSkanow.PageIndex = int.Parse(((DropDownList)sender).SelectedValue);
            gvListSkanow.DataBind();
        }

        protected void ddlPageSelector_DataBound(object sender, EventArgs e)
        {
            if (gvListSkanow.PageCount != ((DropDownList)sender).Items.Count)
            {
                for (int i = 0; i < gvListSkanow.PageCount; i++)
                    ((DropDownList)sender).Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                ((DropDownList)sender).SelectedIndex = gvListSkanow.PageIndex;
            }
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

        public void GoBackToEdit(object sender, EventArgs e)
        {
            string parentPage = (string)Session["parentPage"];
            string rf = IsInvoice ? "&rf=1" : "";
            if (parentPage == "a")
            {
                Response.Redirect("EdycjaSkanowPozycjiDziennika.aspx?pp=a" + rf);
            }
            else //if (parentPage == "s")
            {
                Response.Redirect("EdycjaSkanowPozycjiDziennika.aspx?pp=s" + rf);
            }
        }
    }
}