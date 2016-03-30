//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using Pemi.Esoda.DataAccess;

//namespace Pemi.Esoda.Web.UI.Controls
//{
//  public partial class CaseNav : System.Web.UI.UserControl
//  {
//    protected void Page_Load(object sender, EventArgs e)
//    {
//      if (Request.RawUrl.ToLower().Contains("oczekujacezadania")
//          || Request.RawUrl.ToLower().Contains("/dokumenty/")
//          || Request.RawUrl.ToLower().Contains("/sprawy/")
//          || Request.RawUrl.ToLower().Contains("/akta/")
//      )
//        caseNavigator.Visible = true;

//      if (Session["entryPoint"] != null)
//      {
//        oczekujace.Visible = Session["entryPoint"].ToString() == "OZ";
//        akta.Visible = Session["entryPoint"].ToString() == "AS";

//      }

//      if (Session["idDokumentu"] != null && Request.RawUrl.ToLower().Contains("/dokumenty/"))
//      {
//        int documentId = int.Parse(Session["idDokumentu"].ToString());

//        dokument.Text = string.Format("Dokument({0})", documentId);
//        dokument.Visible = true;

//        string[] items = getItemsDCB(documentId);

//        sprawa.Visible = separatorSprawy.Visible = (items[1] != null);
//        teczka.Visible = separatorTeczki.Visible = (items[2] != null);
//        if (sprawa.Visible)
//          sprawa.Text = string.Format("Sprawa ({0})", items[1]);
//        if (teczka.Visible)
//        {
//          teczka.Text = string.Format("Teczka ({0})", items[2]);
//          teczka.CommandArgument = items[3];
//        }
//      }
//      else
//        if (Session["idSprawy"] != null && (Request.RawUrl.ToLower().Contains("/sprawy/") || Request.RawUrl.ToLower().Contains("/dokumenty/")))
//        {
//          int caseId = int.Parse(Session["idSprawy"].ToString());
//          string[] items = getItemsCB(caseId);
//          sprawa.Visible = separatorTeczki.Visible = teczka.Visible = true;
//          separatorSprawy.Visible = false;
//          sprawa.Text = string.Format("Sprawa ({0})", items[0]);
//          teczka.Text = string.Format("Teczka ({0})", items[2]);
//          teczka.CommandArgument = items[1];
//        }
//        else
//          if (Session["idTeczki"] != null && (Request.RawUrl.ToLower().Contains("/akta/")))
//          {
//            teczka.Visible = true;
//            separatorTeczki.Visible=false;
//            teczka.CommandArgument = Session["idTeczki"].ToString();
//           // teczka.Text = string.Format("Teczka ({0})", Session["nazwaTeczki"]);
//          }
//    }

//    private string[] getItemsDCB(int documentId)
//    {
//      DocumentDAO dao = new DocumentDAO();
//      string[] it = new string[] { documentId.ToString(), null, null,null };
//      using (IDataReader dr = dao.GetCaseAndBriefcase(documentId))
//      {
//        if (dr.Read())
//        {
//          it[1] = dr.GetInt32(1).ToString();
//          it[2] = dr.GetString(2);
//          it[3] = dr.GetInt32(3).ToString();
//        }
//      }
//      return it;
//    }

//    private string[] getItemsCB(int caseId)
//    {
//      CaseDAO dao = new CaseDAO();
//      string[] it = new string[] { caseId.ToString(), null, null };
//      using (IDataReader dr = dao.GetBriefcaseForCase(caseId))
//      {
//        if (dr.Read())
//        {
//          it[1] = dr.GetInt32(0).ToString();
//          it[2] = dr.GetString(1);
//        }
//      }
//      return it;
//    }

//    protected void executeCommand(object sender, CommandEventArgs e)
//    {
//    Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"]="LinkButton1";


