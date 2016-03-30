using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Pull = Pemi.Esoda.Web.UI.ePUAP.Skrytka.PullService;
using Box = Pemi.Esoda.Web.UI.ePUAP.Skrytka.Service;

namespace Pemi.Esoda.Web.UI.Classes
{
    public class ePUAPQueue
    {
        public int QueueCount { get; internal set; }
        public ePUAPStatus Status { get; internal set; }

        public ePUAPQueue(Pull.OdpowiedzPullOczekujaceTyp response)
        {
            QueueCount = response.oczekujace;
            Status = new ePUAPStatus(response.status.kod == 1 ? StatusCode.Success : StatusCode.Failure, response.status.komunikat);
        }
    }

    public class ePUAPDocument
    {
        public string ResponseAddress { get; internal set; }
        public string RequestAddress { get; internal set; }
        public ePUAPSender Sender { get; internal set; }
        public ePUAPEntity Entity { get; internal set; }
        public DateTime SendDate { get; internal set; }
        public ePUAPAttachment Attachment { get; internal set; }
        public byte[] AdditionalData { get; internal set; }
        public ePUAPStatus RequestStatus { get; internal set; }

        public ePUAPDocument(Pull.OdpowiedzPullPobierzTyp response)
        {
            ResponseAddress = response.adresOdpowiedzi;
            RequestAddress = response.adresSkrytki;
            Sender = new ePUAPSender(response.daneNadawcy.uzytkownik, response.daneNadawcy.system);
            Entity = new ePUAPEntity(response.danePodmiotu.identyfikator, response.danePodmiotu.imieSkrot, response.danePodmiotu.nazwiskoNazwa, response.danePodmiotu.nip, response.danePodmiotu.pesel, response.danePodmiotu.regon, response.danePodmiotu.typOsoby);
            SendDate = response.dataNadania;
            Attachment = new ePUAPAttachment(response.dokument.nazwaPliku, response.dokument.typPliku, response.dokument.zawartosc);
            AdditionalData = response.daneDodatkowe;
            RequestStatus = new ePUAPStatus(response.status.kod == 0 ? StatusCode.Success : StatusCode.Failure, response.status.komunikat);
        }
    }

    public class ePUAPUPP
    {
        public string UPPIdentifier { get; internal set; }
        public ePUAPAttachment UPPAttachment { get; internal set; }
        public ePUAPStatus ResponseStatus { get; internal set; }

        public ePUAPUPP(Box.OdpowiedzSkrytkiTyp response)
        {
            UPPIdentifier = response.identyfikatorUpp;
            UPPAttachment = new ePUAPAttachment(response.zalacznik.nazwaPliku, response.zalacznik.typPliku, response.zalacznik.zawartosc);
            ResponseStatus = new ePUAPStatus(response.status.kod == 1 ? StatusCode.Success : StatusCode.Failure, response.status.komunikat);
        }
    }

    public class ePUAPSender
    {
        public string UserName { get; internal set; }
        public string System { get; internal set; }

        public ePUAPSender(string userName, string system)
        {
            UserName = userName;
            System = system;
        }
    }

    public class ePUAPEntity
    {
        public string Identifier { get; internal set; }
        public string FirstName { get; internal set; }
        public string Name { get; internal set; }
        public string NIP { get; internal set; }
        public string PESEL { get; internal set; }
        public string REGON { get; internal set; }
        public string EntityType { get; internal set; }

        public ePUAPEntity(string identifier, string firstName, string name, string nip, string pesel, string regon, string entityType)
        {
            Identifier = identifier;
            FirstName = firstName;
            Name = name;
            NIP = nip;
            PESEL = pesel;
            REGON = regon;
            EntityType = entityType;
        }
    }

    public class ePUAPAttachment
    {
        public string FileName { get; internal set; }
        public string FileType { get; internal set; }
        public byte[] Content { get; internal set; }

        public ePUAPAttachment(string fileName, string fileType, byte[] content)
        {
            FileName = fileName;
            FileType = fileType;
            Content = content;
        }
    }

    public class ePUAPStatus
    {
        public StatusCode Code { get; internal set; }
        public string Message { get; internal set; }

        public ePUAPStatus(StatusCode code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public enum StatusCode { Success, Failure }

}
