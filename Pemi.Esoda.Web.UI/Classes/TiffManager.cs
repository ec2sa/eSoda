using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections;
using System.Xml;
using System.Web;


namespace Pemi.Esoda.Web.UI
{
    /// <summary>
    /// Klasa implementuj¹ca podstawowe operacje wykonywane na skanach w formacie tiff
    /// </summary>
    public class TiffManager : IEnumerable, IEnumerator
    {
        private TiffManagerKonfiguracja _konfiguracja;

        private string _nazwaPlikuTiff;

        public string NazwaPlikuSkanu
        {
            get { return Path.GetFileName(this._nazwaPlikuTiff); }
        }

        public string NazwaPlikuSkanuZeSciezka
        {
            get { return this._nazwaPlikuTiff; }
        }

        private System.Drawing.Image _skan;

        private List<DokumentLogiczny> _dokumentyLogiczne;

        private int aktywnaStrona;

        /// <summary>
        /// Zwraca nazwê pliku miniatury pierwszej strony dokumentu
        /// </summary>
        public string NazwaMiniaturyPierwszejStrony
        {
            get { return this.NazwaPlikuStrony(1, true); }
        }

        private System.Drawing.Image wczytajZPlikuSzybko(string nazwaPliku)
        {
            System.Drawing.Image imgTmp = null;
            System.Drawing.Image img = null;
            try
            {
                MemoryStream mem = new MemoryStream(File.ReadAllBytes(nazwaPliku));
                imgTmp = System.Drawing.Image.FromStream(mem, true, true);

                if (imgTmp.PixelFormat == PixelFormat.Format1bppIndexed)
                    img = new Bitmap(mem);
                
                //img = System.Drawing.Image.FromStream(mem, false,true);
            }
            catch
            {
            }
            if (img == null)
                return imgTmp;
            return img;
        }

        private System.Drawing.Image pobierzStrone(int numerStrony)
        {
            MemoryStream ms = null;
            System.Drawing.Image strona = null;
            try
            {
                ms = new MemoryStream();
                _skan.SelectActiveFrame(FrameDimension.Page, numerStrony - 1);
                _skan.Save(ms, Konfiguracja.FormatStrony);

                strona = System.Drawing.Image.FromStream(ms, true, false);

                return strona;
            }
            catch
            {
                ms.Close();
                strona.Dispose();
                throw;
            }
        }

        private string generujSekcjePochodzenia(string plikSkanu)
        {
            //string s1 = plikSkanu;
            XPathHelperClass xp = new XPathHelperClass(Konfiguracja.PlikKonfiguracyjnySkanerow);
            string katalogGlowny = xp.PobierzWartosc("/konfiguracjaSkanerow/@katalog").ToLower();
            plikSkanu = Path.GetDirectoryName(plikSkanu).ToLower();
            //string s2 = plikSkanu;
            plikSkanu = plikSkanu.Replace(katalogGlowny + "\\", "");
            
            string[] poziomy = plikSkanu.Split('\\');
            string lokalizacja = (poziomy.Length > 0) ? "='" + poziomy[0] + "'" : "";
            string urzadzenie = (poziomy.Length > 1) ? "='" + poziomy[1] + "'" : "";
            string rodzaj = (poziomy.Length > 2) ? "='" + poziomy[2] + "'" : "";
            string zrodlo = (poziomy.Length > 3) ? "='" + poziomy[3] + "'" : "";

            StringBuilder sb = new StringBuilder();

            sb.Append("<pochodzenie><lokalizacja>");
            sb.Append(xp.PobierzWartosc(string.Format("/konfiguracjaSkanerow/lokalizacja[@katalog{0} and skaner/@katalog{1} and (skaner/kryterium/@katalog{2} or not(skaner/kryterium)) and (skaner/kryterium/kryterium/@katalog{3} or not(skaner/kryterium/kryterium))]/@nazwa", lokalizacja, urzadzenie, rodzaj, zrodlo)));
            sb.Append("</lokalizacja>");
            sb.Append("<urzadzenie>");
            sb.Append(xp.PobierzWartosc(string.Format("/konfiguracjaSkanerow/lokalizacja/skaner[../@katalog{0} and @katalog{1} and (kryterium/@katalog{2} or not(kryterium)) and (kryterium/kryterium/@katalog{3} or not(kryterium/kryterium))]/@nazwa", lokalizacja, urzadzenie, rodzaj, zrodlo)));
            sb.Append("</urzadzenie>");
            sb.Append("<rodzaj>");
            sb.Append(xp.PobierzWartosc(string.Format("/konfiguracjaSkanerow/lokalizacja/skaner/kryterium[../../@katalog{0} and ../@katalog{1} and @katalog{2} and (kryterium/@katalog{3} or not(kryterium))]/@nazwa", lokalizacja, urzadzenie, rodzaj, zrodlo)));
            sb.Append("</rodzaj>");
            sb.Append("<zrodlo>");
            sb.Append(xp.PobierzWartosc(string.Format("/konfiguracjaSkanerow/lokalizacja/skaner/kryterium/kryterium[../../../@katalog{0} and ../../@katalog{1} and ../@katalog{2} and @katalog{3}]/@nazwa", lokalizacja, urzadzenie, rodzaj, zrodlo)));
            sb.Append("</zrodlo>");
            sb.Append("</pochodzenie>");
            return sb.ToString();

        }

