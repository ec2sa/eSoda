using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using System.Web.Security;

namespace Pemi.Esoda.Web.UI.Akcje
{
    public partial class UsuniecieDokumentuZeSprawy : System.Web.UI.Page
    {
        protected int docID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { 
            if(!int.TryParse(Session["docToRemoveFromCase"]!=null?Session["docToRemoveFromCase"].ToString():string.Empty,out docID))
                Response.Redirect("~/Akta/AktaSpraw.aspx");
            ViewState["docID"] = docID;
            ViewState["caseID"] =Session["caseToRemoveFrom"];
            Session.Remove("docToRemoveFromCase");
            Session.Remove("caseToRemoveFrom");
            btnCancel.Attributes["onclick"] = "history.back(1);return false;";
            done.Visible = false;
            todo.Visible = true;
            }   
        }

        protected void executeAction(object sender, EventArgs e)
        {
            try
            {
                string d = ViewState["docID"].ToString();
                removeDocumentFromCase(int.Parse(d));
                successMsg.Text = "Dokument został usunięty poprawnie.";
                successMsg.CssClass = "successMsg";
            }
            catch
            {
                successMsg.Text = "Nie udało się usunąć dokumentu";
                successMsg.CssClass = "errorMsg";
            }
            todo.Visible = false;
            done.Visible = true;
        }

        protected void returnFromAction(object sender, EventArgs e)
        {
            int caseID = (int)Session["caseToRemoveFrom"];
            if(caseID>0)
            Response.Redirect("~/Sprawy/DokumentySprawy.aspx?id=" + caseID.ToString(),false);
            else
                Response.Redirect("~/Akta/AktaSpraw.aspx");
            
        }


        protected void cancelAction(object sender, EventArgs e)
        {
          //  Response.Redirect("~/Akta/AktaSpraw.aspx");
        }

        protected void removeDocumentFromCase(int docID)
        {
            //usuniecie dokumentu ze sprawy. zwrotnie idsprawy lub null gdy zostala usunieta
            int caseID;
          
            CaseDAO dao=new CaseDAO();
            caseID = dao.RemoveDocumentFromCase(docID, (Guid)Membership.GetUser().ProviderUserKey);

            string caseNumber;
            if (caseID > 0)
                caseNumber = dao.GetCaseSignature(caseID);
            else
                caseNumber = "(wraz ze sprawą)";
            //odnotowanie wykonania akcji
            string user=Membership.GetUser().Comment+" ("+Membership.GetUser().UserName+")";
            List<string> parameters=new List<string>();
            parameters.Add(docID.ToString());
            parameters.Add(caseNumber);
            parameters.Add(user);

            ActionLogger al = new ActionLogger(new ActionContext(new Guid("FA7D4334-CF40-4086-877B-25E8D0C48CCA"), new Guid(Membership.GetUser().ProviderUserKey.ToString()), Membership.GetUser().UserName, Membership.GetUser().Comment,parameters ));
            al.AppliesToDocuments.Add(docID);
            if(caseID!=0)
                al.AppliesToCases.Add(caseID);

            al.ActionData.Add("NumerDokumentu", docID.ToString());
            al.ActionData.Add("uzytkownik", user);


            al.ActionData.Add("NumerSprawy", caseID>0?caseNumber:"(sprawa usunięta)");
            if(caseID>0)
            al.ActionData.Add("IDSprawy", caseID.ToString());
            al.Execute();
            Session["caseToRemoveFrom"] = caseID;
        }
    }
}
