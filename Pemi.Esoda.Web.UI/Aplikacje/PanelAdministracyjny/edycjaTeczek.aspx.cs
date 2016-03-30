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
using System.Data.Common;
using Pemi.Esoda.Tools;
using Pemi.Esoda.Web.UI.Controls;
using System.Text;
using System.Xml;
using System.Data.SqlClient;
using System.IO;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class edycjaTeczek : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
                bindFilterToFields(null);
        }

        private void UstawAdresatow(string adresat)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(adresat);

            ListBox lstGroups = ((ListBox)frmTeczka.FindControl("lstGroups"));
            ListBox lstWorkers = ((ListBox)frmTeczka.FindControl("lstWorkers"));
            DropDownList ddlPracownik = ((DropDownList)frmTeczka.FindControl("ddlOdpowiedzialny"));

            lstGroups.Items.Clear();
            // wydzia³y
            foreach (XmlElement wydzial in doc.SelectNodes("//wydzial"))
            {
                ListItem item = new ListItem(wydzial.InnerText, wydzial.Attributes["id"].Value);
                lstGroups.Items.Add(item);
            }

            LoadGroupWorkers();

            //pracownicy
            foreach (XmlElement pracownik in doc.SelectNodes("//pracownik"))
            {
                ListItem item = new ListItem(pracownik.InnerText, pracownik.Attributes["id"].Value);
                if (!lstWorkers.Items.Contains(item))
                {
                    lstWorkers.Items.Add(item);
                    ddlPracownik.Items.Add(item);
                }
                if (pracownik.Attributes["glowny"] != null && pracownik.Attributes["glowny"].Value.Equals("tak"))
                {
                    ddlPracownik.SelectedValue = pracownik.Attributes["id"].Value;
                }
            }            
            
        }

        private string PobierzAdresatow()
        {
            string glownyId = "";
            StringBuilder sb = new StringBuilder();            

            ListBox lstGroups = ((ListBox)frmTeczka.FindControl("lstGroups"));
            ListBox lstWorkers = ((ListBox)frmTeczka.FindControl("lstWorkers"));
            DropDownList ddlPracownik = ((DropDownList)frmTeczka.FindControl("ddlOdpowiedzialny"));
            glownyId = ddlPracownik.SelectedValue;

            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("adresat");

            if (lstGroups.Items.Count > 0)
            {
                // wydzia³y
                foreach (ListItem group in lstGroups.Items)
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
                xw.WriteValue("nieokreœlony");
                xw.WriteEndElement(); //wydzial
            }

            if (lstWorkers.Items.Count > 0)
            {
                foreach (ListItem worker in lstWorkers.Items)
                {
                    xw.WriteStartElement("pracownik");
                    xw.WriteAttributeString("id", worker.Value);
                    if (worker.Value.Equals(glownyId))
                        xw.WriteAttributeString("glowny", "tak");
                    xw.WriteValue(worker.Text);
                    xw.WriteEndElement();
                }
            }
            else
            {
                xw.WriteStartElement("pracownik");
                xw.WriteAttributeString("id", "0");
                xw.WriteValue("nieokreœlony");
                xw.WriteEndElement();
            }

            xw.WriteEndElement(); //adresat
            xw.WriteEndDocument();
            xw.Close();

            return sb.ToString();
        }

        private void UstawRodzajeSpraw(string caseKinds)
        {
            if (caseKinds.Length == 0)
                return;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(caseKinds);

            ListBox lstCaseKinds = ((ListBox)frmTeczka.FindControl("lstCaseKinds"));
            lstCaseKinds.Items.Clear();
            // wydzia³y
            foreach (XmlElement caseKind in doc.SelectNodes("//rodzajSprawy"))
            {
                ListItem item = new ListItem(caseKind.InnerText, caseKind.Attributes["id"].Value);
                if(!lstCaseKinds.Items.Contains(item))
                    lstCaseKinds.Items.Add(item);
            }
        }

        private string PobierzRodzajeSpraw()
        {
            StringBuilder sb = new StringBuilder();            

            ListBox lstCaseKinds = ((ListBox)frmTeczka.FindControl("lstCaseKinds"));
            
            XmlWriter xw = XmlWriter.Create(sb);

            xw.WriteStartDocument();
            xw.WriteStartElement("rodzajeSpraw");

            if (lstCaseKinds.Items.Count > 0)
            {
                foreach (ListItem caseKind in lstCaseKinds.Items)
                {
                    xw.WriteStartElement("rodzajSprawy");
                    xw.WriteAttributeString("id", caseKind.Value);
                    xw.WriteValue(caseKind.Text);
                    xw.WriteEndElement();
                }
            }
            else
            {
                xw.WriteStartElement("rodzajSprawy");
                xw.WriteAttributeString("id", "0");
                xw.WriteValue("nieokreœlony");
                xw.WriteEndElement();
            }

            xw.WriteEndElement(); //adresat
            xw.WriteEndDocument();
            xw.Close();

            return sb.ToString();
        }

        private void LoadGroupWorkers()
        {
            UserDAO ud = new UserDAO();
            ListBox lstGroups = ((ListBox)frmTeczka.FindControl("lstGroups"));
            DropDownList ddlWorkers = ((DropDownList)frmTeczka.FindControl("ddlPracownicy"));
            DropDownList ddlOdpowiedzialny = ((DropDownList)frmTeczka.FindControl("ddlOdpowiedzialny"));
            ddlWorkers.Items.Clear();
            ddlWorkers.Items.Insert(0, new ListItem("- wybierz -","-1"));
            ddlOdpowiedzialny.Items.Clear();
            ddlOdpowiedzialny.Items.Insert(0, new ListItem("- wybierz -","-1"));

            foreach (ListItem item in lstGroups.Items)
            {
                SqlDataReader dr = (SqlDataReader)ud.GetWorkersListByGroup(int.Parse(item.Value));
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ListItem wrkItem = new ListItem(dr["pelnaNazwa"].ToString(), dr["id"].ToString());
                        if (!ddlWorkers.Items.Contains(wrkItem))
                            ddlWorkers.Items.Add(wrkItem);
                        //if (!ddlOdpowiedzialny.Items.Contains(wrkItem))
                        //    ddlOdpowiedzialny.Items.Add(wrkItem);
                    }
                }
            }
        }

        protected void frmTeczka_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                BriefcaseDAO bd = new BriefcaseDAO();
                int outid;
                int? idJRWA = null;
                int ?idRodzajuSprawy = null;
                if(int.TryParse( ((HiddenField)frmTeczka.FindControl("hfIdJRWA")).Value , out outid))
                    idJRWA=outid;
                if (int.TryParse(((DropDownList)frmTeczka.FindControl("ddlRodzajSprawy")).SelectedItem.Value, out outid))
                    idRodzajuSprawy = outid;
                               
                NumeracjaSpraw numeracja = ((NumeracjaSpraw)frmTeczka.FindControl("NumeracjaSpraw1"));
                string prefix = numeracja.Prefix;
                string suffix = numeracja.Suffix;

                int rok = int.Parse(((TextBox)frmTeczka.FindControl("txtRok")).Text);
                string tytul = ((TextBox)frmTeczka.FindControl("txtTytul")).Text;
                int nextNum = numeracja.FirstNumber;

                string adresat = PobierzAdresatow();
                string caseKinds = PobierzRodzajeSpraw();

                bool aktywna = ((CheckBox)frmTeczka.FindControl("cbAktywna")).Checked;
                bool archiwalna = ((CheckBox)frmTeczka.FindControl("cbArchiwalna")).Checked;

                bd.InsertBriefcase(idJRWA, idRodzajuSprawy, caseKinds, prefix, suffix, rok, tytul, nextNum, adresat, aktywna, archiwalna);
            
                frmTeczka.Visible = false;
                pnlGroups.Visible = false;
                pnlJRWA.Visible = false;
                lnkCreateBriefcaseGroup.Visible = false;
            }
        }

        protected void frmTeczka_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {               
                BriefcaseDAO bd = new BriefcaseDAO();

                int id = int.Parse(frmTeczka.DataKey["id"].ToString());
              
                int outid;
                int ?idJRWA = null;
                int ?idRodzajuSprawy = null;
                if (int.TryParse(((HiddenField)frmTeczka.FindControl("hfIdJRWA")).Value, out outid))
                    idJRWA = outid;
                if (int.TryParse(((DropDownList)frmTeczka.FindControl("ddlRodzajSprawy")).SelectedItem.Value, out outid))
                    idRodzajuSprawy = outid;

                if (outid == -1)
                {
                    if (int.TryParse(((HiddenField)frmTeczka.FindControl("hfRodzajSprawy")).Value, out outid))
                        idRodzajuSprawy = outid;
                }
                
                NumeracjaSpraw numeracja = ((NumeracjaSpraw)frmTeczka.FindControl("NumeracjaSpraw1"));
                string prefix = numeracja.Prefix;
                string suffix = numeracja.Suffix;

                int rok = int.Parse(((TextBox)frmTeczka.FindControl("txtRok")).Text);
                string tytul = ((TextBox)frmTeczka.FindControl("txtTytul")).Text;
                int nextNum = numeracja.FirstNumber;

                string adresat = PobierzAdresatow();
                string caseKinds = PobierzRodzajeSpraw();

                bool aktywna = ((CheckBox)frmTeczka.FindControl("cbAktywna")).Checked;
                bool archiwalna = ((CheckBox)frmTeczka.FindControl("cbArchiwalna")).Checked;

                bd.UpdateBriefcase(id, idJRWA, idRodzajuSprawy, caseKinds, prefix, suffix, rok, tytul, nextNum, adresat, aktywna, archiwalna);
            
                frmTeczka.Visible = false;
                lnkCreateBriefcaseGroup.Visible = false;
            }
        }

        protected void ObjectDataSource1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
        }

        protected void lnkBriefcaseAdd_Click(object sender, EventArgs e)
        {
            frmTeczka.ChangeMode(FormViewMode.Insert);
            pnlJRWA.Visible = false;
            pnlGroups.Visible = false;
            frmTeczka.Visible = true;
            lnkCreateBriefcaseGroup.Visible = false;
        }

        protected void gvBriefcaseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "select":
                    odsBriefcase.SelectParameters.Clear();
                    //odsBriefcase.SelectParameters.Add("id", gvBriefcaseList.DataKeys[int.Parse(e.CommandArgument.ToString())].Value.ToString());
                    odsBriefcase.SelectParameters.Add("id", e.CommandArgument.ToString());
                    frmTeczka.Visible = true;
                    frmTeczka.ChangeMode(FormViewMode.Edit);
                    frmTeczka.DataBind();
                    if (new BriefcaseDAO().IsBriefcaseGroupPossibility(Convert.ToInt32(e.CommandArgument)))
                    {
                        lnkCreateBriefcaseGroup.Attributes.Add("href", String.Format("GrupaTeczki.aspx?Id={0}", e.CommandArgument.ToString()));
                        lnkCreateBriefcaseGroup.Visible = true;
                    }
                    else
                    {
                        lnkCreateBriefcaseGroup.Visible = false;
                    }
                    break;

                case "preview":
                    int briefcaseId = int.Parse(e.CommandArgument.ToString()); //gvBriefcaseList.DataKeys[int.Parse(e.CommandArgument.ToString())].Value;
                    Response.Redirect(string.Format("~/Akta/AktaSpraw.aspx?source=ADMIN&id={0}",briefcaseId));
                    break;

                default:
                    break;
            }
        }

        protected void lnkJRWATree_Click(object sender, EventArgs e)
        {
            pnlJRWA.Visible = true;
            JrwaDAO jd = new JrwaDAO();
            dsxmlJRWA.Data = jd.GetActiveJRWAListXml();
            tvJRWA.DataBind();
            tvJRWA.CollapseAll();
            pnlGroups.Visible = false;
        }

        protected void lnkAddGroupToBriefcase_Click(object sender, EventArgs e)
        {
            if (tvGroups.SelectedNode != null)
            {
                string nazwa = tvGroups.SelectedNode.Text;
                string id = tvGroups.SelectedNode.Value;

                ListItem item = new ListItem(nazwa, id);

                ((ListBox)frmTeczka.FindControl("lstGroups")).Items.Add(item);

                // za³adowanie pracowników z danego wydzia³u
                LoadGroupWorkers();
            }
            else
                WebMsgBox.Show(this, "Wybierz grupê");
        }

        protected void tvJRWA_SelectedNodeChanged(object sender, EventArgs e)
        {
            lnkAddJRWA_Click(sender, e);
            pnlJRWA.Visible = false;
        }

        protected void lnkCloseGroups_Click(object sender, EventArgs e)
        {
            pnlGroups.Visible = false;
        }

        protected void lnkCloseJRWA_Click(object sender, EventArgs e)
        {
            pnlJRWA.Visible = false;
        }

        protected void lnkAddGroup_Click(object sender, EventArgs e)
        {
            GroupDAO gd = new GroupDAO();
            DataSet dsGroups = gd.GetGroupsListDataSet();

            dsGroups.DataSetName = "Groups";
            dsGroups.Tables[0].TableName = "Group";

            DataRelation dRel = new DataRelation("parentGroups", dsGroups.Tables["Group"].Columns["id"], dsGroups.Tables["Group"].Columns["idRodzica"], true);
            dRel.Nested = true;
            dsGroups.Relations.Add(dRel);

            dsxmlGroups.Data = dsGroups.GetXml();
            tvGroups.DataBind();
            pnlGroups.Visible = true;
            pnlJRWA.Visible = false;
        }

        protected void lnkAddJRWA_Click(object sender, EventArgs e)
        {
            if (tvJRWA.SelectedNode != null)
            {
                JrwaDAO jd = new JrwaDAO();
                int jrwaId = int.Parse(tvJRWA.SelectedNode.Value);
                DbDataReader dr = (DbDataReader)jd.GetJRWA(jrwaId);

                string symbol = "(brak)";
                string nazwa = "(brak)";
                if (dr.HasRows)
                {
                    if (dr.Read())
                    {
                        symbol = dr["symbol"].ToString();
                        nazwa = dr["nazwa"].ToString();
                    }
                }

                ((TextBox)frmTeczka.FindControl("txtJRWA")).Text = symbol;
                ((Label)frmTeczka.FindControl("lblOpisJRWA")).Text = nazwa;
                ((HiddenField)frmTeczka.FindControl("hfIdJRWA")).Value = jrwaId.ToString();
                ((NumeracjaSpraw)frmTeczka.FindControl("NumeracjaSpraw1")).Jrwa = symbol;
            }
            else
                WebMsgBox.Show(this, "Wybierz JRWA");
        }

        protected void frmTeczka_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            pnlGroups.Visible = false;
            pnlJRWA.Visible = false;
        }

        protected void frmTeczka_DataBound(object sender, EventArgs e)
        {
            if (frmTeczka.DataItem != null && frmTeczka.CurrentMode != FormViewMode.ReadOnly)
            {
                DropDownList ddlRodzajSprawy = ((DropDownList)frmTeczka.FindControl("ddlRodzajSprawy"));
                HiddenField hfRodzajSprawy = ((HiddenField)frmTeczka.FindControl("hfRodzajSprawy"));

                if (ddlRodzajSprawy.Items.FindByValue(hfRodzajSprawy.Value) == null)
                {
                    ddlRodzajSprawy.SelectedValue = "";
                }
                else
                {
                    ddlRodzajSprawy.SelectedValue = hfRodzajSprawy.Value;
                }

                NumeracjaSpraw numeracja = ((NumeracjaSpraw)frmTeczka.FindControl("NumeracjaSpraw1"));

                string sjrwa = DataBinder.Eval(frmTeczka.DataItem, "symbolJRWA").ToString();

                string sprzyrostek = DataBinder.Eval(frmTeczka.DataItem, "przyrostek").ToString();
                if(sjrwa.Length>0)
                    sprzyrostek=sprzyrostek.Replace(sjrwa, "");
                numeracja.Prefix = DataBinder.Eval(frmTeczka.DataItem, "przedrostek").ToString();
                numeracja.Suffix = sprzyrostek;
                numeracja.Jrwa = sjrwa;
                numeracja.Year = int.Parse(DataBinder.Eval(frmTeczka.DataItem, "rok").ToString());
                numeracja.FirstNumber = int.Parse(DataBinder.Eval(frmTeczka.DataItem, "nastepnyNumer").ToString());

                UstawAdresatow(DataBinder.Eval(frmTeczka.DataItem,"adresat").ToString());
                UstawRodzajeSpraw(DataBinder.Eval(frmTeczka.DataItem,"rodzajeSpraw").ToString());


            }
        }

        protected void odsBriefcase_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.Cancel = true;
        }

        protected void odsBriefcase_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            e.Cancel = true;
        }

        protected void lnkRemoveGroup_Click(object sender, EventArgs e)
        {
            ListBox lstGroups = ((ListBox)frmTeczka.FindControl("lstGroups"));
            if (lstGroups.SelectedItem != null)
            {
                lstGroups.Items.Remove(lstGroups.SelectedItem);
                //za³adowanie pracowników z wydzia³ów, które pozosta³y na liœcie
                LoadGroupWorkers();
            }
            else
                WebMsgBox.Show(this, "Wybierz wydzia³ do usuniêcia");
        }

        protected void lnkRemoveWorker_Click(object sender, EventArgs e)
        {
            ListBox lstWorkers = ((ListBox)frmTeczka.FindControl("lstWorkers"));
            DropDownList ddlOdpowiedzialny = ((DropDownList)frmTeczka.FindControl("ddlOdpowiedzialny"));

            if (lstWorkers.SelectedItem != null)
            {
                ListItem item = lstWorkers.SelectedItem;
                lstWorkers.Items.Remove(item);
                ddlOdpowiedzialny.Items.Remove(item);
            }
            else
                WebMsgBox.Show(this, "Wybierz pracownika do usuniêcia");
        }

        protected void lnkAddWorker_Click(object sender, EventArgs e)
        {
            DropDownList ddlWorkers = ((DropDownList)frmTeczka.FindControl("ddlpracownicy"));
            DropDownList ddlOdpowiedzialny = ((DropDownList)frmTeczka.FindControl("ddlOdpowiedzialny"));
            ListBox lstWorkers = ((ListBox)frmTeczka.FindControl("lstWorkers"));
            if (ddlWorkers.SelectedItem != null && ddlWorkers.SelectedValue != "-1")
            {
                ListItem worker = new ListItem(ddlWorkers.SelectedItem.Text, ddlWorkers.SelectedItem.Value);
                if (!lstWorkers.Items.Contains(worker))
                {
                    lstWorkers.Items.Add(worker);
                    ddlOdpowiedzialny.Items.Add(worker);
                }
            }
        }

        protected void ddlOdpowiedzialny_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)sender).Items.Insert(0, new ListItem());
        }

        protected void ddlRodzajSprawy_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddlRodzajeSpraw = ((DropDownList)sender);
            ddlRodzajeSpraw.Items.Clear();
            ddlRodzajeSpraw.Items.Insert(0, new ListItem("-- wybierz --","-1"));
        }

        protected void lnkAddCaseKind_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                DropDownList ddlRodzajSprawy = ((DropDownList)frmTeczka.FindControl("ddlRodzajSprawy"));
                ListBox lstCaseKinds = ((ListBox)frmTeczka.FindControl("lstCaseKinds"));
                if (ddlRodzajSprawy.SelectedItem != null && ddlRodzajSprawy.SelectedValue != "-1")
                {
                    ListItem caseKind = new ListItem(ddlRodzajSprawy.SelectedItem.Text, ddlRodzajSprawy.SelectedItem.Value);
                    if (!lstCaseKinds.Items.Contains(caseKind))
                        lstCaseKinds.Items.Add(caseKind);
                    else
                        WebMsgBox.Show(this, "Ten rodzaj sprawy ju¿ zosta³ dodany.");
                }
            }
        }

        protected void lnkRemoveCaseKind_Click(object sender, EventArgs e)
        {
            ListBox lstCaseKinds = ((ListBox)frmTeczka.FindControl("lstCaseKinds"));
            if (lstCaseKinds.SelectedItem != null)
            {
                lstCaseKinds.Items.Remove(lstCaseKinds.SelectedItem);
            }
            else
                WebMsgBox.Show(this, "Wybierz rodzaj sprawy do usuniêcia.");
        }

        protected void gvBriefcaseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal list = (Literal)e.Row.FindControl("litCaseKindsList");
                if (list != null)
                {
                    string sDoc = DataBinder.Eval(e.Row.DataItem, "CaseTypes").ToString();
                    XmlDocument doc = new XmlDocument();
                    try
                    {
                        doc.LoadXml(sDoc);

                        foreach (XmlElement elem in doc.SelectNodes("/rodzajeSpraw/rodzajSprawy"))
                        {
                            if (list.Text.Length > 0)
                                list.Text += "<br/>";
                            list.Text += elem.InnerText;
                        }
                    }
                    catch
                    { }
                }
            }
        }

        protected void tvGroups_SelectedNodeChanged(object sender, EventArgs e)
        {
            lnkAddGroupToBriefcase_Click(sender, e);
        }


        protected void cvRodzajeSpraw_ServerValidate(object source, ServerValidateEventArgs args)
        {
            ListBox lstCaseKinds = ((ListBox)frmTeczka.FindControl("lstCaseKinds"));
            DropDownList ddlRodzajeSpraw = ((DropDownList)frmTeczka.FindControl("ddlRodzajSprawy"));
            if(lstCaseKinds != null && ddlRodzajeSpraw != null)
            {
                args.IsValid = lstCaseKinds.Items.Count > 0;
            }
        }

        protected void cbArchiwalna_CheckedChanged(object sender, EventArgs e)
        {
            Label info = ((Label)frmTeczka.FindControl("lblArchiwalnaInfo"));
            if (((CheckBox)sender).Checked)
            {
                info.Visible = true;
            }
            else
            {
                info.Visible = false;
            }
        }

        protected void odsCasesList_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            CaseListFilter filter = bindFieldsToFilter();

            bindFilterToFields(filter);

            e.InputParameters[0] = filter;

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gvNewBriefcaseList.DataBind();
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            bindFilterToFields(null);
            gvNewBriefcaseList.DataBind();
       
        }

        protected void bindFilterToFields(CaseListFilter filter)
        {
            if (filter == null)
                filter = new CaseListFilter();

            fPrefix.Text = filter.Prefix;
            fSuffix.Text = filter.Suffix;
            fYear.Text = filter.Year.ToString();
            fIsActive.Checked = filter.IsActive.Value;
            fIsArchive.Checked = filter.IsArchive.Value;
        }

        protected CaseListFilter bindFieldsToFilter()
        {
            CaseListFilter filter = new CaseListFilter();

            int year;
            if (int.TryParse(fYear.Text, out year))
                filter.Year = year;

            filter.Prefix = fPrefix.Text.Trim();
            filter.Suffix = fSuffix.Text.Trim();
            filter.IsArchive = fIsArchive.Checked;
            filter.IsActive = fIsActive.Checked;

            return filter;
        }
    }
}