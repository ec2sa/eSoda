using System;
using System.Collections.Generic;
using System.Text;

namespace Pemi.Esoda.Presenters
{
    public class SearchCustomersEventArgs : EventArgs
    {
        public readonly int IdTypu;
        public readonly int IdKategorii;
        public readonly string Imie;
        public readonly string Nazwisko;
        public readonly string Nazwa;
        public readonly string Miasto;
        public readonly string Kod;
        public readonly string Ulica;
        public readonly string Budynek;
        public readonly string Lokal;
        public  string Nip;
        public readonly string Poczta;
        public  string NumerSMS;

        public readonly bool SearchListVisible;

        public SearchCustomersEventArgs(int idTypu, int idKategorii, string imie, string nazwisko, string nazwa, string miasto, string kod,
            string ulica, string budynek, string lokal,string nip,string poczta,string numerSMS)
        {
            this.IdTypu = idTypu;
            this.IdKategorii = idKategorii;
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Nazwa = nazwa;
            this.Miasto = miasto;
            this.Kod = kod;
            this.Ulica = ulica;
            this.Budynek = budynek;
            this.Lokal = lokal;
            this.SearchListVisible = true;
            this.Nip = nip;
            this.Poczta = poczta;
            this.NumerSMS = numerSMS;
        }

        public SearchCustomersEventArgs(bool visible)
        {
            this.SearchListVisible = visible;
        }
    }
}
