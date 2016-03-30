using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using Pemi.Esoda.DataAccess;

namespace Pemi.Esoda.Tools
{
    public class ImageHelper
    {
			private int poczatkowaSzerokosc = 400;

			private string _nazwaPlikuOryginalu;

			public string NazwaPlikuRoboczego
			{
				get { return Path.GetFileNameWithoutExtension(_nazwaPlikuOryginalu) + ".gif"; }
			
			}

        public string UrlObrazka
        {
            get
            {
                return string.Format("{0}/{1}",Configuration.VirtualTemporaryDirectory,Path.GetFileNameWithoutExtension(_nazwaPlikuOryginalu)+".gif");
            }
        }

        private int _skala;

        public int Skala
        {
            get { return _skala; }
            set
            {
                if (value < 1){ _skala=1; return;}
                if (value > 100) { _skala = 100; return; }
                _skala = value;
                generujAktualnyObrazek();
            }
        }

        private OrientacjaObrazka _orientacja;

        public OrientacjaObrazka Orientacja
        {
            get { return _orientacja; }
            set
            {
                _orientacja = value;
                generujAktualnyObrazek();
            }
        }

        private int _liczbaStron;

        /// <summary>
        /// Okreœla liczbê stron zawartych w dokumencie
        /// </summary>
        public int LiczbaStron
        {
            get { return _liczbaStron; }
        }

        private int _aktualnaStrona;

        /// <summary>
        /// Okreœla numer aktualnej strony (pocz¹wszy 
        /// </summary>
        public int AktualnaStrona
        {
            get { return _aktualnaStrona+1; }
            set
            {
                if (value > LiczbaStron || value < 1) return;
                _aktualnaStrona = value-1;
                generujAktualnyObrazek();
            }
        }

        private int _szerokosc;

        /// <summary>
        /// Okreœla szerokoœc obrazu w pikselach
        /// </summary>
        public int Szerokosc
        {
            get { return _szerokosc; }
        }

        private int _wysokosc;

        /// <summary>
        /// Okreœla wysokoœæ obrazu w pikselach
        /// </summary>
        public int Wysokosc
        {
            get { return _wysokosc; }
        }

        private int _aktualnaSzerokosc;

        public int AktualnaSzerokosc
        {
            get { return _aktualnaSzerokosc; }
            set {
                if (value > _szerokosc) { _aktualnaSzerokosc = _szerokosc; _skala = 100; return; }
                _aktualnaSzerokosc = value;
                Skala =(int)(value*100 / _szerokosc);
                _aktualnaWysokosc = _wysokosc * Skala / 100;
        }
        }

        private int _aktualnaWysokosc;

        public int AktualnaWysokosc
        {
            get { return _aktualnaWysokosc; }
            set
            {
                if (value > _wysokosc) { _aktualnaWysokosc = _wysokosc; _skala = 100; return; }
                _aktualnaWysokosc = value;
                Skala = (int)(value * 100 / _wysokosc);
                _aktualnaWysokosc = _wysokosc * Skala / 100;
            }
        }


        /// <summary>
        /// Okreœla stosunek szerokoœci obrazu do jego wysokoœci
        /// </summary>
        public decimal ProporcjeKsztaltu
        {
            get
            {
                if (_wysokosc == 0) return 0;
                return ((decimal)_szerokosc) / ((decimal)_wysokosc);
            }
        }

        private System.Drawing.Image _obrazekRoboczy;

			public ImageHelper(Guid id)
			{
				this._nazwaPlikuOryginalu = id.ToString()+"p";
				IItemStorage storage=ItemStorageFactory.Create();
				if(!storage.Exists(id)) throw new ArgumentException("Nie ma takiego skanu");
				_obrazekRoboczy = System.Drawing.Image.FromStream(storage.Load(id), false, false);
				this._liczbaStron = _obrazekRoboczy.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
				this._aktualnaStrona = 0;
				this._szerokosc = _obrazekRoboczy.Width;
				this._wysokosc = _obrazekRoboczy.Height;
				this._skala = poczatkowaSzerokosc * 100 / this._szerokosc;
				this.generujAktualnyObrazek();
			}

        public ImageHelper(string nazwaPlikuOryginalu)
        {
            
            this._nazwaPlikuOryginalu = nazwaPlikuOryginalu;
            string sciezkaFizyczna = HttpContext.Current.Server.MapPath(this.UrlObrazka);
            _obrazekRoboczy = System.Drawing.Image.FromFile(_nazwaPlikuOryginalu);
            this._liczbaStron = _obrazekRoboczy.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);
            this._aktualnaStrona = 0;
            this._szerokosc = _obrazekRoboczy.Width;
            this._wysokosc = _obrazekRoboczy.Height;
            this._skala=poczatkowaSzerokosc*100/this._szerokosc;
            this.generujAktualnyObrazek();
        }

        private void generujAktualnyObrazek(){

            _obrazekRoboczy.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, this._aktualnaStrona);
            this._szerokosc = _obrazekRoboczy.Width;
            this._wysokosc = _obrazekRoboczy.Height;
            System.Drawing.Image tmp=_obrazekRoboczy.GetThumbnailImage(this._szerokosc * this._skala / 100, this._wysokosc * this._skala / 100, null, IntPtr.Zero);
            switch (_orientacja)
            {
                case OrientacjaObrazka.ObrotPrawo90: tmp.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone); break;
                case OrientacjaObrazka.ObrotLewo90: tmp.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone); break;
                case OrientacjaObrazka.Obrot180: tmp.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone); break;
            }
            tmp.Save(HttpContext.Current.Server.MapPath(UrlObrazka),System.Drawing.Imaging.ImageFormat.Gif); // wywalic do konfiguracji
            tmp.Dispose();
        }

        public void WyczyscKatalogRoboczy()
        {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(this.UrlObrazka));

        }
    }

    public enum OrientacjaObrazka
    {
        Oryginalna ,
        ObrotPrawo90 ,
        ObrotLewo90,
        Obrot180
    }
}
