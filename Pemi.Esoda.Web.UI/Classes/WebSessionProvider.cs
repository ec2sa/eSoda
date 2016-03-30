using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Pemi.Esoda.Presenters;
using System.Web.SessionState;

namespace Pemi.Esoda.Web.UI
{
	public class WebSessionProvider:ISessionProvider
	{
		private HttpSessionState session
		{
			get { return HttpContext.Current.Session; }
		}

	
		#region ISessionProvider Members
		
		object ISessionProvider.this[string name]
		{
			get
			{
				return session[name];
			}
			set
			{
				session[name] = value;
			}
		}

		object ISessionProvider.this[int index]
		{
			get
			{
				return session[index];
			}
			set
			{
				session[index] = value;
			}
		}


		bool ISessionProvider.Contains(string name)
		{
			return session[name] != null;
		}

		bool ISessionProvider.Contains(int index)
		{
			return session[index] != null;
		}

		void ISessionProvider.Remove(string name)
		{
			session.Remove(name);
		}

		
		void ISessionProvider.Clear()
		{
			session.Clear();
		}

		#endregion

		#region ISessionProvider Members


		void ISessionProvider.Abandon()
		{
			session.Abandon();
		}

		#endregion
	}
}