        /// <summary>
        /// Kolekcja zawieraj¹ca dokumenty logiczne wykryte w tiffie.
        /// </summary>
        public List<DokumentLogiczny> DokumentyLogiczne
        {
            get
            {
                return this._dokumentyLogiczne;
            }
        }

        /// <summary>
        /// Generuje nazwê dla pliku strony wg zdefniowanego szablonu
        /// </summary>
        /// <param name="nrStrony">numer strony</param>
        /// <param name="czyMiniatura">okreœla czy generowana jest nazwa dla miniatury czy strony</param>
        /// <returns>Zwraca wygenerowan¹ nazwê</returns>
        public string NazwaPlikuStrony(int nrStrony, bool czyMiniatura)
        {
            DateTime d;
            string nazwaPliku = null;
            if (this.DataUtworzenia != null)
            {
                d = this.DataUtworzenia.Value;
                nazwaPliku = string.Format(this._konfiguracja.FormatNazwyPliku, d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Millisecond, nrStrony, (czyMiniatura) ? _konfiguracja.PrzedrostekMiniatury : "", (czyMiniatura) ? _konfiguracja.FormatMiniatury : _konfiguracja.FormatStrony);
            }
            else
                nazwaPliku = Guid.NewGuid().ToString();
            return nazwaPliku;
        }

        /// <summary>
        /// Generuje nazwê pliku (z rozszerzeniem) dla dokumentów logicznych, miniaur, stron i metadanych
        /// </summary>
        /// <param name="numer">Okreœla numer dokumentu, strony, miniatury. Bez znaczenia przy metadanych</param>
        /// <param name="typ">Okreœla tp generowanej nazwy</param>
        /// <returns></returns>
        public string NazwaPliku(int numerDokumentu, int numerStrony, RodzajNazwy typ)
        {
            string nazwa = Guid.NewGuid().ToString();
            switch (typ)
            {
                case RodzajNazwy.Dokument:
                    nazwa = Path.GetFileNameWithoutExtension(this.NazwaPlikuSkanu) + Konfiguracja.PrzedrostekDokumentu + numerDokumentu.ToString() + ".tif";
                    break;
                case RodzajNazwy.Strona:
                    nazwa = Path.GetFileNameWithoutExtension(this.NazwaPlikuSkanu) + Konfiguracja.PrzedrostekDokumentu + numerDokumentu.ToString() + Konfiguracja.PrzedrostekStrony + numerStrony.ToString() + "." + this.Konfiguracja.FormatStrony.ToString();
                    break;
                case RodzajNazwy.StronaPodgladWeb:
                    nazwa = Path.GetFileNameWithoutExtension(this.NazwaPlikuSkanu) + Konfiguracja.PrzedrostekDokumentu + numerDokumentu.ToString() + Konfiguracja.PrzedrostekPodgladu + numerStrony.ToString() + "." + this.Konfiguracja.FormatPodgladu.ToString();
                    break;
                case RodzajNazwy.StronaPelnaWeb:
                    nazwa = Path.GetFileNameWithoutExtension(this.NazwaPlikuSkanu) + Konfiguracja.PrzedrostekDokumentu + numerDokumentu.ToString() + Konfiguracja.PrzedrostekStrony + numerStrony.ToString() + "." + this.Konfiguracja.FormatPodgladu.ToString();
                    break;
                case RodzajNazwy.Miniatura:
                    nazwa = Path.GetFileNameWithoutExtension(this.NazwaPlikuSkanu) + Konfiguracja.PrzedrostekDokumentu + numerDokumentu.ToString() + Konfiguracja.PrzedrostekMiniatury + numerStrony.ToString() + "." + this.Konfiguracja.FormatMiniatury.ToString();
                    break;
                case RodzajNazwy.Metadane:
                    nazwa = Path.GetFileNameWithoutExtension(this.NazwaPlikuSkanu) + Konfiguracja.PrzedrostekDokumentu + numerDokumentu.ToString() + ".xml";
                    break;
            }
            return nazwa;
        }

        /// <summary>
        /// Okreœla czy zosta³ wczytany plik tiff
        /// </summary>
        public bool PlikWczytany
        {
            get { return this._skan is System.Drawing.Image; }
        }

