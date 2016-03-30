using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pull = Pemi.Esoda.Web.UI.ePUAP.Skrytka.PullService;
using Box = Pemi.Esoda.Web.UI.ePUAP.Skrytka.Service;
using System.Configuration;

namespace Pemi.Esoda.Web.UI.Classes
{
    public class ePUAPHelper
    {
        private string _entityName = null;

        private string _boxName = null;

        #region boxAddress
        /// <summary>
        /// zawiera adres skrytki w formacie /podmiot/nazwaSkrytki
        /// </summary>
        protected string boxAddress
        {
            get
            {
                return string.Format("/{0}/{1}", _entityName, _boxName);
            }
        }
        #endregion

        #region isConfigured
        /// <summary>
        /// określa czy konfiguracja skrytki zawiera potrzebne informacje (podmiot i nazwa skrytki)
        /// </summary>
        protected bool isConfigured
        {
            get
            {
                return !(string.IsNullOrEmpty(_entityName) || string.IsNullOrEmpty(_boxName));
            }
        }
        #endregion

        #region pullProxy
        /// <summary>
        /// zwraca instancję klasy implementującej interfejs pull
        /// </summary>
        protected Pull.pull pullProxy
        {
            get
            {
                Pull.pullClient proxy = new Pull.pullClient();
                proxy.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
                return proxy;
            }
        }
        #endregion

        #region boxProxy
        /// <summary>
        /// zwraca instancję klasy implementującej interfejs skrytka
        /// </summary>
        protected Box.skrytka boxProxy
        {
            get
            {
                Box.skrytkaClient proxy = new Box.skrytkaClient();
                proxy.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;
                return proxy;
            }
        }
        #endregion

        #region default constructor
        /// <summary>
        /// tworzy instancję helpera i wstępnie ją konfiguruje do pracy ze skrytką
        /// </summary>
        public ePUAPHelper()
        {
            _entityName = ConfigurationManager.AppSettings["ePUAPPodmiot"];
            _boxName = ConfigurationManager.AppSettings["ePUAPNazwaSkrytki"];
        }
        #endregion

        #region constructor
        /// <summary>
        /// tworzy instancję helpera i wstępnie ją konfiguruje do pracy ze skrytką
        /// </summary>
        /// <param name="podmiot">identyfikator podmiotu z ePUAP-u</param>
        /// <param name="nazwaSkrytki">nazwa skrytki z ePUAP-u</param>
        public ePUAPHelper(string podmiot, string nazwaSkrytki)
        {
            _entityName = podmiot;
            _boxName = nazwaSkrytki;
        }
        #endregion

        #region GetQueueCount
        /// <summary>
        /// zwraca liczbę dokumentów oczekujących w skrytce
        /// </summary>
        /// <returns>liczba dokumentów</returns>
        public ePUAPQueue GetQueueCount()
        {
            if (!isConfigured)
                throw new ArgumentException("Brak podmiotu lub nazwy skrytki");

            Pull.ZapytaniePullOczekujaceTyp query = new Pull.ZapytaniePullOczekujaceTyp();
            query.podmiot = _entityName;
            query.nazwaSkrytki = _boxName;
            query.adresSkrytki = boxAddress;

            Pull.oczekujaceDokumentyRequest reqest = new Pull.oczekujaceDokumentyRequest(query);

            try
            {
                Pull.oczekujaceDokumentyResponse response = pullProxy.oczekujaceDokumenty(reqest);
                if (response != null)
                {
                        return new ePUAPQueue(response.OdpowiedzPullOczekujace);
                }
                else
                    throw new Exception("Wystąpił błąd na etapie komunikacji z serwisem: nie otrzymano odpowiedzi.");
            }
            catch (Exception ex)
            {
                throw new Exception("Wystąpił wyjątek na etapie komunikacji z serwisem", ex);
            }
        }
        #endregion

