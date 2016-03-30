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

namespace Pemi.Esoda.Web.UI.Controls
{
  public partial class NumeracjaSpraw : System.Web.UI.UserControl
  {

    public string Prefix
    {
      get { return prefix.Text; }
      set { prefix.Text = value; }
    }

    public string Jrwa
    {
      get { return jrwa.Text; }
      set { jrwa.Text = value; }
    }

    public string Suffix
    {
      get { return jrwa.Text + suffix.Text; }
      set { suffix.Text = value; }
    }
 
    public int FirstNumber
    {
      get {
        int nr;
        if (!int.TryParse(firstNumber.Text, out nr)) return 1;
        return nr;
      }
      set { firstNumber.Text = value.ToString(); }
    }

   

    public int Year
    {
      get { if (Session["{C6853BB7-691A-437c-BDBE-E2A7D89E934D}"]==null)
          Session["{C6853BB7-691A-437c-BDBE-E2A7D89E934D}"]=DateTime.Now.Year;
      return (int)Session["{C6853BB7-691A-437c-BDBE-E2A7D89E934D}"];}
      set { Session["{C6853BB7-691A-437c-BDBE-E2A7D89E934D}"] = value;
      numberAndYear.Text = string.Format("{0} / {1}", FirstNumber, value);
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if(!IsPostBack)
      generateNumbers(5);
    }

    protected void disableJRWA_CheckedChanged(object sender, EventArgs e)
    {
      jrwa.Enabled = !disableJRWA.Checked;
      generateNumbers(5);
    }

    protected void generateNumbers(int count)
    {
      generatedNumbers.InnerHtml = string.Empty;
      string format=null;
      if(disableJRWA.Checked)
         format = "{0}.{2}.{3}.{4}<br/>";
      else
        format="{0}.{1}{2}.{3}.{4}<br/>";
    
       

      for (int i = FirstNumber; i < FirstNumber + count; i++)
      {
        generatedNumbers.InnerHtml += string.Format(format, Prefix, Jrwa, suffix.Text, i, Year);
      }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
      generateNumbers(5);
    }
  }
}