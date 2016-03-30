using System;
using System.Collections.Generic;
using System.Text;
using System.Web.SessionState;

namespace Pemi.Esoda.Presenters
{
	public interface IView
	{
		string ErrorMessage { set;}
		void RedirectToPreviousView();
		Guid UserID { get;}
		string ViewTitle { set;}
        HttpSessionState Session { get; }
	}
}