//    Response.Redirect(e.CommandName.ToString()+"?idTeczki="+e.CommandArgument.ToString());
//    }
//  }
//}

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

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class CaseNav : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.RawUrl.ToLower().Contains("oczekujacezadania")
                || Request.RawUrl.ToLower().Contains("/dokumenty/")
                || Request.RawUrl.ToLower().Contains("/sprawy/")
                || Request.RawUrl.ToLower().Contains("/akta/")
            )
                caseNavigator.Visible = true;

            if (Session["entryPoint"] != null)
            {
                oczekujace.Visible = Session["entryPoint"].ToString() == "OZ";
                akta.Visible = Session["entryPoint"].ToString() == "AS";

            }

            //if (Session["idDokumentu"] != null && Request.RawUrl.ToLower().Contains("/dokumenty/"))
            if (CoreObject.GetId(Request)>0 && Request.RawUrl.ToLower().Contains("/dokumenty/"))
            {
                int documentId = CoreObject.GetId(Request);  //int.Parse(Session["idDokumentu"].ToString()); // TODO

                dokument.Text = string.Format("Dokument({0})", documentId);
                dokument.Visible = true;
                dokument.CommandArgument = documentId.ToString();

                string[] items = getItemsDCB(documentId);

                sprawa.Visible = separatorSprawy.Visible = (items[1] != null);
                teczka.Visible = separatorTeczki.Visible = (items[2] != null);
                if (sprawa.Visible)
                {
                    sprawa.Text = string.Format("Sprawa ({0})", items[1]);
                    sprawa.CommandArgument = items[1];
                }
                if (teczka.Visible)
                {
                    teczka.Text = string.Format("Teczka ({0})", items[2]);
                    teczka.CommandArgument = items[3];
                }
            }
            else
                //if (Session["idSprawy"] != null && (Request.RawUrl.ToLower().Contains("/sprawy/") || Request.RawUrl.ToLower().Contains("/dokumenty/")))
                if (CoreObject.GetId(Request)>0 && (Request.RawUrl.ToLower().Contains("/sprawy/") || Request.RawUrl.ToLower().Contains("/dokumenty/")))
                {
                    int caseId = CoreObject.GetId(Request);  // int.Parse(Session["idSprawy"].ToString()); // TODO
                    string[] items = getItemsCB(caseId);
                    sprawa.Visible = separatorTeczki.Visible = teczka.Visible = true;
                    separatorSprawy.Visible = false;
                    sprawa.Text = string.Format("Sprawa ({0})", items[0]);
                    sprawa.CommandArgument = caseId.ToString();
                    teczka.Text = string.Format("Teczka ({0})", items[2]);
                    teczka.CommandArgument = items[1];
                }
                else
                    if (Session["idTeczki"] != null && (Request.RawUrl.ToLower().Contains("/akta/")))
                    {
                        teczka.Visible = true;
                        separatorTeczki.Visible = false;
                        teczka.CommandArgument = Session["idTeczki"].ToString(); // TODO
                        // teczka.Text = string.Format("Teczka ({0})", Session["nazwaTeczki"]);
                    }
        }

        private string[] getItemsDCB(int documentId)
        {
            DocumentDAO dao = new DocumentDAO();
            string[] it = new string[] { documentId.ToString(), null, null, null };
            using (IDataReader dr = dao.GetCaseAndBriefcase(documentId))
            {
                if (dr.Read())
                {
                    it[1] = dr.GetInt32(1).ToString();
                    it[2] = dr.GetString(2);
                    it[3] = dr.GetInt32(3).ToString();
                }
            }
            return it;
        }

        private string[] getItemsCB(int caseId)
        {
            CaseDAO dao = new CaseDAO();
            string[] it = new string[] { caseId.ToString(), null, null };
            using (IDataReader dr = dao.GetBriefcaseForCase(caseId))
            {
                if (dr.Read())
                {
                    it[1] = dr.GetInt32(0).ToString();
                    it[2] = dr.GetString(1);
                }
            }
            return it;
        }

        protected void executeCommand(object sender, CommandEventArgs e)
        {
            Session["{B0BC3548-7973-4881-BBDD-7470E743C0DC}"] = "LinkButton1";
            Response.Redirect(e.CommandName.ToString() + "?idTeczki=" + e.CommandArgument.ToString());
        }

        protected void redirectToCase(object sender, CommandEventArgs e)
        {
            Response.Redirect(string.Format("~/Sprawy/Sprawa.aspx?id={0}", e.CommandArgument));
        }

        protected void redirectToDocument(object sender, CommandEventArgs e)
        {
            Response.Redirect(string.Format("~/Dokumenty/Dokument.aspx?id={0}", e.CommandArgument));
        }
    }
}