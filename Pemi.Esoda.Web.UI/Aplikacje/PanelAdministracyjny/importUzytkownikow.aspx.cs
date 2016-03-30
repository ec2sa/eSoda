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

using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Web.UI
{
	public partial class importUzytkownikow : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
           
		}

		protected void importuj(object sender, EventArgs e)
		{
            litResults.Text = "";
            if (fuplImport.HasFile)
            {
                bool fileValid = Path.GetExtension(fuplImport.FileName).Equals(".csv");
                if (!fileValid)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "wrongExt", "<script type=\"text/javascript\" language=\"javascript\">alert('Dozwolony jest import tylko z plików .csv !');</script>");
                    return;
                }
                else
                {
                    // pobranie danych z csv
                    try
                    {
                        string filePath = Server.MapPath(ConfigurationManager.AppSettings["katalogRoboczy"].ToString()) + "\\" + fuplImport.FileName;
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        fuplImport.SaveAs(filePath);

                        string strConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + Path.GetDirectoryName(filePath) + "; Extended Properties=\"Text;HDR=YES;FMT=Delimited(;)\"";
                        OleDbConnection conText = new OleDbConnection(strConnectionString);
                        OleDbDataAdapter odda = new OleDbDataAdapter("SELECT * FROM " + Path.GetFileName(filePath), conText);
                        DataSet dsImport = new DataSet();
                        conText.Open();
                        odda.Fill(dsImport);
                        conText.Close();

                        DataTable dtImport = dsImport.Tables[0];

                        foreach (DataRow row in dtImport.Rows)
                        {
                            UserDAO ud = new UserDAO();

                            string userLogin = Membership.GetUserNameByEmail(row["email"].ToString());
                            if (userLogin != null && userLogin.Equals(row["login"].ToString()))
                            {
                                litResults.Text += string.Format("<p>konto: {0} ju¿ istnieje</p>", row["login"].ToString());
                            }
                            else
                            {
                                MembershipUser usr = Membership.CreateUser(row["login"].ToString(), row["haslo"].ToString(), row["email"].ToString());

                                ud.CreateEmployee(row["nazwisko"].ToString(), row["imie"].ToString(), (Guid)usr.ProviderUserKey,"");
                                litResults.Text += string.Format("<p>konto: {0} utworzone</p>", usr.UserName);

                                ud.CreateOrganizationalUnit(row["wydzial"].ToString(), row["skrotwydzialu"].ToString(), true, 0);
                                litResults.Text += string.Format("<p>konto: {0} przypisane do: {1}</p>", usr.UserName, row["wydzial"].ToString());

                                Roles.AddUserToRole(usr.UserName, row["wydzial"].ToString());
                            }
                        }
          
                        File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
              
                        litResults.Text += string.Format("<p> B³¹d importu danych:<br/>{0}<br/>{1}</p>", ex.Message, ex.Source);
                    }
                }
            }			
		}
	}
}
