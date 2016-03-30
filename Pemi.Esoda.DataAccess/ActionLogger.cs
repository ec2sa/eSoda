using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml.XPath;

namespace Pemi.Esoda.DataAccess
{
    /// <summary>
    /// Performs action execution logging in database. Uses transaction to ensure correct logging
    /// </summary>
    public class ActionLogger
    {
        /// <summary>
        /// internal object stage:
        /// 0 - just created
        /// 1 - context passed
        /// 2 - action data and targets passed (docs/cases)
        /// 3 - data validated
        /// </summary>
        private byte stage = 0;

        private ActionContext context;

        private IDictionary<string, string> actionData;

        public IDictionary<string, string> ActionData
        {
            get { return actionData; }
        }

        private ICollection<int> appliesToDocuments;

        public ICollection<int> AppliesToDocuments
        {
            get { return appliesToDocuments; }
        }

        private ICollection<int> appliesToCases;

        public ICollection<int> AppliesToCases
        {
            get { return appliesToCases; }
        }

        public ActionLogger(ActionContext context)
        {
            this.context = context;
            if (this.context != null) stage = 1;
            this.actionData = new Dictionary<string, string>();
            this.appliesToCases = new List<int>();
            this.appliesToDocuments = new List<int>();
        }

        public int Execute()
        {
            ActionDAO dao = new ActionDAO();
            using (XmlReader xr = dao.GetActionDefinition(context.DefinitionID))
            {
                //if (!xr.Read()) throw new ArgumentException("Nie ma takiej akcji");
                XPathDocument xpd = new XPathDocument(xr);
                XPathNavigator xpn = xpd.CreateNavigator();
                string template = xpn.SelectSingleNode("/akcja/szablon").Value;
                int pCount;
                if (!int.TryParse(xpn.SelectSingleNode("/akcja/definicja/liczbaParametrow").Value, out pCount))
                    throw new ArgumentException("Niew³aœciwa liczba parametrów wywo³ania akcji");
                string description = null;
                string[] pTab = new string[context.Parameters.Count];
                context.Parameters.CopyTo(pTab, 0);
                if (pCount == 0)
                    description = template;
                if (pCount == 1)
                    description = string.Format(template, pTab[0]);
                if (pCount == 2)
                    description = string.Format(template, pTab[0], pTab[1]);
                if (pCount == 3)
                    description = string.Format(template, pTab[0], pTab[1], pTab[2]);

                return dao.WriteAction(context.DefinitionID, context.UserID, generateContent(), description);
            }

        }

        //public bool Validate()
        //{
        //  //if (allOK) stage = 3;

        //  return stage == 3;
        //}

        private string generateContent()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            xw.WriteStartDocument();
            xw.WriteStartElement("wykonanieAkcji");
            xw.WriteStartElement("dotyczy");
            xw.WriteRaw(generateDocsAndCasesReferences());
            xw.WriteEndElement();
            xw.WriteStartElement("parametry");
            xw.WriteRaw(generateParameters());
            xw.WriteEndElement();
            xw.WriteStartElement("dane");
            xw.WriteRaw(generateActionData());
            xw.WriteEndElement();
            xw.WriteEndElement();
            xw.WriteEndDocument();
            xw.Close();
            return sb.ToString();
        }

        private string generateActionData()
        {


            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<uzytkownik login=\"{0}\">{1}</uzytkownik>", context.UserName, context.FullName);
            foreach (string key in ActionData.Keys)
                sb.AppendFormat("<{0}>{1}</{0}>", key, ActionData[key]);
            return sb.ToString();
        }

        private string generateParameters()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string parameter in context.Parameters)
                sb.AppendFormat("<parametr>{0}</parametr>", parameter);
            return sb.ToString();
        }

        private string generateDocsAndCasesReferences()
        {
            StringBuilder sb = new StringBuilder();
            foreach (int did in AppliesToDocuments)
                sb.AppendFormat("<dokument id=\"{0}\" />", did);
            foreach (int cid in AppliesToCases)
                sb.AppendFormat("<sprawa id=\"{0}\" />", cid);
            return sb.ToString();
        }
    }

    public class ActionContext
    {
        private Guid definitionID;

        public Guid DefinitionID
        {
            get { return definitionID; }

        }

        private Guid userID;

        public Guid UserID
        {
            get { return userID; }

        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string fullName;

        public string FullName
        {
            get { return fullName; }

        }

        private ICollection<string> parameters;

        public ICollection<string> Parameters
        {
            get { return parameters; }

        }

        public ActionContext(Guid definitionID, Guid userID, string userName, string fullName, ICollection<string> parameters)
        {
            this.definitionID = definitionID;
            this.userID = userID;
            this.userName = userName;
            this.fullName = fullName;
            this.parameters = parameters;
        }
    }
}