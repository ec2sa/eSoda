using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public abstract class BasePresenter
	{
		public abstract void Initialize();
		protected abstract void subscribeToEvents();
		protected abstract void redirectToPreviousView();
	}
}
