using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Data.Common;
using Pemi.Esoda.DataAccess;
using System.Text;

namespace Pemi.Esoda.Web.UI.Controls
{
    public partial class ESPDocumentPreview : System.Web.UI.UserControl
    {
        private string xml, xsl;
        private Guid ?docGuid;
        private int ?docId;
        private int ?itemId;

        public string XmlData { set { xml = value; } }
        public string XslData { set { xsl = value; } }

        public Guid DocGuid { set { docGuid = value; docId = null; itemId = null; LoadDocData(); } }
        public int DocId { set { docId = value; docGuid = null; itemId = null; LoadDocData(); } }
        public int ItemId { set { itemId = value; docGuid = null; docId = null; LoadDocData(); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshDocument();
            }
        }

        void LoadDocData()
        {
            if (docId != null)
            {
                using (DbDataReader dr = (DbDataReader)(new DocumentDAO()).GetDocumentXMLXSL(docId))
                {
                    if (dr.Read())
                    {
                        xml = dr["espXML"].ToString();
                        xsl = dr["espXSL"].ToString();
                        RefreshDocument();
                    }
                }
            }
            if (docGuid != null)
            {
                using (DbDataReader dr = (DbDataReader)(new DocumentDAO()).GetESPDocumentData(docGuid))
                {
                    if (dr.Read())
                    {
                        xml = dr["dane"].ToString();
                        xsl = dr["styl"].ToString();
                        RefreshDocument();
                    }
                }
            }
            if (itemId != null)
            {
                using (DbDataReader dr = (DbDataReader)(new DocumentDAO()).GetDocumentXMLXSLFromReg(itemId))
                {
                    if (dr.Read())
                    {
                        xml = dr["espXML"].ToString();
                        xsl = dr["espXSL"].ToString();
                        RefreshDocument();
                    }
                }
            }
        }

        void RefreshDocument()
        {
            if (xml != null && xsl != null && xml.Length > 0 && xsl.Length > 0)
            {
                xmlDocumentData.DocumentContent = xml;
                XslTransform xslTransform = new XslTransform();
                xslTransform.Load(XmlReader.Create(new StringReader(xsl)));
                xmlDocumentData.Transform = xslTransform;

                // compiled xslt transform
                //XslCompiledTransform docXsltc = new XslCompiledTransform();
                //docXsltc.Load(XmlReader.Create(new StringReader(xsl)));
                //XmlReader xr = XmlReader.Create(new StringReader(xml));
                //StringBuilder sb = new StringBuilder();
                //XmlWriter xw = XmlWriter.Create(sb);

                //docXsltc.Transform(xr, xw);
                //xr.Close();
                //xw.Close();
                //litDocumentData.Text = sb.ToString().Substring(sb.ToString().LastIndexOf("<html"));
                
                this.Visible = true;
            }
            else
            {
                this.Visible = false;
                xmlDocumentData.DocumentContent = string.Empty;
            }
        }
    }
}