        #region GetDocument
        /// <summary>
        /// zwraca dokument pobrany ze skrytki
        /// </summary>
        /// <returns>dokument w postaci instancji klasy DokumentTyp</returns>
        public ePUAPDocument GetDocument()
        {
            if (!isConfigured)
                throw new ArgumentException("Brak podmiotu lub nazwy skrytki");

            Pull.ZapytaniePullPobierzTyp query = new Pull.ZapytaniePullPobierzTyp();
            query.adresSkrytki = boxAddress;
            query.nazwaSkrytki = _boxName;
            query.podmiot = _entityName;
            Pull.pobierzNastepnyRequest request = new Pull.pobierzNastepnyRequest(query);
            try
            {
                Pull.pobierzNastepnyResponse response = pullProxy.pobierzNastepny(request);
                if (response != null)
                {
                    return new ePUAPDocument(response.OdpowiedzPullPobierz);
                }
                else
                    throw new Exception("Wystąpił błąd na etapie komunikacji z serwisem: nie otrzymano odpowiedzi.");
            }
            catch (Exception ex)
            {
                throw new Exception("Wystąpił wyjątek na etapie komunikacji z serwisem", ex);
            }
        }
        #endregion

        #region ConfirmReceipt
        /// <summary>
        /// Potwierdza poprawne odebranie dokumentu ze skrytki
        /// </summary>
        public bool ConfirmReceipt(string shortcut, out string errorMessage)
        {
            errorMessage = null;

            if (!isConfigured)
                throw new ArgumentException("Brak podmiotu lub nazwy skrytki");

            Pull.ZapytaniePullPotwierdzTyp query = new Pull.ZapytaniePullPotwierdzTyp();
            query.adresSkrytki = boxAddress;
            query.nazwaSkrytki = _boxName;
            query.podmiot = _entityName;
            query.skrot = shortcut;
            Pull.potwierdzOdebranieRequest request = new Pull.potwierdzOdebranieRequest(query);

            try
            {
                Pull.potwierdzOdebranieResponse response = pullProxy.potwierdzOdebranie(request);
                if (response != null)
                {
                    if (response.OdpowiedzPullPotwierdz.status.kod == 1)
                        return true;
                    else
                        errorMessage = response.OdpowiedzPullPotwierdz.status.komunikat;
                }
                else
                    throw new Exception("Wystąpił błąd na etapie komunikacji z serwisem: nie otrzymano odpowiedzi.");
            }
            catch (Exception ex)
            {
                throw new Exception("Wystąpił wyjątek na etapie komunikacji z serwisem", ex);
            }

            return false;
        }
        #endregion

        #region SendResponse
        /// <summary>
        /// Przekazuje dokument do skrytki
        /// </summary>
        /// <param name="documentContent">zawartość przekazywanego dokumentu</param>
        public ePUAPUPP SendResponse(string responseAddress, byte[] documentContent)
        {
            if (!isConfigured)
                throw new ArgumentException("Brak podmiotu lub nazwy skrytki");

            Box.nadajRequest request = new Box.nadajRequest();
            request.AdresOdpowiedzi = boxAddress;
            request.AdresSkrytki = responseAddress;
            request.CzyProbne = false;
            request.Dokument = new Box.DokumentTyp();
            request.Dokument.nazwaPliku = "UPO.XML";
            request.Dokument.typPliku = "XML";
            request.Dokument.zawartosc = documentContent;
            request.IdentyfikatorPodmiotu = _entityName;

            try
            {
                Box.nadajResponse response = boxProxy.nadaj(request);

                if (response != null)
                {
                    return new ePUAPUPP(response.OdpowiedzSkrytki);
                }
                else
                    throw new Exception("Wystąpił błąd na etapie komunikacji z serwisem: nie otrzymano odpowiedzi.");
            }
            catch (Exception ex)
            {
                throw new Exception("Wystąpił wyjątek na etapie komunikacji z serwisem", ex);
            }
        }
        #endregion
    }
}
