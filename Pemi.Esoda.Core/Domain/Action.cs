using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;
using System.Xml;
using System.Xml.XPath;

namespace Pemi.Esoda.Core.Domain
{
    public class Action
    {
        private Guid definitionId;

        public Guid DefinitionId
        {
            get { return definitionId; }
        //    set { definitionId = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
        //    set { name = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
        //    set { description = value; }
        }

        private string logEntryTemplate;

        public string LogEntryTemplate
        {
            get { return logEntryTemplate; }
         //   set { logEntryTemplate = value; }
        }

        private int parameterCount;

        public int ParameterCount
        {
            get { return parameterCount; }
           // set { parameterCount = value; }
        }

        public Action(Guid definitionId)
        {
            this.definitionId = definitionId;
            using (XmlReader xr = new ActionDAO().GetActionDefinition(definitionId))
            {
                if (!xr.Read()) throw new ArgumentException("Nie ma takiej definicji akcji");
                XPathDocument xpd = new XPathDocument(xr);
                XPathNavigator xpn = xpd.CreateNavigator();
                this.name = xpn.SelectSingleNode("/akcja/nazwa").Value;
                this.description = xpn.SelectSingleNode("/akcja/opis").Value;
                this.logEntryTemplate = xpn.SelectSingleNode("/akcja/szablon").Value;
                this.parameterCount = xpn.SelectSingleNode("/akcja/definicja/liczbaParametrow").ValueAsInt;
            }
        }

        
    }
}
