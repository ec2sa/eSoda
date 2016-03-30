using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Pemi.Esoda.DataAccess
{
	/// <summary>
	/// Interfejs mechanizmu dostêpu do repozytorium elementów
	/// </summary>
	public interface IItemStorage
	{
		/// <summary>
		/// Zapisuje element w repozytorium
		/// </summary>
		/// <param name="contentStream">Zawartoœæ elementu</param>
		/// <returns>Identyfikator nadany elementowi w repozytorium</returns>
		/// <exception cref="ItemException"/>
		Guid Save(Stream contentStream);
		
		/// <summary>
		/// Pobiera element z repozytorium
		/// </summary>
		/// <param name="itemID">Identyfikator pobieranego elementu</param>
		/// <returns>Zawartoœæ pobieranego elementu</returns>
		/// <exception cref="ItemException"/>
		Stream Load(Guid itemID);

		Stream Load(string itemGuid);

        Stream LoadGuid(Guid itemId);

		/// <summary>
		/// Okreœla czy w repozytorium istnieje element 
		/// </summary>
		/// <param name="itemID">Identyfikator elementu w repozytorium</param>
		bool Exists(Guid itemID);

		bool Exists(string itemGuid);
        
        bool ExistsGuid(Guid itemId);

    bool Delete(Guid itemID);

    bool Delete(string itemGuid);

    bool DeleteGuid(Guid itemId);
	}
}
