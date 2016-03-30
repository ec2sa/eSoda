using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Pemi.Esoda.DataAccess;
using System.Xml;

namespace Pemi.Esoda.Web.UI.services
{
    /// <summary>
    /// Summary description for eSoda
    /// </summary>
    [WebService(Namespace = "http://esoda.pl/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class eSoda : System.Web.Services.WebService
    {

        [WebMethod]
        public XmlDocument StatusSprawy(int numerPozycjiDziennika,int year)
        {
            using (XmlReader xr = new CaseDAO().GetCaseOrDocumentStatus(numerPozycjiDziennika, year))
            {
                var doc = new XmlDocument();
                doc.Load(xr);
                return doc;
            }
        }
    }
}
