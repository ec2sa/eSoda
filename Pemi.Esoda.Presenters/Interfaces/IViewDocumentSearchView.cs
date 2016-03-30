using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters
{
    public interface IViewDocumentSearchView
    {
        /// <summary>
        /// Sets the message.
        /// </summary>
        string Message { set; }

        /// <summary>
        /// Gets or sets the search conditions.
        /// </summary>
        SearchDocumentConditions SearchConditions { get; set; }

        /// <summary>
        /// Gets or sets the search criteria state.
        /// </summary>
        SearchCriteriasState SearchCriteriaState { get; }

        /// <summary>
        /// Sets the search results.
        /// </summary>
        IList<SearchDocumentResultItem> SearchResults { set; }

        /// <summary>
        /// Sets the clients list.
        /// </summary>
        IList<SimpleLookupDTO> Categories{ set; }

        /// <summary>
        /// Sets the departments.
        /// </summary>
        IList<SimpleLookupDTO> Types { set; }

        /// <summary>
        /// Sets the statuses.
        /// </summary>
        IList<SimpleLookupDTO> Statuses{ set; }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        int CurrentPage { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Fires when the page has been changed.
        /// </summary>
        event EventHandler<PagerEventArgs> PageChanged;

        void SaveSearchState();
    }
}
