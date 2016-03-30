using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.XPath;

namespace Pemi.Esoda.Presenters
{
	public class KonfiguratorUrzadzen
	{
		private string _plikXml;

		/// <summary>
		/// nazwa pliku konfiguracyjnego
		/// </summary>
		public string PlikXml
		{
			get { return this._plikXml; }
		}

		/// <summary>
		/// Tworzy now? instancj? konfiguratora urz?dze? bazuj?c na pliku konfiguracyjnym
		/// </summary>
		/// <param name="plikXml">nazwa pliku konfiguracyjnego</param>
		public KonfiguratorUrzadzen(string plikXml)
		{
			if (!System.IO.File.Exists(plikXml))
			{
				throw new System.IO.FileNotFoundException(string.Format("Nie mo¿na odnaleŸæ pliku XML {0}", plikXml));
			}
			this._plikXml = plikXml;
		}

		private List<ListItem> pobierzZawartosc(string xPath)
		{

			List<ListItem> tmp = new List<ListItem>();
			XPathDocument xpd = new XPathDocument(this._plikXml);
			XPathNavigator xpn = xpd.CreateNavigator();
			//	XmlNamespaceManager nsmgr = new XmlNamespaceManager(xpn.NameTable);
			//	nsmgr.AddNamespace("ec", "http://ec2.pl/esoda/konfiguracjaSkanerow");
			//,nsmgr
			XPathNodeIterator xpni = xpn.Select(xPath);
			while (xpni.MoveNext())
			{
				tmp.Add(new ListItem(xpni.Current.Value, xpni.Current.Value));
			}
			return tmp;
		}

		/// <summary>
		/// Zwraca list? lokalizacji
		/// </summary>
		/// <returns>Kolekcj? obiektów ListItem</returns>
		public List<ListItem> PobierzLokalizacje()
		{
			List<ListItem> tmp = this.pobierzZawartosc("/konfiguracjaSkanerow/lokalizacja[not(@nazwa=preceding-sibling::lokalizacja/@nazwa)]/@nazwa");
			tmp.Insert(0, new ListItem("-- dowolna --", "*"));
			return tmp;
		}

		/// <summary>
		/// Zwraca list? urz?dze?
		/// </summary>
		/// <returns>Kolekcj? obiektów ListItem</returns>
		public List<ListItem> PobierzUrzadzenia()
		{
			List<ListItem> tmp = this.pobierzZawartosc("/konfiguracjaSkanerow/lokalizacja/skaner[not(@nazwa=preceding-sibling::skaner/@nazwa)]/@nazwa");
			tmp.Insert(0, new ListItem("-- dowolne --", "*"));
			return tmp;
		}

		/// <summary>
		/// Zwraca list? rodzajów dokumentów
		/// </summary>
		/// <returns>Kolekcj? obiektów ListItem</returns>
		public List<ListItem> PobierzRodzajeDokumentow()
		{
			List<ListItem> tmp = this.pobierzZawartosc("/konfiguracjaSkanerow/lokalizacja/skaner//kryterium[@typ='rodzajDokumentu' and not(@nazwa=preceding::kryterium/@nazwa)]/@nazwa");
			tmp.Insert(0, new ListItem("-- dowolny --", "*"));
			return tmp;
		}

		/// <summary>
		/// Zwraca list? ?róde? dokumentów
		/// </summary>
		/// <returns>Kolekcj? obiektów ListItem</returns>
		public List<ListItem> PobierzZrodlaDokumentow()
		{
			List<ListItem> tmp = this.pobierzZawartosc("/konfiguracjaSkanerow/lokalizacja/skaner//kryterium[@typ='zrodloDokumentu' and not(@nazwa=preceding::kryterium/@nazwa)]/@nazwa");
			tmp.Insert(0, new ListItem("-- dowolne --", "*"));
			return tmp;
		}

		public List<string> WyszukajKatalogi(string lokalizacja, string urzadzenie, string rodzaj, string zrodlo)
		{
			lokalizacja = ((lokalizacja != "*") ? "='" + lokalizacja + "']" : "]");
			List<string> tmp = new List<string>();
			XPathDocument xpd = new XPathDocument(this._plikXml);
			XPathNavigator xpn = xpd.CreateNavigator();
			XPathNodeIterator xpnir = xpn.Select("/konfiguracjaSkanerow/@katalog");
			string katalogBazowy = null;
			if (xpnir.MoveNext())
			{
				katalogBazowy = xpnir.Current.Value + "\\";
			}
			XPathNodeIterator xpni = xpn.Select(string.Format("/konfiguracjaSkanerow/lokalizacja[@nazwa{0}/@katalog", lokalizacja));
			while (xpni.MoveNext())
			{
				string katalogLokalizacji = katalogBazowy + xpni.Current.Value + "\\";
				xpni.Current.MoveToParent();
				string filtrUrzadzenia = ((urzadzenie != "*") ? "='" + urzadzenie + "']" : "]");
				XPathNodeIterator skanery = xpni.Current.Select("skaner[@nazwa" + filtrUrzadzenia + "/@katalog");
				if (skanery.Count > 0)
				{
					while (skanery.MoveNext())
					{
						string katalogUrzadzenia = katalogLokalizacji + skanery.Current.Value + "\\";
						skanery.Current.MoveToParent();
						string filtrRodzaju = ((rodzaj != "*") ? "='" + rodzaj + "']" : "]");
						XPathNodeIterator kryteria = skanery.Current.Select("kryterium[@nazwa" + filtrRodzaju + "/@katalog");
						if (kryteria.Count > 0)
						{
							while (kryteria.MoveNext())
							{
								string katalogRodzaju = katalogUrzadzenia + kryteria.Current.Value + "\\";
								kryteria.Current.MoveToParent();
								string filtrZrodla = ((zrodlo != "*") ? "='" + zrodlo + "']" : "]");
								XPathNodeIterator zrodla = kryteria.Current.Select("kryterium[@nazwa" + filtrZrodla + "/@katalog");
								if (zrodla.Count > 0)
								{
									while (zrodla.MoveNext())
									{
										string katalogZrodla = katalogRodzaju + zrodla.Current.Value;
										tmp.Add(katalogZrodla);
									}
								}
								else
								{
									tmp.Add(katalogRodzaju);
								}
							}
						}
						else
						{
							tmp.Add(katalogUrzadzenia);
						}
					}
				}
			}
			return tmp;
		}

		public void TworzStruktureKatalogow()
		{
			foreach (string katalog in WyszukajKatalogi("*", "*", "*", "*"))
			{
				System.IO.Directory.CreateDirectory(katalog);
			}
		}
	}
}
