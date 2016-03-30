using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
	public interface IAssignedItemsView
	{
		string Items { set;}
		Guid UserId { get;}
        int ObjectId { get; set; }
		event EventHandler<ExecutingCommandEventArgs> ExecutingCommand;
		string TargetView { set;}
	}
}