        /// <summary>
        /// Okreœla liczbê stron we wczytanym pliku tiff
        /// </summary>
        public int LiczbaStron
        {
            get
            {
                if (!PlikWczytany) return 0;
                return this._skan.GetFrameCount(FrameDimension.Page);
            }
        }

        /// <summary>
        /// Okreœla wymiary wczytanego pliku tiff
        /// </summary>
        public Size Wymiary
        {
            get
            {
                if (!PlikWczytany) return new Size(0, 0);
                return _skan.Size;
            }
        }

        /// <summary>
        /// zwraca datê utworzenia pliku tiff
        /// </summary>
        public DateTime? DataUtworzenia
        {
            get
            {
                // w naszych tiffach nie ma tej w³aœciwoœci ustawianej
                //if (_tiff.PropertyItems[306] == null)
                if (!PlikWczytany)
                    return null;
                return new FileInfo(_nazwaPlikuTiff).CreationTime;

                // nie jestem pewnien co tu zwróci tostring()
                //return DateTime.Parse(_tiff.PropertyItems[306].ToString());
            }
        }

        /// <summary>
        /// Daje dostêp do ustawien konfiguracyjnych
        /// </summary>
        public TiffManagerKonfiguracja Konfiguracja
        {
            get
            {
                return this._konfiguracja;
            }
        }

        /// <summary>
        /// Indekser zwracaj¹cy poszczególne strony lub miniatury z tiffa
        /// </summary>
        /// <param name="nrStrony">Kolejny numer strony (pocz¹wszy od 1)</param>
        /// <param name="czyMiniatura">Okreœla czy zwróciæ miniaturê czy stronê </param>
        /// <returns></returns>
        public System.Drawing.Image this[int nrStrony, bool czyMiniatura]
        {
            get
            {
                if (nrStrony < 1 || nrStrony > this.LiczbaStron)
                    return null;
                System.Drawing.Image strona = pobierzStrone(nrStrony);

                if (czyMiniatura)
                    return strona.GetThumbnailImage(_konfiguracja.SzerokoscMiniatury, _konfiguracja.WysokoscMiniatury, null, IntPtr.Zero);
                return strona;
            }
        }

        /// <summary>
        /// Tworzy obiekt TiffManager i inicjuje konfiguracjê
        /// </summary>
        /// <param name="konfiguracja">Obiekt TiffManagerKonfiguracja zawieraj¹cy ustawienia</param> 
        public TiffManager(TiffManagerKonfiguracja konfiguracja)
        {
            this._konfiguracja = konfiguracja;
            this._dokumentyLogiczne = new List<DokumentLogiczny>();
        }

        /// <summary>
        /// Tworzy obiekt TiffManager z domyœln¹ konfiguracj¹
        /// </summary>
        public TiffManager() : this(new TiffManagerKonfiguracja()) { }

        public void WczytajZPliku(string plikTiff)
        {
            this.WczytajZPliku(plikTiff, true);
        }

        /// <summary>
        /// Wczytuje obraz Tiff z wskazanego pliku
        /// </summary>
        /// <param name="plikTiff">lokalizacja pliku tiff</param>
        public void WczytajZPliku(string plikTiff, bool czySeparatory)
        {
            List<int> separatory = new List<int>();
            int pozycja = 1;
            this._skan = null;
            this._nazwaPlikuTiff = null;

            try
            {
                if (File.Exists(plikTiff))
                {
                    DateTime ds = DateTime.Now;
                    this._skan = wczytajZPlikuSzybko(plikTiff);
                    if (this._skan != null)
                    {
                        DateTime dk = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine("czas 1: ", (dk - ds).ToString());
                        this._nazwaPlikuTiff = plikTiff;
                       
                        if (czySeparatory)
                        {
                            for (int i = 1; i <= this.LiczbaStron; i++)
                            {
                                if (WykrywaczSeparatora.JestSeparatorem(this, i))
                                    separatory.Add(i);
                            }
                        }
                        this._dokumentyLogiczne.Clear();
                        if (separatory.Count == 0)
                            this._dokumentyLogiczne.Add(new DokumentLogiczny(1, this.LiczbaStron, this));
                        else
                        {

                            foreach (int strona in separatory)
                            {
                                this._dokumentyLogiczne.Add(new DokumentLogiczny(pozycja, strona - 1, this));
                                pozycja = strona + 1;
                            }
                            if (pozycja <= this.LiczbaStron)
                                this._dokumentyLogiczne.Add(new DokumentLogiczny(pozycja, this.LiczbaStron, this));
                        }
                        DateTime dk2 = DateTime.Now;
                        System.Diagnostics.Debug.WriteLine("czas calosci: ", (dk2 - ds).ToString());
                    }
                }

            }
            catch (System.OutOfMemoryException ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }

        }


