using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters.Interfaces
{
    public interface IAwaitingESPDocumentView
    {
        int ItemID { set; }
        int RegistryID { set; get; }
        event EventHandler AddESPDocument;
        event EventHandler<ExecutingCommandEventArgs> CommandExecuting;
    }
}
