using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Tasks;
using System.Collections;

namespace Pemi.Esoda.Presenters
{
    public class ViewRegistryPresenter : BasePresenter
    {
        private int registryId;

        private IViewRegistryView view = null;

        private IViewRegistryTask service = null;

        private ISessionProvider session = null;

        public ViewRegistryPresenter(IViewRegistryView view, ISessionProvider sessionProvider)
        {
            if (view == null) throw new ArgumentNullException("Widok nie mo¿e mieæ wartoœci null");
            this.view = view;
            this.service = new ViewRegistryTask();
            this.session = sessionProvider;
            subscribeToEvents();
            view.IsCurrentDailyLogDef = true;

            if (view.RegistryID == 0)
            {
                registryId = service.GetCurrentRegistryId(1, DateTime.Today.Year);
                if (registryId == 0)
                    view.IsCurrentDailyLogDef = false;                
                view.RegistryID = registryId;
            }
            else
                registryId = view.RegistryID;
            session["registryId"] = registryId;
        }

        public event EventHandler<ExecutingCommandEventArgs> ExecuteCommand;

        private void setInitialValues()
        {
            ViewRegistryPresenterState state = session["{F48C43C9-6723-4a30-B114-A40295BDE72A}"] as ViewRegistryPresenterState;
            view.IsInDateChoosingState = false;

            if (state == null)
            {
                view.CurrentPage = 1;
                view.PageSize = 20;
                view.CurrentDateRange = "tydzien";
                dateRangeChanging(null, null);
                return;
            }
            view.CurrentDateRange = state.currentDateRange;
            view.StartDate = state.startDate;
            view.EndDate = state.endDate;
            dateRangeChanging(null, null);
            if (view.CurrentDateRange == "zakres")
                dateRangeChanged(null, null);
            view.PageSize = state.pageSize;
            view.CurrentPage = state.currentPage;
            view.ShowInvoices = state.browsingRF;
        }

        private void getData()
        {
            int currentPage = -1;
            ViewRegistryPresenterState state = session["{F48C43C9-6723-4a30-B114-A40295BDE72A}"] as ViewRegistryPresenterState;
            if (state != null && view.PageSize != state.pageSize && view.CurrentPage==view.PageCount)
            {
                currentPage = (int) (view.CurrentPage * state.pageSize / view.PageSize);
            }

            view.RegistryItems = service.GetItemsPage(view.UserID, registryId, (currentPage != -1) ? currentPage : view.CurrentPage, view.PageSize, view.StartDate, view.EndDate,
                view.SearchIncomeDate, view.SearchDocumentDate, view.SearchDocumentNumber, view.SearchSenderName,
                view.SearchCorrespondenceCategory, view.SearchCorrespondenceType, view.SearchTypeValue,
                view.SearchCorrespondenceKind, view.SearchCategoryValue,
                view.SearchCorrespondenceStatus, view.SearchCorrespondenceDept, view.SearchCorrespondenceWorker,view.ShowInvoices);
            view.PageCount = service.TotalItemsCount / view.PageSize + (service.TotalItemsCount % view.PageSize > 0 ? 1 : 0);
            if (currentPage != -1)
            {
                view.CurrentPage = currentPage;
            }
            
            saveState();
        }

        private void saveState()
        {
            session["{F48C43C9-6723-4a30-B114-A40295BDE72A}"] = new ViewRegistryPresenterState(view.CurrentPage, view.PageSize, view.CurrentDateRange, view.StartDate, view.EndDate,view.ShowInvoices);
        }

        public override void Initialize()
        {
            view.CorrespondenceCategories = service.GetCorrespondenceCategories();
            view.CorrespondenceTypes = service.GetCorrespondenceTypes();
            view.CorrespondenceStatus = service.GetCorrespondenceStatus();
            view.CorrespondenceDepts = service.GetCorrespondenceDepts();

            view.CorrespondenceKinds = service.GetCorrespondenceKinds(view.SearchCorrespondenceCategory);
            view.CorrespondenceWorkers = service.GetCorrespondenceWorkers(view.SearchCorrespondenceDept);

            ((IView)view).ViewTitle = "Przegl¹danie dziennika kancelaryjnego";
            setInitialValues();
            getData();
        }

        protected void OnExecuteCommand(ExecutingCommandEventArgs e)
        {
            if (ExecuteCommand != null)
            {
                saveState();
                ExecuteCommand(this, e);
            }
        }

        protected override void subscribeToEvents()
        {
            view.AcquiringItemID += new EventHandler(AcquireItemID);
            view.ActivePageChanged += new EventHandler<PagerEventArgs>(changeCurrentPage);
            view.ActiveDateRangeChanging += new EventHandler(dateRangeChanging);
            view.ActiveDateRangeChanged += new EventHandler(dateRangeChanged);
            view.CorrespondenceCategoriesChanged += new EventHandler(changeDocumentCategory);
            view.CorrespondenceDeptChanged += new EventHandler(changeCorrespondenceDept);
            view.CommandExecuting += new EventHandler<ExecutingCommandEventArgs>(commandExecuting);
            view.Filtering += new EventHandler(Filtering);
            view.ClearFilter += new EventHandler(ClearFilter);
        }