        public void ZapiszStroneDoPliku(int nrStrony, bool czyMiniatura, string nazwaPliku, ImageFormat format, RotateFlipType obrot)
        {
            System.Drawing.Image strona = this[nrStrony, czyMiniatura];
            strona.RotateFlip(obrot);
            if (!Directory.Exists(Path.GetDirectoryName(nazwaPliku)))
                Directory.CreateDirectory(Path.GetDirectoryName(nazwaPliku));
            strona.Save(nazwaPliku, format);
        }

        /// <summary>
        /// Zapisuje wybran¹ stronê/miniaturê we wskazanym pliku i formacie
        /// </summary>
        /// <param name="nrStrony">numer wybranej strony z tiffa</param>
        /// <param name="czyMiniatura">okreœla czy zapisaæ miniaturê czy stronê</param>
        /// <param name="nazwaPliku">okreœla pe³n¹ nazwê pliku, w którym zapisaæ stroneê/miniaturê</param>
        /// <param name="format">Okreœla docelowy format pliku graficznego</param>
        public void ZapiszStroneDoPliku(int nrStrony, bool czyMiniatura, string nazwaPliku, ImageFormat format)
        {
            this.ZapiszStroneDoPliku(nrStrony, czyMiniatura, nazwaPliku, format, RotateFlipType.RotateNoneFlipNone);

        }

        /// <summary>
        /// Zapisuje wybran¹ stronê/miniaturê we wskazanym pliku w formacie tiff
        /// </summary>
        /// <param name="nrStrony">numer wybranej strony z tiffa</param>
        /// <param name="czyMiniatura">okreœla czy zapisaæ miniaturê czy stronê</param>
        /// <param name="nazwaPliku">okreœla pe³n¹ nazwê pliku, w którym zapisaæ stroneê/miniaturê</param>
        public void ZapiszStroneDoPliku(int nrStrony, bool czyMiniatura, string nazwaPliku)
        {
            this.ZapiszStroneDoPliku(nrStrony, czyMiniatura, nazwaPliku, czyMiniatura ? _konfiguracja.FormatMiniatury : _konfiguracja.FormatStrony);
        }

        /// <summary>
        /// Zapisuje wybran¹ stronê/miniaturê w formacie tiff pod nazw¹ wynikaj¹c¹ z ustawieñ konfiguracyjnych
        /// </summary>
        /// <param name="nrStrony">numer wybranej strony z tiffa</param>
        /// <param name="czyMiniatura">okreœla czy zapisaæ miniaturê czy stronê</param>
        public void ZapiszStroneDoPliku(int nrStrony, bool czyMiniatura)
        {
            this.ZapiszStroneDoPliku(nrStrony, czyMiniatura, this._konfiguracja.KatalogWyjsciowy + "\\" + this.NazwaPlikuStrony(nrStrony, czyMiniatura));
        }

        /// <summary>
        /// Zapisuje poszczególne strony i/lub miniatury i/lub metadane we wskazanym katalogu
        /// </summary>
        /// <param name="katalog">Katalog, w którym umieszczone bêd¹ pliki wyjœciowe</param>
        /// <param name="czyMiniatury">Okreœla czy generowaæ pliki miniatur</param>
        /// <param name="czyStrony">Okreœla czy generowaæ pliki stron</param>
        /// <param name="czyMetadane">Okreœla czy generowaæ plik metadanych</param>
        public void ZapiszStronyDoKatalogu(string katalog, bool czyMiniatury, bool czyStrony, bool czyMetadane)
        {
            if (!Directory.Exists(katalog))
                Directory.CreateDirectory(katalog);
            DateTime d;
            string nazwaPliku = null;
            if (this.DataUtworzenia != null)
                d = this.DataUtworzenia.Value;
            else
                d = DateTime.Now;

            for (int i = 1; i <= this.LiczbaStron; i++)
            {

                if (czyMiniatury)
                {
                    nazwaPliku = this.NazwaPlikuStrony(i, true);
                    ZapiszStroneDoPliku(i, true, katalog + "\\" + nazwaPliku);
                }
                if (czyStrony)
                {
                    nazwaPliku = this.NazwaPlikuStrony(i, false);
                    ZapiszStroneDoPliku(i, false, katalog + "\\" + nazwaPliku);
                }
            }
            if (czyMetadane)
                ZapiszMetadane(katalog);
        }

        /// <summary>
        /// Zapisuje poszczególne Strony i/lub miniatury i/lub metadane w katalogu okreœlonym w konfiguracji
        /// </summary>
        /// <param name="czyMiniatury">Okreœla czy generowaæ pliki miniatur</param>
        /// <param name="czyStrony">Okreœla czy generowaæ pliki stron</param>
        /// <param name="czyMetadane">Okreœla czy generowaæ plik metadanych</param>
        public void ZapiszStronyDoKatalogu(bool czyMiniatury, bool czyStrony, bool czyMetadane)
        {
            this.ZapiszStronyDoKatalogu(this._konfiguracja.KatalogWyjsciowy, czyMiniatury, czyStrony, czyMetadane);
        }

