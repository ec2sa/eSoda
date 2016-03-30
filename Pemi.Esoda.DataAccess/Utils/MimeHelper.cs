using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.DataAccess.Utils
{
    public class MimeHelper
    {
        private static List<TypMime> _typy;

        public static TypMime PobierzTypDlaRozszerzenia(string ext)
        {
					if (ext[0] == '.')
						ext = ext.Remove(0, 1);
                foreach (TypMime t in _typy)
                {
                    foreach (string r in t.Rozszerzenia)
                    {
                        if (r.ToLower().Trim() == ext.ToLower().Trim())
                            return t;
                    }
                }
                return null;
        }
        static MimeHelper()
        {
            _typy = new List<TypMime>();
            _typy.Add(new TypMime("text/xml", "dokument XML", "xmlDoc.png", new string[] {"xml"},false));
            _typy.Add(new TypMime("image/tiff", "obraz Tiff", "tiffImage.png", new string[] { "tif","tiff" },true));
            _typy.Add(new TypMime("image/gif", "obraz GIF", "gifImage.png", new string[] { "gif" },true));
            _typy.Add(new TypMime("image/png", "obraz PNG", "pngImage.png", new string[] { "png" },true));
            _typy.Add(new TypMime("image/jpeg", "obraz JPG", "jpgImage.png", new string[] { "jpg","jpeg" },true));
            _typy.Add(new TypMime("application/pdf", "dokument PDF", "pdfDoc.png", new string[] { "pdf" },false));
            _typy.Add(new TypMime("application/msword", "dokument Microsoft Word", "WordDoc.png", new string[] { "doc","docx" },false));
            _typy.Add(new TypMime("application/msexcel", "dokument Microsoft Excel", "ExcelDoc.png", new string[] { "xls", "xlsx" }, false));
        }
    }

    public class TypMime
    {

			private bool _browsable;

			public bool Browsable
			{
				get { return _browsable; }
		
			}

        private string _nazwa;

        public string Nazwa
        {
            get { return _nazwa; }

        }

        private string _opis;

        public string Opis
        {
            get { return _opis; }

        }

        private string _ikona;

        public string Ikona
        {
            get { return _ikona; }

        }

        private string[] _rozszerzenia;

        public string[] Rozszerzenia
        {
            get { return _rozszerzenia; }
  
        }

        public TypMime(string nazwa, string opis, string ikona,string[] rozszerzenia,bool browsable)
        {
					this._browsable = browsable;
            this._nazwa = nazwa;
            this._opis = opis;
            this._ikona = ikona;
            this._rozszerzenia = rozszerzenia;
        }
    }
}
