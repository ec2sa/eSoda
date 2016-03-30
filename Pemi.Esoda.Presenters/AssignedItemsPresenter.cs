using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Xml.XPath;
using System.IO;

namespace Pemi.Esoda.Presenters
{
	public class AssignedItemsPresenter:BasePresenter
	{
		private IAssignedItemsView view;
		private IAssignedItemsTask service;
		private ISessionProvider session;

		private string assignedItems
		{
			get
			{
				if (session["{3BA3EA44-0A5E-4b27-94A7-0C6AD10889EC}"] != null)
					return session["{3BA3EA44-0A5E-4b27-94A7-0C6AD10889EC}"].ToString();
				return string.Empty;
			}
			set
			{
				if (value == null) 
					session.Remove("{3BA3EA44-0A5E-4b27-94A7-0C6AD10889EC}");
				else
					session["{3BA3EA44-0A5E-4b27-94A7-0C6AD10889EC}"] = value;
			}
		}

		public AssignedItemsPresenter(IAssignedItemsView view, ISessionProvider session)
		{
			this.view = view;
			this.service = new AssignedItemsTask();
			this.session = session;
			subscribeToEvents();
		}

		public override void Initialize()
		{
			assignedItems = service.GetAssignedItems(view.UserId);
            view.Items = assignedItems;
			((IView)view).ViewTitle = "Oczekuj¹ce zadania";						
		}

		protected override void subscribeToEvents()
		{
			view.ExecutingCommand += new EventHandler<ExecutingCommandEventArgs>(view_ExecutingCommand);
		}

        void view_ExecutingCommand(object sender, ExecutingCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "dokument" || e.CommandName.ToLower() == "sprawa" || e.CommandName.ToLower() == "faktura")
            {
                /*XPathDocument xpd = new XPathDocument(new StringReader(assignedItems));
                XPathNavigator xpn = xpd.CreateNavigator();
                XPathNodeIterator xpni = xpn.Select(string.Format("/zadania/zadanie[id='{0}']", e.CommandArgument));
                if (xpni.MoveNext())
                    session["context"] = xpni.Current.OuterXml;*/
                if (e.CommandName.ToLower() == "dokument" || e.CommandName.ToLower() == "faktura")
                  {
                    session.Remove("idSprawy");
                    //session["idDokumentu"] = e.CommandArgument.ToString();
                  }
                  if (e.CommandName.ToLower() == "sprawa")
                  {
                    session.Remove("idDokumentu");
                    //session["idSprawy"] = e.CommandArgument.ToString();
                  }
                  string cmdName = e.CommandName;
                  if (e.CommandName.ToLower() == "faktura")
                      cmdName = "Dokument";
                
                view.TargetView = string.Format("{1}/{0}.aspx?id={2}", cmdName, cmdName == "Sprawa" ? "Sprawy" : "Dokumenty",e.CommandArgument.ToString());
            }
            if (e.CommandName.ToLower() == "dokument_s")
            {
                view.TargetView = string.Format("{1}/{0}.aspx?id={2}", "SkladnikiDokumentu","Dokumenty", e.CommandArgument.ToString());
            }
        }

		protected override void redirectToPreviousView()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