        /// <summary>
        /// Przetwarza skan na dokumenty logiczne, które s¹ zapisywane w formacie tif
        /// </summary>
        /// <param name="katalog">Okreœla lokalizacjê plików wynikowych</param>
        /// <param name="czyMinitura">Okreœla czy ma byc generowana miniatura pierwszej strony </param>
        /// <param name="czyStrona">Okreœla czy ma byæ generowana pierwsza strona dokumentu logicznego</param>
        /// <param name="czMetadane">Okreœla czy generowany jest plik xml z metadanymi</param>
        public void ZapiszDokumentyLogiczneDoKatalogu(string katalog, bool czyMiniatura, bool czyStrona, bool czyPodglad, bool czyMetadane)
        {
            foreach (DokumentLogiczny dl in this._dokumentyLogiczne)
            {
                int nrDok = this._dokumentyLogiczne.IndexOf(dl) + 1;
                string nazwaPliku = katalog + "\\" + this.NazwaPliku(nrDok, 1, RodzajNazwy.Dokument);
                dl.Zapisz(nazwaPliku);
                if (czyMiniatura)
                {
                    nazwaPliku = katalog + "\\" + this.NazwaPliku(nrDok, 1, RodzajNazwy.Miniatura);
                    dl.Skan.SelectActiveFrame(FrameDimension.Page, 0);
                    dl.Skan.GetThumbnailImage(this._konfiguracja.SzerokoscMiniatury, this._konfiguracja.WysokoscMiniatury, null, IntPtr.Zero).Save(nazwaPliku, this._konfiguracja.FormatMiniatury);
                }
                if (czyStrona)
                {
                    nazwaPliku = katalog + "\\" + this.NazwaPliku(nrDok, 1, RodzajNazwy.StronaPelnaWeb);
                    dl.Skan.SelectActiveFrame(FrameDimension.Page, 0);
                    dl.Skan.Save(nazwaPliku, this._konfiguracja.FormatPodgladu);
                }
                if (czyPodglad)
                {
                    nazwaPliku = katalog + "\\" + this.NazwaPliku(nrDok, 1, RodzajNazwy.StronaPodgladWeb);
                    dl.Skan.SelectActiveFrame(FrameDimension.Page, 0);
                    dl.Skan.GetThumbnailImage(Konfiguracja.SzerokoscPodgladu, Konfiguracja.WysokoscPodgladu, null, IntPtr.Zero).Save(nazwaPliku, this._konfiguracja.FormatPodgladu);
                }
                if (czyMetadane)
                {
                    this.ZapiszMetadaneDokumentuLogicznego(katalog, nrDok);
                }
            }
        }

        /// <summary>
        /// Przetwarza skan na dokumenty logiczne, które s¹ zapisywane w formacie tif. Generuje miniaturê, pierwsz¹ stronê oraz metadane
        /// </summary>
        /// <param name="katalog">Okreœla lokalizacjê plików wynikowych</param>
        public void ZapiszDokumentyLogiczneDoKatalogu(string katalog)
        {
            this.ZapiszDokumentyLogiczneDoKatalogu(katalog, true, true, true, true);
        }

        /// <summary>
        /// Przetwarza skan na dokumenty logiczne, które s¹ zapisywane w formacie tif. 
        /// Generuje miniaturê, pierwsz¹ stronê oraz metadane. 
        /// Pliki wynikowe umieszczone w katalogu okreœlonym w konfiguracji.
        /// </summary>
        public void ZapiszDokumentyLogiczneDoKatalogu()
        {
            this.ZapiszDokumentyLogiczneDoKatalogu(this._konfiguracja.KatalogWyjsciowy);
        }

        /// <summary>
        /// Generuje metadane dla pliku tiff
        /// </summary>
        /// <returns>
        /// Instancjê XmlReaders zawieraj¹cego metadane.
        ///</returns>
        public string GenerujMetadane()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            xw.WriteStartDocument();
            xw.WriteStartElement("skan");
            xw.WriteAttributeString("nazwaPliku", Path.GetFileName(this._nazwaPlikuTiff));
            xw.WriteAttributeString("dataUtworzenia", this.DataUtworzenia.ToString());
            xw.WriteAttributeString("liczbaStron", this.LiczbaStron.ToString());
            xw.WriteAttributeString("liczbaDokumentowLogicznych", "1");

            xw.WriteStartElement("daneOgolne");

            xw.WriteStartElement("miniatura");
            xw.WriteAttributeString("nrStrony", "1");
            xw.WriteString(this.NazwaPlikuStrony(1, true));
            xw.WriteEndElement();//miniatura


