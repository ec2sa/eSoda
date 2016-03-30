using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Pemi.Esoda.Web.UI
{
    public class XPathHelperClass
    {
        private XPathDocument _doc;
        private XPathNavigator _nav;

        /// <summary>
        /// Tworzy now� instancj� skojarzon� z dokumentem Xml
        /// </summary>
        /// <param name="plikXml">�cie�ka do pliku Xml</param>
        public XPathHelperClass(string plikXml)
        {
            _doc = new XPathDocument(plikXml);
            _nav = _doc.CreateNavigator();
        }

        public string PobierzWartosc(string xpath)
        {
            _nav.MoveToRoot();
            XPathNodeIterator it = _nav.Select(xpath);
            if (it.MoveNext())
                return it.Current.Value;
            return null;
        }
    }
}
