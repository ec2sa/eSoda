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
using System.Xml;
using System.IO;
using System.Xml.XPath;
using System.Collections.Generic;

namespace Pemi.Esoda.Web.UI.Controls
{
  public partial class KryteriaWyszukiwaniaWRejestrze : System.Web.UI.UserControl
  {
    private int idRejestru;
    public event EventHandler<SearchEventArgs> Search;

    public int IdRejestru
    {
      get
      {
        return idRejestru;
      }
      set
      {
        idRejestru = value;
        loadSearchCriteriaRegistryDefinition();
      }
    }

    public string[] SearchCriteria
    {
      get
      {
        List<string> sc = new List<string>();
        foreach (Control c in kryteria.Controls)
        {
          if(c is TextBox){
            TextBox tb=c as TextBox;
            sc.Add(tb.Text);
          }
        }
        return sc.ToArray();
      }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      if(!IsPostBack)
        loadSearchCriteriaRegistryDefinition();
    }

    private void loadSearchCriteriaRegistryDefinition()
    {
      Rejestry.RejestryDSTableAdapters.DaneRejestruTableAdapter ta = new Rejestry.RejestryDSTableAdapters.DaneRejestruTableAdapter();
      //XmlReader xr=XmlReader.Create(new StringReader(ta.GetData(IdRejestru)[0].definicja));
      Rejestry.RejestryDS.DaneRejestruDataTable dt = ta.GetData(IdRejestru);
      if (dt.Rows.Count == 0) return;
      XPathDocument xpd = new XPathDocument(new StringReader(dt[0].definicja));
      XPathNavigator xpn = xpd.CreateNavigator();
      XPathNodeIterator xpni = xpn.Select("/definicjaRejestru/wyszukiwanie/kryterium");
      if (xpni.Count == 0)
      {
        kryteria.Controls.Add(new LiteralControl("Brak zdefiniowanych kryteriów wyszukiwania"));
        return;
      }
 
      while (xpni.MoveNext())
      {
        Label etykieta = new Label();
        TextBox pole = new TextBox();
        HtmlGenericControl br = new HtmlGenericControl("br");
        etykieta.ID = "e" + xpni.CurrentPosition.ToString();
        etykieta.AssociatedControlID = "p" + xpni.CurrentPosition.ToString();
        etykieta.Text = xpni.Current.SelectSingleNode("@etykieta").Value;
        
        pole.ID = etykieta.AssociatedControlID;
        kryteria.Controls.Add(etykieta);
        kryteria.Controls.Add(pole);
        kryteria.Controls.Add(br);
        
      }
    }

    protected void wyszukaj(object sender, EventArgs e)
    {
      if (Search != null)
        Search(this, new SearchEventArgs(SearchCriteria));
    }
  }

  public class SearchEventArgs : EventArgs
  {
    private string[] searchCriteria;

    public string[] SearchCriteria
    {
      get { return searchCriteria; }
    }

    public SearchEventArgs(string[] searchCriteria)
    {
      this.searchCriteria = searchCriteria;
    }
  }
}