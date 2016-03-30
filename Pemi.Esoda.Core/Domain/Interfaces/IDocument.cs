using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Pemi.Esoda.DTO;

namespace Pemi.Esoda.Core.Domain
{
	public interface IDocument:IMetadata
	{
        /// <summary>
		/// Nazwa dokumentu
		/// </summary>
		string Name { get; set;}
		
		/// <summary>
		/// Opis dokumentu
		/// </summary>
		string Description { get; set;}

		/// <summary>
		/// Data utworzenia dokumentu w systemie
		/// </summary>
		DateTime CreationDate { get;}

		/// <summary>
		/// W³aœciciel - twórca dokumentu w systemie
		/// </summary>
		string Owner { get;}
		
		/// <summary>
		/// Zwraca listê elementów dokumentu
		/// </summary>
		Collection<DocumentItemDTO> GetItems();
		
		/// <summary>
		/// Dodaje nowy element do dokumentu
		/// </summary>
		/// <param name="item">Element, którego dotyczy operacja</param>
		void AddItem(IDocumentItem item);
		
		/// <summary>
		/// Usuwa element z dokumentu
		/// </summary>
		/// <param name="item">Element, którego dotyczy operacja</param>
		void RemoveItem(IDocumentItem item);

		/// <summary>
		/// Modyfikuje element dokumentu
		/// </summary>
		/// <param name="item">Element, którego dotyczy operacja</param>
		void UpdateItem(IDocumentItem item);

		/// <summary>
		/// Pobiera element z dokumentu
		/// </summary>
		/// <param name="itemID">ID elementu do pobrania</param>
		/// <returns></returns>
		IDocumentItem GetItem(Guid itemID);

		//Collection<IHistoryItem> GetHistory();
		
	}
}
