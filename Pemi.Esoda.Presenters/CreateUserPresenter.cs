using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;

namespace Pemi.Esoda.Presenters
{
	public class CreateUserPresenter:BasePresenter
	{
		private ICreateUserView view;
		private ICreateUserTask service;

		public CreateUserPresenter(ICreateUserView view)
		{
			if (view == null) throw new Exception("Niew³aœciwy widok");
			this.view = view;
			this.service = new CreateUserTask();
			subscribeToEvents();
		}
		
		void view_UserCreated(object sender, EventArgs e)
		{
			service.CreateUser(view.LastName, view.FirstName.Trim().Length>0?view.FirstName:null,view.UserID,view.Position);
		}

		public override void Initialize()
		{
		}

		protected override void subscribeToEvents()
		{
			view.UserCreated += new EventHandler(view_UserCreated);
		}

		protected override void redirectToPreviousView()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}
