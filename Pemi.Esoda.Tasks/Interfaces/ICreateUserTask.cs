using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Tasks
{
	public interface ICreateUserTask
	{
		void CreateUser(string name, string firstName,Guid userID,string stanowisko);
	}
}