        void ClearFilter(object sender, EventArgs e)
        {
            getData();
        }

        void Filtering(object sender, EventArgs e)
        {
            //int i = 0;
        }

        void changeDocumentCategory(object sender, EventArgs e)
        {
            //int catId;

            //if (int.TryParse(view.SearchCorrespondenceCategory, out catId))
            //{
            //    view.CorrespondenceKinds = service.GetCorrespondenceKinds(catId);
            //    getData();
            //}
            
            view.CorrespondenceKinds = service.GetCorrespondenceKinds(view.SearchCorrespondenceCategory);
            getData();
        }

        void changeCorrespondenceDept(object sender, EventArgs e)
        {
            //int deptId;
            //if (int.TryParse(view.SearchCorrespondenceDept, out deptId))
            //{
            //    view.CorrespondenceWorkers = service.GetCorrespondenceWorkers(deptId);
            //    getData();
            //}
            view.CorrespondenceWorkers = service.GetCorrespondenceWorkers(view.SearchCorrespondenceDept);
            getData();
        }

        void AcquireItemID(object sender, EventArgs e)
        {
            session["itemIdRequest"] = registryId;
            OnExecuteCommand(new ExecutingCommandEventArgs("itemEdit", null));
        }

        private void dateRangeChanging(object sender, EventArgs e)
        {
            bool rangeSelected = true;
            switch (view.CurrentDateRange.ToLower())
            {
                case "dzisiaj":
                    view.StartDate = DateTime.Today;
                    view.EndDate = DateTime.Today;
                    view.CurrentDateRangeDescription = string.Format("({0:yyyy-MM-dd})", view.StartDate);
                    break;
                case "tydzien":
                    view.StartDate = DateTime.Today.AddDays(-(int)(DateTime.Today.DayOfWeek - 1)).Date;
                    view.EndDate = DateTime.Today.AddDays(1).AddSeconds(-1).Date;
                    view.CurrentDateRangeDescription = string.Format("(od {0:yyyy-MM-dd} do {1:yyyy-MM-dd})", view.StartDate, view.EndDate);
                    break;
                case "miesiac":
                    view.StartDate = DateTime.Parse(string.Format("{0}-{1}-01", DateTime.Today.Year, DateTime.Today.Month)).Date;
                    view.EndDate = DateTime.Parse(string.Format("{0}-{1}-{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)));
                    view.CurrentDateRangeDescription = string.Format("(od {0:yyyy-MM-dd} do {1:yyyy-MM-dd})", view.StartDate, view.EndDate);
                    break;
                case "zakres":
                    view.IsInDateChoosingState = true;
                    rangeSelected = false;
                    break;
            }
            if (rangeSelected)
            {
                view.CurrentPage = 1;
                getData();
            }
        }

        private void dateRangeChanged(object sender, EventArgs e)
        {
            view.IsInDateChoosingState = false;
            view.CurrentDateRangeDescription = string.Format("(od {0:yyyy-MM-dd} do {1:yyyy-MM-dd})", view.StartDate, view.EndDate);
            view.CurrentPage = 1;
            getData();
        }

        private void changeCurrentPage(object sender, PagerEventArgs e)
        {
            switch (e.EventType)
            {
                case PagerPage.GoToPage: view.CurrentPage = view.CurrentPage; break;
                case PagerPage.First: view.CurrentPage = 1; break;
                case PagerPage.Previous: if (view.CurrentPage > 1) view.CurrentPage -= 1; break;
                case PagerPage.Next: if (view.CurrentPage < view.PageCount) view.CurrentPage += 1; break;
                case PagerPage.Last: view.CurrentPage = view.PageCount; break;
            }
            getData();
        }

        private void commandExecuting(object sender, ExecutingCommandEventArgs e)
        {
            bool ok = true;
            if (ok)
                OnExecuteCommand(e);
        }

        protected override void redirectToPreviousView()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    class ViewRegistryPresenterState
    {
        public readonly int currentPage;
        public readonly int pageSize;
        public readonly string currentDateRange;
        public readonly DateTime startDate;
        public readonly DateTime endDate;
        public readonly bool browsingRF;

        public ViewRegistryPresenterState(int currentPage, int pageSize, string currentDateRange, DateTime startDate, DateTime endDate,bool browsingRF)
        {
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.currentDateRange = currentDateRange;
            this.startDate = startDate;
            this.endDate = endDate;
            this.browsingRF = browsingRF;
        }
    }

    public class PagerEventArgs : EventArgs
    {

        public readonly PagerPage EventType;

        public PagerEventArgs(PagerPage eventType)
        {
            this.EventType = eventType;
        }
    }

    public enum PagerPage
    {
        First,
        Previous,
        Next,
        Last,
        GoToPage
    }

    public enum DateRange
    {
        Today,
        CurrentWeek,
        CurrentMonth,
        CustomRange
    }
}