using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pemi.Esoda.Web.UI.ePUAP.Skrytka.PullService;
using System.Text;

namespace Pemi.Esoda.Web.UI.Classes
{
    public class ePUAPPullMock:pull
    {
        private List<OdpowiedzPullPobierzTyp> docs = new List<OdpowiedzPullPobierzTyp>();

        private int count;

        public ePUAPPullMock() : this(5) { }

        public ePUAPPullMock(int itemCount)
        {
            
            for (int i = 1; i <= itemCount; i++)
            {
                DokumentTyp dt = new DokumentTyp();
                dt.typPliku = "text/xml";
                dt.nazwaPliku = string.Format("dokument_{0}", i);
                dt.zawartosc = Encoding.Unicode.GetBytes(string.Format("<dane>testowe dane dokumentu nr {0}",i));
                OdpowiedzPullPobierzTyp odp = new OdpowiedzPullPobierzTyp();
                odp.dokument = dt;
                odp.dataNadania = DateTime.Today.AddDays(-3 * i);
                odp.daneNadawcy = new DaneNadawcyTyp();
                odp.daneNadawcy.uzytkownik = string.Format("Jan Nowak {0}",i);
                docs.Add(odp);
                this.count++;
            }
            
        }
        #region pull Members

        public oczekujaceDokumentyResponse oczekujaceDokumenty(oczekujaceDokumentyRequest request)
        {
            OdpowiedzPullOczekujaceTyp odp = new OdpowiedzPullOczekujaceTyp();
            odp.oczekujace = count;
            oczekujaceDokumentyResponse resp = new oczekujaceDokumentyResponse(odp);
            return resp;
        }

        public IAsyncResult BeginoczekujaceDokumenty(oczekujaceDokumentyRequest request, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public oczekujaceDokumentyResponse EndoczekujaceDokumenty(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public pobierzNastepnyResponse pobierzNastepny(pobierzNastepnyRequest request)
        {
            if (count <= 0)
                return null;
            //throw new ArgumentOutOfRangeException("No documents found");

            OdpowiedzPullPobierzTyp odp = docs[count - 1];
            pobierzNastepnyResponse resp= new pobierzNastepnyResponse(odp);
            docs.Remove(docs[count - 1]);
            count--;
            return (resp);
        }

        public IAsyncResult BeginpobierzNastepny(pobierzNastepnyRequest request, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public pobierzNastepnyResponse EndpobierzNastepny(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public potwierdzOdebranieResponse potwierdzOdebranie(potwierdzOdebranieRequest request)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginpotwierdzOdebranie(potwierdzOdebranieRequest request, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public potwierdzOdebranieResponse EndpotwierdzOdebranie(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
