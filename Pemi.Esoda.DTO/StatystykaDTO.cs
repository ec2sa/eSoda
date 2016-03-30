using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Pemi.Esoda.DTO
{
	public class StatystykaDTO:BaseDTO
	{
		private string _nazwaProcedury;

		public string NazwaProcedury
		{
			get { return _nazwaProcedury; }
			set { _nazwaProcedury = value; }
		}

		private string _tytul;

		public string Tytul
		{
			get { return _tytul; }
			set { _tytul = value; }
		}

		private string _opis;

		public string Opis
		{
			get { return _opis; }
			set { _opis = value; }
		}

		private string _xslt;

		public string Xslt
		{
			get { return _xslt; }
		}

		public bool PosiadaXslt
		{
			get
			{
				return _xslt != null && _xslt != string.Empty;
			}
		}

		private List<ParametrStatystyki> _parametry;

		public List<ParametrStatystyki> Parametry
		{
			get { return _parametry; }
			set { _parametry = value; }
		}

		public StatystykaDTO(int id, string nazwaProcedury, string tytul, string opis, List<ParametrStatystyki> parametry,string xslt)
		{
			this.ID = id;
			this._nazwaProcedury = nazwaProcedury;
			this._tytul = tytul;
			this._opis = opis;
			this._parametry = parametry;
			this._xslt = xslt;
		}
	}

	public class ParametrStatystyki
	{
		private string _nazwa;

		public string Nazwa
		{
			get { return _nazwa; }
			set { _nazwa = value; }
		}

		private string _typ;

		public string Typ
		{
			get { return _typ; }
			set { _typ = value; }
		}

		private bool _wymagany;

		public bool Wymagany
		{
			get { return _wymagany; }
			set { _wymagany = value; }
		}

		private string _domyslnaWartosc;

		public string DomyslnaWartosc
		{
			get { return _domyslnaWartosc; }
			set { _domyslnaWartosc = value; }
		}

        private string _zrodloDanych;

        public string ZrodloDanych
        {
            get { return _zrodloDanych; }
            set { _zrodloDanych = value; }
        }

		public ParametrStatystyki(string nazwa, string typ, bool wymagany, string domyslnaWartosc, string zrodloDanych)
		{
			this._nazwa = nazwa;
			this._typ = typ;
			this._wymagany = wymagany;
			this._domyslnaWartosc = domyslnaWartosc;
            this._zrodloDanych = zrodloDanych;
			
		}
	}
}
