using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Xml.XPath;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace Pemi.Esoda.Web.UI
{
    public class RegistryHelper
    {
        public static Control CreateField(bool isHeader, string position, int level, string name, string label, string dataType, bool required,string defaultValue)
        {
            Panel div = new Panel();

            div.CssClass = string.Format("registryItemField registryItemLevel{0}", level);

            if (isHeader)
            {
                HtmlGenericControl h4 = new HtmlGenericControl("h4");
                h4.InnerText = label;
                div.Controls.Add(h4);

                return div;
            }
            TextBox tb = new TextBox();
            tb.ID = "p" + position;

            if (!string.IsNullOrEmpty(defaultValue))
                tb.Text = defaultValue;

            Label lb = new Label();
            HtmlGenericControl br = new HtmlGenericControl("br");

            div.Controls.Add(lb);
            div.Controls.Add(tb);

            lb.Text = label;
            lb.AssociatedControlID = tb.ClientID;

            if (required)
            {
                RequiredFieldValidator rfv = new RequiredFieldValidator();
                div.Controls.Add(rfv);
                rfv.ControlToValidate = tb.ClientID;
                rfv.ErrorMessage = "Pole jest wymagane";
                rfv.Display = ValidatorDisplay.Dynamic;
                lb.CssClass += " requiredField";
                rfv.ValidationGroup = "registryItem";
            }
            CompareValidator cv = new CompareValidator();
            cv.ValidationGroup = "registryItem";
            switch (dataType)
            {
                case "date":

                    cv.ErrorMessage = "Niepoprawny format daty";
                    cv.ControlToValidate = tb.ClientID;
                    cv.Type = ValidationDataType.Date;
                    cv.SetFocusOnError = true;
                    cv.Operator = ValidationCompareOperator.DataTypeCheck;
                    cv.Display = ValidatorDisplay.Dynamic;
                    div.Controls.Add(cv);
                    break;
                case "int":

                    cv.ErrorMessage = "Niepoprawny format liczby całkowitej";
                    cv.ControlToValidate = tb.ClientID;
                    cv.Type = ValidationDataType.Integer;
                    cv.SetFocusOnError = true;
                    cv.Operator = ValidationCompareOperator.DataTypeCheck;
                    cv.Display = ValidatorDisplay.Dynamic;
                    div.Controls.Add(cv);
                    break;
                case "decimal":

                    cv.ErrorMessage = "Niepoprawny format liczby rzeczywistej";
                    cv.ControlToValidate = tb.ClientID;
                    cv.Type = ValidationDataType.Double;
                    cv.SetFocusOnError = true;
                    cv.Operator = ValidationCompareOperator.DataTypeCheck;
                    cv.Display = ValidatorDisplay.Dynamic;
                    div.Controls.Add(cv);
                    break;
            }
            div.Controls.Add(br);
            return div;
        }

        /// <summary>
        /// Creates xml content of registry item
        /// </summary>
        /// <param name="registryDefinition">Content of registry definition on which item will be based</param>
        /// <param name="values">Collection of item's field values (in the same order as in definition)</param>
        /// <param name="objectID">ID of document or case </param>
        /// <returns>Complete registry item content </returns>
        public static string CreateRegistryItem(string registryDefinition, IList<string> values)
        {
            XPathDocument xpd = new XPathDocument(new StringReader(registryDefinition));
            XPathNavigator xpn = xpd.CreateNavigator();

            XPathNodeIterator xpni = xpn.Select("/definicjaRejestru/pola/pole/@nazwa");
            StringBuilder sb = new StringBuilder();
            sb.Append("<wpis>");
            while (xpni.MoveNext())
            {
                sb.Append(generateElement(xpn, xpni.Current.Value, values));
            }
            sb.Append("</wpis>");
            return sb.ToString();
        }

        private static string generateElement(XPathNavigator xpn, string nodeName, IList<string> values)
        {
            StringBuilder sb=new StringBuilder();
            XPathNodeIterator xpni=xpn.Select(string.Format("//pole[@nazwa='{0}']/pole/@nazwa",nodeName));
            if(xpni.Count>0)
            {
                sb.Append(string.Format("<{0}>",nodeName));
            while(xpni.MoveNext())
            {
                sb.Append(string.Format("{0}",generateElement(xpn,xpni.Current.Value,values)));
            }
            sb.Append(string.Format("</{0}>", nodeName));
            }
            else{
                sb.Append(string.Format("<{0}>{1}</{0}>",nodeName,values[0]));
                values.RemoveAt(0);
            }
            


   
            return sb.ToString();
        }
    }
}
