using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;
using System.IO;
using System.Xml;

namespace Pemi.Esoda.Tools
{
    public class XPathHelper
    {
        private XPathDocument _xpathDocument;

        private XPathNavigator _navigator;

        /// <summary>
        /// XML document to work with
        /// </summary>
        public XPathDocument Document
        {
            get
            {
                return _xpathDocument;
            }
            set
            {
                _xpathDocument = value;
                _navigator = _xpathDocument.CreateNavigator();
            }

        }

        /// <summary>
        /// Creates instance of XPathHelper and assigns it to XML document
        /// </summary>
        /// <param name="reader">XmlReader containing XML document</param>
        public XPathHelper(XmlReader reader)
        {
            _xpathDocument = new XPathDocument(reader);
            _navigator = _xpathDocument.CreateNavigator();
        }

        /// <summary>
        /// Creates instance of XPathHelper and assigns it to XML document
        /// </summary>
        /// <param name="reader">String containing XML document content</param>
        public XPathHelper(string xmlContent)
        {
            _xpathDocument = new XPathDocument(new StringReader(xmlContent));
            _navigator = _xpathDocument.CreateNavigator();
        }

        /// <summary>
        /// Gets node value from XML document with no defined namespace
        /// </summary>
        /// <param name="xpath">XPath expression</param>
        /// <returns>Selected node value</returns>
        public string GetNodeValue(string xpath)
        {
            return GetNodeValue(xpath, null, null);
        }

        /// <summary>
        /// Gets node value from XML document with defined namespace and prefix
        /// </summary>
        /// <param name="xpath">XPath expression</param>
        /// <returns>Selected node value</returns>
        public string GetNodeValue(string xpath, string prefix,string namespaceUri)
        {
            XmlNamespaceManager mgr=null;

            if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(namespaceUri))
            {
                mgr = new XmlNamespaceManager(_navigator.NameTable);
                mgr.AddNamespace(prefix, namespaceUri);
                return _navigator.SelectSingleNode(xpath, mgr).Value;
            }

            return _navigator.SelectSingleNode(xpath).Value;
            
            
        }
    }
}
