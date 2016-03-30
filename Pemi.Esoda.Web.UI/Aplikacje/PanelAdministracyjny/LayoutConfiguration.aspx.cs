using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pemi.Esoda.DataAccess;
using System.IO;
using Pemi.Esoda.Web.UI.Classes;

namespace Pemi.Esoda.Web.UI.Aplikacje.PanelAdministracyjny
{
    public partial class LayoutConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var dao = new EsodaConfigParametersDAO();
                Dictionary<string, string> configData = dao.GetConfig();

                if (!configData.ContainsKey("logoBG"))
                {
                    dao.SetConfigParam("logoBG", "#FFF4DE");
                    configData = dao.GetConfig();
                }

                bgColor.Text = configData["logoBG"].ToUpper();

            }
        }

        protected void setConfig(object sender, CommandEventArgs e)
        {
            var dao = new EsodaConfigParametersDAO();
            bool success = true;
            msg.Attributes["style"] = "color:green;";
            msg.Text = "";

            if (e.CommandName == "saveLogo1")
            {
                try
                {
                    if (uploadLogo1.PostedFile != null && uploadLogo1.PostedFile.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(uploadLogo1.PostedFile.FileName);
                        var name = "L" + Guid.NewGuid().ToString();
                        var targetFile = Server.MapPath("~/app_themes/standardlayout/logos/" + name + ext);

                        foreach (var file in Directory.GetFiles(Server.MapPath("~/app_themes/standardlayout/logos"), "L*.*"))
                            File.Delete(file);

                        File.WriteAllBytes(targetFile, uploadLogo1.FileBytes);
                        dao.SetConfigParam("logo1", name + ext);
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Błąd w trakcie zapisu pliku zawierającego logo";
                    success = false;
                }
                if (success)
                {
                    msg.Text = "Konfiguracja zapisana poprawnie";
                    LayoutConfig.Invalidate();
                    return;
                }
            }



            if (e.CommandName == "saveLogo2")
            {
                try
                {
                    if (uploadLogo2.PostedFile != null && uploadLogo2.PostedFile.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(uploadLogo2.PostedFile.FileName);
                        var name = "P" + Guid.NewGuid().ToString();
                        var targetFile = Server.MapPath("~/app_themes/standardlayout/logos/" + name + ext);

                        foreach (var file in Directory.GetFiles(Server.MapPath("~/app_themes/standardlayout/logos"), "P*.*"))
                            File.Delete(file);

                        File.WriteAllBytes(targetFile, uploadLogo2.FileBytes);
                        dao.SetConfigParam("logo2", name + ext);
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Błąd w trakcie zapisu pliku zawierającego logo";
                    success = false;
                }
                if (success)
                {
                    msg.Text = "Konfiguracja zapisana poprawnie";
                    LayoutConfig.Invalidate();
                }
                return;
            }


            if (e.CommandName == "saveBG")
            {

                var currentColor = bgColor.Text.Replace(";", "").Replace("#", "");
                int res;
                if ((currentColor.Length == 3 || currentColor.Length == 6) && int.TryParse(currentColor, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.NumberFormatInfo.InvariantInfo, out res) && bgColor.Text[0] == '#')
                {
                    dao.SetConfigParam("logoBG", bgColor.Text.Replace(";", ""));
                }
                else
                {
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Niepoprawna wartość koloru tła!";
                    success = false;
                }

                if (success)
                {
                    msg.Text = "Kolor tła zapisany poprawnie";
                    LayoutConfig.Invalidate();
                }
                return;
            }


            if (e.CommandName == "setDefault")
            {
                dao.SetConfigParam("logoBG", "#FFF4DE");
                dao.SetConfigParam("logo1", "Ld.png");
                dao.SetConfigParam("logo2", "Pd.png");

                bgColor.Text = "#FFF4DE";
                var sourceFile1 = Server.MapPath("~/app_themes/standardlayout/img/defaultlogo/logo1.png");
                var targetFile1 = Server.MapPath("~/app_themes/standardlayout/logos/Ld.png");
                var sourceFile2 = Server.MapPath("~/app_themes/standardlayout/img/defaultlogo/logo2.png");
                var targetFile2 = Server.MapPath("~/app_themes/standardlayout/logos/Pd.png");

                try
                {
                    byte[] content1 = File.ReadAllBytes(sourceFile1);
                    byte[] content2 = File.ReadAllBytes(sourceFile2);

                    foreach (var file in Directory.GetFiles(Server.MapPath("~/app_themes/standardlayout/logos"), "*.*"))
                        File.Delete(file);

                    File.WriteAllBytes(targetFile1, content1);
                    File.WriteAllBytes(targetFile2, content2);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Błąd w trakcie przywracania wartości domyślnych";
                    success = false;
                }

                if (success)
                {
                    msg.Text = "Konfiguracja przywrócona do wartości domyślnych";
                    LayoutConfig.Invalidate();
                }
                return;
            }

            if (e.CommandName == "deleteLogo1")
            {
                try
                {
                    foreach (var file in Directory.GetFiles(Server.MapPath("~/app_themes/standardlayout/logos"), "L*.*"))
                        File.Delete(file);
                    var sourceFile1 = Server.MapPath("~/app_themes/standardlayout/img/defaultlogo/empty.gif");
                    var targetFile1 = Server.MapPath("~/app_themes/standardlayout/logos/Ld.gif");
                    byte[] content1 = File.ReadAllBytes(sourceFile1);
                    File.WriteAllBytes(targetFile1, content1);
                    dao.SetConfigParam("logo1", "Ld.gif");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Błąd w trakcie usuwania logo";
                    success = false;
                }
                if (success)
                {
                    msg.Text = "Logo zostało usunięte";
                    LayoutConfig.Invalidate();
                }
                return;
            }

            if (e.CommandName == "deleteLogo2")
            {
                try
                {
                    foreach (var file in Directory.GetFiles(Server.MapPath("~/app_themes/standardlayout/logos"), "P*.*"))
                        File.Delete(file);
                    var sourceFile1 = Server.MapPath("~/app_themes/standardlayout/img/defaultlogo/empty.gif");
                    var targetFile1 = Server.MapPath("~/app_themes/standardlayout/logos/Pd.gif");
                    byte[] content1 = File.ReadAllBytes(sourceFile1);
                    File.WriteAllBytes(targetFile1, content1);
                    dao.SetConfigParam("logo2", "Pd.gif");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Błąd w trakcie usuwania logo";
                    success = false;
                }
                if (success)
                {
                    msg.Text = "Logo zostało usunięte";
                    LayoutConfig.Invalidate();
                }
                return;
            }

            if (e.CommandName == "deleteBG")
            {
                try
                {
                    dao.SetConfigParam("logoBG", "transparent");
                    bgColor.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    msg.Attributes["style"] = "color:red;";
                    msg.Text = "Błąd w trakcie usuwania koloru tła";
                    success = false;
                }
                if (success)
                {
                    msg.Text = "Kolor tła został usunięty";
                    LayoutConfig.Invalidate();
                }
                return;
            }

        }
    }
}