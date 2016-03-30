using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tasks
{
	public class CreateUserTask:ICreateUserTask
	{
		private UserDAO dao = new UserDAO();

		#region ICreateUserTask Members

		void ICreateUserTask.CreateUser(string name, string firstName,Guid userID,string stanowisko)
		{
			dao.CreateEmployee(name, firstName, userID,stanowisko);
		}

		#endregion
	}
}
