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
using System.Collections.Generic;

namespace Pemi.Esoda.Web.UI
{
    public partial class OdbiorDokumentow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDocsForDept();
                LoadDocsForWorker();
            }
            lblError.Text = "";
            lblError.Visible = false;
            if (scan.PageWasChanged == true)
                scanPreviewModal.Show();
        }

        private void LoadDocsForDept()
        {
            gvDokumentyWydzialu.DataSource = (new ActionDAO()).GetDocToReceive(new Guid(Membership.GetUser().ProviderUserKey.ToString()), false);
            gvDokumentyWydzialu.DataBind();
        }

        private void LoadDocsForWorker()
        {
            gvDokumentyPracownika.DataSource = (new ActionDAO()).GetDocToReceive(new Guid(Membership.GetUser().ProviderUserKey.ToString()), true);
            gvDokumentyPracownika.DataBind();
        }

        protected void lnkConfirmReceive_Click(object sender, EventArgs e)
        {
            ArrayList dokumenty = new ArrayList();
            if ((new UserDAO()).PINIsValid(new Guid(Membership.GetUser().ProviderUserKey.ToString()), txtPIN.Text))
            {
                foreach (GridViewRow row in gvDokumentyWydzialu.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox toReceive = (CheckBox)row.FindControl("ckbRecvDoc");
                        if(toReceive != null)
                        {
                            if (toReceive.Checked)
                                dokumenty.Add(gvDokumentyWydzialu.DataKeys[row.RowIndex].Value);
                        }
                    }
                }

                foreach (GridViewRow row in gvDokumentyPracownika.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox toReceive = (CheckBox)row.FindControl("ckbRecvDoc");
                        if (toReceive != null)
                        {
                            if (toReceive.Checked)
                                dokumenty.Add(gvDokumentyPracownika.DataKeys[row.RowIndex].Value);
                        }
                    }
                }

                DocumentDAO dd = new DocumentDAO();

                foreach (int docId in dokumenty)
                {
                    Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
                    dd.UpdateDocRecvStatus(userId , docId, 1); // odebrany
                    int nrDoc = (new DocumentDAO()).GetRegistryPositionForDocument(docId);
                    List<string> parameters = new List<string>();
                    parameters.Add(Membership.GetUser().Comment);
                    ActionLogger al = new ActionLogger(new ActionContext(new Guid("76e1e680-ad90-4439-abd7-240d306777b6"), userId, Membership.GetUser().UserName, Membership.GetUser().Comment, parameters));
                    al.AppliesToDocuments.Add(docId);
                    al.ActionData.Add("WykonanaAkcja", "odebranie dokumentu z kancelarii");
                    al.ActionData.Add("Data", DateTime.Now.ToString());
                    al.ActionData.Add("NrDziennikaKancelaryjnego", (nrDoc != -1) ? nrDoc.ToString() : "(brak wpisu)" );
                    al.ActionData.Add("Odebra³", Membership.GetUser().Comment);
                    al.Execute();                    
                }
                LoadDocsForDept();
                LoadDocsForWorker();
            }
            else
            {
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Text = "Nieprawid³owy PIN";
            }
        }

        protected void gvDokumentyWydzialu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int status = int.Parse(DataBinder.Eval(e.Row.DataItem,"statusOdbioru").ToString());
                ((Label)e.Row.FindControl("lblDocReceived")).Visible = (status == 1);
                ((CheckBox)e.Row.FindControl("ckbRecvDoc")).Visible = (status == 0);
            }
        }

        protected void gvDokumentyPracownika_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int status = int.Parse(DataBinder.Eval(e.Row.DataItem, "statusOdbioru").ToString());
                ((Label)e.Row.FindControl("lblDocReceived")).Visible = (status == 1);
                ((CheckBox)e.Row.FindControl("ckbRecvDoc")).Visible = (status == 0);
            }
        }

        protected void ckbAllWydzial_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox zaznaczony = null;
            foreach (GridViewRow row in gvDokumentyWydzialu.Rows)
            {
                zaznaczony = (CheckBox)row.FindControl("ckbRecvDoc");
                if (zaznaczony != null)
                {
                    zaznaczony.Checked = ckbAllWydzial.Checked;
                }
            }
        }

        protected void ckbAllPracownik_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox zaznaczony = null;
            foreach (GridViewRow row in gvDokumentyPracownika.Rows)
            {
                zaznaczony = (CheckBox)row.FindControl("ckbRecvDoc");
                if (zaznaczony != null)
                {
                    zaznaczony.Checked = ckbAllPracownik.Checked;
                }
            }
        }

        protected void previewScanCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "preview")
            {
                int docid = 0;
                if (!int.TryParse(e.CommandArgument.ToString(), out docid))
                    return;
                scan.PageNumber = 1;
                scan.DocumentID = docid;
                scanPreviewModal.Show();
            }
        }
    }
}
