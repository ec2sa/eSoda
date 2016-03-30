using System;
using System.Collections.Generic;
using System.Text;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Presenters
{
	public interface IViewCaseSearchView
	{
        /// <summary>
        /// Sets the message.
        /// </summary>
        string Message { set; }

        /// <summary>
        /// Gets or sets the search conditions.
        /// </summary>
        SearchCaseConditions SearchConditions { get; set; }

        /// <summary>
        /// Sets the search results.
        /// </summary>
        IList<SearchCaseResultItem> SearchResults { set; }

        /// <summary>
        /// Sets the clients list.
        /// </summary>
        IList<SimpleLookupDTO> Clients { set; }

        /// <summary>
        /// Sets the departments.
        /// </summary>
        IList<SimpleLookupDTO> Departments { set; }

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
	}
}
