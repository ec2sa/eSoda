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
using Pemi.Esoda.Tools;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class NowyDokumentLogiczny : System.Web.UI.Page
    {
        private void setLinksVisibility()
        {
            bool visibility = fkategoria.SelectedValue != "0" && frodzajDokumentu.SelectedValue != "0";
            utworz.Visible = visibility;
        }
        private int caseId;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;

            int.TryParse(Page.Request["idSprawy"], out caseId);

            SetPageTitle();

            if (!IsPostBack)
            {
                RegistryDAO dao = new RegistryDAO();
                fkategoria.DataSource = dao.GetDocumentCategories();
                if ((fkategoria.DataSource as ICollection).Count > 0)
                {
                    fkategoria.DataTextField = "Description";
                    fkategoria.DataValueField = "ID";
                    fkategoria.DataBind();
                    fkategoria.Items.Insert(0, new ListItem("-- wybierz --", "0"));
                    frodzajDokumentu.Items.Clear();
                    frodzajDokumentu.Items.Add(new ListItem("-- wybierz --", "0"));
                }
                else
                {
                    BaseContentPage.SetError("Brak zdefiniowanych kategorii i rodzajów dokumentów", "~/OczekujaceZadania.aspx");
                }
                setLinksVisibility();

            }

        }

        protected void fkategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            frodzajDokumentu.Items.Clear();
            frodzajDokumentu.Items.Add(new ListItem("-- wybierz --", "0"));
            frodzajDokumentu.DataSource = new RegistryDAO().GetDocumentTypes(int.Parse(fkategoria.SelectedValue));
            if (fkategoria.Items[0].Value == "0")
                fkategoria.Items.RemoveAt(0);


            if ((frodzajDokumentu.DataSource as ICollection).Count > 0)
            {
                frodzajDokumentu.DataTextField = "Description";
                frodzajDokumentu.DataValueField = "ID";
                frodzajDokumentu.DataBind();
                frodzajDokumentu.Items.Insert(0, new ListItem("-- wybierz --", "0"));
                setLinksVisibility();
                MSOFormVisiblity(int.Parse(frodzajDokumentu.SelectedValue));
                MSOTemplateVisiblity();
            }
            else
            {
                setLinksVisibility();
                MSOFormVisiblity(int.Parse(frodzajDokumentu.SelectedValue));
                MSOTemplateVisiblity();
            }
            //else
            //{
            //       // BaseContentPage.SetError("Brak zdefiniowanych rodzajów dokumentów", "~/OczekujaceZadania.aspx");
            //    frodzajDokumentu.Items.Clear();
            //    frodzajDokumentu.Items.Add(new ListItem("-- wybierz --", "0"));
            //}

        }

        protected void utworz_Click(object sender, EventArgs e)
        {
            try
            {
                int docId = CreateDocument();
                Response.Redirect("~/Dokumenty/Dokument.aspx?id=" + docId.ToString(), false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void anuluj_Click(object sender, EventArgs e)
        {
            int caseId = CoreObject.GetId(Request);
            //if (Session["idSprawy"] != null && int.TryParse(Session["idSprawy"].ToString(), out caseId))
            if (caseId > 0)
            {
                Response.Redirect("~/Sprawy/Sprawa.aspx?id=" + caseId.ToString(), false);
            }
            else
                Response.Redirect("~/OczekujaceZadania.aspx", false);
        }

        protected void createDocAndOpenWordForm_Click(object sender, EventArgs e)
        {
            try
            {
                int docId = CreateDocument();

                Response.Redirect("~/Dokumenty/FormularzWidokGlowny.aspx?id=" + docId.ToString() + "&mode=c");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void createDocAndWordTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                int docId = CreateDocument();
                Response.Redirect("~/Dokumenty/Dokument.aspx?id=" + docId.ToString() + "&mode=c");
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void frodzajDokumentu_SelectedIndexChanged(object sender, EventArgs e)
        {
            setLinksVisibility();
            MSOFormVisiblity(int.Parse(frodzajDokumentu.SelectedValue));
            MSOTemplateVisiblity();

        }

        private void MSOFormVisiblity(int typeId)
        {
            if (fkategoria.SelectedValue == "0" || frodzajDokumentu.SelectedValue == "0")
            {
                createDocAndOpenWordForm.Visible = false;
                return;
            }
            try
            {
                createDocAndOpenWordForm.Visible = new DocumentDAO().CanCreateMSOForm(typeId);
            }
            catch (SqlException ex)
            {
                lblMessage.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void MSOTemplateVisiblity()
        {
            createDocAndWordTemplate.Visible = false;
            //if (fkategoria.SelectedValue == "0" || frodzajDokumentu.SelectedValue == "0")
            //{
            //    createDocAndWordTemplate.Visible = false;
            //    return;
            //}
            //try
            //{
            //    createDocAndWordTemplate.Visible = new DocumentDAO().CanCreateMSOTemplate();
            //}
            //catch (SqlException ex)
            //{
            //    lblMessage.Text = ex.Message;
            //}
            //catch (Exception ex)
            //{
            //    lblMessage.Text = ex.Message;
            //}
        }

        private int CreateDocument()
        {
            try
            {
                Metadata md = new Metadata();

                StringBuilder sb = new StringBuilder();
                XmlWriter xw = XmlWriter.Create(sb);

                xw.WriteStartDocument();
                xw.WriteStartElement("metadane");

                xw.WriteStartElement("dataWplywu"); xw.WriteEndElement();
                xw.WriteStartElement("dataPisma");
                xw.WriteString(txtDataPisma.Text);
                xw.WriteEndElement();
                xw.WriteStartElement("numerPisma"); xw.WriteEndElement();
                xw.WriteStartElement("nadawca");
                xw.WriteAttributeString("typ", "-1");
                xw.WriteAttributeString("kategoria", "-1");
                xw.WriteAttributeString("id", "-1");
                xw.WriteEndElement();
                xw.WriteStartElement("opis");
                xw.WriteString(txtOpis.Text);
                xw.WriteEndElement();
                xw.WriteStartElement("klasyfikacjaDokumentu");
                xw.WriteStartElement("kategoria");
                xw.WriteAttributeString("id", fkategoria.SelectedItem.Value);
                xw.WriteValue(fkategoria.SelectedItem.Text);
                xw.WriteEndElement(); // kategoria

                xw.WriteStartElement("rodzaj");
                xw.WriteAttributeString("id", frodzajDokumentu.SelectedItem.Value);
                xw.WriteValue(frodzajDokumentu.SelectedItem.Text);
                xw.WriteEndElement(); // rodzaj

                xw.WriteStartElement("wartosc");
                xw.WriteEndElement(); // wartosc

                xw.WriteEndElement();
                xw.WriteStartElement("typKorespondencji");
                xw.WriteAttributeString("id", "-1");
                xw.WriteAttributeString("nazwa", "");
                xw.WriteEndElement();
                xw.WriteStartElement("uwagi"); xw.WriteEndElement();
                xw.WriteStartElement("znakReferenta");
                xw.WriteStartElement("wydzial");
                xw.WriteAttributeString("id", "0");
                xw.WriteValue("- nieokreœlony -");
                xw.WriteEndElement(); // wydzial

                xw.WriteStartElement("pracownik");
                xw.WriteAttributeString("id", "0");
                xw.WriteValue("- nieokreœlony -");
                xw.WriteEndElement(); // pracownik
                xw.WriteEndElement();


                xw.WriteEndElement(); // metadane
                xw.WriteEndDocument();
                xw.Close();


                DocumentDAO dao = new DocumentDAO();
                //int docId = dao.AddNewDocument(new Guid(Membership.GetUser().ProviderUserKey.ToString()), md.GetXml());
                int docId = dao.AddNewDocument(new Guid(Membership.GetUser().ProviderUserKey.ToString()), sb.ToString());

                ActionLogger al = new ActionLogger(new ActionContext(new Guid("e2e8d217-1e83-4f5b-ba3a-31412d771bf1"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment, new List<string>()));
                al.AppliesToDocuments.Add(docId);
                al.Execute();

                Session.Remove("idSkojarzonejSprawy");


                if (caseId > 0)
                {
                    AssignDocumentToCase(docId, caseId);
                    Session["idSkojarzonejSprawy"] = caseId;
                }

                return docId;

            }
            catch (ArgumentException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AssignDocumentToCase(int docId, int caseId)
        {
            try
            {
                CaseDAO caseDao = new CaseDAO();
                caseDao.AssignDocumentToExistingCase(new Guid(Membership.GetUser().ProviderUserKey.ToString()), docId, caseId);
            }
            catch
            {
                throw new ArgumentException("Wyst¹pi³ problem z przypisaniem dokumentu do sprawy!");
            }

        }

        private void SetPageTitle()
        {
            try
            {
                if (caseId > 0)
                {
                    CaseDAO caseDao = new CaseDAO();
                    Page.Title = String.Format("Tworzenie nowego dokumentu dla sprawy {0}", caseDao.GetCaseSignature(caseId));
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

    }
}