            xw.WriteEndElement();//daneOgolne;
            xw.WriteStartElement("dokumentyLogiczne");
            foreach (DokumentLogiczny dok in this._dokumentyLogiczne)
            {
                xw.WriteStartElement("dokument");
                xw.WriteAttributeString("stronaPoczatkowa", dok.StronaPoczatkowa.ToString());
                xw.WriteAttributeString("stronaKoncowa", dok.StronaKoncowa.ToString());
                for (int i = 1; i <= this.LiczbaStron; i++)
                {
                    xw.WriteStartElement("strona");
                    xw.WriteAttributeString("nr", i.ToString());
                    xw.WriteString(this.NazwaPlikuStrony(i, false));
                    xw.WriteEndElement();//strona;
                }
                xw.WriteEndElement();//dokument;
            }
            xw.WriteEndElement();//dokumentyLogiczne
            xw.WriteEndElement();//skan
            xw.WriteEndDocument();
            xw.Close();
            return sb.ToString();
        }

        public string GenerujMetadaneDokumentuLogicznego(int nrDokumentu)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter xw = XmlWriter.Create(sb);
            xw.WriteStartDocument();
            xw.WriteStartElement("dokument");
            xw.WriteElementString("dataPobrania", DateTime.Now.ToString());
            xw.WriteElementString("nazwaPlikuDokumentu", this.NazwaPliku(nrDokumentu, 1, RodzajNazwy.Dokument));
            xw.WriteElementString("katalogPlikuDokumentu", Konfiguracja.KatalogWyjsciowy);
            xw.WriteElementString("nazwaPlikuSkanu", this._nazwaPlikuTiff);
            xw.WriteElementString("liczbaStron", this.DokumentyLogiczne[nrDokumentu - 1].LiczbaStron.ToString());
            xw.WriteStartElement("miniatura");
            xw.WriteAttributeString("szerokosc", this.Konfiguracja.SzerokoscMiniatury.ToString());
            xw.WriteAttributeString("wysokosc", this.Konfiguracja.WysokoscMiniatury.ToString());
            xw.WriteAttributeString("typ", this.Konfiguracja.FormatMiniatury.ToString());
            xw.WriteString(this.NazwaPliku(nrDokumentu, 1, RodzajNazwy.Miniatura));
            xw.WriteEndElement();//miniatura
            xw.WriteStartElement("pierwszaStrona");
            xw.WriteAttributeString("szerokosc", this._skan.Width.ToString());
            xw.WriteAttributeString("wysokosc", this._skan.Height.ToString());
            xw.WriteAttributeString("typ", this.Konfiguracja.FormatPodgladu.ToString());
            xw.WriteString(this.NazwaPliku(nrDokumentu, 1, RodzajNazwy.StronaPodgladWeb));
            xw.WriteEndElement();//pierwszaStrona
            xw.WriteNode(XmlReader.Create(new StringReader(generujSekcjePochodzenia(this._nazwaPlikuTiff))), false);
            xw.WriteEndElement();//dokument
            xw.WriteEndDocument();
            xw.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Zapisuje metadane dla pliku tiff w katalogu wyjœciowym okreœlonym w konfiguracji
        /// </summary>
        /// 
        public void ZapiszMetadane()
        {
            this.ZapiszMetadane(this._konfiguracja.KatalogWyjsciowy);
        }

        /// <summary>
        /// Zapisuje metadane dla pliku tiff we wskazanym katalogu
        /// </summary>
        /// <param name="katalog">katalog, w którym maj¹ zostaæ wygenerowane metadane</param>
        public void ZapiszMetadane(string katalog)
        {
            XmlReader xr = XmlReader.Create(new StringReader(this.GenerujMetadane()));
            FileStream fs = File.Create(katalog + "\\" + Path.GetFileNameWithoutExtension(this._nazwaPlikuTiff) + ".xml");
            XmlTextWriter xtw = new XmlTextWriter(fs, Encoding.Unicode);
            xtw.WriteNode(xr, true);
            xtw.Close();
            fs.Close();
        }

        public void ZapiszMetadaneDokumentuLogicznego(string katalog, int nrDokumentu)
        {
            XmlReader xr = XmlReader.Create(new StringReader(this.GenerujMetadaneDokumentuLogicznego(nrDokumentu)));
            FileStream fs = File.Create(katalog + "\\" + this.NazwaPliku(nrDokumentu, 1, RodzajNazwy.Metadane));
            XmlTextWriter xtw = new XmlTextWriter(fs, Encoding.Unicode);
            xtw.WriteNode(xr, true);
            xtw.Close();
            fs.Close();
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.aktywnaStrona = 0;
            return this as IEnumerator;
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get { return this[aktywnaStrona, false]; }
        }

        bool IEnumerator.MoveNext()
        {
            return (++this.aktywnaStrona <= this.LiczbaStron);
        }

        void IEnumerator.Reset()
        {

            this.aktywnaStrona = 0;
        }

