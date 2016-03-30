using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Presenters.Interfaces;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Presenters
{
    public class AwaitingESPDocumentsPresenter : BasePresenter, IAwaitingESPDocumentView
    {
        private ISessionProvider session = null;
        private IAwaitingESPDocumentView view = null;
        private int registryId;

        public event EventHandler<ExecutingCommandEventArgs> ExecuteCommand;

        public AwaitingESPDocumentsPresenter(IAwaitingESPDocumentView view, ISessionProvider sessionProvider)
        {
            this.session = sessionProvider;
            this.view = view;
            this.subscribeToEvents();
            if (view.RegistryID == 0)
                registryId = (new RegistryDAO()).GetCurrentRegistryId(1, DateTime.Today.Year);
            else
                registryId = view.RegistryID;
            session["registryId"] = registryId;
        }

        public override void Initialize()
        {
            subscribeToEvents();
        }

        void view_AddESPDocument(object sender, EventArgs e)
        {
            session["itemIdRequest"] = registryId;
            OnExecuteCommand(new ExecutingCommandEventArgs("newItem", null));
        }

        protected void OnExecuteCommand(ExecutingCommandEventArgs e)
        {
            if (ExecuteCommand != null)
            {
               ExecuteCommand(this, e);
            }
        }

        protected override void subscribeToEvents()
        {
            view.AddESPDocument += new EventHandler(view_AddESPDocument);
            view.CommandExecuting += new EventHandler<ExecutingCommandEventArgs>(view_CommandExecuting);
        }

        void view_CommandExecuting(object sender, ExecutingCommandEventArgs e)
        {
            OnExecuteCommand(e);
        }

        protected override void redirectToPreviousView()
        {
            throw new NotImplementedException();
        }

        #region IAwaitingESPDocumentView Members

        public int ItemID
        {
            set { throw new NotImplementedException(); }
        }

        public event EventHandler AddESPDocument;

        #endregion

        #region IAwaitingESPDocumentView Members


        public int RegistryID
        {
            get
            {
                return 1;
            }
            set
            {                
            }
        }

        #endregion

        #region IAwaitingESPDocumentView Members


        public event EventHandler<ExecutingCommandEventArgs> CommandExecuting;

        #endregion
    }
}
