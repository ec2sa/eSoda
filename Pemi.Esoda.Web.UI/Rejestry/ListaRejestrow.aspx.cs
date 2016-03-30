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
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Text;

namespace Pemi.Esoda.Web.UI.Rejestry
{
    public partial class ListaRejestrow : BaseContentPage
    {
        public string APath
        {
            get
            {
                return Page.Request.ApplicationPath == "/" ? "" : Page.Request.ApplicationPath;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["pageNumber"] = 1;
                ViewState["pageSize"] = int.Parse(rozmiarStrony.SelectedValue);
                LoadYears();
                int id;
                if (User.IsInRole("Administratorzy") && Request.QueryString["regid"] != null && int.TryParse(Request.QueryString["regid"].ToString(), out id))
                {
                    lnkRegistry.Visible = (Session["regPreview"] != null);
                    ViewState["registryId"] = id;

                    LoadRegistryData(id);
                }
            }
            if (Session["kryteriaRejestru"] != null)
                loadSearchCriteriaRegistryDefinition(Session["kryteriaRejestru"].ToString());
        }

        private void EnableDisablePdfPrinter()
        {
            try
            {

                if (ViewState["registryId"] != null)
                {
                    int regID;
                    if (!int.TryParse(ViewState["registryId"].ToString(), out regID))
                        regID = -1;

                    if (new RegistryDAO().IsXslFoExist(regID))
                    {
                        printPdfLink.InnerHtml = String.Format("Drukuj");
                        printPdfLink.Attributes["href"] = string.Format("~/Rejestry/Pdf.aspx?regId={0}", regID.ToString());

                    }
                    else
                    {
                        printPdfLink.InnerHtml = String.Format("Drukuj");
                        printPdfLink.Attributes.Remove("href");
                    }
                }

            }
            catch (Exception) { }

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is GridView)
            {
                ViewState["pageNumber"] = "1";
                Session.Remove("SearchCriteria");
            }

            int id = -1;
            wyszukiwanie.Visible = true;
            printpdf.Visible = true;
            if (GridView1.SelectedDataKey != null)
            {
                if (!int.TryParse(GridView1.SelectedDataKey.Value.ToString(), out id))
                {
                    BaseContentPage.SetError("Nie ma takiego rejestru", "");
                    return;
                }
            }
            LoadRegistryData(id);
        }

        private void LoadRegistryData(int regId)
        {

            ViewState["registryId"] = regId;
            RejestryDSTableAdapters.DaneRejestruTableAdapter ds = new Pemi.Esoda.Web.UI.Rejestry.RejestryDSTableAdapters.DaneRejestruTableAdapter();

            RegistryDAO dao = new RegistryDAO();

            RejestryDS.DaneRejestruDataTable dt = ds.GetData(regId);

            //przeniesione do metofy BindItems
            //XslTransform itemsXslt = new XslTransform();

            //string customXslt = new RegistryDAO().GetRegistryXslt(regId);

            //if (String.IsNullOrEmpty(customXslt))
            //{
            //    itemsXslt.Load(Server.MapPath("~/xslt/registryView.xslt"));
            //}
            //else
            //{
            //    itemsXslt.Load(XmlReader.Create(new StringReader(customXslt)));
            //}


            registryName.Text = dt[0].nazwa;
            if (dt[0].wydzialGlowny.Length > 0)
                registryName.Text += string.Format(" ({0})", dt[0].wydzialGlowny);

            //registryContent.Transform = itemsXslt;
            Session["kryteriaRejestru"] = dt[0].definicja;

            loadSearchCriteriaRegistryDefinition(dt[0].definicja);

            List<string> sc = new List<string>();
            foreach (Control c in kryteria.Controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    sc.Add(tb.Text);
                }
            }

            StringBuilder criteria = new StringBuilder();

            for (int i = 0; i < sc.Count; i++)
            {
                criteria.Append(sc[i]);
                if (i != sc.Count - 1)
                    criteria.Append("|");
            }

            Session.Add("SearchCriteria", criteria);

            using (XmlReader xr = getCurrentSearchRegistryPage(criteria.ToString()))
            {
                if (xr != null)
                    BindItems(xr, regId);
                else
                    EnablePager(false);
            }

            EnableDisablePdfPrinter();

            //using (XmlReader xr = getCurrentRegistryPage())
            //{
            //    if (xr != null)
            //        BindItems(xr, regId);
            //    else
            //        EnablePager(false);
            //}

