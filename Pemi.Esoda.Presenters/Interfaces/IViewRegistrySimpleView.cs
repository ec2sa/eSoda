using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.Core.Domain;
using Pemi.Esoda.DTO;
using System.Collections.ObjectModel;

namespace Pemi.Esoda.Presenters
{
    public interface IViewRegistrySimpleView : IView
    {
        int RegistryID { get; set; }

        int CurrentPage { get; set;}

        int PageSize { get; set;}

        int PageCount { get; set;}

        DateTime StartDate { get; set;}

        DateTime EndDate { get; set;}

        int Year { get; set; }

        bool ShowInvoices { get; set; }
        //string SearchIncomeDate { get;}

        //string SearchDocumentDate { get;}

        //string SearchDocumentNumber { get;}

        //string SearchSenderName { get;}

        //int SearchCorrespondenceCategory { get;}

        //int SearchCorrespondenceKind { get; }

        //int SearchCorrespondenceType { get;}

        //int SearchCorrespondenceStatus { get; }

        //int SearchCorrespondenceDept { get; }

        //int SearchCorrespondenceWorker { get; }

        //string SearchCategoryValue { get;}

        //string SearchTypeValue { get;}

        string RegistryItems { set;}

        string CurrentDateRange { get; set;}

        string CurrentDateRangeDescription { set;}

        bool IsInDateChoosingState { set;}

        bool IsCurrentDailyLogDef { get;  set; }

        //Collection<SimpleLookupDTO> CorrespondenceCategories { set;}
        //Collection<SimpleLookupDTO> CorrespondenceKinds { set; }
        //Collection<SimpleLookupDTO> CorrespondenceTypes { set;}
        //Collection<SimpleLookupDTO> CorrespondenceStatus { set; }
        //Collection<SimpleLookupDTO> CorrespondenceDepts { set;}
        //Collection<SimpleLookupDTO> CorrespondenceWorkers { set; }

        //event EventHandler CorrespondenceCategoriesChanged;
        //event EventHandler CorrespondenceDeptChanged;
        event EventHandler Filtering;
        //event EventHandler ClearFilter;

        event EventHandler<PagerEventArgs> ActivePageChanged;

        event EventHandler ActiveDateRangeChanging;

        event EventHandler ActiveDateRangeChanged;

        event EventHandler<ExecutingCommandEventArgs> CommandExecuting;

        event EventHandler AcquiringItemID;
    }
}
        