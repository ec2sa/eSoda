using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Pemi.Esoda.Presenters
{
	public class ExecutingCommandEventArgs : EventArgs
	{
		public readonly string CommandName;

		public readonly object CommandArgument;

        public readonly object CommandSource;

		public ExecutingCommandEventArgs(string commandName, object commandArgument)
		{
			this.CommandName = commandName;
			this.CommandArgument = commandArgument;
		}

        public ExecutingCommandEventArgs(string commandName, object commandArgument, object commandSource)
        {
            this.CommandName = commandName;
            this.CommandArgument = commandArgument;
            this.CommandSource = commandSource;
        }
	}
}