        #endregion
    }

    /// <summary>
    /// Implementuje algorytm wykrycia separatora w tiffie.
    /// Dzia³anie oparte jest na procencie pikseli, których kolor jest testowany,
    /// na okreœlonej tolerancji dla koloru uznawanego za czarny oraz na progu,
    /// który okreœla minimaln¹ procentow¹ zawartoœæ czerni w próbce
    /// </summary>
    internal class WykrywaczSeparatora
    {
        /// <summary>
        /// Okreœla liczebnoœæ próbki. Domyœlnie ma wartoœæ 0.2  (20%)
        /// </summary>
        public static decimal Probka = 0.2M;

        /// <summary>
        /// Okreœla minimaln¹ zawartoœæ punktów zakwalifikowanych jako czarne w separatorze.
        /// Domyœlnie ma wartoœæ 0.8  (80%);
        /// </summary>
        public static decimal Prog = 0.8M;

        /// <summary>
        /// Definiuje czerñ, jako kolor, dla którego suma wartoœci sk³adowych RGB jest mniejsza od wartoœci podanej/
        /// Domyœlnie ma wartoœæ 10;
        /// </summary>
        public static int Tolerancja = 10;

        /// <summary>
        /// Okreœla domyœln¹ szerokoœæ miniatury, która bedzie testowana
        /// </summary>
        public static int SzerokoscProbki = 50;

        /// <summary>
        /// Okreœla domyœln¹ wysokoœæ miniatury, która bêdzie testowana
        /// </summary>
        public static int WysokoscProbki = 50;

        /// <summary>
        /// Okreœla czy wskazana strona w tiffie jest separatorem
        /// </summary>
        /// <param name="nrStrony">numer sprawdzanej strony</param>
        /// <returns>Informacjê czy wskazana strona jest separatorem wg zaimplementowanego algorytmu</returns>
        public static bool JestSeparatorem(TiffManager tm, int nrStrony)
        {
            Bitmap b = new Bitmap(tm[nrStrony, false].GetThumbnailImage(SzerokoscProbki, WysokoscProbki, null, IntPtr.Zero));

            int szer = b.Width;
            int wys = b.Height;
            int probka = (int)(szer * wys * Probka);
            int czarne = 0;
            int wartoscProgowa = (int)(szer * wys * Probka * Prog);

            Random r = new Random();
            for (int i = 0; i < probka; i++)
            {
                Color k = b.GetPixel(r.Next(szer), r.Next(wys));
                if (k.R + k.G + k.B < 30) czarne++;
                if (czarne > wartoscProgowa) break; //jeœli przekroczono próg - nie ma sensu dalej sprawdzaæ
                if (probka - i + czarne < wartoscProgowa) break; //jeœli nie ma ju¿ szans na przekroczenie progu - nie ma sensu dalej sprawdzaæ
            }
            return czarne > wartoscProgowa;
        }
    }

    /// <summary>
    /// Klasa zawieraj¹ca ustawienia konfiguracji dla klasy <see cref="TiffManager"/>.
    /// </summary>
    public class TiffManagerKonfiguracja
    {
        public ImageFormat FormatMiniatury = ImageFormat.Gif;
        public ImageFormat FormatStrony = ImageFormat.Tiff;
        public ImageFormat FormatPodgladu = ImageFormat.Gif;
        public int SzerokoscMiniatury = 150;
        public int WysokoscMiniatury = 250;
        public int SzerokoscPodgladu = 500;
        public int WysokoscPodgladu = 705;
        public string PlikKonfiguracyjnySkanerow = null;

        /// <summary>
        /// Okreœla format nazwy pliku: 
        ///  {0} - rok, {1} - miesi¹c, {2} - dzieñ, {3} - godzina, {4} - minuta, {5} - sekunda, {6} milisekunda, {7} - numer strony, {8} - przedrostek miniatury, {9} - rozszerzenie pliku
        /// </summary>
        public string FormatNazwyPliku = "{8}{0:0000}{1:00}{2:00}_{3:00}{4:00}{5:00}{6:000}S{7}.{9}"; //rok,miesiac,dzien,godzina,minuta,sekunda,ms,nrStrony,przedrostek miniatury,rozszerzenie pliku
        public string PrzedrostekMiniatury = "M";
        public string PrzedrostekPodgladu = "P";
        public string PrzedrostekStrony = "S";
        public string PrzedrostekDokumentu = "D";
        public string KatalogWyjsciowy = Path.GetTempPath();

        /// <summary>
        /// Tworzy domyœlne ustawienia konfigurcyjne.
        /// </summary>
        public TiffManagerKonfiguracja()
        {

        }

    }

    /// <summary>
    /// Klasa reprezentuj¹cca dokument logiczny.
    /// Zawiera informacje o lokalizacji stron dokumentu w skanie oraz poszczególne strony se skanu
    /// </summary>
    public class DokumentLogiczny
    {
        private int _stronaPoczatkowa;

        /// <summary>
        /// Numer pierwszej strony dokumentu logicznego w skanie
        /// </summary>
        public int StronaPoczatkowa
        {
            get { return this._stronaPoczatkowa; }
        }

        private int _stronaKoncowa;

        /// <summary>
        /// Numer ostatniej strony dokuementu logicznego w skanie
        /// </summary>
        public int StronaKoncowa
        {
            get { return this._stronaKoncowa; }
        }

        private System.Drawing.Image _skan;

        /// <summary>
        /// Zawartoœæ skanu zawieraj¹ca strony dokumentu logicznego
        /// </summary>
        public System.Drawing.Image Skan
        {
            get
            {
                return this._skan;
            }
        }

        public int LiczbaStron
        {
            get
            {
                return this._skan.GetFrameCount(FrameDimension.Page);
            }
        }

        private static ImageCodecInfo pobierzInfoKodeka(string typMime)
        {
            int j;
            ImageCodecInfo[] enkodery;
            enkodery = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < enkodery.Length; ++j)
            {
                if (enkodery[j].MimeType == typMime)
                    return enkodery[j];
            }
            return null;
        }

        /// <summary>
        /// Tworzy nowy dokument logiczny
        /// </summary>
        /// <param name="stronaPoczatkowa">Numer pierwszej stronyw skanie nale¿¹cej do dokumentu logicznego</param>
        /// <param name="stronaKoncowa">Numer ostatniej stronyw skanie nale¿¹cej do dokumentu logicznego</param>
        /// <param name="skan">Kompletny skan, z którego wydobywany jest dokument logiczny</param>
        public DokumentLogiczny(int stronaPoczatkowa, int stronaKoncowa, TiffManager tm)
        {
            this._stronaPoczatkowa = stronaPoczatkowa;
            this._stronaKoncowa = stronaKoncowa;
            this._skan = null;
            MemoryStream ms = new MemoryStream();
            MemoryStream msdl = new MemoryStream();

            ImageCodecInfo ici = pobierzInfoKodeka("image/tiff");
            System.Drawing.Imaging.Encoder enkoder = System.Drawing.Imaging.Encoder.SaveFlag;
            EncoderParameters eps = new EncoderParameters(2);

            eps.Param[0] = new EncoderParameter(enkoder, (long)EncoderValue.MultiFrame);
            eps.Param[1] = new EncoderParameter(enkoder, (long)EncoderValue.CompressionNone); //.CompressionLZW);
            tm[stronaPoczatkowa, false].Save(ms, ImageFormat.Tiff);
            Bitmap dl = (Bitmap)System.Drawing.Image.FromStream(ms, true, false);
            dl.Save(msdl, ici, eps);

            for (int i = stronaPoczatkowa + 1; i <= stronaKoncowa; i++)
            {
                eps.Param[0] = new EncoderParameter(enkoder, (long)EncoderValue.FrameDimensionPage);
                dl.SaveAdd(tm[i, false], eps);
            }
            eps.Param[0] = new EncoderParameter(enkoder, (long)EncoderValue.Flush);
            dl.SaveAdd(eps);
            this._skan = System.Drawing.Image.FromStream(msdl, true, false);
        }

        /// <summary>
        /// Zapisuje zawartoœc dokumentu logicznego do wskazanego pliku
        /// </summary>
        /// <param name="nazwaPliku">Nazwa pliku, w którym ma zostaæ zapisany dokument</param>
        public void Zapisz(string nazwaPliku)
        {
            System.Drawing.Imaging.Encoder enkoder = System.Drawing.Imaging.Encoder.SaveFlag;

            EncoderParameters eps = new EncoderParameters(2);
            eps.Param[0] = new EncoderParameter(enkoder, (long)EncoderValue.MultiFrame);
            eps.Param[1] = new EncoderParameter(enkoder, (long)EncoderValue.CompressionCCITT3); // dziala tylko LZW!!! why?
            this._skan.Save(nazwaPliku, pobierzInfoKodeka("image/tiff"), eps);
            int ileStron = this._skan.GetFrameCount(FrameDimension.Page);
            eps.Param[0] = new EncoderParameter(enkoder, (long)EncoderValue.FrameDimensionPage);
            for (int i = 1; i < ileStron; i++)
            {
                this._skan.SelectActiveFrame(FrameDimension.Page, i);
                this._skan.SaveAdd(this._skan, eps);
            }
            eps.Param[0] = new EncoderParameter(enkoder, (long)EncoderValue.Flush);
            this._skan.SaveAdd(eps);
        }

    }

    public enum RodzajNazwy
    {
        Miniatura,
        Strona,
        StronaPodgladWeb,
        StronaPelnaWeb,
        Dokument,
        Metadane
    }
}