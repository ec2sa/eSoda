using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface ICreateUserView
	{
		Guid UserID { get;}
		string LastName { get;}
		string FirstName { get;}
        string Position { get; }
		event EventHandler UserCreated;
	}
}
