using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Data.Common;
using Pemi.Esoda.Tools;
using System.Text;
using System.IO;
using System.Xml.Xsl;
using System.Data.SqlClient;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class edycjaRejestru : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadGroupsList();
                txtRok.Text = DateTime.Today.Year.ToString();

                if (Session["idRejestru"] != null)
                {
                    int regId = int.Parse(Session["idRejestru"].ToString());

                    using (IDataReader dr = (new RegistryDAO()).GetRegistry(regId))
                    {
                        if (dr.Read())                        
                        {
                            Session["idRejestru"] = int.Parse(dr["id"].ToString());
                            Session["idDefinicji"] = int.Parse(dr["idDefinicji"].ToString());
                            
                            ddlMainGroup.SelectedValue = dr["wydzialGlowny"].ToString();

                            txtNazwaRejestru.Text = dr["nazwa"].ToString();
                            hfJRWAId.Value=dr["jrwa"].ToString();
                            ckbSetJRWA.Checked = hfJRWAId.Value.Length == 0;
                            ckbSetJRWA_CheckedChanged(sender, e);
                            
                            LoadReadingGroups(dr["wydzialy"].ToString());
                            LoadEntryTypes(dr["wpisy"].ToString());
                           
                            string xslt = dr["xsltRejestru"].ToString();
                            string xslFo = dr["xslFO"].ToString();
                            if (xslt.Length > 0)
                            {
                                Session["regXslt"] = xslt;
                                lblCustomXSLT.Visible = true;
                                lnkRemoveXSLT.Visible = true;
                            }
                            if (xslFo.Length > 0)
                            {
                                Session["regXslFo"] = xslFo;
                                lblCustomXslFo.Visible = true;
                                lnkRemoveXslFo.Visible = true;
                            }
                            ckbShowEntryDate.Checked = bool.Parse(dr["showEntryDate"].ToString());
                            ckbShowAddingUser.Checked = bool.Parse(dr["showCreatingUser"].ToString());
                            ckbActive.Checked = bool.Parse(dr["aktualny"].ToString());
                            ckbArchive.Checked = bool.Parse(dr["archiwalny"].ToString());
                            txtRok.Text = dr["rok"].ToString();
                        }
                    }
                    int jrwaId = -1;
                    if (int.TryParse(hfJRWAId.Value, out jrwaId))
                    {
                        using (DbDataReader dr = (DbDataReader)(new JrwaDAO()).GetJRWA(jrwaId))
                        {
                            if (dr.Read())
                            {
                                txtJRWA.Text = dr["nazwa"].ToString();
                            }
                        }
                        //txtJRWA.Text = (new JrwaDAO()).GetJRWASymbolById(jrwaId);
                    }
                    else                    
                    {
                        ckbSetJRWA.Checked = true;
                        ckbSetJRWA_CheckedChanged(sender, e);
                    }

                    if (Session["idDefinicji"] != null)
                    {                        
                        string definicja = string.Empty;
                        using (DbDataReader dr = (DbDataReader)(new RegistryDAO()).GetRegistryDefinition(int.Parse(Session["idDefinicji"].ToString())))
                        {
                            if (dr.Read())
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(dr["definicja"].ToString());
                                Session["RegDefinition"] = doc;
                            }
                        }
                    }

                    if (Session["RegEdit"] != null)
                    { }
                    else
                        if (true)
                        {
                           
                        }
                }
                else
                {
                    ddlTypWpisu_SelectedIndexChanged(sender, e);
                    // frmRejestr.ChangeMode(FormViewMode.Insert);
                }                
            }
        }

        void LoadReadingGroups(string groups)
        {
            XmlDocument doc = new XmlDocument();
            if (groups.Length > 0)
            {
                doc.LoadXml(groups);

                lstRedableGroups.Items.Clear();
                // wydziały
                foreach (XmlElement wydzial in doc.SelectNodes("//wydzial[@id>0]"))
                {
                    ListItem item = new ListItem(wydzial.InnerText, wydzial.Attributes["id"].Value);
                    lstRedableGroups.Items.Add(item);
                }
            }
        }

        void LoadEntryTypes(string entries)
        {
            XmlDocument doc = new XmlDocument();
            if (entries.Length > 0)
            {
                doc.LoadXml(entries);
                ddlTypWpisu.SelectedValue = doc.SelectSingleNode("/wpisy").Attributes["typ"].Value;
                ddlTypWpisu_SelectedIndexChanged(null, null);
                ddlTypWpisu.Enabled = false;

                lstRodzajeWpisow.Items.Clear();

                foreach (XmlElement wpis in doc.SelectNodes("//wpis"))
                {
                    if (wpis.Attributes["typeid"] != null) // && !wpis.Attributes["typeid"].Value.Equals("0"))
                    {
                        ListItem item = new ListItem();
                        if (wpis.Attributes["typeid"] != null)
                            item.Value = wpis.Attributes["typeid"].Value;
                        if (wpis.Attributes["catid"] != null)
                            item.Value += "#" + wpis.Attributes["catid"].Value;

                        if (wpis.InnerText.Length > 0)
                            item.Text = wpis.InnerText;
                        lstRodzajeWpisow.Items.Add(item);
                    }
                }
            }
        }

        void LoadDocTypes(string category)
        {
            if (category != null && category != string.Empty)
            {
                ddlDocType.DataSource = (new DocumentDAO()).GetDocTypesForCategory(int.Parse(category));
                ddlDocType.DataValueField = "id";
                ddlDocType.DataTextField = "nazwa";
                ddlDocType.DataBind();
                ddlDocType.Items.Insert(0, new ListItem("( dowolny )", "0"));
            }
        }

        void LoadDocCats()
        {
            ddlDocCat.DataSource = (new DocumentDAO()).GetDocumentCategories();
            ddlDocCat.DataValueField = "id";
            ddlDocCat.DataTextField = "nazwa";
            ddlDocCat.DataBind();            
        }

        void LoadCaseTypes()
        {
            ddlCaseTypes.DataSource = (new CaseDAO()).GetCaseKinds();
            ddlCaseTypes.DataValueField = "ID";
            ddlCaseTypes.DataTextField = "description";
            ddlCaseTypes.DataBind();            
        }

        void LoadGroupsList()
        {
            ddlListaWydzialow.DataSource = (new GroupDAO()).GetGroupsList();
            ddlListaWydzialow.DataTextField = "nazwa";
            ddlListaWydzialow.DataValueField = "id";
            ddlListaWydzialow.DataBind();
            //ddlListaWydzialow.Items.Insert(0, new ListItem("- wybierz -", "-1"));

            ddlMainGroup.DataSource = (new GroupDAO()).GetGroupsList();
            ddlMainGroup.DataTextField = "nazwa";
            ddlMainGroup.DataValueField = "id";
            ddlMainGroup.DataBind();
            ddlMainGroup.Items.Insert(0, new ListItem("- wybierz -", "-1"));
        }

        protected void lnkZapiszRejestr_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int idDefinicji = int.Parse(Session["idDefinicji"].ToString());

                    string nazwa = txtNazwaRejestru.Text;
                    int? jrwa = null;
                    int jrwaId;

                    if (!ckbSetJRWA.Checked)
                        if (int.TryParse(hfJRWAId.Value, out jrwaId))
                            jrwa = jrwaId;

                    string wydzialy = PobierzWydzialy();
                    string wpisy = PobierzWpisy();

                    int rok = int.Parse(txtRok.Text);//DateTime.Now.Year;
                    int wydzialGlowny = int.Parse(ddlMainGroup.SelectedValue);

                    int regId = -1;
                    if (Session["idRejestru"] != null)
                        int.TryParse(Session["idRejestru"].ToString(), out regId);

                    string customXslt = (Session["regXslt"] != null) ? Session["regXslt"].ToString() : string.Empty;

                    string customXslFo = (Session["regXslFo"] != null) ? Session["regXslFo"].ToString() : null;

                    if (regId > 0)
                        (new RegistryDAO()).UpdateRegistry(regId, wydzialGlowny, idDefinicji, nazwa, jrwa, wydzialy, wpisy, customXslt, rok, ckbShowEntryDate.Checked, ckbShowAddingUser.Checked, ckbActive.Checked, ckbArchive.Checked, customXslFo);
                    else
                        regId = (new RegistryDAO()).AddRegistry(wydzialGlowny, idDefinicji, nazwa, jrwa, wydzialy, wpisy, customXslt, rok, ckbShowEntryDate.Checked, ckbShowAddingUser.Checked, ckbActive.Checked, ckbArchive.Checked, customXslFo);
                    if (regId > 0)
                    {
                        WebMsgBox.Show(this, "Dane rejestru zostały zapisane.");
                        if (Session["idRejestru"] == null)
                            Session["idRejestru"] = regId;                        
                    }
                    else
                        WebMsgBox.Show(this, "Wystąpił problem z zapisaniem danych rejestru.");
                }
                catch (SqlException ex)
                {
                    WebMsgBox.Show(this, String.Format("{0}:{1}","Wystąpił problem z zapisaniem danych rejestru",ex.Message));
                }
                catch (Exception ex)
                {
                    WebMsgBox.Show(this, String.Format("{0}:{1}", "Wystąpił problem z zapisaniem danych rejestru", ex.Message));
                }
            }
        }

        private string PobierzWpisy()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("wpisy");
            xw.WriteAttributeString("typ", ddlTypWpisu.SelectedItem.Value);

            if (lstRodzajeWpisow.Items.Count > 0)
            {
                // wpisy
                foreach (ListItem wpis in lstRodzajeWpisow.Items)
                {
                    xw.WriteStartElement("wpis");

                    string []idWpisu = wpis.Value.Split('#');
                    
                    xw.WriteAttributeString("typeid", idWpisu[0]);
                    if(ddlTypWpisu.SelectedValue.Equals("doc") && idWpisu.Length==2)
                        xw.WriteAttributeString("catid", idWpisu[1]);
                    xw.WriteValue(wpis.Text);
                    xw.WriteEndElement(); //wydzial
                }
            }
            //else
            //{
            //    xw.WriteStartElement("wpis");
            //    xw.WriteAttributeString("typeid", "0");
            //    xw.WriteValue("nieokreślony");
            //    xw.WriteEndElement(); //wpis
            //}

            xw.WriteEndElement(); //wpisy
            xw.WriteEndDocument();
            xw.Close();

            return sb.ToString();
        }

        private string PobierzWydzialy()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("wydzialy");

            if (lstRedableGroups.Items.Count > 0)
            {
                // wydziały
                foreach (ListItem group in lstRedableGroups.Items)
                {
                    xw.WriteStartElement("wydzial");
                    xw.WriteAttributeString("id", group.Value);
                    xw.WriteValue(group.Text);
                    xw.WriteEndElement(); //wydzial
                }
            }
            else
            {
                xw.WriteStartElement("wydzial");
                xw.WriteAttributeString("id", "0");
                xw.WriteValue("nieokreślony");
                xw.WriteEndElement(); //wydzial
            }

            xw.WriteEndElement(); //wydziały
            xw.WriteEndDocument();
            xw.Close();

            return sb.ToString();
        }

        protected void cvDefinicjaRejestru_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(Session["RegDefinition"] != null)
            {
                XmlDocument doc = (XmlDocument)Session["RegDefinition"];
                args.IsValid = doc.SelectSingleNode("/definicjaRejestru/pola").HasChildNodes;
            }
            else
                args.IsValid = false;
        }

        protected void ckbSetJRWA_CheckedChanged(object sender, EventArgs e)
        {
            rfvJRWARejestru.Enabled = !ckbSetJRWA.Checked;
            lnkSelectJRWA.Visible = !ckbSetJRWA.Checked;
            txtJRWA.Enabled = !ckbSetJRWA.Checked;
        }

        protected void ddlTypWpisu_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlTypWpisu.SelectedValue)
            {
                case "doc":
                    pnlCaseTypes.Visible = false;
                    pnlDocTypes.Visible = true;
                    LoadDocCats();
                    LoadDocTypes(ddlDocCat.SelectedValue);
                    break;

                case "case":
                    pnlCaseTypes.Visible = true;
                    pnlDocTypes.Visible = false;
                    LoadCaseTypes();
                    break;

                default:
                    pnlCaseTypes.Visible = false;
                    pnlDocTypes.Visible = false;
                    break;
            }
        }

        protected void ddlDocCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDocCat.SelectedIndex > -1)
            {
                LoadDocTypes(ddlDocCat.SelectedValue);
            }
        }

        protected void lnkDodajWydzial_Click(object sender, EventArgs e)
        {
            if (ddlListaWydzialow.SelectedItem != null)
            {
                ListItem wydzial = new ListItem(ddlListaWydzialow.SelectedItem.Text, ddlListaWydzialow.SelectedItem.Value);
                if (!lstRedableGroups.Items.Contains(wydzial))
                    lstRedableGroups.Items.Add(wydzial);
            }
            else
                WebMsgBox.Show(this, "Wybierz wydział do dodania");
        }

        protected void lnkUsunWydzial_Click(object sender, EventArgs e)
        {
            if (lstRedableGroups.SelectedItem != null)
            {
                lstRedableGroups.Items.Remove(lstRedableGroups.SelectedItem);
            }
            else
                WebMsgBox.Show(this, "Wybierz wydzial do usunięcia");
        }

        protected void lnkDodajRodzaj_Click(object sender, EventArgs e)
        {
            string text=string.Empty, value=string.Empty;
            switch (ddlTypWpisu.SelectedValue)
            {
                case "doc":
                    if (ddlDocCat.SelectedItem != null)
                    {
                        text = ddlDocCat.SelectedItem.Text;
                        value = ddlDocCat.SelectedItem.Value;
                        if (ddlDocType.SelectedItem != null)
                        {
                            if(!ddlDocType.SelectedItem.Value.Equals("0"))
                                text += " / " + ddlDocType.SelectedItem.Text;
                            value = ddlDocType.SelectedItem.Value + "#" + ddlDocCat.SelectedItem.Value;
                        }
                    }
                    else
                        WebMsgBox.Show(this, "Wybierz typ dokumentu");                    
                    break;

                case "case":
                    if (ddlCaseTypes.SelectedItem != null)
                    {
                        text = ddlCaseTypes.SelectedItem.Text;
                        value = ddlCaseTypes.SelectedItem.Value;
                    }
                    else
                        WebMsgBox.Show(this, "Wybierz rodzaj sprawy");
                    break;

                default:
                    break;
            }

            if (text.Length > 0 && value.Length > 0)
            {
                ListItem wpis = new ListItem(text, value);
                if (!lstRodzajeWpisow.Items.Contains(wpis))
                    lstRodzajeWpisow.Items.Add(wpis);                
            }
            ddlTypWpisu.Enabled = (lstRodzajeWpisow.Items.Count == 0);
        }

        protected void lnkUsunRodzaj_Click(object sender, EventArgs e)
        {
            if (lstRodzajeWpisow.SelectedItem != null)
            {
                lstRodzajeWpisow.Items.Remove(lstRodzajeWpisow.SelectedItem);
                ddlTypWpisu.Enabled = (lstRodzajeWpisow.Items.Count == 0);
            }
        }

        protected void lnkSelectJRWA_Click(object sender, EventArgs e)
        {
            pnlJRWA.Visible = true;
            JrwaDAO jd = new JrwaDAO();
            dsxmlJRWA.Data = jd.GetActiveJRWAListXml();
            tvJRWA.DataBind();
        }

        protected void tvJRWA_SelectedNodeChanged(object sender, EventArgs e)
        {
            lnkAddJRWA_Click(sender, e);
            pnlJRWA.Visible = false;
        }

        protected void lnkAddJRWA_Click(object sender, EventArgs e)
        {
            if (tvJRWA.SelectedNode != null)
            {
                JrwaDAO jd = new JrwaDAO();
                int jrwaId = int.Parse(tvJRWA.SelectedNode.Value);

                using (DbDataReader dr = (DbDataReader)jd.GetJRWA(jrwaId))
                {                    
                    if (dr.HasRows)
                    {
                        if (dr.Read())
                        {
                            hfJRWAId.Value = jrwaId.ToString(); //dr["symbol"].ToString();
                            txtJRWA.Text = dr["nazwa"].ToString();
                        }
                    }
                }
            }
            else
                WebMsgBox.Show(this, "Wybierz JRWA");
        }

        protected void lnkCloseJRWA_Click(object sender, EventArgs e)
        {
            pnlJRWA.Visible = false;
        }

        protected void lnkDodajXSLT_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (fuplXSLT.HasFile)
            {
                try
                {
                    filePath = Server.MapPath(ConfigurationManager.AppSettings["katalogRoboczy"].ToString()) + "\\" + fuplXSLT.FileName;
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    fuplXSLT.SaveAs(filePath);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XslCompiledTransform regXSLT = new XslCompiledTransform();
                    regXSLT.Load(XmlReader.Create(new StreamReader(filePath)));
                    Session["regXslt"] = doc.OuterXml;
                    lblCustomXSLT.Visible = true;
                    lnkRemoveXSLT.Visible = true;
                }
                catch
                {
                    WebMsgBox.Show(this, "Nie udało się wczytać pliku. Sprawdź, czy wskazany plik jest poprawnym dokumentem XSLT.");
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
            else
                WebMsgBox.Show(this, "Wybierz plik XSLT do wczytania");
        }

        protected void lnkRemoveXSLT_Click(object sender, EventArgs e)
        {
            if (Session["regXslt"] != null)
            {
                Session["regXslt"] = null;
                lblCustomXSLT.Visible = false;
                lnkRemoveXSLT.Visible = false;
            }
        }

        protected void cusWpisy_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = lstRodzajeWpisow.Items.Count > 0;
        }


        protected void lnkAddXslFo_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (uploadXslFo.HasFile)
            {
                try
                {
                    filePath = Server.MapPath(ConfigurationManager.AppSettings["katalogRoboczy"].ToString()) + "\\" + uploadXslFo.FileName;
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    uploadXslFo.SaveAs(filePath);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);
                    XslCompiledTransform regXSLT = new XslCompiledTransform();
                    regXSLT.Load(XmlReader.Create(new StreamReader(filePath)));
                    Session["regXslFo"] = doc.OuterXml;
                    lblCustomXslFo.Visible = true;
                    lnkRemoveXslFo.Visible = true;
                }
                catch (Exception ex)
                {
                    WebMsgBox.Show(this, String.Format("Nie udało się wczytać pliku. Sprawdź, czy wskazany plik jest poprawnym dokumentem XSLT. {0}", ex.Message));
                }
            }
            else
                WebMsgBox.Show(this, "Wybierz plik XSLT do wczytania");
        }

        protected void lnkRemoveXslFo_Click(object sender, EventArgs e)
        {
            if (Session["regXslFo"] != null)
            {
                Session["regXslFo"] = null;
                lblCustomXslFo.Visible = false;
                lnkRemoveXslFo.Visible = false;
            }
        }
    }
}