            //EnableDisablePdfPrinter();
        }

        void EnablePager(bool enabled)
        {
            nastepnaStrona.Enabled = pierwszaStrona.Enabled = poprzedniaStrona.Enabled = ostatniaStrona.Enabled = nrStrony.Enabled = enabled;
        }

        private XmlReader getCurrentRegistryPage()
        {
            int registryId;
            int currentPage;
            int pageSize;
            int totalRows;
            int pageCount;

            if (int.TryParse(ViewState["registryId"].ToString(), out registryId))
                if (int.TryParse(ViewState["pageNumber"].ToString(), out currentPage))
                    if (int.TryParse(ViewState["pageSize"].ToString(), out pageSize))
                    {
                        XmlReader xr = (new RegistryDAO()).GetRegistryItems(new Guid(Membership.GetUser().ProviderUserKey.ToString()), registryId, currentPage, pageSize, out totalRows);
                        pageCount = totalRows / pageSize + (totalRows % pageSize > 0 ? 1 : 0);
                        liczbaStron.Text = pageCount.ToString();
                        nrStrony.Items.Clear();
                        for (int i = 1; i <= pageCount; i++)
                            nrStrony.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        //if (nrStrony.Items.Count > 0 && nrStrony.Items.FindByValue(currentPage.ToString()) != null)
                        //{
                        if (nrStrony.Items.FindByValue(currentPage.ToString()) != null)
                            nrStrony.SelectedValue = currentPage.ToString();
                        else
                            nrStrony.SelectedValue = "1";
                        return xr; // zamykany
                        //}
                    }
            return null;
        }

        private void BindItems(XmlReader xr, int idR)
        {
            if (xr.Read())
            {
                XslTransform itemsXslt = new XslTransform();

                string customXslt = new RegistryDAO().GetRegistryXslt(idR);

                if (String.IsNullOrEmpty(customXslt))
                {
                    itemsXslt.Load(Server.MapPath("~/xslt/registryView.xslt"));
                }
                else
                {
                    itemsXslt.Load(XmlReader.Create(new StringReader(customXslt)));
                }

                string itemsXml = xr.ReadOuterXml();
                int id = new RegistryDAO().GetDailyLogID();
                if (id == idR)
                {
                    XsltArgumentList args = new XsltArgumentList();
                    args.AddParam("history", "", "true");
                    registryContent.TransformArgumentList = args;
                }

                registryContent.DocumentContent = itemsXml;
                registryContent.Transform = itemsXslt;

                EnablePager(itemsXml.Contains("pozycja"));
            }
        }

        private void LoadYears()
        {
            RegistryDAO dao = new RegistryDAO();
            rok.DataSource = dao.GetRegistryYears();
            rok.DataTextField = "rok";
            rok.DataBind();
            rok.SelectedValue = DateTime.Today.Year.ToString();
        }

        protected void GridView1_Sorted(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
        }

        private void loadSearchCriteriaRegistryDefinition(string definicja)
        {
            string[] sCriteria = null;
            if (Session["SearchCriteria"] != null)
            {
                sCriteria = Session["SearchCriteria"].ToString().Split('|');
            }


            kryteria.Controls.Clear();
            if (definicja == null)
                return;

            XPathDocument xpd = new XPathDocument(new StringReader(definicja));
            XPathNavigator xpn = xpd.CreateNavigator();
            XPathNodeIterator xpni = xpn.Select("/definicjaRejestru/wyszukiwanie/kryterium");
            if (xpni.Count == 0)
            {
                kryteria.Controls.Add(new LiteralControl("Brak zdefiniowanych kryteriów wyszukiwania"));
                szukaj.Visible = false;
                wyczysc.Visible = false;
                return;
            }
            else
            {
                szukaj.Visible = true;
                wyczysc.Visible = true;
            }

            int i = 0;
            while (xpni.MoveNext())
            {
                Label etykieta = new Label();
                TextBox pole = new TextBox();
                HtmlGenericControl br = new HtmlGenericControl("br");
                pole.ID = "p" + xpni.CurrentPosition.ToString();
                if (sCriteria != null)
                {
                    if (i < sCriteria.Length)
                    {
                        pole.Text = sCriteria[i];
                        i++;
                    }
                }
                etykieta.ID = "e" + xpni.CurrentPosition.ToString();
                etykieta.AssociatedControlID = pole.ID;// "p" + xpni.CurrentPosition.ToString();
                etykieta.Text = xpni.Current.SelectSingleNode("@etykieta").Value;

                //pole.ID = etykieta.AssociatedControlID;
                kryteria.Controls.Add(etykieta);
                kryteria.Controls.Add(pole);
                kryteria.Controls.Add(br);
            }
        }

        protected void szukaj_Click(object sender, EventArgs e)
        {
            List<string> sc = new List<string>();
            foreach (Control c in kryteria.Controls)
            {
                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    sc.Add(tb.Text);
                }
            }

            //int registryId = 0;

            //if (ViewState["registryId"] != null && int.TryParse(ViewState["registryId"].ToString(), out registryId))
            //{
            //    using (XmlReader xr = (new RegistryDAO()).GetRegistryItemsForCriteria(registryId, 1, 10000, sc.ToArray()))
            //    {
            //        BindItems(xr, registryId);
            //    }
            //}

            int registryId = 0;

            if (ViewState["registryId"] != null && int.TryParse(ViewState["registryId"].ToString(), out registryId))
            {
                StringBuilder criteria = new StringBuilder();

                for (int i = 0; i < sc.Count; i++)
                {
                    criteria.Append(sc[i]);
                    if (i != sc.Count - 1)
                        criteria.Append("|");
                }

                Session.Add("SearchCriteria", criteria);
                ViewState["pageNumber"] = 1;//wyniki wyszukiwania zawsze od pierwszej strony
                using (XmlReader xr = getCurrentSearchRegistryPage(criteria.ToString()))
                {
                    if (xr != null)
                        BindItems(xr, registryId);
                    else
                        EnablePager(false);
                }

                EnableDisablePdfPrinter();
            }

        }

        private XmlReader getCurrentSearchRegistryPage(string criteria)
        {
            int registryId;
            int currentPage;
            int pageSize;
            int totalRows;
            int pageCount;

            if (int.TryParse(ViewState["registryId"].ToString(), out registryId))
                if (int.TryParse(ViewState["pageNumber"].ToString(), out currentPage))
                    if (int.TryParse(ViewState["pageSize"].ToString(), out pageSize))
                    {
                        XmlReader xr = (new RegistryDAO()).GetRegistrySearchItems(new Guid(Membership.GetUser().ProviderUserKey.ToString()), registryId, currentPage, pageSize, criteria, out totalRows);
                        pageCount = totalRows / pageSize + (totalRows % pageSize > 0 ? 1 : 0);
                        liczbaStron.Text = pageCount.ToString();
                        nrStrony.Items.Clear();
                        for (int i = 1; i <= pageCount; i++)
                            nrStrony.Items.Add(new ListItem(i.ToString(), i.ToString()));
                        if (nrStrony.Items.FindByValue(currentPage.ToString()) != null)
                            nrStrony.SelectedValue = currentPage.ToString();
                        else
                            nrStrony.SelectedValue = "1";
                        return xr; // zamykany                        
                    }
            return null;
        }

        protected void wyczysc_Click(object sender, EventArgs e)
        {
            Session.Remove("SearchCriteria");
            ViewState["pageNumber"] = 1;
            GridView1_SelectedIndexChanged(this, null);
        }

        protected void zmianaStrony(object sender, CommandEventArgs e)
        {
            int registryId;
            int currentPage;
            int pageSize;
            int pageCount;

            if (ViewState["registryId"] == null || !int.TryParse(ViewState["registryId"].ToString(), out registryId))
                return;
            if (ViewState["pageNumber"] == null || !int.TryParse(ViewState["pageNumber"].ToString(), out currentPage))
                return;
            if (ViewState["pageSize"] == null || !int.TryParse(ViewState["pageSize"].ToString(), out pageSize))
                return;

            currentPage = (nrStrony.SelectedValue.Equals("")) ? 1 : int.Parse(nrStrony.SelectedValue);
            pageCount = (nrStrony.Items.Count > 0) ? int.Parse(nrStrony.Items[nrStrony.Items.Count - 1].Value) : 1;

            switch (e.CommandArgument.ToString())
            {
                case "First":
                    currentPage = 1;
                    break;
                case "Prev":
                    currentPage -= 1;
                    if (currentPage == 0)
                        currentPage = 1;
                    break;
                case "Next":
                    currentPage += 1;
                    if (currentPage > pageCount)
                        currentPage = pageCount;
                    break;
                case "Last":
                    currentPage = pageCount;
                    break;

            }

            ViewState["pageNumber"] = currentPage;
            if (GridView1.SelectedDataKey != null)
                GridView1_SelectedIndexChanged(null, null);
            else
                LoadRegistryData(int.Parse(ViewState["registryId"].ToString()));

        }

        protected void zmianaNumeruStrony(object sender, EventArgs e)
        {
            ViewState["pageNumber"] = (nrStrony.SelectedValue.Equals("")) ? "1" : nrStrony.SelectedValue;
            ViewState["pageSize"] = int.Parse(rozmiarStrony.SelectedValue);
            zmianaStrony(this, new CommandEventArgs("Page", "Current"));
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal litRegName = (Literal)e.Row.FindControl("litRegName");
                if (litRegName != null)
                {
                    string sRegName = DataBinder.Eval(e.Row.DataItem, "nazwa").ToString();

                    int sLength = 28;
                    for (int i = 0; i < sRegName.Length; i += sLength)
                    {
                        if (i > 0)
                            litRegName.Text += "<br/>";

                        int sEndLength = (i + sLength < sRegName.Length) ? sLength : sRegName.Length - i;
                        litRegName.Text += sRegName.Substring(i, sEndLength);
                    }
                }
            }
        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            if (ObjectDataSource1.SelectParameters["userId"] != null)
            {
                ObjectDataSource1.SelectParameters["userId"].DefaultValue = Membership.GetUser().ProviderUserKey.ToString();
            }
        }

        protected void lnkPrintPdf_Click(object sender, EventArgs e)
        {
            int regID;
            if (!int.TryParse(ViewState["registryId"].ToString(), out regID))
                regID = -1;

            Page.Response.Redirect(String.Format("~/Rejestry/Pdf.aspx?RegId={0}", regID));
        }

        protected void goToRC(object sender, CommandEventArgs e)
        {
            Session["rcid"] = e.CommandName;
            Response.Redirect("~/Rejestry/rejestrCentralny.aspx",true);
        }
    }
}

