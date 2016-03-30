using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.IO;
using p=Pemi.Esoda.Tools;

namespace Pemi.Esoda.Web.UI
{
	/// <summary>
	/// Klasa odpowiadaj¹ca za monitorowanie folderów urz¹dzeñ i okreœlanie liczby skanów do przetworzenia
	/// </summary>
	public class MonitorUrzadzen
	{
		public static string DomyslnyKatalog
		{
			get
			{
				XPathHelperClass xp = new XPathHelperClass(p.Configuration.ScannersConfigurationFile);
				return xp.PobierzWartosc("/konfiguracjaSkanerow/@katalog");
			}
		}
		/// <summary>
		/// Zlicza  skany dostępne we wskazanym katalogu (oraz ewentalnie podkatalogach)
		/// </summary>
		/// <param name="katalog">wskazuje katalog, w którym będ¹ wyszukiwane skany</param>
		/// <param name="sposob">Okreœla czy szukanie odbywa się tylko we wskazanym katalogu czy tak¿e w jego podkatalogach</param>
		/// <returns>Liczbę odnalezionych skanów</returns>
		public static int LiczbaOczekujacychSkanow(string katalog, SearchOption sposob)
		{
			return Directory.GetFiles(katalog, "*.tif?", sposob).Length;
		}

		/// <summary>
		/// Zlicza dostępne skany w katalogu wskazanym w pliku konfiguracyjnym skanerów
		/// </summary>
		/// <param name="sposob">Okreœla czy szukanie odbywa się tylko w katalogu czy tak¿e w jego podkatalogach</param>
		/// <returns>Liczbę odnalezionych skanów</returns>
		public static int LiczbaOczekujacychSkanow(SearchOption sposob)
		{
			return LiczbaOczekujacychSkanow(DomyslnyKatalog, sposob);
		}

		/// <summary>
		/// Zlicza dostępne skany w katalogu wskazanym w pliku konfiguracyjnym skanerów, oraz w jego podkatalogach
		/// </summary>
		/// <returns>Liczbę odnalezionych skanów</returns>
		public static int LiczbaOczekujacychSkanow()
		{
			return LiczbaOczekujacychSkanow(DomyslnyKatalog, SearchOption.AllDirectories);
		}
	}
}